namespace Elsa.CustomModels;
public class PotScoreOption : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }

}
