using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Describers;
using Elsa.Metadata;
using Elsa.Server.Providers;
using MediatR;
using SQLitePCL;

namespace Elsa.Server.Features.Activities.CustomActivityProperties
{
    public class CustomPropertyCommandHandler : IRequestHandler<CustomPropertyCommand, Dictionary<string, List<HeActivityInputDescriptor>>>
    {
        private readonly ICustomPropertyDescriber _describer;
        private readonly ILogger<CustomPropertyCommandHandler> _logger;
        public CustomPropertyCommandHandler(ICustomPropertyDescriber describer, ILogger<CustomPropertyCommandHandler> logger)
        {
            _describer = describer;
            _logger = logger;
        }

        public Task<Dictionary<string, List<HeActivityInputDescriptor>>> Handle(CustomPropertyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Dictionary<string, List<HeActivityInputDescriptor>> propertyResponses = new Dictionary<string, List<HeActivityInputDescriptor>>();
                propertyResponses.Add("QuestionProperties", _describer.DescribeInputProperties(typeof(Question)));
                return Task.FromResult(propertyResponses);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error thrown whilst obtaining custom properties");
                return Task.FromResult(new Dictionary<string, List<HeActivityInputDescriptor>>());
            }
        }
    }
}
