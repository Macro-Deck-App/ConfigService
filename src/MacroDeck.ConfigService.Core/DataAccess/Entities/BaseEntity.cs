namespace MacroDeck.ConfigService.Core.DataAccess.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedTimestamp { get; set; } = DateTime.Now;
    public DateTime? UpdatedTimestamp { get; set; }
}