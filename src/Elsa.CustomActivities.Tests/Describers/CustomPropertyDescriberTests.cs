using Elsa.CustomActivities.Describers;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomActivities.Tests.Describers
{
    public class CustomPropertyDescriberTests
    {
        [Theory]
        [AutoMoqData]
        public void DescribeInputProperties_ShouldReturnEmptyProperties_GivenTypeWithoutElsaProperties(
            CustomPropertyDescriber sut)
        {
            //Arrange
            var type = new TestClassWithOneStringProperty().GetType();

            //Act
            var result = sut.DescribeInputProperties(type);

            //Assert
            Assert.Empty(result);
        }

        [Theory]
        [AutoMoqData]
        public void DescribeInputProperties_ShouldReturnProperties_GivenTypeWithHeActivityInputAttributeProperties(
            CustomPropertyDescriber sut)
        {
            //Arrange
            var type = new TestClassWithOneHeActivityInputAttributeProperty().GetType();

            //Act
            var result = sut.DescribeInputProperties(type);

            //Assert
            var property = result.Single();
            Assert.Equal("HeActivityInput", property.Name);
            Assert.Equal("He Activity Input", property.Label);
            Assert.Null(property.Category);
        }

        [Theory]
        [AutoMoqData]
        public void DescribeInputProperties_ShouldReturnProperties_GivenTypeWithCustomElsaAttributeProperties(
            CustomPropertyDescriber sut)
        {
            //Arrange
            var type = new TestClassWithOneCustomElsaAttributeProperty().GetType();

            //Act
            var result = sut.DescribeInputProperties(type);

            //Assert
            var property = result.Single();
            Assert.Equal("CustomElsaAttributeProperty_HeActivityInput", property.Name);
            Assert.Equal("He Activity Input", property.Label);
            Assert.Null(property.Category);
        }

        [Theory]
        [AutoMoqData]
        public void DescribeInputProperties_ShouldReturnProperties_GivenTypeWithCustomElsaAttributePropertiesWithCategorySet(
            CustomPropertyDescriber sut)
        {
            //Arrange
            var type = new TestClassWithOneCustomElsaAttributePropertyAndCategory().GetType();

            //Act
            var result = sut.DescribeInputProperties(type);

            //Assert
            var property = result.Single();
            Assert.Equal("CustomElsaAttributeProperty_HeActivityInput", property.Name);
            Assert.Equal("He Activity Input", property.Label);
            Assert.Equal("TestCategory", property.Category);
        }

        [Theory]
        [AutoMoqData]
        public void DescribeInputProperties_ShouldReturnPropertiesInOrder_GivenTypeWithMultipleHeActivityInputAttributeProperties(
            CustomPropertyDescriber sut)
        {
            //Arrange
            var type = new TestClassWithMultipleHeActivityInputAttributeProperties().GetType();

            //Act
            var result = sut.DescribeInputProperties(type);

            //Assert
            Assert.Equal(3, result.Count);

            var firstProperty = result[0];
            Assert.Equal("First", firstProperty.Name);
            Assert.Equal("First", firstProperty.Label);
            Assert.Equal(1, firstProperty.Order);

            var secondProperty = result[1];
            Assert.Equal("HeActivityInput", secondProperty.Name);
            Assert.Equal("He Activity Input", secondProperty.Label);
            Assert.Equal(2, secondProperty.Order);

            var thirdProperty = result[2];
            Assert.Equal("Last", thirdProperty.Name);
            Assert.Equal("Last", thirdProperty.Label);
            Assert.Equal(5, thirdProperty.Order);
        }
    }
}
