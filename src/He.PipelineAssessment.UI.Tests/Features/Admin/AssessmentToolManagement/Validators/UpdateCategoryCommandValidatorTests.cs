using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.ManageCategory;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Validators
{
    public class UpdateCategoryCommandValidatorTests
    {
        [Theory]
        [InlineAutoMoqData("", "The Category Name cannot be empty")]
        [InlineAutoMoqData("", "The Category Name cannot be empty")]
        public void Should_ReturnValidationMessage_WhenPropertyIsInvalid(
            string name,
            string expectedValidationMessage,
            Mock<IAdminCategoryRepository> repository)
        {
            //Arrange
            var validator = new UpdateCategoryCommandValidator(repository.Object);


            repository.Setup(x => x.GetCategories()).ReturnsAsync(new List<Category>());

            var command = new UpdateCategoryCommand
            {
                CategoryName = name,
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Single(result.Errors);
            Assert.Contains(expectedValidationMessage,
                result.Errors.Where(x => x.PropertyName == "CategoryName").Select(x => x.ErrorMessage).First());
            Assert.NotEqual("The Category Name can be empty",
                result.Errors.First(x => x.PropertyName == "CategoryName").ErrorMessage);
        }

        [Theory]
        [AutoMoqData]
        public void Should_ReturnValidationMessage_WhenCategoryNameIsNotUnique(
            string categoryName,
            Mock<IAdminCategoryRepository> repository,
            List<Category> categories)
        {
            //Arrange
            var validator = new UpdateCategoryCommandValidator(repository.Object);

            categories.ForEach(x => x.CategoryName = categoryName);
            repository.Setup(x => x.GetCategories()).ReturnsAsync(categories);

            var command = new UpdateCategoryCommand
            {
                CategoryName = categoryName,
            };

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.Single(result.Errors);
            Assert.Equal("The Category Name must be unique",
                result.Errors.First().ErrorMessage);
        }
    }
}
