namespace UserModule.Interfaces;

public interface IAuthenticationService
{
    string CreateUserJwt(string emailAddress, string userRole, string userId);
}