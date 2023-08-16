using Elsa.CustomActivities.Describers;
using Elsa.Metadata;
using MediatR;

namespace Elsa.Server.Features.Activities.CustomActivityProperties
{
    public class CustomPropertyCommand : IRequest<Dictionary<string, string>>
    {
    }
}
