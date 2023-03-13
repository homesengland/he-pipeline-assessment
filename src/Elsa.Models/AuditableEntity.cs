namespace Elsa.CustomModels;
public abstract class AuditableEntity
{
    public DateTime CreatedDateTime { get; set; }


    public DateTime? LastModifiedDateTime { get; set; }

}
