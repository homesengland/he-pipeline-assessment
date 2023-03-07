using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Commands
{

    public class CreateAssessmentToolWorkflowCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowException_GivenDependencyThrows
        (
       [Frozen] Mock<IAssessmentToolWorkflowMapper> assessmentToolWorkflowMapper,
       CreateAssessmentToolWorkflowCommand createAssessmentToolWorkflowCommand,
       Exception exception,
       CreateAssessmentToolWorkflowCommandHandler sut
        )
        {
            //Arrange
            assessmentToolWorkflowMapper.Setup(x => x.CreateAssessmentToolWorkflowCommandToAssessmentToolWorkflow(createAssessmentToolWorkflowCommand))
                .Throws(exception);

            //Act
            var result =
                await Assert.ThrowsAsync<Exception>(() => sut.Handle(createAssessmentToolWorkflowCommand, CancellationToken.None));

            //Assert          
            Assert.Equal(exception.Message, result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_CallsCreateAssessmentWorkflowRepositoryWithCorrectValues
      (
          [Frozen] Mock<IAssessmentToolWorkflowMapper> assessmentToolWorkflowMapper,
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
          CreateAssessmentToolWorkflowCommand createAssessmentToolWorkflowCommand,
          AssessmentToolWorkflow assessmentToolWorkflow,
          CreateAssessmentToolWorkflowCommandHandler sut
      )
        {
            //Arrange
            assessmentToolWorkflowMapper.Setup(x => x.CreateAssessmentToolWorkflowCommandToAssessmentToolWorkflow(createAssessmentToolWorkflowCommand))
                .Returns(assessmentToolWorkflow);

            //Act
            var result = await sut.Handle(createAssessmentToolWorkflowCommand, CancellationToken.None);

            //Assert          
            adminAssessmentToolWorkflowRepository.Verify(x => x.CreateAssessmentToolWorkflow(assessmentToolWorkflow), Times.Once);
            Assert.Equal(assessmentToolWorkflow.Id, result);
        }
    }
}