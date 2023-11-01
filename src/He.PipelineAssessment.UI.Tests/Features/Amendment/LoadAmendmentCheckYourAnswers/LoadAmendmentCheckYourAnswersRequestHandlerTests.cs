using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Amendment.LoadAmendmentCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Amendment.LoadAmendmentCheckYourAnswers
{
    public class LoadAmendmentCheckYourAnswersRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldIgnoreError_GivenInterventionServiceThrowsError(
            [Frozen]Mock<IInterventionService> interventionService,
                   LoadAmendmentCheckYourAnswersRequest request,
                   LoadAmendmentCheckYourAnswersRequestHandler sut,
                    Exception e)
        {
            //Arrange
            interventionService.Setup(x => x.LoadInterventionCheckYourAnswerAssessorRequest(request)).ThrowsAsync(e);
            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(ex.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnAmmendmentCommand_GivenNoErrorsInInterventionService(
            [Frozen] Mock<IInterventionService> interventionService,
            AssessmentInterventionCommand command,
            AssessmentIntervention intervention,
            LoadAmendmentCheckYourAnswersRequest request,
            LoadAmendmentCheckYourAnswersRequestHandler sut)
        {
            //Arrange
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitAmendmentCommand = JsonConvert.DeserializeObject<SubmitAmendmentCommand>(serializedCommand);
            interventionService.Setup(x => x.LoadInterventionCheckYourAnswerAssessorRequest(request)).ReturnsAsync(command);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SubmitAmendmentCommand>(result);
            Assert.Equal(JsonConvert.SerializeObject(submitAmendmentCommand), JsonConvert.SerializeObject(result));
        }
    }
}
