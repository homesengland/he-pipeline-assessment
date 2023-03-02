using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.Tests.Common
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() :
            base(() => new Fixture()
                .Customize(new AutoMoqCustomization())
                .Customize(new OmitOnRecursionFixtureCustomization())
                .Customize(new AspNetCustomization()))
        {
        }
    }

    public class OmitOnRecursionFixtureCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        }
    }

    public class AspNetCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new ControllerBasePropertyOmitter());
        }
    }

    public class ControllerBasePropertyOmitter : Omitter
    {
        public ControllerBasePropertyOmitter()
            : base(new OrRequestSpecification(GetPropertySpecifications()))
        {
        }

        private static IEnumerable<IRequestSpecification> GetPropertySpecifications()
        {
            return typeof(Controller).GetProperties().Where(x => x.CanWrite)
                .Select(x => new PropertySpecification(x.PropertyType, x.Name));
        }
    }

    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object?[] objects) : base(new AutoMoqDataAttribute(), objects) { }

        //Example usage
        //[Theory]
        //[InlineAutoMoqData(1)]
        //[InlineAutoMoqData(2)]
        //[InlineAutoMoqData(3)]
        //public void Test1(int myInt, StartWorkflowCommand startWorkflowCommand)
        //{
        //    startWorkflowCommand.WorkflowDefinitionId = myInt.ToString();
        //}
    }
}