using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.Infrastructure
{

    public interface IUserProvider
    {
        string? GetUserName();
        string? GetUserEmail();
    }
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserProvider> _logger;

        public UserProvider(IHttpContextAccessor httpContextAccessor, ILogger<UserProvider> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public string? GetUserName()
        {
            if (_httpContextAccessor.HttpContext != null)
            {

                var userName = _httpContextAccessor.HttpContext.User.Identities.First().Claims.First(c => c.Type == "name").Value;
                return userName;
            }
            else
            {
                _logger.LogError("The HttpContext is null");
                return null;

            }
        }

        public string? GetUserEmail()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var userEmail = _httpContextAccessor.HttpContext.User.Identities.First().Claims.First(c => c.Type.Contains("emailaddress")).Value;
                return userEmail;
            }
            else
            {
                _logger.LogError("The HttpContext is null");
                return null;
            }
        }

    }
}
