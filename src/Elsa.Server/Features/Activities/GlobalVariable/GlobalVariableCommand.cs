using Elsa.CustomActivities.Describers;
using Elsa.Metadata;
using MediatR;

namespace Elsa.Server.Features.Activities.GlobalVariableProvider
{
    public class GlobalVariableCommand : IRequest<string>
    {
        public bool IncludeArchived { get; set; } = false;
    }
}
