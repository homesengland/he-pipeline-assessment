using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Describers;
using Elsa.Metadata;
using MediatR;

namespace Elsa.Server.Features.Activities.CustomActivityProperties
{
    public class CustomPropertyRequestHandler : IRequestHandler<CustomPropertyRequest, IDictionary<string, IEnumerable<ActivityInputDescriptor>>>
    {
        public Task<IDictionary<string, IEnumerable<ActivityInputDescriptor>>> Handle(CustomPropertyRequest request, CancellationToken cancellationToken)
        {
            IDictionary<string, IEnumerable<ActivityInputDescriptor>> propertyResponses = new Dictionary<string, IEnumerable<ActivityInputDescriptor>>();
            propertyResponses.Add("QuestionProperty", CustomPropertyDescribers.DescribeInputProperties(typeof(Question)));
            return Task.FromResult(propertyResponses);
        }
    }
}
