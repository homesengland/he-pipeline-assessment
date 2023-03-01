using He.Identity.Auth0;
using He.Identity.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace He.PipelineAssessment.UI.Extensions
{
    public static class CustomExtentionsMethods
    {
        public static void AddCustomAuth0Configuration(this WebApplicationBuilder builder)
        {

            string auth0AppClientId = builder.Configuration["Auth0Config:ClientId"];

            string auth0AppClientSecret = builder.Configuration["Auth0Config:ClientSecret"];

            string auth0Domain = builder.Configuration["Auth0Config:Domain"];

            string identifier = builder.Configuration["Auth0Config:Identifier"];

            string supportEmail = builder.Configuration["Auth0Config:SupportEmail"];

            var auth0Config = new Auth0Config(auth0Domain, auth0AppClientId, auth0AppClientSecret);

            var heIdentityConfiguration = new HeIdentityCookieConfiguration
            {
                Domain = auth0Config.Domain,
                ClientId = auth0Config.ClientId,
                ClientSecret = auth0Config.ClientSecret,
                SupportEmail = supportEmail
            };

            var auth0ManagementConfig = new Auth0ManagementConfig(
                                        auth0Config.Domain,
                                        auth0Config.ClientId,
                                        auth0Config.ClientSecret,
                                        identifier,
                                        "???");

            var env = builder.Environment;
            var mvcBuilder = builder.Services.AddControllersWithViews(config =>
            {
                config.Filters.Add(new AuthorizeFilter(AuthorizationPolicies.AssignmentToPipelineObserverRoleRequired));
            }
            ).AddHeIdentityCookieAuth(heIdentityConfiguration, env);

            builder.Services.AddSingleton<HttpClient>();
            builder.Services.ConfigureIdentityManagementService(x => x.UseAuth0(auth0Config, auth0ManagementConfig));

            builder.Services.ConfigureHeCookieSettings(mvcBuilder,
                configure => { configure.WithAspNetCore().WithHeIdentity().WithApplicationInsights(); });
        }

        public static void AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                .Build();

                options.AddPolicy(AuthorizationPolicies.AssignmentToPipelineObserverRoleRequired, policy => policy.RequireRole(AppRole.PipelineObserver));
                options.AddPolicy(AuthorizationPolicies.AssignmentToPipelineProjectManagerRoleRequired, policy => policy.RequireRole(AppRole.PipelineProjectManager));
                options.AddPolicy(AuthorizationPolicies.AssignmentToPipelineAssessorMPPRoleRequired, policy => policy.RequireRole(AppRole.PipelineAssessorMPP));
                options.AddPolicy(AuthorizationPolicies.AssignmentToPipelineAssessorInvestmentRoleRequired, policy => policy.RequireRole(AppRole.PipelineAssessorInvestment));
                options.AddPolicy(AuthorizationPolicies.AssignmentToPipelineAssessorDevelopmentRoleRequired, policy => policy.RequireRole(AppRole.PipelineAssessorDevelopment));
                options.AddPolicy(AuthorizationPolicies.AssignmentToPipelineAdminOperationsRoleRequired, policy => policy.RequireRole(AppRole.PipelineAdminOperations));
                options.AddPolicy(AuthorizationPolicies.AssignmentToPipelineAdminBuildRoleRequired, policy => policy.RequireRole(AppRole.PipelineAdminBuild));
                options.AddPolicy(AuthorizationPolicies.AssignmentToPipelineEconomistRoleRequired, policy => policy.RequireRole(AppRole.PipelineEconomist));

            });

        }
    }
    public static class AppRole
    {
        public const string PipelineAdminOperations = "PipelineAssessment.AdminOperations";
        public const string PipelineAdminBuild = "PipelineAssessment.AdminBuild";
        public const string PipelineAssessorMPP = "PipelineAssessment.AssessorMPP";
        public const string PipelineAssessorInvestment = "PipelineAssessment.AssessorInvestment";
        public const string PipelineAssessorDevelopment = "PipelineAssessment.AssessorDevelopment";
        public const string PipelineEconomist = "PipelineAssessment.Economist";
        public const string PipelineProjectManager = "PipelineAssessment.ProjectManager";
        public const string PipelineObserver = "PipelineAssessment.Observer";

    }

    public static class AuthorizationPolicies
    {
        public const string AssignmentToPipelineAdminOperationsRoleRequired = "AssignmentToPipelineAdminOperationsRoleRequired";
        public const string AssignmentToPipelineAdminBuildRoleRequired = "AssignmentToPipelineAdminBuildRoleRequired";
        public const string AssignmentToPipelineAssessorMPPRoleRequired = "AssignmentToPipelineAssessorMPPRoleRequired";
        public const string AssignmentToPipelineAssessorInvestmentRoleRequired = "AssignmentToPipelineAssessorInvestmentRoleRequired";
        public const string AssignmentToPipelineAssessorDevelopmentRoleRequired = "AssignmentToPipelineAssessorDevelopmentRoleRequired";
        public const string AssignmentToPipelineEconomistRoleRequired = "AssignmentToPipelineEconomistRoleRequired";
        public const string AssignmentToPipelineProjectManagerRoleRequired = "AssignmentToPipelineProjectManagerRoleRequired";
        public const string AssignmentToPipelineObserverRoleRequired = "AssignmentToPipelineObserverRoleRequired";
    }
}