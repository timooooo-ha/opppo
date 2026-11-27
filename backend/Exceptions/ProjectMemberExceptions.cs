namespace WebApiApp.Exceptions;

public class ProjectMemberValidationException : Exception {
    public Dictionary<string, string> Errors { get; }

    public ProjectMemberValidationException(Dictionary<string, string> errors) {
        Errors = errors;
    }
}

