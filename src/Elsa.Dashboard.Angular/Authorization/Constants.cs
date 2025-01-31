namespace Elsa.Dashboard.Authorization
{
  public class Constants
  {
    public static class AppRole
    {
      public const string ElsaDashboardAdmin = "ElsaDashboard.Admin";
    }

    public static class AuthorizationPolicies
    {
      public const string AssignmentToElsaDashboardAdminRoleRequired = "AssignmentToElsaDashboardAdminRoleRequired";
    }
  }
}
