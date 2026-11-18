using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiApp.Data.Entities;

[Table("notifications")]
public class NotificationEntity {
    public Guid Id { get; set; }
    public Guid RecipientId { get; set; }
    public Guid? TicketId { get; set; }
    public Guid? ProjectId { get; set; }
    public EventType Event { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public DateTime? EmailSentAt { get; set; }

    public UserEntity Recipient { get; set; } = null!;
    public TicketEntity? Ticket { get; set; }
    public ProjectEntity? Project { get; set; }
}
