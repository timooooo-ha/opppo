namespace WebApiApp.Exceptions;

public class UserAccountConflictException : Exception {
    public Dictionary<string, string> Errors { get; }

    public UserAccountConflictException(Dictionary<string, string> errors) {
        Errors = errors;
    }
}