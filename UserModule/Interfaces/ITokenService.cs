namespace UserModule.Interfaces;

public interface ITokenService
{
    string CreateUserJwt(string emailAddress, string userRole, string userId);
}