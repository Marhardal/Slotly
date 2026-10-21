using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Slotly.Server;

public class DBContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DBContext(DbContextOptions options) : base(options)
    {
        
    }
}

