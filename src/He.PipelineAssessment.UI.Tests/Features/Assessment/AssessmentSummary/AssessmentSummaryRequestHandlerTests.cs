using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Assessment.AssessmentSummary
{
    public class AssessmentSummaryRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenRepoThrowsError(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            AssessmentSummaryRequest request,
            Exception exception,
            AssessmentSummaryRequestHandler sut
        )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessment(It.IsAny<int>())).Throws(exception);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsAssessmentSummaryResponseWithNoStages_GivenNoStagesExistYet(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IStoredProcedureRepository> storeProcRepository,
            Models.Assessment assessment,
            AssessmentSummaryRequest request,
            AssessmentSummaryRequestHandler sut
        )
        {

            //Arrange
            var emptyList = new List<AssessmentStageViewModel>();
            assessmentRepository.Setup(x => x.GetAssessment(It.IsAny<int>())).ReturnsAsync(assessment);
            storeProcRepository.Setup(x => x.GetAssessmentStages(It.IsAny<int>())).ReturnsAsync(emptyList);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(request.CorrelationId, result!.CorrelationId);
            Assert.Equal(request.AssessmentId, result.AssessmentId);
            Assert.Equal(assessment.SiteName, result.SiteName);
            Assert.Equal(assessment.Counterparty, result.CounterParty);
            Assert.Equal(assessment.Reference, result.Reference);
            Assert.Empty(result.Stages);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsAssessmentSummaryResponseWithStages_GivenStagesExist(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
             [Frozen] Mock<IStoredProcedureRepository> storeProcRepository,
            Models.Assessment assessment,
            List<AssessmentStageViewModel> stages,
            AssessmentSummaryRequest request,
            AssessmentSummaryRequestHandler sut
        )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessment(It.IsAny<int>())).ReturnsAsync(assessment);
            storeProcRepository.Setup(x => x.GetAssessmentStages(It.IsAny<int>())).ReturnsAsync(stages);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(request.CorrelationId, result!.CorrelationId);
            Assert.Equal(request.AssessmentId, result.AssessmentId);
            Assert.Equal(assessment.SiteName, result.SiteName);
            Assert.Equal(assessment.Counterparty, result.CounterParty);
            Assert.Equal(assessment.Reference, result.Reference);
            Assert.NotEmpty(result.Stages);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsAssessmentStageWithDataFromStartableTools_GivenStageIsStartable(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IStoredProcedureRepository> storeProcRepository,
            Models.Assessment assessment,
            AssessmentStageViewModel stage,
            AssessmentSummaryRequest request,
            string workflowDefinitionId,
            List<StartableToolViewModel> startableToolViewModels,
            AssessmentSummaryRequestHandler sut)
        {

            //Arrange
            stage.WorkflowDefinitionId = null;
            stage.IsFirstWorkflow = null;
            stage.AssessmentToolWorkflowInstanceId = null;
            var stages = new List<AssessmentStageViewModel>()
            {
                stage
            };
            var startableToolViewModel = new StartableToolViewModel()
            {
                AssessmentToolId = stage.AssessmentToolId!.Value,
                WorkflowDefinitionId = workflowDefinitionId,
                IsFirstWorkflow = true
            };
            startableToolViewModels.Add(startableToolViewModel);
            assessmentRepository.Setup(x => x.GetAssessment(request.AssessmentId)).ReturnsAsync(assessment);
            storeProcRepository.Setup(x => x.GetAssessmentStages(request.AssessmentId)).ReturnsAsync(stages);
            storeProcRepository.Setup(x => x.GetStartableTools(request.AssessmentId)).ReturnsAsync(startableToolViewModels);


            //Act
            var result = await sut.Handle(request, CancellationToken.None);


            //Assert
            Assert.NotNull(result);
            Assert.Equal(startableToolViewModel.WorkflowDefinitionId, result!.Stages.First().WorkflowDefinitionId);
            Assert.Equal(startableToolViewModel.IsFirstWorkflow, result.Stages.First().IsFirstWorkflow);
            Assert.Null(result.Stages.First().AssessmentToolWorkflowInstanceId);


        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsAssessmentStageWithOutDataFromStartableTools_GivenStageCanNotbeStarted
            (
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IStoredProcedureRepository> storeProcRepository,
            Models.Assessment assessment,
            AssessmentStageViewModel stage,
            AssessmentSummaryRequest request,
            List<StartableToolViewModel> startableToolViewModels,
            AssessmentSummaryRequestHandler sut)
        {

            //Arrange
            stage.WorkflowDefinitionId = null;
            stage.IsFirstWorkflow = null;
            stage.AssessmentToolWorkflowInstanceId = null;
            var stages = new List<AssessmentStageViewModel>()
            {
                stage
            };
            assessmentRepository.Setup(x => x.GetAssessment(request.AssessmentId)).ReturnsAsync(assessment);
            storeProcRepository.Setup(x => x.GetAssessmentStages(request.AssessmentId)).ReturnsAsync(stages);
            storeProcRepository.Setup(x => x.GetStartableTools(request.AssessmentId)).ReturnsAsync(startableToolViewModels);


            //Act
            var result = await sut.Handle(request, CancellationToken.None);


            //Assert
            Assert.NotNull(result);
            Assert.Equal(stage.WorkflowDefinitionId, result!.Stages.First().WorkflowDefinitionId);
            Assert.Equal(stage.IsFirstWorkflow, result.Stages.First().IsFirstWorkflow);
            Assert.Null(result.Stages.First().AssessmentToolWorkflowInstanceId);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsAssessmentStageWithOutDataFromStartableTools_GivenStageHasBeenStarted(
           [Frozen] Mock<IAssessmentRepository> assessmentRepository,
           [Frozen] Mock<IStoredProcedureRepository> storeProcRepository,
            Models.Assessment assessment,
            AssessmentStageViewModel stage,
            AssessmentSummaryRequest request,
            string workflowDefinitionId,
            List<StartableToolViewModel> startableToolViewModels,
            AssessmentSummaryRequestHandler sut)
        {
            //Arrange
            stage.WorkflowDefinitionId = null;
            stage.IsFirstWorkflow = null;
            var randomInt = 1111111;
            stage.AssessmentToolWorkflowInstanceId = randomInt;
            var stages = new List<AssessmentStageViewModel>()
            {
                stage
            };
            var startableToolViewModel = new StartableToolViewModel()
            {
                AssessmentToolId = stage.AssessmentToolId!.Value,
                WorkflowDefinitionId = workflowDefinitionId,
                IsFirstWorkflow = true
            };
            startableToolViewModels.Add(startableToolViewModel);
            assessmentRepository.Setup(x => x.GetAssessment(request.AssessmentId)).ReturnsAsync(assessment);
            storeProcRepository.Setup(x => x.GetAssessmentStages(request.AssessmentId)).ReturnsAsync(stages);
            storeProcRepository.Setup(x => x.GetStartableTools(request.AssessmentId)).ReturnsAsync(startableToolViewModels);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(stage.WorkflowDefinitionId, result!.Stages.First().WorkflowDefinitionId);
            Assert.Equal(false, result.Stages.First().IsFirstWorkflow);
            Assert.Equal(stage.AssessmentToolWorkflowInstanceId, result.Stages.First().AssessmentToolWorkflowInstanceId);
        }

    }
}
