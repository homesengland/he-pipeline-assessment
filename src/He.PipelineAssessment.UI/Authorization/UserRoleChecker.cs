using He.PipelineAssessment.Infrastructure;

namespace He.PipelineAssessment.UI.Authorization
{
    public interface IUserRoleChecker
    {
        bool CheckUserRole(string roleName);
        bool IsAdmin();
        bool IsProjectManager();
        bool IsEconomist();
    }

    public class UserRoleChecker: IUserRoleChecker
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserProvider> _logger;

        public UserRoleChecker(IHttpContextAccessor httpContextAccessor, ILogger<UserProvider> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public bool CheckUserRole(string roleName)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                return _httpContextAccessor.HttpContext.User.IsInRole(roleName);
            }
            else
            {
                _logger.LogError("The HttpContext is null when checking role: {RoleName}", roleName);
                return false;
            }
        }

        public bool IsAdmin()
        {
            return CheckUserRole(Constants.AppRole.PipelineAdminOperations);
        }

        public bool IsProjectManager() 
        {
            return CheckUserRole(Constants.AppRole.PipelineProjectManager);
        }

        public bool IsEconomist()
        {
            return CheckUserRole(Constants.AppRole.PipelineEconomist);
        }

    }
}
