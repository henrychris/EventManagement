using ErrorOr;

namespace UserModule.ServiceErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error NotFound => Error.NotFound(
            code: "User.NotFound",
            description: "User not found.");
        
        public static Error DuplicateEmail => Error.NotFound(
            code: "User.DuplicateEmail",
            description: "This email is already in use.");
        public static Error IsLockedOut => Error.Unauthorized(
            code: "User.IsLockedOut",
            description: "User is locked out. Please contact admin.");
        
        public static Error IsNotAllowed => Error.Unauthorized(
            code: "User.IsNotAllowed",
            description: "User is not allowed to access the system. Please contact admin.");
    }
}