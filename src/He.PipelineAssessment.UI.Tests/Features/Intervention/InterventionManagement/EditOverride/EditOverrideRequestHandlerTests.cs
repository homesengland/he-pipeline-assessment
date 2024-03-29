﻿using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Override.EditOverride;
using He.PipelineAssessment.UI.Features.Intervention;

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
          EditOverrideRequestHandler sut
      )
        {
            //Arrange
            var emptyListOfWorkflows = new List<AssessmentToolWorkflow>();

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns(command);
            adminRepository
                .Setup(x => x.GetAssessmentToolWorkflowsForOverride(intervention.AssessmentToolWorkflowInstance
                    .AssessmentToolWorkflow.AssessmentTool.Order)).ReturnsAsync(emptyListOfWorkflows);

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"No suitable assessment tool workflows found for override", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsError_GivenMapperReturnsNull(
              [Frozen] Mock<IAssessmentRepository> assessmentRepository,
              [Frozen] Mock<IAssessmentInterventionMapper> mapper,
              AssessmentIntervention intervention,
              EditOverrideRequest request,
              ArgumentException exception,
              EditOverrideRequestHandler sut
)
        {
            //Arrange
            var emptyDto = new AssessmentInterventionDto();

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Throws(exception);

            //Act
            ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotHandleError_GivenMapperThrowsError(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
          AssessmentIntervention intervention,
          EditOverrideRequest request,
          ArgumentException exception,
          EditOverrideRequestHandler sut
)
        {
            //Arrange
            var emptyDto = new AssessmentInterventionDto();

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Throws(exception);

            //Act
            ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
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
            adminRepository
                .Setup(x => x.GetAssessmentToolWorkflowsForOverride(intervention.AssessmentToolWorkflowInstance
                    .AssessmentToolWorkflow.AssessmentTool.Order)).ReturnsAsync(workflows);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(dto.ToString(), result.ToString());
        }
    }
}
