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
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditOverride;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using He.PipelineAssessment.UI.Common.Exceptions;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.EditOverride
{
    public class EditOverrideRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task HandleThrowsError_GivenRepoReturnsNullResponse(
                     [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                     EditOverrideRequest request,
                     AssessmentIntervention? intervention,
                     EditOverrideRequestHandler sut
                 )
        {
            //Arrange
            intervention = null;

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(($"Assessment Intervention with Id {request.InterventionId} not found"), ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsException_GivenAdminRepoReturnsEmptyList(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminRepository,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
          AssessmentInterventionCommand command,
          AssessmentIntervention intervention,
          EditOverrideRequest request,
          Exception exception,
          EditOverrideRequestHandler sut
      )
        {
            //Arrange
            var emptyDto = new AssessmentInterventionDto();
            var emptyListOfWorkflows = new List<AssessmentToolWorkflow>();

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns(command);
            adminRepository.Setup(x => x.GetAssessmentToolWorkflows()).ReturnsAsync(emptyListOfWorkflows);

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(($"No Assessment tool workflows found"), ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsValidResult_GivenNoErrors(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminRepository,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
          AssessmentInterventionCommand command,
          List<AssessmentToolWorkflow> workflows,
          List<TargetWorkflowDefinition> targetWorkflowDefinitions,
          AssessmentIntervention intervention,
          EditOverrideRequest request,
          EditOverrideRequestHandler sut
    )
        {
            var dto = new AssessmentInterventionDto
            {
                AssessmentInterventionCommand = command,
                TargetWorkflowDefinitions = targetWorkflowDefinitions
            };
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns(command);
            mapper.Setup(x => x.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(workflows)).Returns(targetWorkflowDefinitions);
            adminRepository.Setup(x => x.GetAssessmentToolWorkflows()).ReturnsAsync(workflows);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(dto.ToString(), result.ToString());
        }
    }
}
