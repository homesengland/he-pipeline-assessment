﻿using He.Identity.Auth0;
using He.Identity.Mvc;
using Microsoft.AspNetCore.Authorization;

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
            var mvcBuilder = builder.Services.AddControllersWithViews().AddHeIdentityCookieAuth(heIdentityConfiguration, env);

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
            });
        }
    }
    public static class AppRole
    {

        public const string PipelineAdmin = "Pipeline.Admin";
        public const string PipelineAssessor = "Pipeline.Assessor";
    }

    public static class AuthorizationPolicies
    {
        public const string AssignmentToPipelineAssessorRoleRequired = "AssignmentToPipelineAssessorRoleRequired";
        public const string AssignmentToPipelineAdminRoleRequired = "AssignmentToPipelineAdminRoleRequired";
    }
}