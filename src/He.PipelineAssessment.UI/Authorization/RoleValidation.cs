using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Models;
using HibernatingRhinos.Profiler.Appender.CosmosDB;

namespace He.PipelineAssessment.UI.Authorization;

public interface IRoleValidation
{
    Task<bool> ValidateRole(int assessmentId, string workflowDefinitionId);

    bool ValidateSensitiveRecords(Assessment assessment);
    bool ValidateSensitiveRecordsForProjectManagerAndAdmin(Assessment assessment);
    bool ValidateForBusinessArea(string? businessArea);
    bool IsAdmin();
    bool IsProjectManagerForAssessment(string? assessmentProjectManager);
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
                bool isEconomistRoleExist = (_userProvider.CheckUserRole(Constants.AppRole.PipelineEconomist) || _userProvider.CheckUserRole(Constants.AppRole.PipelineAdminOperations));
                return isEconomistRoleExist;
        }
        var assessment = await _assessmentRepository.GetAssessment(assessmentId);

        if(assessmentToolWorkflow != null && !assessmentToolWorkflow.IsEarlyStage)
        {
            if (!ValidateForBusinessArea(assessment))
            {
                return false;
            }
        }

        var canSeeRecord = ValidateSensitiveRecords(assessment);

        if (!canSeeRecord && assessment != null && assessment.IsSensitiveRecord())
        {
            canSeeRecord = await IsUserWhitelistedForSensitiveRecord(assessmentId);
        }

        return canSeeRecord;
    }

    public bool IsAdmin()
    {
        return _userProvider.CheckUserRole(Constants.AppRole.PipelineAdminOperations);
    }

    //To Be Removed
    public bool ValidateForBusinessArea(Assessment? assessment)
    {
        if (assessment != null)
        {
            return ValidateForBusinessArea(assessment?.BusinessArea);
        }
        return false;
    }

    //To Be Removed
    public bool ValidateForBusinessArea(string? businessArea)
    {
        bool isRoleExist = false;
        bool isAdmin = IsAdmin();
        switch (businessArea)
            {
                case Constants.AppRole.PipelineProjectManager:
                    isRoleExist = (_userProvider.CheckUserRole(Constants.AppRole.PipelineProjectManager) || isAdmin);
                    return isRoleExist;
                default: return isRoleExist;
            }
    }

    public bool IsProjectManagerForAssessment(string? assessmentProjectManager)
    {

        if (assessmentProjectManager == _userProvider.GetUserName())
        {
            return true;
        }
        return false;
    }

    public bool ValidateSensitiveRecords(Assessment? assessment)
    {
        return ValidateSensitiveRecordsInternal(assessment, includeAdmin: false);
    }

    public bool ValidateSensitiveRecordsForProjectManagerAndAdmin(Assessment? assessment)
    {
        return ValidateSensitiveRecordsInternal(assessment, includeAdmin: true);
    }

    /// <summary>
    /// Checks if the current user's email is whitelisted for viewing a specific sensitive assessment
    /// </summary>
    /// <param name="assessmentId">The ID of the assessment to check</param>
    /// <returns>True if user's email is in the whitelist, otherwise false</returns>
    public async Task<bool> IsUserWhitelistedForSensitiveRecord(int assessmentId)
    {
        var userEmail = _userProvider.GetUserEmail();

        if (string.IsNullOrWhiteSpace(userEmail))
        {
            return false;
        }

        var formattedEmail = userEmail.ToLower();
        var whitelist = await _assessmentRepository.GetSensitiveRecordWhitelist(assessmentId);

        return whitelist.Any(w => w.Email.ToLower() == formattedEmail);
    }

    private bool ValidateSensitiveRecordsInternal(Assessment? assessment, bool includeAdmin)
    {
        if (assessment == null)
        {
            return true;
        }

        if (!assessment.IsSensitiveRecord())
        {
            return true;
        }

        var isProjectManager = assessment.ProjectManager == _userProvider.GetUserName();

        if (includeAdmin)
        {
            return isProjectManager || IsAdmin();
        }

        return isProjectManager;
    }
}

