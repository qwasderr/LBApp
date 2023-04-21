
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace LBApp.Models
{
    public class IdentityContext: IdentityDbContext<Reader>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
    }
}
