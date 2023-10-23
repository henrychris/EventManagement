using Microsoft.EntityFrameworkCore;
using Shared.Repositories.Implementations;
using UserModule.Data;
using UserModule.Data.Models;

namespace UserModule.Repositories;

public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }

    private UserDbContext UserDbContext => Context as UserDbContext ??
                                           throw new InvalidCastException(
                                               "User DB Context not passed from unit of work.");
}