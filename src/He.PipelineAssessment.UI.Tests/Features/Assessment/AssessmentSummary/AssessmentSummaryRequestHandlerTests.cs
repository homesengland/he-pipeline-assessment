using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace He.PipelineAssessment.UI.Tests.Features.Assessment.AssessmentSummary
{
    public class AssessmentSummaryRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsError_GivenRepoThrowsError(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            AssessmentSummaryRequest request,
            Exception exception,
            AssessmentSummaryRequestHandler sut
        )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessment(It.IsAny<int>())).Throws(exception);

            //Act
            var result = await Assert.ThrowsAsync<ApplicationException>(()=>sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to get the assessment summary. AssessmentId: {request.AssessmentId}", result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsError_GivenAssessmentNotFound(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            AssessmentSummaryRequest request,
            AssessmentSummaryRequestHandler sut
        )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessment(request.AssessmentId)).ReturnsAsync((Models.Assessment?)null);

            //Act
            var result = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to get the assessment summary. AssessmentId: {request.AssessmentId}", result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsUnauthorizedException_GivenUserCannotViewSensitiveRecords(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            Models.Assessment assessment,
            AssessmentSummaryRequest request,
            AssessmentSummaryRequestHandler sut
        )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessment(request.AssessmentId)).ReturnsAsync(assessment);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessment)).Returns(false);

            //Act
            var result = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", result.Message);
        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsAssessmentSummaryResponseWithNoStages_GivenNoStagesExistYet(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IStoredProcedureRepository> storeProcRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            Models.Assessment assessment,
            AssessmentSummaryRequest request,
            List<AssessmentStageViewModel> historyStages,
            AssessmentSummaryRequestHandler sut
        )
        {

            //Arrange
            var emptyList = new List<AssessmentStageViewModel>();
            assessmentRepository.Setup(x => x.GetAssessment(It.IsAny<int>())).ReturnsAsync(assessment);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessment)).Returns(true);
            storeProcRepository.Setup(x => x.GetAssessmentStages(It.IsAny<int>())).ReturnsAsync(emptyList);
            storeProcRepository.Setup(x => x.GetAssessmentInterventionList(It.IsAny<int>())).ReturnsAsync(new List<AssessmentInterventionViewModel>());
            storeProcRepository.Setup(x => x.GetAssessmentHistory(request.AssessmentId)).ReturnsAsync(historyStages);

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
            [Frozen] Mock<IRoleValidation> roleValidation,
            Models.Assessment assessment,
            List<AssessmentStageViewModel> stages,
            List<AssessmentStageViewModel> historyStages,
            AssessmentSummaryRequest request,
            List<StartableToolViewModel> startableToolViewModels,
            AssessmentSummaryRequestHandler sut
        )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessment(It.IsAny<int>())).ReturnsAsync(assessment);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessment)).Returns(true);
            storeProcRepository.Setup(x => x.GetStartableTools(request.AssessmentId))
                .ReturnsAsync(startableToolViewModels);
            storeProcRepository.Setup(x => x.GetAssessmentStages(It.IsAny<int>())).ReturnsAsync(stages);
            storeProcRepository.Setup(x => x.GetAssessmentInterventionList(It.IsAny<int>()))
                .ReturnsAsync(new List<AssessmentInterventionViewModel>());
            storeProcRepository.Setup(x => x.GetAssessmentHistory(request.AssessmentId)).ReturnsAsync(historyStages);

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
            Assert.Equal(stages.First().FirstActivityId, result.Stages.First().FirstActivityId);
            Assert.Equal(stages.First().FirstActivityType, result.Stages.First().FirstActivityType);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_StageFirstActivitySetToCurrentActivity_GivenFirstActivityIsNullOrEmpty(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IStoredProcedureRepository> storeProcRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            Models.Assessment assessment,
            List<AssessmentStageViewModel> stages,
            List<AssessmentStageViewModel> historyStages,
            AssessmentSummaryRequest request,
            List<StartableToolViewModel> startableToolViewModels,
            AssessmentSummaryRequestHandler sut
        )
        {
            //Arrange
            stages.ForEach(x => x.FirstActivityId = String.Empty);
            stages.ForEach(x => x.FirstActivityType = String.Empty);
            assessmentRepository.Setup(x => x.GetAssessment(It.IsAny<int>())).ReturnsAsync(assessment);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessment)).Returns(true);
            storeProcRepository.Setup(x => x.GetStartableTools(request.AssessmentId)).ReturnsAsync(startableToolViewModels);
            storeProcRepository.Setup(x => x.GetAssessmentStages(It.IsAny<int>())).ReturnsAsync(stages);
            storeProcRepository.Setup(x => x.GetAssessmentInterventionList(It.IsAny<int>())).ReturnsAsync(new List<AssessmentInterventionViewModel>());
            storeProcRepository.Setup(x => x.GetAssessmentHistory(request.AssessmentId)).ReturnsAsync(historyStages);

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
            Assert.Equal(stages.First().CurrentActivityId, result.Stages.First().FirstActivityId);
            Assert.Equal(stages.First().CurrentActivityType, result.Stages.First().FirstActivityType);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsAssessmentStageWithDataFromStartableTools_GivenStageIsStartable(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IStoredProcedureRepository> storeProcRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            Models.Assessment assessment,
            AssessmentStageViewModel stage,
            List<AssessmentStageViewModel> historyStages,
            AssessmentSummaryRequest request,
            string workflowDefinitionId,
            List<StartableToolViewModel> startableToolViewModels,
            bool isVariation,
            AssessmentSummaryRequestHandler sut)
        {

            //Arrange
            stage.WorkflowDefinitionId = null;
            stage.IsFirstWorkflow = null;
            stage.AssessmentToolWorkflowInstanceId = null;
            stage.AssessmentToolId = 123;
            stage.IsVariation = isVariation;

            var stages = new List<AssessmentStageViewModel>()
            {
                stage
            };
            var startableToolViewModel = new StartableToolViewModel()
            {
                AssessmentToolId = stage.AssessmentToolId!.Value,
                WorkflowDefinitionId = workflowDefinitionId,
                IsFirstWorkflow = true,
                IsVariation = isVariation
            };
            startableToolViewModels.Add(startableToolViewModel);
            assessmentRepository.Setup(x => x.GetAssessment(request.AssessmentId)).ReturnsAsync(assessment);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessment)).Returns(true);
            storeProcRepository.Setup(x => x.GetAssessmentStages(request.AssessmentId)).ReturnsAsync(stages);
            storeProcRepository.Setup(x => x.GetStartableTools(request.AssessmentId)).ReturnsAsync(startableToolViewModels);
            storeProcRepository.Setup(x => x.GetAssessmentHistory(request.AssessmentId)).ReturnsAsync(historyStages);
            storeProcRepository.Setup(x => x.GetAssessmentInterventionList(It.IsAny<int>())).ReturnsAsync(new List<AssessmentInterventionViewModel>());


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
        public async Task Handle_ReturnsAssessmentStageWithJustName_GivenStageIsNotStartableAndNoInstance(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IStoredProcedureRepository> storeProcRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            Models.Assessment assessment,
            AssessmentStageViewModel stage,
            List<AssessmentStageViewModel> historyStages,
            AssessmentSummaryRequest request,
            bool isVariation,
            AssessmentSummaryRequestHandler sut)
        {

            //Arrange
            stage.WorkflowDefinitionId = null;
            stage.IsFirstWorkflow = null;
            stage.AssessmentToolWorkflowInstanceId = null;
            stage.AssessmentToolId = 123;
            stage.IsVariation = isVariation;

            var stages = new List<AssessmentStageViewModel>()
            {
                stage
            };
            
            assessmentRepository.Setup(x => x.GetAssessment(request.AssessmentId)).ReturnsAsync(assessment);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessment)).Returns(true);
            storeProcRepository.Setup(x => x.GetAssessmentStages(request.AssessmentId)).ReturnsAsync(stages);
            storeProcRepository.Setup(x => x.GetStartableTools(request.AssessmentId)).ReturnsAsync(new List<StartableToolViewModel>());
            storeProcRepository.Setup(x => x.GetAssessmentHistory(request.AssessmentId)).ReturnsAsync(historyStages);
            storeProcRepository.Setup(x => x.GetAssessmentInterventionList(It.IsAny<int>())).ReturnsAsync(new List<AssessmentInterventionViewModel>());


            //Act
            var result = await sut.Handle(request, CancellationToken.None);


            //Assert
            Assert.NotNull(result);
            Assert.Null(result!.Stages.First().WorkflowDefinitionId);
            Assert.Equal(stage.Name, result.Stages.First().Name);
            Assert.Equal(stage.Order, result.Stages.First().Order);
            Assert.Null(result.Stages.First().AssessmentToolWorkflowInstanceId);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsAssessmentSummaryResponseWithEmptyInterventionsList_GivenNoInterventionsExist(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IStoredProcedureRepository> storeProcRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            Models.Assessment assessment,
            List<AssessmentStageViewModel> stages,
            List<StartableToolViewModel> startableTools,
            List<AssessmentStageViewModel> historyStages,
            AssessmentSummaryRequest request,
            AssessmentSummaryRequestHandler sut
        )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessment(It.IsAny<int>())).ReturnsAsync(assessment);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessment)).Returns(true);
            storeProcRepository.Setup(x => x.GetAssessmentStages(It.IsAny<int>())).ReturnsAsync(stages);
            storeProcRepository.Setup(x => x.GetStartableTools(It.IsAny<int>())).ReturnsAsync(startableTools);
            storeProcRepository.Setup(x => x.GetAssessmentInterventionList(It.IsAny<int>()))
                .ReturnsAsync(new List<AssessmentInterventionViewModel>());
            storeProcRepository.Setup(x => x.GetAssessmentHistory(request.AssessmentId)).ReturnsAsync(historyStages);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result!.Interventions);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsAssessmentSummaryResponseInterventionsList_GivenAnyInterventionsExist(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IStoredProcedureRepository> storeProcRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            Models.Assessment assessment,
            List<AssessmentStageViewModel> stages,
            List<StartableToolViewModel> startableTools,
            List<AssessmentInterventionViewModel> interventions,
            List<AssessmentStageViewModel> historyStages,
            AssessmentSummaryRequest request,
            AssessmentSummaryRequestHandler sut
)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessment(It.IsAny<int>())).ReturnsAsync(assessment);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessment)).Returns(true);
            storeProcRepository.Setup(x => x.GetAssessmentStages(It.IsAny<int>())).ReturnsAsync(stages);
            storeProcRepository.Setup(x => x.GetStartableTools(It.IsAny<int>())).ReturnsAsync(startableTools);
            storeProcRepository.Setup(x => x.GetAssessmentInterventionList(It.IsAny<int>())).ReturnsAsync(interventions);
            storeProcRepository.Setup(x => x.GetAssessmentHistory(request.AssessmentId)).ReturnsAsync(historyStages);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result!.Interventions);
            Assert.Equal(interventions.Count(), result.Interventions.Count());


        }

    }

}
