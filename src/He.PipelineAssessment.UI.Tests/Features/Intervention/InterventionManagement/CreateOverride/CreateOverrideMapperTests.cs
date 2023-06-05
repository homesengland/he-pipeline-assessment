using AutoFixture.Xunit2;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride;
using Moq;
using He.PipelineAssessment.UI.Common.Utility;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void CreateOverrideCommandToAssessmentIntervention_ReturnsCorrectAssessmentInterventionObject(
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            CreateOverrideCommand createOverrideCommand,
            CreateOverrideMapper sut
        )
        {
            //Arrange
            var date = new DateTime(2023, 6, 5);
            dateTimeProvider.Setup(x => x.UtcNow()).Returns(date);

            //Act
            var result = sut.CreateOverrideCommandToAssessmentIntervention(createOverrideCommand);

            //Assert
            Assert.Equal(date, result.CreatedDateTime);
            Assert.Equal(createOverrideCommand.Administrator, result.Administrator);
            Assert.Equal(createOverrideCommand.AdministratorRationale, result.AdministratorRationale);
            Assert.Equal(createOverrideCommand.AdministratorEmail, result.AdministratorEmail);
            Assert.Equal(createOverrideCommand.AssessmentToolWorkflowInstanceId, result.AssessmentToolWorkflowInstanceId);
            Assert.Equal(createOverrideCommand.AssessorRationale, result.AssessorRationale);
            Assert.Equal(createOverrideCommand.RequestedBy, result.CreatedBy);
            Assert.Equal(date, result.DateSubmitted);
            Assert.Equal(createOverrideCommand.DecisionType, result.DecisionType);
            Assert.Equal(createOverrideCommand.RequestedBy, result.LastModifiedBy);
            Assert.Equal(createOverrideCommand.RequestedByEmail, result.RequestedByEmail);
            Assert.Equal(createOverrideCommand.SignOffDocument, result.SignOffDocument);
            Assert.Equal(InterventionStatus.NotSubmitted, result.Status);
            Assert.Equal(createOverrideCommand.AssessmentResult, result.AssessmentResult);
        }

        [Theory]
        [AutoMoqData]
        public void CreateOverrideCommandToAssessmentIntervention_ReturnsCorrectAssessmentInterventionObjectWithDefaultValues(
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            CreateOverrideCommand createOverrideCommand,
            CreateOverrideMapper sut
        )
        {
            //Arrange
            var date = new DateTime(2024, 6, 5);
            dateTimeProvider.Setup(x => x.UtcNow()).Returns(date);
            createOverrideCommand.RequestedBy = null;
            createOverrideCommand.RequestedByEmail = null;

            //Act
            var result = sut.CreateOverrideCommandToAssessmentIntervention(createOverrideCommand);

            //Assert
            Assert.Equal(string.Empty, result.CreatedBy);
            Assert.Equal(string.Empty, result.RequestedBy);
            Assert.Equal(string.Empty, result.RequestedByEmail);
        }
    }
}
