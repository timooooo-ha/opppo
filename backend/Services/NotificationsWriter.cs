

using WebApiApp.Data;
using WebApiApp.Data.Entities;

namespace WebApiApp.Services;

public class NotificationsWriter : INotificationsWriter {
    private readonly AppDbContext _context;

    public NotificationsWriter(
        AppDbContext context
    ) {
        _context = context;
    }

    public void Add(
        Guid recipientId,
        EventType eventType,
        string text,
        DateTime createdAt,
        Guid? projectId = null,
        Guid? ticketId = null
    ) {
        if (ticketId.HasValue == projectId.HasValue) {
            throw new ArgumentException(
                "Exactly one notification target must be specified"
            );
        }

        _context.Notifications.Add(new NotificationEntity {
            RecipientId = recipientId,
            TicketId = ticketId,
            ProjectId = projectId,
            Event = eventType,
            Text = text,
            CreatedAt = createdAt,
            IsRead = false
        });
    }
}
