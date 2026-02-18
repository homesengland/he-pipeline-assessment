using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Models;
using HibernatingRhinos.Profiler.Appender.CosmosDB;

namespace He.PipelineAssessment.UI.Authorization;

public interface IRoleValidation
{
    Task<bool> ValidateRole(int assessmentId, string workflowDefinitionId);

    bool CanViewAssessment(Assessment assessment, bool includeAdmin = false);
    bool IsAdmin();
    bool IsProjectManagerForAssessment(string? assessmentProjectManager);
    Task<bool> IsUserWhitelistedForSensitiveRecord(int assessmentId);
}
public class RoleValidation : IRoleValidation
{
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
    private readonly IUserProvider _userProvider;

    public RoleValidation(IAssessmentRepository assessmentRepository, IAdminAssessmentToolRepository adminAssessmentToolRepository, IUserProvider userProvider)
    {
        _assessmentRepository = assessmentRepository;
        _adminAssessmentToolRepository = adminAssessmentToolRepository;
        _userProvider = userProvider;
    }
    public async Task<bool> ValidateRole(int assessmentId, string workflowDefinitionId)
    {
        var assessmentToolWorkflow = await _adminAssessmentToolRepository.GetAssessmentToolByWorkflowDefinitionId(workflowDefinitionId);

        if (assessmentToolWorkflow != null && assessmentToolWorkflow.IsEconomistWorkflow)
        {
                bool isEconomistRoleExist = (_userProvider.IsEconomist() || _userProvider.IsAdmin());
                return isEconomistRoleExist;
        }
        var assessment = await _assessmentRepository.GetAssessment(assessmentId);

        var canSeeRecord = CanViewAssessment(assessment);

        if (!canSeeRecord && assessment != null && assessment.IsSensitiveRecord())
        {
            canSeeRecord = await IsUserWhitelistedForSensitiveRecord(assessmentId);
        }

        return canSeeRecord;
    }

    public bool IsAdmin()
    {
        return _userProvider.IsAdmin();
    }

    public bool IsProjectManagerForAssessment(string? assessmentProjectManager)
    {

        if (assessmentProjectManager == _userProvider.UserName())
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if the current user's email is whitelisted for viewing a specific sensitive assessment
    /// </summary>
    /// <param name="assessmentId">The ID of the assessment to check</param>
    /// <returns>True if user's email is in the whitelist, otherwise false</returns>
    public async Task<bool> IsUserWhitelistedForSensitiveRecord(int assessmentId)
    {
        var userEmail = _userProvider.Email();

        if (string.IsNullOrWhiteSpace(userEmail))
        {
            return false;
        }

        bool isOnWhiteList = await _assessmentRepository.IsUserWhitelistedForSensitiveRecord(assessmentId, userEmail);
        return isOnWhiteList;
    
    }

    public bool CanViewAssessment(Assessment? assessment, bool includeAdmin = false)
    {
        if (assessment == null)
        {
            return true;
        }

        if (!assessment.IsSensitiveRecord())
        {
            return true;
        }

        var isProjectManager = assessment.ProjectManager == _userProvider.UserName();

        if (includeAdmin)
        {
            return isProjectManager || IsAdmin();
        }

        return isProjectManager;
    }
}

