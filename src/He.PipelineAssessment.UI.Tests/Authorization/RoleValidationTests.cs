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
    public async Task RoleValidation_ReturnsTrue_IfBusinessAreaIsMPPAndUserHasRoleMPP(
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
        assessment.BusinessArea = "MPP";
        assessmentToolRepository.Setup(x => x.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId)).ReturnsAsync(assessmentTool);
        assessmentRepository.Setup(x => x.GetAssessment(assessmentId)).ReturnsAsync(assessment);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineEconomist)).Returns(false);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAssessorMPP)).Returns(true);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.True(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsFalse_IfBusinessAreaIsMPPAndUserHasNotGotRoleMPP(
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
        assessment.BusinessArea = "MPP";
        assessmentToolRepository.Setup(x => x.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId)).ReturnsAsync(assessmentTool);
        assessmentRepository.Setup(x => x.GetAssessment(assessmentId)).ReturnsAsync(assessment);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineEconomist)).Returns(false);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAssessorMPP)).Returns(false);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.False(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsTrue_IfBusinessAreaIsInvestmentAndUserHasRoleInvestment(
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
        assessment.BusinessArea = "Investment";
        assessmentToolRepository.Setup(x => x.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId)).ReturnsAsync(assessmentTool);
        assessmentRepository.Setup(x => x.GetAssessment(assessmentId)).ReturnsAsync(assessment);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineEconomist)).Returns(false);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAssessorInvestment)).Returns(true);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.True(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsFalse_IfBusinessAreaIsInvestmentAndUserHasNotGotRoleInvestment(
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
        assessment.BusinessArea = "Investment";
        assessmentToolRepository.Setup(x => x.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId)).ReturnsAsync(assessmentTool);
        assessmentRepository.Setup(x => x.GetAssessment(assessmentId)).ReturnsAsync(assessment);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineEconomist)).Returns(false);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAssessorInvestment)).Returns(false);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.False(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsTrue_IfBusinessAreaIsDevelopmentAndUserHasRoleDevelopment(
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
        assessment.BusinessArea = "Development";
        assessmentToolRepository.Setup(x => x.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId)).ReturnsAsync(assessmentTool);
        assessmentRepository.Setup(x => x.GetAssessment(assessmentId)).ReturnsAsync(assessment);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineEconomist)).Returns(false);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAssessorDevelopment)).Returns(true);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.True(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsFalse_IfBusinessAreaIsDevelopmnentAndUserHasNotGotRoleDevelopment(
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
        assessment.BusinessArea = "Development";
        assessmentToolRepository.Setup(x => x.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId)).ReturnsAsync(assessmentTool);
        assessmentRepository.Setup(x => x.GetAssessment(assessmentId)).ReturnsAsync(assessment);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineEconomist)).Returns(false);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAssessorDevelopment)).Returns(false);

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
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAssessorDevelopment)).Returns(true);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.False(result);
    }

    [Theory]
    [AutoMoqData]
    public async Task RoleValidation_ReturnsTrue_IfSensitiveRecordAndUserHasGotRoleSensitiveRecordViewer(
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
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.SensitiveRecordsViewer)).Returns(true);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAssessorDevelopment)).Returns(true);

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
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.SensitiveRecordsViewer)).Returns(false);
        userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAssessorDevelopment)).Returns(true);
        userProvider.Setup(x => x.GetUserName()).Returns(projectManager);

        //Act
        var result = await sut.ValidateRole(assessmentId, workflowDefinitionId);

        //Assert
        Assert.True(result);
    }
}

