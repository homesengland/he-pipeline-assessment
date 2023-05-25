using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Activities.QuestionScreen.Helpers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.QuestionScreen.Helpers
{
    public class DataTableQuestionHelperTests
    {

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsEmptyString_GivenWorkflowFindByNameAsyncReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync((WorkflowBlueprint?)null);

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId,tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsEmptyString_WorkflowActivitesReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            DataTableQuestionHelper sut)
        {

            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsEmptyString_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            DataTableQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsEmptyString_GivenQuestionRecordAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers = null;

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsEmptyString_GivenQuestionRecordFirstAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers[0] = null;

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsEmptyString_GivenQuestionRecordAnswerDoesNotDeserialise(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers[0].AnswerText = JsonSerializer.Serialize(question.Answers[0].AnswerText);

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsEmptyString_GivenDataTableAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, null);
            datable.Inputs[0] = input;
            question.Answers[0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsCorrectString_GivenDataTableAnswerIsPopulated(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            string answerText,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, answerText);
            datable.Inputs[0] = input;
            question.Answers[0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(answerText, result);
        }



        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsNull_GivenWorkflowFindByNameAsyncReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync((WorkflowBlueprint?)null);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }


        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsNull_GivenWorkflowActivitesReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            DataTableQuestionHelper sut)
        {

            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsNull_GivenGetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            DataTableQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsNull_GivenQuestionRecordAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers = null;

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimal_ReturnsNull_GivenQuestionRecordAnswerDoesNotDeserialise(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers[0].AnswerText = JsonSerializer.Serialize(question.Answers[0].AnswerText);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Null( result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsEmptyString_GivenDataTableAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, null);
            datable.Inputs[0] = input;
            question.Answers[0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Null( result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsNull_GivenDataTableInputIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, null);
            datable.InputType = DataTableInputTypeConstants.DecimalDataTableInput;
            datable.Inputs[0] = input;
            question.Answers[0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsCorrectValue_GivenDataTableInputIsNotNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, "123.45");
            datable.InputType = DataTableInputTypeConstants.DecimalDataTableInput;
            datable.Inputs[0] = input;
            question.Answers[0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal((decimal)123.45,result);
        }
    }
}
