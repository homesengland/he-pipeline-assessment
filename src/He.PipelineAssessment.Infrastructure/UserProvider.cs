using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace He.PipelineAssessment.Infrastructure
{

    public interface IUserProvider
    {
        string? UserName();
        string? Email();
        bool IsEconomist();
        bool IsProjectManager();
        bool IsAdmin();
        bool CheckUserRole(string roleName);
    }
    public partial class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserProvider> _logger;

        public UserProvider(IHttpContextAccessor httpContextAccessor, ILogger<UserProvider> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public string? UserName()
        { 
            if (_httpContextAccessor.HttpContext == null)
            {
                LogHttpContextNull();
                return null;
            }

            try
            {
                var identities = _httpContextAccessor.HttpContext.User.Identities;
                if (!identities.Any())
                {
                    LogNoIdentitiesFound();
                    return null;
                }

                var claims = identities.First().Claims;
                if (!claims.Any())
                {
                    LogNoClaimsFound();
                    return null;
                }

                var userName = (claims.First().OriginalIssuer == "LOCAL AUTHORITY") 
                    ? claims.First().Value 
                    : claims.FirstOrDefault(c => c.Type == "name")?.Value;

                if (userName == null)
                {
                    LogUserNameClaimNotFound();
                }

                return userName;
            }
            catch (Exception ex)
            {
                LogErrorRetrievingUserName(ex);
                return null;
            }
        }

        public string? Email()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                LogHttpContextNull();
                return null;
            }

            try
            {
                var identities = _httpContextAccessor.HttpContext.User.Identities;
                if (!identities.Any())
                {
                    LogNoIdentitiesFound();
                    return null;
                }

                var claims = identities.First().Claims;
                var emailClaim = claims.FirstOrDefault(c => c.Type.Contains("emailaddress"));
                
                if (emailClaim == null)
                {
                    LogEmailClaimNotFound();
                    return null;
                }

                return emailClaim.Value;
            }
            catch (Exception ex)
            {
                LogErrorRetrievingEmail(ex);
                return null;
            }
        }

        public bool CheckUserRole(string roleName)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                return _httpContextAccessor.HttpContext.User.IsInRole(roleName);
            }
            else
            {
                LogHttpContextNullWhenCheckingRole(roleName);
                return false;
            }
        }

        public bool IsAdmin()
        {
            return CheckUserRole(RoleConstants.AppRole.PipelineAdminOperations);
        }

        public bool IsProjectManager()
        {
            return CheckUserRole(RoleConstants.AppRole.PipelineProjectManager);
        }

        public bool IsEconomist()
        {
            return CheckUserRole(RoleConstants.AppRole.PipelineEconomist);
        }

        [LoggerMessage(
            EventId = 2001,
            Level = LogLevel.Error,
            Message = "The HttpContext is null")]
        private partial void LogHttpContextNull();

        [LoggerMessage(
            EventId = 2002,
            Level = LogLevel.Error,
            Message = "The HttpContext is null when checking role: {RoleName}")]
        private partial void LogHttpContextNullWhenCheckingRole(string roleName);

        [LoggerMessage(
            EventId = 2003,
            Level = LogLevel.Error,
            Message = "No identities found in the HttpContext")]
        private partial void LogNoIdentitiesFound();

        [LoggerMessage(
            EventId = 2004,
            Level = LogLevel.Error,
            Message = "No claims found in the identity")]
        private partial void LogNoClaimsFound();

        [LoggerMessage(
            EventId = 2005,
            Level = LogLevel.Warning,
            Message = "User name claim not found")]
        private partial void LogUserNameClaimNotFound();

        [LoggerMessage(
            EventId = 2006,
            Level = LogLevel.Error,
            Message = "Error retrieving user name")]
        private partial void LogErrorRetrievingUserName(Exception ex);

        [LoggerMessage(
            EventId = 2007,
            Level = LogLevel.Warning,
            Message = "Email claim not found")]
        private partial void LogEmailClaimNotFound();

        [LoggerMessage(
            EventId = 2008,
            Level = LogLevel.Error,
            Message = "Error retrieving email")]
        private partial void LogErrorRetrievingEmail(Exception ex);
    }
}
