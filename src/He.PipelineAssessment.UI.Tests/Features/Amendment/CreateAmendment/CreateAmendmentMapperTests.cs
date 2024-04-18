﻿using AutoFixture.Xunit2;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Amendment.CreateAmendment;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Amendment.CreateAmendment
{
    public class CreateAmendmentMapperTests
    {

        [Theory]
        [AutoMoqData]
        public void CreateAmendmentCommandToAssessmentIntervention_MapsCorrectly(
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            CreateAmendmentCommand command,
            CreateAmendmentMapper sut
            )
        {

            //Arrange
            var date = DateTime.UtcNow;
            dateTimeProvider.Setup(x => x.UtcNow()).Returns(date);

            //Act
            var result = sut.CreateAmendmentCommandToAssessmentIntervention(command);

            //Assert
            Assert.Equal(date, result.CreatedDateTime);
            Assert.Equal(date, result.DateSubmitted);
            Assert.Equal(date, result.LastModifiedDateTime);
            Assert.Equal(command.Administrator, result.Administrator);
            Assert.Equal(command.AdministratorRationale, result.AdministratorRationale);
            Assert.Equal(command.AdministratorEmail, result.AdministratorEmail);
            Assert.Equal(command.AssessmentToolWorkflowInstanceId, result.AssessmentToolWorkflowInstanceId);
            #pragma warning disable 0612, 0618
            Assert.Equal(command.TargetWorkflowId, result.TargetAssessmentToolWorkflowId);
            #pragma warning restore 0612, 0618
            Assert.Equal(command.AssessorRationale, result.AssessorRationale);
            Assert.Equal(command.RequestedBy, result.CreatedBy);
            Assert.Equal(command.RequestedBy, result.CreatedBy);
            Assert.Equal(command.DecisionType, result.DecisionType);
            Assert.Equal(command.RequestedBy, result.LastModifiedBy);
            Assert.Equal(command.RequestedByEmail, result.RequestedByEmail);
            Assert.Equal(command.SignOffDocument, result.SignOffDocument);
            Assert.Equal(InterventionStatus.Draft, result.Status);
            Assert.Equal(command.AssessmentResult, result.AssessmentResult);
        }
    }
}
