using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiApp.Data.Entities;

[Table("projects")]
public class ProjectEntity {
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string RepositoryUrl { get; set; } = null!;
    public DateTime RegisteredAt { get; set; }
    public int NextTicketNumber { get; set; }

    public ICollection<ProjectMemberEntity> Members { get; set; } = [];
    public ICollection<ModuleEntity> Modules { get; set; } = [];
    public ICollection<TicketEntity> Tickets { get; set; } = [];
    public ICollection<NotificationEntity> Notifications { get; set; } = [];
}
