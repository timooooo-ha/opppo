

using WebApiApp.Data.Entities;

namespace WebApiApp.Services;

public interface INotificationsWriter {

    void Add(
        Guid recipientId,
        EventType eventType,
        string text,
        DateTime createdAt,
        Guid? projectId = null,
        Guid? ticketId = null
    );
}
