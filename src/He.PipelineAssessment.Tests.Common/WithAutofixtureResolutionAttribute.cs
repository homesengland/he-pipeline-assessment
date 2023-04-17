using AutoFixture.Xunit2;
using AutoFixture;
using System.Reflection;

namespace He.PipelineAssessment.Tests.Common
{
    public class WithAutofixtureResolutionAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new AutofixtureServiceProviderCustomization();

        class AutofixtureServiceProviderCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                var behavior = BehaviorFactory.GetPostprocessingBehavior<IServiceProvider>(AutofixtureResolutionServiceProviderSpecimenBuilder.AddAutofixtureResolution);
                fixture.Behaviors.Add(behavior);
            }
        }
    }
}
