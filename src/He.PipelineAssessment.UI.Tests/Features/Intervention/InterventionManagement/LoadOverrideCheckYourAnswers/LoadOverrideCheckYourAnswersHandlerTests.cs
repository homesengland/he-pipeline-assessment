﻿using AutoFixture.Xunit2;
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

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers
{
    public class LoadOverrideCheckYourAnswersRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsException_GivenNullResponseFromRepo(
                     [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                     LoadOverrideCheckYourAnswersRequest request,
                     AssessmentIntervention? intervention,
                     LoadOverrideCheckYourAnswersRequestHandler sut
                 )
        {
            //Arrange
            intervention = null;

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to load override's check details page. InterventionId: {request.InterventionId}. ", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsApplicationException_GivenRepoThrowsException(
                     [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                     LoadOverrideCheckYourAnswersRequest request,
                     DbUpdateException exception,
                     LoadOverrideCheckYourAnswersRequestHandler sut
                 )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ThrowsAsync(exception);

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to load override's check details page. InterventionId: {request.InterventionId}. ", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsApplicatoinException_GivenMapperThrowsError(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
          AssessmentIntervention intervention,
          LoadOverrideCheckYourAnswersRequest request,
          ArgumentException exception,
          LoadOverrideCheckYourAnswersRequestHandler sut
    )
        {
            //Arrange
            var emptyDto = new AssessmentInterventionDto();

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Throws(exception);

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to load override's check details page. InterventionId: {request.InterventionId}. ", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsApplicationException_GivenMapperReturnsNull(
              [Frozen] Mock<IAssessmentRepository> assessmentRepository,
              [Frozen] Mock<IAssessmentInterventionMapper> mapper,
              AssessmentInterventionCommand command,
              AssessmentIntervention intervention,
              LoadOverrideCheckYourAnswersRequest request,
              LoadOverrideCheckYourAnswersRequestHandler sut
)
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitOverrideCommand = JsonConvert.DeserializeObject<SubmitOverrideCommand>(serializedCommand);
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns((AssessmentInterventionCommand)null!);

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to load override's check details page. InterventionId: {request.InterventionId}. ", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsValidResult_GivenNoErrors(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminRepository,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
          AssessmentInterventionCommand command,
          List<AssessmentToolWorkflow> workflows,
          AssessmentIntervention intervention,
          LoadOverrideCheckYourAnswersRequest request,
          LoadOverrideCheckYourAnswersRequestHandler sut
    )
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitOverrideCommand = JsonConvert.DeserializeObject<SubmitOverrideCommand>(serializedCommand);
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns(command);
            adminRepository.Setup(x => x.GetAssessmentToolWorkflowsForOverride(intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order)).ReturnsAsync(workflows);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(submitOverrideCommand!.ToString(), result!.ToString());
        }
    }
}
