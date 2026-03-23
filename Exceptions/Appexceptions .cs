namespace AuthDemo.Exceptions
{
    // ── 404 Not Found ──────────────────────────────────────────────────────

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string name, object key)
            : base($"'{name}' with key '{key}' was not found.") { }
    }

    // ── 400 Bad Request ────────────────────────────────────────────────────

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }

    // ── 422 Validation ─────────────────────────────────────────────────────

    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(List<string> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }

        public ValidationException(string error)
            : base("One or more validation errors occurred.")
        {
            Errors = new List<string> { error };
        }
    }

    // ── 401 Unauthorized ───────────────────────────────────────────────────

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Unauthorized. Please log in.") : base(message) { }
    }

    // ── 403 Forbidden ──────────────────────────────────────────────────────

    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message = "You do not have permission to perform this action.") : base(message) { }
    }

    // ── 409 Conflict ───────────────────────────────────────────────────────

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }

    // ── 503 Database Error ─────────────────────────────────────────────────

    public class DatabaseException : Exception
    {
        public DatabaseException(string message = "A database error occurred.") : base(message) { }
    }

    // ── 409 Database Conflict (unique / FK violation) ──────────────────────

    public class DatabaseConflictException : Exception
    {
        public string Field { get; }

        public DatabaseConflictException(string message, string field = "") : base(message)
        {
            Field = field;
        }
    }

}