using Shared.Repositories.Interfaces;
using UserModule.Data.Models;

namespace UserModule.Repositories;

public interface IUserRepository : IBaseRepository<ApplicationUser>
{
    
}