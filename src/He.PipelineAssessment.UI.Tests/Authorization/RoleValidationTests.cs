using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Authorization;

public class RoleValidationTests
{
    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsTrue_IfEconomistWorkflowAndUserIsEconomist(
        [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
        [Frozen] Mock<IUserProvider> userProvider,
        AssessmentToolWorkflow assessmentTool,
        string workflowDefinitionId,
        int assessmentId,
        RoleValidation sut
     )
    {
        //Arrange
        assessmentTool.IsEconomistWorkflow = true;
        adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId)).ReturnsAsync(assessmentTool);
        userProvider.Setup(x => x.IsEconomist()).Returns(true);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.True(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsFalse_IfEconomistWorkflowAndUserIsNotEconomist(
        [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
        [Frozen] Mock<IUserProvider> userProvider,
        AssessmentToolWorkflow assessmentTool,
        string workflowDefinitionId,
        int assessmentId,
        RoleValidation sut)
    {
        //Arrange
        assessmentTool.IsEconomistWorkflow = true;
        adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId)).ReturnsAsync(assessmentTool);
        userProvider.Setup(x => x.IsEconomist()).Returns(false);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.False(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsTrue_IfWorkflowIsEarlyStage(
    [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
    [Frozen] Mock<IUserProvider> userProvider,
    AssessmentToolWorkflow assessmentTool,
    string workflowDefinitionId,
    int assessmentId,
    RoleValidation sut)
    {
        //Arrange
        assessmentTool.IsEconomistWorkflow = false;
        assessmentTool.IsEarlyStage = true;
        adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId)).ReturnsAsync(assessmentTool);
        userProvider.Setup(x => x.IsProjectManager()).Returns(true);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.True(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsTrue_IfSensitiveRecordAndUserHasNotGotRoleSensitiveRecordViewerButIsProjectManager(
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        [Frozen] Mock<IUserProvider> userProvider,
        Assessment assessment,
        string workflowDefinitionId,
        int assessmentId,
        string projectManager,
        RoleValidation sut)
    {
        //Arrange
        assessment.SensitiveStatus = "Sensitive - NDA in place";
        assessment.BusinessArea = "Development";
        assessment.ProjectManager = projectManager;
        assessmentRepository.Setup(x => x.GetAssessment(assessmentId)).ReturnsAsync(assessment);
        userProvider.Setup(x => x.UserName()).Returns(projectManager);
        userProvider.Setup(x => x.IsProjectManager()).Returns(true);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.True(result);
    }

    #region IsUserWhitelistedForSensitiveRecord Tests

    [Theory]
    [AutoMoqData]
    public async Task IsUserWhitelistedForSensitiveRecord_ReturnsTrue_WhenUserEmailIsInWhitelist(
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        [Frozen] Mock<IUserProvider> userProvider,
        int assessmentId,
        RoleValidation sut)
    {
        //Arrange
        var userEmail = "test.user@homesengland.gov.uk";
        var whitelist = new List<SensitiveRecordWhitelist>
            {
                new SensitiveRecordWhitelist { Id = 1, AssessmentId = assessmentId, Email = userEmail }
            };

        userProvider.Setup(x => x.Email()).Returns(userEmail);
        assessmentRepository.Setup(x => x.IsUserWhitelistedForSensitiveRecord(assessmentId, userEmail)).ReturnsAsync(true);

        //Act
        var result = await sut.IsUserWhitelistedForSensitiveRecord(assessmentId);

        //Assert
        Assert.True(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task IsUserWhitelistedForSensitiveRecord_ReturnsTrue_WhenUserEmailIsInWhitelist_CaseInsensitive(
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        [Frozen] Mock<IUserProvider> userProvider,
        int assessmentId,
        RoleValidation sut)
    {
        //Arrange
        var userEmail = "Test.User@HomesEngland.gov.uk";
        var whitelist = new List<SensitiveRecordWhitelist>
            {
                new SensitiveRecordWhitelist { Id = 1, AssessmentId = assessmentId, Email = "test.user@homesengland.gov.uk" }
            };

        userProvider.Setup(x => x.Email()).Returns(userEmail);
        assessmentRepository.Setup(x => x.IsUserWhitelistedForSensitiveRecord(assessmentId, userEmail)).ReturnsAsync(true);

        //Act
        var result = await sut.IsUserWhitelistedForSensitiveRecord(assessmentId);

        //Assert
        Assert.True(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task IsUserWhitelistedForSensitiveRecord_ReturnsFalse_WhenUserEmailIsNotInWhitelist(
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        [Frozen] Mock<IUserProvider> userProvider,
        int assessmentId,
        RoleValidation sut)
    {
        //Arrange
        var userEmail = "test.user@homesengland.gov.uk";
        var whitelist = new List<SensitiveRecordWhitelist>
            {
                new SensitiveRecordWhitelist { Id = 1, AssessmentId = assessmentId, Email = "other.user@homesengland.gov.uk" }
            };

        userProvider.Setup(x => x.Email()).Returns(userEmail);
        assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(assessmentId)).ReturnsAsync(whitelist);

        //Act
        var result = await sut.IsUserWhitelistedForSensitiveRecord(assessmentId);

        //Assert
        Assert.False(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task IsUserWhitelistedForSensitiveRecord_ReturnsFalse_WhenUserEmailIsNull(
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        [Frozen] Mock<IUserProvider> userProvider,
        int assessmentId,
        RoleValidation sut)
    {
        //Arrange
        userProvider.Setup(x => x.Email()).Returns((string?)null);

        //Act
        var result = await sut.IsUserWhitelistedForSensitiveRecord(assessmentId);

        //Assert
        Assert.False(result);
        assessmentRepository.Verify(x => x.GetSensitiveRecordWhitelist(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task IsUserWhitelistedForSensitiveRecord_ReturnsFalse_WhenUserEmailIsEmpty(
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        [Frozen] Mock<IUserProvider> userProvider,
        int assessmentId,
        RoleValidation sut)
    {
        //Arrange
        userProvider.Setup(x => x.Email()).Returns(string.Empty);

        //Act
        var result = await sut.IsUserWhitelistedForSensitiveRecord(assessmentId);

        //Assert
        Assert.False(result);
        assessmentRepository.Verify(x => x.GetSensitiveRecordWhitelist(It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task IsUserWhitelistedForSensitiveRecord_ReturnsFalse_WhenWhitelistIsEmpty(
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        [Frozen] Mock<IUserProvider> userProvider,
        int assessmentId,
        RoleValidation sut)
    {
        //Arrange
        var userEmail = "test.user@homesengland.gov.uk";
        var whitelist = new List<SensitiveRecordWhitelist>();

        userProvider.Setup(x => x.Email()).Returns(userEmail);
        assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(assessmentId)).ReturnsAsync(whitelist);

        //Act
        var result = await sut.IsUserWhitelistedForSensitiveRecord(assessmentId);

        //Assert
        Assert.False(result);
    }

    #endregion
}

