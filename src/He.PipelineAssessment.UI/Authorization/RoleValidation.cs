using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure;

namespace He.PipelineAssessment.UI.Authorization;

public interface IRoleValidation
{
    Task<bool> ValidateRole(int assessmentId, string workflowDefinitionId);

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
        if (assessmentToolWorkflow != null)
        {
            if (assessmentToolWorkflow.IsEconomistWorkflow)
            {
                bool isEconomistRoleExist = (_userProvider.CheckUserRole(Constants.AppRole.PipelineEconomist) || _userProvider.CheckUserRole(Constants.AppRole.PipelineAdminOperations));
                return isEconomistRoleExist;
            }
        }

        var isValidForBusinessArea = ValidateForBusinessArea(assessmentId);
        return isValidForBusinessArea.Result;
    }

    private async Task<bool> ValidateForBusinessArea(int assessmentId)
    {
        bool isRoleExist = false;
        var assessment = await _assessmentRepository.GetAssessment(assessmentId);
        if (assessment != null)
        {
            switch (assessment?.BusinessArea)
            {
                case "MPP":
                    isRoleExist = (_userProvider.CheckUserRole(Constants.AppRole.PipelineAssessorMPP) || _userProvider.CheckUserRole(Constants.AppRole.PipelineAdminOperations));
                    return isRoleExist;
                case "Investment":
                    isRoleExist = (_userProvider.CheckUserRole(Constants.AppRole.PipelineAssessorInvestment) || _userProvider.CheckUserRole(Constants.AppRole.PipelineAdminOperations));
                    return isRoleExist;
                case "Development":
                    isRoleExist = (_userProvider.CheckUserRole(Constants.AppRole.PipelineAssessorDevelopment) || _userProvider.CheckUserRole(Constants.AppRole.PipelineAdminOperations));
                    return isRoleExist;
                default: return isRoleExist;
            }
        }
        return false;
    }

}

