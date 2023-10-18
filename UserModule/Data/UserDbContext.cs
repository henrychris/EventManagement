using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserModule.Data.Models;

namespace UserModule.Data;

// we inherit identityDbContext, so the default tables already exist here.
public class UserDbContext : IdentityDbContext<ApplicationUser>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }
}