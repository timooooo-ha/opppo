namespace WebApiApp.Exceptions;

public class AuthValidationException : Exception {
    public Dictionary<string, string> Errors { get; }

    public AuthValidationException(Dictionary<string, string> errors) {
        Errors = errors;
    }
}

public class AuthUserNotFoundException : Exception;
