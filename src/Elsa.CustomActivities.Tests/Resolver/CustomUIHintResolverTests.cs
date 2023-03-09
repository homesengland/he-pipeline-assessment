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
            var result = CustomUIHintResolver.GetUIHint<HeActivityInputAttribute>(property);

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
            var result = CustomUIHintResolver.GetUIHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal(HePropertyUIHints.Checkbox, result);
        }

        [Fact]
        public void GetUIHint_ShouldReturnSingleLineOutput_GivenAnInt()
        {
            //Arrange
            var testClassType = new TestClassWithOneIntProperty().GetType();
            var property = testClassType.GetProperties().First();

            //Act
            var result = CustomUIHintResolver.GetUIHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal(HePropertyUIHints.SingleLine, result);
        }

        [Fact]
        public void GetUIHint_ShouldReturnDropdownOutput_GivenACollection()
        {
            //Arrange
            var testClassType = new TestClassWithOneListProperty().GetType();
            var property = testClassType.GetProperties().First();

            //Act
            var result = CustomUIHintResolver.GetUIHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal(HePropertyUIHints.Dropdown, result);
        }

        [Fact]
        public void GetUIHint_ShouldReturnDropdownOutput_GivenAUIHintAttributeProperty()
        {
            //Arrange
            var testClassType = new TestClassWithOneUIHintProperty().GetType();
            var property = testClassType.GetProperties().First();

            //Act
            var result = CustomUIHintResolver.GetUIHint<HeActivityInputAttribute>(property);

            //Assert
            Assert.Equal("TestUIHint", result);
        }
    }
}
