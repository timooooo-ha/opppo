using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiApp.Data.Entities;

[Table("comments")]
public class CommentEntity {
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Guid AuthorId { get; set; }
    public string Text { get; set; } = null!;
    public CommentKind Kind { get; set; }
    public DateTime CreatedAt { get; set; }

    public TicketEntity Ticket { get; set; } = null!;
    public UserEntity Author { get; set; } = null!;
}
