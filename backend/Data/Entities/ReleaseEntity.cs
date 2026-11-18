using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiApp.Data.Entities;

[Table("releases")]
public class ReleaseEntity {
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Version { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public ProjectEntity Project { get; set; } = null!;

    public ICollection<TicketEntity> FixedTickets { get; set; } = [];
}
