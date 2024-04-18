using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using He.PipelineAssessment.Models;
using Newtonsoft.Json;
using He.PipelineAssessment.UI.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using He.PipelineAssessment.UI.Features.Override.LoadOverrideCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Override.SubmitOverride;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers
{
    public class LoadOverrideCheckYourAnswersRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_IgnoresThrowsException_GivenInterventionServiceThrowsException(
                     [Frozen] Mock<IInterventionService> interventionService,
                     LoadOverrideCheckYourAnswersRequest request,
                     Exception exception,
                     LoadOverrideCheckYourAnswersRequestHandler sut
                 )
        {
            //Arrange
            interventionService.Setup(x => x.LoadInterventionCheckYourAnswersRequest(request)).ThrowsAsync(exception);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_IgnoresThrowsException_GivenSerializationThrowsException(
             [Frozen] Mock<IInterventionService> interventionService,
             LoadOverrideCheckYourAnswersRequest request,
             AssessmentInterventionCommand? command,
             LoadOverrideCheckYourAnswersRequestHandler sut
         )
        {
            //Arrange
            command = null;
            string serializedCommand = JsonConvert.SerializeObject(command);
            interventionService.Setup(x => x.LoadInterventionCheckYourAnswersRequest(request)).ReturnsAsync(command!);

            //Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to deserialise SubmitOverrideCommand: {serializedCommand} from serialized AssessmentInterventionCommand", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsExpectedValue_GivenNoExpectionsThrown(
            [Frozen] Mock<IInterventionService> interventionService,
             LoadOverrideCheckYourAnswersRequest request,
             AssessmentInterventionCommand command,
             LoadOverrideCheckYourAnswersRequestHandler sut
 )
        {
            //Arrange          
            string serializedCommand = JsonConvert.SerializeObject(command);
            var expectedResult = JsonConvert.DeserializeObject<SubmitOverrideCommand>(serializedCommand);
            interventionService.Setup(x => x.LoadInterventionCheckYourAnswersRequest(request)).ReturnsAsync(command);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SubmitOverrideCommand>(result);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }
    }
}
