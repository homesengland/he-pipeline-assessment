using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Describers;
using Elsa.Metadata;
using MediatR;

namespace Elsa.Server.Features.Activities.CustomActivityProperties
{
    public class CustomPropertyCommandHandler : IRequestHandler<CustomPropertyCommand, Dictionary<string, List<HeActivityInputDescriptor>>>
    {
        private readonly ICustomPropertyDescriber _describer;
        public CustomPropertyCommandHandler(ICustomPropertyDescriber describer)
        {
            _describer = describer;
        }

        public Task<Dictionary<string, List<HeActivityInputDescriptor>>> Handle(CustomPropertyCommand request, CancellationToken cancellationToken)
        {
            IDictionary<string, IEnumerable<HeActivityInputDescriptor>> propertyResponses = new Dictionary<string, IEnumerable<HeActivityInputDescriptor>>();
            propertyResponses.Add("QuestionProperties", _describer.DescribeInputProperties(typeof(Question)));
            return Task.FromResult(propertyResponses);
        }
    }
}
