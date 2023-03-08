using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.PropertyDecorator;
using Elsa.CustomActivities.Resolver;
using Xunit;

namespace Elsa.CustomActivities.Tests.Resolver
{
    public class CustomUIHintResolverTests
    {
        [Fact]
        public void GetUIHint_ShouldReturnSingleLineOutput_GivenAString()
        {
            //Arrange
            var testClassType = new TestClassWithOneStringProperty().GetType();
            var property = testClassType.GetProperties().First();

            //Act
            var result = CustomUIHintResolver.GetUIHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal(HePropertyUIHints.SingleLine, result);
        }

        [Fact]
        public void GetUIHint_ShouldReturnCheckboxOutput_GivenABool()
        {
            //Arrange
            var testClassType = new TestClassWithOneBooleanProperty().GetType();
            var property = testClassType.GetProperties().First();

            //Act
            var result = OutputTypeHintResolver.GetOutputTypeHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal(HePropertyUIHints.Checkbox, result);
        }

        [Fact]
        public void GetUIHint_ShouldReturnCheckboxOutput_GivenANullableBool()
        {
            //Arrange
            var testClassType = new TestClassWithOneNullableBooleanProperty().GetType();
            var property = testClassType.GetProperties().First();

            //Act
            var result = OutputTypeHintResolver.GetOutputTypeHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal(HePropertyUIHints.Checkbox, result);
        }

        //[Fact]
        //public void GetOutputTypeHint_ShouldReturnNumberOutput_GivenAnInt()
        //{
        //    //Arrange
        //    var testClassType = new TestClassWithOneIntProperty().GetType();
        //    var property = testClassType.GetProperties().First();

        //    //Act
        //    var result = OutputTypeHintResolver.GetOutputTypeHint<HeActivityInputAttribute>(property);

        //    //Assert
        //    Assert.Equal(ExpectedOutputHints.Number, result);
        //}

        //[Fact]
        //public void GetOutputTypeHint_ShouldReturnTextOutput_GivenAnyOtherTypeWhenNoExpectedOutputTypeProvided()
        //{
        //    //Arrange
        //    var testClassType = new TestClassWithOneOtherProperty().GetType();
        //    var property = testClassType.GetProperties().First();

        //    //Act
        //    var result = OutputTypeHintResolver.GetOutputTypeHint<HeActivityInputAttribute>(property);

        //    //Assert
        //    Assert.Equal(ExpectedOutputHints.Text, result);
        //}

        //[Fact]
        //public void GetOutputTypeHint_ShouldReturnTextOutput_GivenAnyOtherTypeAndExpectedOutputTypeProvided()
        //{
        //    //Arrange
        //    var testClassType = new TestClassWithOnePropertyAndExpectedOutputType().GetType();
        //    var property = testClassType.GetProperties().First();

        //    //Act
        //    var result = OutputTypeHintResolver.GetOutputTypeHint<HeActivityInputAttribute>(property);

        //    //Assert
        //    Assert.Equal("test", result);
        //}
    }
}
