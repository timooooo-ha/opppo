using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiApp.Data.Entities;

[Table("attachments")]
public class AttachmentEntity {
    public Guid Id { get; set; }
    public Guid? TicketId { get; set; }
    public Guid UploadedById { get; set; }
    public string FileName { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public long SizeBytes { get; set; }
    public string StorageKey { get; set; } = null!;
    public DateTime UploadedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }

    public TicketEntity? Ticket { get; set; }
    public UserEntity UploadedBy { get; set; } = null!;

}
