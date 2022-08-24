using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Elsa.Server.Tests
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() :
            base(() => new Fixture()
                .Customize(new AutoMoqCustomization())
                .Customize(new OmitOnRecursionFixtureCustomization()))
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

    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] objects) : base(new AutoMoqDataAttribute(), objects) { }

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