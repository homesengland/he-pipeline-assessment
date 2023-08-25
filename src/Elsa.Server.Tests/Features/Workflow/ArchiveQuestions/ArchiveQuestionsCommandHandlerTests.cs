using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Workflow.ArchiveQuestions;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Elsa.Server.Features.Workflow.ExecuteWorkflow;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.ArchiveQuestions
{
    public class ArchiveQuestionsCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnArchiveQuestionsCommandResponse(
            ArchiveQuestionsCommand command,
            ArchiveQuestionsCommandHandler sut)
        {
            //Arrange

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_GivenCallToElsaCustomRepositoryFails(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            ArchiveQuestionsCommand command,
            Exception exception,
            ArchiveQuestionsCommandHandler sut)
        {
            //Arrange

            elsaCustomRepository.Setup(x => x.ArchiveQuestions(command.WorkflowInstanceIds, CancellationToken.None))
                .Throws(exception);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            var errorMessage = result.ErrorMessages.First();
            Assert.Equal(exception.Message, errorMessage);
        }
    }
}
