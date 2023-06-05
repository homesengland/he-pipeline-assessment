using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using Moq;
using Xunit;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsEmptyDto_GivenRepoThrowsError(
                  [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                  CreateOverrideRequest request,
                  Exception exception,
                  CreateOverrideRequestHandler sut
              )
        {
            //Arrange
            var emptyDto = new AssessmentInterventionDto();

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).Throws(exception);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(emptyDto.ToString(), result.ToString());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsEmptyDto_GivenAdminRepoThrowsError(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminRepository,
          AssessmentToolWorkflowInstance workflowInstance,
          CreateOverrideRequest request,
          Exception exception,
          CreateOverrideRequestHandler sut
      )
        {
            //Arrange
            var emptyDto = new AssessmentInterventionDto();

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            adminRepository.Setup(x => x.GetAssessmentToolWorkflows()).Throws(exception);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(emptyDto.ToString(), result.ToString());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsEmptyDto_GivenMapperThrowsError(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminRepository,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
          AssessmentToolWorkflowInstance workflowInstance,
          CreateOverrideRequest request,
          List<AssessmentToolWorkflow> workflows,
          Exception exception,
          CreateOverrideRequestHandler sut
)
        {
            //Arrange
            var emptyDto = new AssessmentInterventionDto();

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            adminRepository.Setup(x => x.GetAssessmentToolWorkflows()).ReturnsAsync(workflows);
            mapper.Setup(x => x.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(workflows)).Throws(exception);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(emptyDto.ToString(), result.ToString());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsValidResult_GivenNoErrors(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminRepository,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
          AssessmentToolWorkflowInstance workflowInstance,
          string userName,
          string email,
          CreateOverrideRequest request,
          List<AssessmentToolWorkflow> workflows,
          List<TargetWorkflowDefinition> targetDefinitions,
          AssessmentInterventionDto dto,
          Exception exception,
          CreateOverrideRequestHandler sut
)
        {
            //Arrange
            dto.TargetWorkflowDefinitions = targetDefinitions;

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            adminRepository.Setup(x => x.GetAssessmentToolWorkflows()).ReturnsAsync(workflows);
            mapper.Setup(x => x.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(workflows)).Throws(exception);
            mapper.Setup(x => x.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance, userName, email)).Returns(dto);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(dto.ToString(), result.ToString());
        }
    }
}
