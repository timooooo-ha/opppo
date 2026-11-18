using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiApp.Data.Entities;

[Table("modules")]
public class ModuleEntity {
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ProjectEntity Project { get; set; } = null!;

    public ICollection<TicketEntity> Tickets { get; set; } = [];
}
