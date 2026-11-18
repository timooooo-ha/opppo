using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiApp.Data.Entities;

[Table("project_members")]
public class ProjectMemberEntity {
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public ProjectRole Role { get; set; }
    public DateTime AddedAt { get; set; }

    public ProjectEntity Project { get; set; } = null!;
    public UserEntity User { get; set; } = null!;
}
