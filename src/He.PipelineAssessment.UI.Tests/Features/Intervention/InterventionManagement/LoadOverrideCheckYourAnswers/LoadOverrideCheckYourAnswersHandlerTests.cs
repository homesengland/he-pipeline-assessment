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
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitOverride;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers
{
    public class LoadOverrideCheckYourAnswersRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenEmptryResponseFromRepo(
                     [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                     LoadOverrideCheckYourAnswersRequest request,
                     AssessmentIntervention intervention,
                     LoadOverrideCheckYourAnswersRequestHandler sut
                 )
        {
            //Arrange
            intervention = null;

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenRepoThrowsException(
                     [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                     LoadOverrideCheckYourAnswersRequest request,
                     AssessmentIntervention intervention,
                     Exception exception,
                     LoadOverrideCheckYourAnswersRequestHandler sut
                 )
        {
            //Arrange
            intervention = null;

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).Throws(exception);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenMapperThrowsError(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
          AssessmentIntervention intervention,
          LoadOverrideCheckYourAnswersRequest request,
          Exception exception,
          LoadOverrideCheckYourAnswersRequestHandler sut
    )
        {
            //Arrange
            var emptyDto = new AssessmentInterventionDto();

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Throws(exception);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Null(result);
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
          LoadOverrideCheckYourAnswersRequest request,
          LoadOverrideCheckYourAnswersRequestHandler sut
    )
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitOverrideCommand = JsonConvert.DeserializeObject<SubmitOverrideCommand>(serializedCommand);
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns(command);
            adminRepository.Setup(x => x.GetAssessmentToolWorkflows()).ReturnsAsync(workflows);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(submitOverrideCommand.ToString(), result.ToString());
        }
    }
}
