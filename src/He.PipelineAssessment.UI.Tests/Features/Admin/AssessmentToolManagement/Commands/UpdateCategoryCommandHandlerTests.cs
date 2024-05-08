using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.ManageCategory;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Commands
{
    public class UpdateCategoryCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowException_GivenDependencyThrows
        (
            [Frozen] Mock<IAdminCategoryRepository> adminCategroyRepository,
            UpdateCategoryCommand updateCategoryCommand,
            Exception exception,
            UpdateCategoryCommandHandler sut
        )
        {
            //Arrange
            adminCategroyRepository.Setup(x => x.GetCategories())
                .Throws(exception);

            //Act
            var result =
                await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(updateCategoryCommand, CancellationToken.None));

            //Assert          
            Assert.Equal($"Unable to update category.", result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_CallsRepositoryWithNoCategories_GivenNoErrors
        (
            [Frozen] Mock<IAdminCategoryRepository> adminCategroyRepository,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            UpdateCategoryCommand updateCategoryCommand,
            UpdateCategoryCommandHandler sut,
            List<Category> categories,
            Category category
        )
        {
            adminCategroyRepository.Setup(x => x.GetCategories()).ReturnsAsync(categories);
            adminCategroyRepository.Setup(x => x.UpdateCategory(category.CategoryId, updateCategoryCommand.CategoryName)).ReturnsAsync(1);
            adminAssessmentToolWorkflowRepository.Setup(x => x.UpdateWorkflowNameAndCategory(updateCategoryCommand.CategoryName, updateCategoryCommand.CategoryOldName));
            //Arrange

            //Act
            await sut.Handle(updateCategoryCommand, CancellationToken.None);

            //Assert          
            adminCategroyRepository.Verify(
                x => x.GetCategories(),Times.Once);
            adminCategroyRepository.Verify(
                x => x.UpdateCategory(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            adminAssessmentToolWorkflowRepository.Verify(
                x => x.UpdateWorkflowNameAndCategory(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_CallsRepositoryWithCategories_GivenNoErrors
        (
            [Frozen] Mock<IAdminCategoryRepository> adminCategroyRepository,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            UpdateCategoryCommand updateCategoryCommand,
            UpdateCategoryCommandHandler sut,
            List<Category> categories,
            Category category
        )
        {
            categories.Add(new Category()
            {
                CategoryId = 1,
                CategoryName = "Test",
            });
            updateCategoryCommand.CategoryOldName = "Test";
            updateCategoryCommand.CategoryName = "Test1";
            adminCategroyRepository.Setup(x => x.GetCategories()).ReturnsAsync(categories);
            adminCategroyRepository.Setup(x => x.UpdateCategory(category.CategoryId, updateCategoryCommand.CategoryName)).ReturnsAsync(1);
            adminAssessmentToolWorkflowRepository.Setup(x => x.UpdateWorkflowNameAndCategory(updateCategoryCommand.CategoryName, updateCategoryCommand.CategoryOldName));
            //Arrange

            //Act
            await sut.Handle(updateCategoryCommand, CancellationToken.None);

            //Assert          
            adminCategroyRepository.Verify(
                x => x.GetCategories(), Times.Once);
            adminCategroyRepository.Verify(
                x => x.UpdateCategory(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            adminAssessmentToolWorkflowRepository.Verify(
                x => x.UpdateWorkflowNameAndCategory(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
