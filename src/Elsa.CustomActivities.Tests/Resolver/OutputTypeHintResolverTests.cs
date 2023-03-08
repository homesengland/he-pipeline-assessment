using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.PropertyDecorator;
using Elsa.CustomActivities.Resolver;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomActivities.Tests.Resolver
{
    public class OutputTypeHintResolverTests
    {
        [Fact]
        public void GetOutputTypeHint_ShouldReturnTextOutput_GivenAString()
        {
            //Arrange
            var testClassType = new TestClassWithOneStringProperty().GetType();
            var property = testClassType.GetProperties().First();

            //Act
            var result = OutputTypeHintResolver.GetOutputTypeHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal(ExpectedOutputHints.Text, result);
        }

        [Fact]
        public void GetOutputTypeHint_ShouldReturnBooleanOutput_GivenABool()
        {
            //Arrange
            var testClassType = new TestClassWithOneBooleanProperty().GetType();
            var property = testClassType.GetProperties().First();

            //Act
            var result = OutputTypeHintResolver.GetOutputTypeHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal(ExpectedOutputHints.Boolean, result);
        }

        [Fact]
        public void GetOutputTypeHint_ShouldReturnBooleanOutput_GivenANullableBool()
        {
            //Arrange
            var testClassType = new TestClassWithOneNullableBooleanProperty().GetType();
            var property = testClassType.GetProperties().First();

            //Act
            var result = OutputTypeHintResolver.GetOutputTypeHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal(ExpectedOutputHints.Boolean, result);
        }

        [Fact]
        public void GetOutputTypeHint_ShouldReturnNumberOutput_GivenAnInt()
        {
            //Arrange
            var testClassType = new TestClassWithOneIntProperty().GetType();
            var property = testClassType.GetProperties().First();

            //Act
            var result = OutputTypeHintResolver.GetOutputTypeHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal(ExpectedOutputHints.Number, result);
        }

        [Fact]
        public void GetOutputTypeHint_ShouldReturnTextOutput_GivenAnyOtherTypeWhenNoExpectedOutputTypeProvided()
        {
            //Arrange
            var testClassType = new TestClassWithOneOtherProperty().GetType();
            var property = testClassType.GetProperties().First();

            //Act
            var result = OutputTypeHintResolver.GetOutputTypeHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal(ExpectedOutputHints.Text, result);
        }

        [Fact]
        public void GetOutputTypeHint_ShouldReturnTextOutput_GivenAnyOtherTypeAndExpectedOutputTypeProvided()
        {
            //Arrange
            var testClassType = new TestClassWithOnePropertyAndExpectedOutputType().GetType();
            var property = testClassType.GetProperties().First();

            //Act
            var result = OutputTypeHintResolver.GetOutputTypeHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal("test", result);
        }

        [Fact]
        public void GetTypeFromTypeHint_ShouldReturnBooleanType_GivenBooleanOutputType()
        {
            //Arrange

            //Act
            var result = OutputTypeHintResolver.GetTypeFromTypeHint(ExpectedOutputHints.Boolean);

            //Assert
            Assert.Equal(typeof(bool), result);
        }

        [Fact]
        public void GetTypeFromTypeHint_ShouldReturnNumberType_GivenIntOutputType()
        {
            //Arrange

            //Act
            var result = OutputTypeHintResolver.GetTypeFromTypeHint(ExpectedOutputHints.Number);

            //Assert
            Assert.Equal(typeof(int), result);
        }

        [Theory]
        [InlineAutoMoqData(ExpectedOutputHints.Checkbox)]
        [InlineAutoMoqData(ExpectedOutputHints.Radio)]
        [InlineAutoMoqData(ExpectedOutputHints.Text)]
        public void GetTypeFromTypeHint_ShouldReturnStringType_GivenAnyOtherOutputType(string output)
        {
            //Arrange

            //Act
            var result = OutputTypeHintResolver.GetTypeFromTypeHint(output);

            //Assert
            Assert.Equal(typeof(string), result);
        }
    }
}
