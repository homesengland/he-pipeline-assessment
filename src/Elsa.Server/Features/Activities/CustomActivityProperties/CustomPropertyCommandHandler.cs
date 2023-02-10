using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Describers;
using Elsa.Metadata;
using MediatR;

namespace Elsa.Server.Features.Activities.CustomActivityProperties
{
    public class CustomPropertyCommandHandler : IRequestHandler<CustomPropertyCommand, IDictionary<string, IEnumerable<ActivityInputDescriptor>>>
    {
        private readonly ICustomPropertyDescriber _describer;
        public CustomPropertyCommandHandler(ICustomPropertyDescriber describer)
        {
            _describer = describer;
        }

        public Task<IDictionary<string, IEnumerable<ActivityInputDescriptor>>> Handle(CustomPropertyCommand request, CancellationToken cancellationToken)
        {
            IDictionary<string, IEnumerable<ActivityInputDescriptor>> propertyResponses = new Dictionary<string, IEnumerable<ActivityInputDescriptor>>();
            propertyResponses.Add("QuestionProperty", _describer.DescribeInputProperties(typeof(Question)));
            return Task.FromResult(propertyResponses);
        }
    }
}
