using Elsa.Metadata;
using MediatR;

namespace Elsa.Server.Features.Activities.CustomActivityProperties
{
    public class CustomPropertyRequest : IRequest<IDictionary<string, IEnumerable<ActivityInputDescriptor>>>
    {
    }
}
