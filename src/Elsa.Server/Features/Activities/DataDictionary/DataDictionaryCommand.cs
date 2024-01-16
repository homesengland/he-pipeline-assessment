using Elsa.CustomActivities.Describers;
using Elsa.Metadata;
using MediatR;

namespace Elsa.Server.Features.Activities.DataDictionary
{
    public class DataDictionaryCommand : IRequest<string>
    {
        public bool IncludeArchived { get; set; } = false;
    }
}
