using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentTool;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Commands
{
    public class DeleteAssessmentToolCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowException_GivenDependencyThrows
        (
             
            [Frozen]Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
             Exception exception,
             DeleteAssessmentToolCommand deleteAssessmentToolCommand,
            DeleteAssessmentToolCommandHandler sut
        )
        {
            //Arrange
            adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolById(deleteAssessmentToolCommand.Id))
                .Throws(exception);

            //Act
            var result =
                await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(deleteAssessmentToolCommand, CancellationToken.None));

            //Assert          
            Assert.Equal("Unable to delete assessment tool.", result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_SetsToolAndToolWorkflowStatusToDeleted
         (
          
            [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
            DeleteAssessmentToolCommand deleteAssessmentToolCommand,
            AssessmentTool assessmentTool,
            DeleteAssessmentToolCommandHandler sut
         )
        {
            //Arrange
            adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolById(deleteAssessmentToolCommand.Id))
                .ReturnsAsync(assessmentTool);
            adminAssessmentToolRepository.Setup(x => x.SaveChanges())
                .ReturnsAsync(45);

            //Act
            var result = await sut.Handle(deleteAssessmentToolCommand, CancellationToken.None);

            //Assert          
            Assert.Equal(AssessmentToolStatus.Deleted, assessmentTool.Status);
            Assert.All(assessmentTool.AssessmentToolWorkflows!, workflow => Assert.Equal(AssessmentToolStatus.Deleted, workflow.Status));
            adminAssessmentToolRepository.Verify(x => x.SaveChanges(), Times.Once);
            Assert.Equal(45, result);
        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowException_GivenAssessmentToolNotFound
        (
            [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,           
            DeleteAssessmentToolCommand deleteAssessmentToolCommand,
            DeleteAssessmentToolCommandHandler sut
        )
        {
            //Arrange
            string exceptionMessage = $"Assessment Tool with Id {deleteAssessmentToolCommand.Id} not found";

            adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolById(deleteAssessmentToolCommand.Id))
                .ReturnsAsync((AssessmentTool?) null);

            //Act
            var result = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(deleteAssessmentToolCommand, CancellationToken.None));

            //Assert          
            Assert.Equal("Unable to delete assessment tool.", result.Message);
        }
    }
}
