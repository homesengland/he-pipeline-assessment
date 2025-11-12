using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Models;
using HibernatingRhinos.Profiler.Appender.CosmosDB;

namespace He.PipelineAssessment.UI.Authorization;

public interface IRoleValidation
{
    Task<bool> ValidateRole(int assessmentId, string workflowDefinitionId);

    bool ValidateSensitiveRecords(Assessment assessment);
    bool ValidateForBusinessArea(string? businessArea);
    bool IsAdmin();
    bool IsProjectManagerForAssessment(string? projectManager);
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

    public bool IsProjectManagerForAssessment(string? projectManager)
    {

        if (projectManager == _userProvider.GetUserName())
        {
            return true;
        }
        return false;
    }

    public bool ValidateSensitiveRecords(Assessment? assessment)
    {
        if (assessment != null)
        {
            if (assessment.IsSensitiveRecord())
            {
                if (_userProvider.CheckUserRole(Constants.AppRole.SensitiveRecordsViewer) ||
                    assessment.ProjectManager == _userProvider.GetUserName())
                {
                    return true;
                }

                return false;
            }
        }

        return true;
    }

}

