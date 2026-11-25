using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiApp.Data.Entities;

[Table("users")]
public class UserEntity {
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool MustChangePassword { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<ProjectMemberEntity> ProjectMemberships { get; set; } = [];
    public ICollection<TicketEntity> AuthoredTickets { get; set; } = [];
    public ICollection<TicketEntity> AssignedTickets { get; set; } = [];
    public ICollection<CommentEntity> Comments { get; set; } = [];
    public ICollection<AttachmentEntity> UploadedAttachments { get; set; } = [];
    public ICollection<HistoryEntryEntity> HistoryEntries { get; set; } = [];
    public ICollection<NotificationEntity> Notifications { get; set; } = [];
}
