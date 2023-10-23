using ErrorOr;

namespace UserModule.ServiceErrors;

public static partial class Errors
{
    public static class Auth
    {
        public static Error LoginFailed => Error.Unauthorized(
            code: "Auth.LoginFailed",
            description: "Username or password are incorrect.");
    }
}