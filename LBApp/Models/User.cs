using Microsoft.AspNetCore.Identity;
namespace LBApp.Models

{
    public class User: IdentityUser
    {
        public int Year { get; set; }
    }
}
