using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Override.EditOverride;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.EditOverride
{
    public class EditOverrideRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task HandleIgnoresThrownError_GivenInterventionServiceThrowsErrorWhenHandlingEditInterventionRequest(
                     [Frozen] Mock<IInterventionService> interventionService,
                     EditOverrideRequest request,
                     Exception e,
                     EditOverrideRequestHandler sut
                 )
        {
            //Arrange
            interventionService.Setup(x => x.EditInterventionRequest(request)).ThrowsAsync(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task HandleIgnoresThrownError_GivenInterventionServiceThrowsErrorWhenGettingAssessmentToolWorkflows(
     [Frozen] Mock<IInterventionService> interventionService,
     EditOverrideRequest request,
     AssessmentInterventionDto dto,
     Exception e,
     EditOverrideRequestHandler sut
 )
        {
            //Arrange
            interventionService.Setup(x => x.EditInterventionRequest(request)).ReturnsAsync(dto);
            interventionService.Setup(x => x.GetAssessmentToolWorkflowsForOverride(dto.AssessmentInterventionCommand.WorkflowInstanceId))
                .ThrowsAsync(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task HandleIgnoresThrownError_GivenMapperThrowsException(
             [Frozen] Mock<IInterventionService> interventionService,
             [Frozen] Mock <IAssessmentInterventionMapper> mapper,
             List<AssessmentToolWorkflow> workflows,
             EditOverrideRequest request,
             AssessmentInterventionDto dto,
             Exception e,
             EditOverrideRequestHandler sut
         )
        {
            //Arrange
            interventionService.Setup(x => x.EditInterventionRequest(request)).ReturnsAsync(dto);
            interventionService.Setup(x => x.GetAssessmentToolWorkflowsForOverride(dto.AssessmentInterventionCommand.WorkflowInstanceId))
                .ReturnsAsync(workflows);
            mapper.Setup(x => x.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(workflows, dto.AssessmentInterventionCommand.SelectedWorkflowDefinitions))
                .Throws(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task HandleReturnsCorrectDto_GivenNoExceptionsThrownDuringRequest(
                 [Frozen] Mock<IInterventionService> interventionService,
                 [Frozen] Mock<IAssessmentInterventionMapper> mapper,
                 List<AssessmentToolWorkflow> workflows,
                 List<TargetWorkflowDefinition> definitions,
                 EditOverrideRequest request,
                 AssessmentInterventionDto dto,
                 EditOverrideRequestHandler sut
 )
        {
            //Arrange
            dto.TargetWorkflowDefinitions = definitions;
            interventionService.Setup(x => x.EditInterventionRequest(request)).ReturnsAsync(dto);
            interventionService.Setup(x => x.GetAssessmentToolWorkflowsForOverride(dto.AssessmentInterventionCommand.WorkflowInstanceId))
                .ReturnsAsync(workflows);
            mapper.Setup(x => x.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(workflows, dto.AssessmentInterventionCommand.SelectedWorkflowDefinitions))
                .Returns(definitions);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AssessmentInterventionDto>(result);
            Assert.Equal(JsonConvert.SerializeObject(dto), JsonConvert.SerializeObject(result));
        }
    }
}
