using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Amendment.SubmitAmendment
{
    public class SubmitAmendmentCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrorMessage_GivenInterventionRecordCannotBeFound(
           SubmitAmendmentCommand command,
           SubmitAmendmentCommandHandler sut)
        {
            //Arrange

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Confirm amendment failed. AssessmentInterventionId: {command.AssessmentInterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_UpdatesIntervention_GivenInterventionRecordCanBeFound(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SubmitAmendmentCommand command,
            AssessmentIntervention intervention,
            SubmitAmendmentCommandHandler sut,
            List<AssessmentToolWorkflowInstance> workflowsToDelete)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
                .ReturnsAsync(intervention);

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
            .ReturnsAsync(intervention);
            assessmentRepository.Setup(x => x.GetWorkflowInstancesToDeleteForAmendment(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order))
            .ReturnsAsync(workflowsToDelete);
            //Act

            await sut.Handle(command, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x => x.UpdateAssessmentIntervention(It.IsAny<AssessmentIntervention>()), Times.Once);
            Assert.Equal(intervention.Status, InterventionStatus.Approved);
            Assert.Equal(intervention.AssessmentToolWorkflowInstance.Status, AssessmentToolWorkflowInstanceConstants.Draft);
        }

    //    [Theory]
    //    [AutoMoqData]
    //    public async Task Handle_DeletesAssessmentToolWorkflows_GivenInterventionRecordCanBeFound(
    //[Frozen] Mock<IAssessmentRepository> assessmentRepository,
    //SubmitAmendmentCommand command,
    //AssessmentIntervention intervention,
    //SubmitAmendmentCommandHandler sut,
    //List<AssessmentToolWorkflowInstance> workflowsToDelete)
    //    {
    //        //Arrange
    //        assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
    //            .ReturnsAsync(intervention);

    //        assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
    //        .ReturnsAsync(intervention);
    //        assessmentRepository.Setup(x => x.GetWorkflowInstancesToDeleteForAmendment(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order))
    //        .ReturnsAsync(workflowsToDelete);
    //        //Act

    //        await sut.Handle(command, CancellationToken.None);

    //        //Assert
    //        foreach (var workflowToDelete in workflowsToDelete)
    //        {
    //            Assert.Equal(AssessmentToolWorkflowInstanceConstants.SuspendedAmendment, workflowToDelete.Status);
    //        }
        }
    }
}
