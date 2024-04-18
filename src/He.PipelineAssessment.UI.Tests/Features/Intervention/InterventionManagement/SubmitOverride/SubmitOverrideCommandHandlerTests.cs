using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.Providers;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Override.SubmitOverride;
using He.PipelineAssessment.UI.Services;
using MediatR;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.SubmitOverride
{
    public class SubmitOverrideCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsException_GivenADependencyThrowsException(
            [Frozen] Mock<IInterventionService> service,
            SubmitOverrideCommand command,
            Exception exception,
            SubmitOverrideCommandHandler sut
        )
        {
            //Arrange
            service.Setup(x => x.SubmitIntervention(command)).Throws(exception);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }

        [Theory]
        [InlineAutoMoqData(InterventionStatus.Rejected)]
        [InlineAutoMoqData(InterventionStatus.Pending)]
        public async Task Handle_ReturnsWithoutError_GivenNoExceptionsThrown(
            string status,
            [Frozen] Mock<IInterventionService> service,
        SubmitOverrideCommand command,
            SubmitOverrideCommandHandler sut
        )
        {
            //Arrange
            command.Status = status;
            service.Setup(x => x.SubmitIntervention(command)).Returns(Task.CompletedTask);

            try
            {
                //Act
                //Assert
                await sut.Handle(command, CancellationToken.None);
                Assert.True(true, "No Exeption thrown");
            }
 
            catch(Exception e)
            {
                Assert.False(true, e.Message);
            }

            
        }

    }
}
