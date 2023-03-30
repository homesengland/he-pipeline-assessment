using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure;

namespace He.PipelineAssessment.UI.Authorization;

public class RoleValidation
{
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly IUserProvider _userProvider;

    public RoleValidation(IAssessmentRepository assessmentRepository, IUserProvider userProvider)
    {
        _assessmentRepository = assessmentRepository;
        _userProvider = userProvider;
    }
    private async Task<bool> ValidateRole(int assessmentId)
    {
        bool isRoleExist = false;
        var assessment = await _assessmentRepository.GetAssessment(assessmentId);

        if (assessment != null)
        {
            switch (assessment?.BusinessArea)
            {
                case "MPP":
                    isRoleExist = _userProvider.CheckUserRole(Constants.AppRole.PipelineAssessorMPP);
                    return isRoleExist;
                case "Investment":
                    isRoleExist = _userProvider.CheckUserRole(Constants.AppRole.PipelineAssessorInvestment);
                    return isRoleExist;
                case "Development":
                    isRoleExist = _userProvider.CheckUserRole(Constants.AppRole.PipelineAssessorDevelopment);
                    return isRoleExist;
                default: return isRoleExist;
            }
        }

        return isRoleExist;
    }
}
