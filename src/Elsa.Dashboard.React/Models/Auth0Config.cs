namespace Elsa.Dashboard.Models
{
  public class Auth0Config
  {
    public string Audience { get; set; } = null!;
    public string Domain  { get; set; } = null!;
    public string ClientId { get; set; } = null!;
  }
}
