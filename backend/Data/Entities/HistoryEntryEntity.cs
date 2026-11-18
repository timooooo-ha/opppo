using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiApp.Data.Entities;

[Table("history_entries")]
public class HistoryEntryEntity {
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Guid ChangedById { get; set; }
    public string Field { get; set; } = null!;
    public string? OldValue { get; set; }
    public string NewValue { get; set; } = null!;
    public DateTime ChangedAt { get; set; }

    public TicketEntity Ticket { get; set; } = null!;
    public UserEntity ChangedBy { get; set; } = null!;
}
