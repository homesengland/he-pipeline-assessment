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
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineEconomist)).Returns(true);

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
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineEconomist)).Returns(false);

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
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineProjectManager)).Returns(true);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.True(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsFalse_IfWorkflowIsEarlyStage(
    [Frozen] Mock<IAssessmentRepository> assementRepository,
    [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
    [Frozen] Mock<IUserProvider> userProvider,
    Assessment assessment,
    AssessmentToolWorkflow assessmentTool,
    string workflowDefinitionId,
    int assessmentId,
    RoleValidation sut)
    {
        //Arrange
        assessmentTool.IsEconomistWorkflow = false;
        assessmentTool.IsEarlyStage = false;
        adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId)).ReturnsAsync(assessmentTool);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineProjectManager)).Returns(true);
        assementRepository.Setup(x => x.GetAssessment(assessmentId)).ReturnsAsync(assessment);
        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.False(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsFalse_IfBusinessAreaIsNotRecognised(
    [Frozen] Mock<IAdminAssessmentToolRepository> assessmentToolRepository,
    [Frozen] Mock<IAssessmentRepository> assessmentRepository,
    [Frozen] Mock<IUserProvider> userProvider,
    AssessmentToolWorkflow assessmentTool,
    Assessment assessment,
    string workflowDefinitionId,
    int assessmentId,
    RoleValidation sut)
    {
        //Arrange
        assessmentTool.IsEconomistWorkflow = false;
        assessmentTool.IsEarlyStage = false;
        assessment.BusinessArea = "";
        assessmentToolRepository.Setup(x => x.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId)).ReturnsAsync(assessmentTool);
        assessmentRepository.Setup(x => x.GetAssessment(assessmentId)).ReturnsAsync(assessment);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineEconomist)).Returns(false);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.False(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsFalse_IfAssessmentNotFound(
    [Frozen] Mock<IAdminAssessmentToolRepository> assessmentToolRepository,
    [Frozen] Mock<IAssessmentRepository> assessmentRepository,
    [Frozen] Mock<IUserProvider> userProvider,
    AssessmentToolWorkflow assessmentTool,
    Assessment assessment,
    string workflowDefinitionId,
    int assessmentId,
    RoleValidation sut)
    {
        //Arrange
        assessmentTool.IsEconomistWorkflow = false;
        assessmentTool.IsEarlyStage = false;
        assessment.BusinessArea = "";
        assessmentToolRepository.Setup(x => x.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId)).ReturnsAsync(assessmentTool);
        assessmentRepository.Setup(x => x.GetAssessment(assessmentId)).ReturnsAsync((Assessment?)null);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineEconomist)).Returns(false);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.False(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsFalse_IfSensitiveRecordAndUserHasNotGotRoleSensitiveRecordViewer(
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        [Frozen] Mock<IUserProvider> userProvider,
        Assessment assessment,
        string workflowDefinitionId,
        int assessmentId,
        RoleValidation sut)
    {
        //Arrange
        assessment.SensitiveStatus = "Sensitive - NDA in place";
        assessment.BusinessArea = "Development";
        assessmentRepository.Setup(x => x.GetAssessment(assessmentId)).ReturnsAsync(assessment);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.SensitiveRecordsViewer)).Returns(false);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineProjectManager)).Returns(true);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.False(result);
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
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.SensitiveRecordsViewer)).Returns(false);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineProjectManager)).Returns(true);
        userProvider.Setup(x => x.GetUserName()).Returns(projectManager);

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

        userProvider.Setup(x => x.GetUserEmail()).Returns(userEmail);
        assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(assessmentId)).ReturnsAsync(whitelist);

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

        userProvider.Setup(x => x.GetUserEmail()).Returns(userEmail);
        assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(assessmentId)).ReturnsAsync(whitelist);

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

        userProvider.Setup(x => x.GetUserEmail()).Returns(userEmail);
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
        userProvider.Setup(x => x.GetUserEmail()).Returns((string?)null);

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
        userProvider.Setup(x => x.GetUserEmail()).Returns(string.Empty);

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

        userProvider.Setup(x => x.GetUserEmail()).Returns(userEmail);
        assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(assessmentId)).ReturnsAsync(whitelist);

        //Act
        var result = await sut.IsUserWhitelistedForSensitiveRecord(assessmentId);

        //Assert
        Assert.False(result);
    }

    #endregion
}

