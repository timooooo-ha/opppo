namespace WebApiApp.Exceptions;

public class ProjectConflictException : Exception {
    public Dictionary<string, string> Errors { get; }

    public ProjectConflictException(Dictionary<string, string> errors) {
        Errors = errors;
    }
}

public class ProjectNotFoundException : Exception;

public class ProjectAccesDeniedException : Exception;

