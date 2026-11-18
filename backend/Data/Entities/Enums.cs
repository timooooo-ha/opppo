namespace WebApiApp.Data.Entities;

public enum ProjectRole {
    Lead,
    Developer,
    Tester
}

public enum Priority {
    Low,
    Medium,
    High,
    Critical
}

public enum TicketStatus {
    New,
    Assigned,
    InProgress,
    InReview,
    Returned,
    Closed,
    Rejected
}

public enum RejectReason {
    Duplicate,
    NotABug,
    CannotReproduce,
    Other
}

public enum CommentKind {
    User,
    ReturnReason,
    RejectReason,
    FixNote
}

public enum EventType {
    TicketCreated,
    TicketAssigned,
    TicketInProgress,
    TicketInReview,
    TicketReturned,
    TicketClosed,
    TicketRejected,
    CommentAdded,
    AddedToProject,
    DeadlineSoon,
    DeadlineOverdue
}

public enum TicketAction {
    Edit,
    Plan,
    Take,
    MarkFixed,
    Close,
    Return,
    Reject,
    Comment
}
