using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.ManageCategory;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Commands
{
    public class CreateCategoryCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_CallsRepositoryWithNoCategories_GivenNoErrors
        (
            [Frozen] Mock<IAdminCategoryRepository> adminCategroyRepository,
            CreateCategoryCommand createCategoryCommand,
            CreateCategoryCommandHandler sut,
            Category category
        )
        {
            adminCategroyRepository.Setup(x => x.CreateCategory(category)).ReturnsAsync(1);
            //Arrange

            //Act
            await sut.Handle(createCategoryCommand, CancellationToken.None);

            //Assert          
            adminCategroyRepository.Verify(
                x => x.CreateCategory(It.IsAny<Category>()), Times.Once);
        }
    }
}
