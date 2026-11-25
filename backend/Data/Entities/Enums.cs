namespace WebApiApp.Data.Entities;

public enum ProjectRole {
    LEAD,
    DEVELOPER,
    TESTER
}

public enum Priority {
    LOW,
    MEDIUM,
    HIGH,
    CRITICAL
}

public enum TicketStatus {
    NEW,
    ASSIGNED,
    IN_PROGRESS,
    IN_REVIEW,
    RETURNED,
    CLOSED,
    REJECTED
}

public enum RejectReason {
    DUPLICATE,
    NOT_A_BUG,
    CANNOT_REPRODUCE,
    OTHER
}

public enum CommentKind {
    USER,
    RETURN_REASON,
    REJECT_REASON,
    FIX_NOTE
}

public enum EventType {
    TICKET_CREATED,
    TICKET_ASSIGNED,
    TICKET_IN_PROGRESS,
    TICKET_IN_REVIEW,
    TICKET_RETURNED,
    TICKET_CLOSED,
    TICKET_REJECTED,
    COMMENT_ADDED,
    ADDED_TO_PROJECT
}

