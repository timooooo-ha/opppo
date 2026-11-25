using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiApp.Data.Entities;

[Table("tickets")]
public class TicketEntity {
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? ModuleId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid? AssigneeId { get; set; }
    public Guid? DuplicateOfId { get; set; }
    public int Number { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ReproductionSteps { get; set; } = null!;
    public Priority Priority { get; set; } = Priority.MEDIUM;
    public TicketStatus Status { get; set; } = TicketStatus.NEW;
    public DateTime? Deadline { get; set; }
    public string? PlanJustification { get; set; }
    public string? FixLink { get; set; }
    public RejectReason? RejectReason { get; set; }
    public string? FixedInVersion { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ProjectEntity Project { get; set; } = null!;
    public ModuleEntity? Module { get; set; }
    public UserEntity Author { get; set; } = null!;
    public UserEntity? Assignee { get; set; }
    public TicketEntity? DuplicateOf { get; set; }

    public ICollection<TicketEntity> Duplicates { get; set; } = [];
    public ICollection<CommentEntity> Comments { get; set; } = [];
    public ICollection<AttachmentEntity> Attachments { get; set; } = [];
    public ICollection<HistoryEntryEntity> History { get; set; } = [];
    public ICollection<NotificationEntity> Notifications { get; set; } = [];
}
