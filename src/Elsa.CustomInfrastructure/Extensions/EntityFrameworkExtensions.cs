using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomInfrastructure.Extensions
{
    public static class EntityFrameworkExtensions
    {
        // Source - https://stackoverflow.com/a/67301571
        // Posted by Eric Patrick
        // Retrieved 2026-03-06, License - CC BY-SA 4.0

        /// <summary>
        /// Saves a range of items, handing duplicates. Returns the number of items saved.
        /// <see href="https://github.com/dotnet/efcore/issues/24780"/>
        /// </summary>
        /// <param name="dbset">DbSet to save <paramref name="items"/> to.</param>
        /// <param name="items">Items to save. May contains items with same key(s).</param>
        /// <param name="context">DbContext that DbSet belongs to. If not specified, it will be fetched via <see cref="ICurrentDbContext"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public static async ValueTask<int> SaveRangeAsync<T>(this DbSet<T> dbset, IEnumerable<T> items, DbContext? context = null, CancellationToken cancellationToken = default) where T : class
        {
            var count = 0;
            context = context ?? dbset.GetService<ICurrentDbContext>().Context;

            var keys = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(e => e.Name);

            foreach (var item in items)
            {
                var existing = context.SameOrDefault(item, keys);

                // If we hit a duplicate key, we need to save and then resume adding.
                if (existing != null)
                {
                    count += await context.SaveChangesAsync();
                    existing.CurrentValues.SetValues(item);
                }
                else
                    context.Add(item);
                if (cancellationToken.IsCancellationRequested)
                    break;
            }
            count += await context.SaveChangesAsync();
            return count;
        }

        /// <summary>
        /// Finds the first <see cref="EntityEntry"/> with keys matching <paramref name="item"/>.
        /// </summary>
        public static EntityEntry<T>? SameOrDefault<T>(this DbContext context, T item, IEnumerable<string> keys) where T : class
        {
            var entry = context.Entry(item);
            foreach (var entity in context.ChangeTracker.Entries<T>())
            {
                bool mismatch = false;
                foreach (var key in keys)
                {
                    if (!Equals(entity.Property(key).CurrentValue, entry.Property(key).CurrentValue))
                    {
                        mismatch = true;
                        break;
                    }
                }
                if (!mismatch)
                    return entity;
            }
            return default;
        }

    }
}
