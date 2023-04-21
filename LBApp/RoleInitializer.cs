using DocumentFormat.OpenXml.Spreadsheet;
using LBApp.Models;
using Microsoft.AspNetCore.Identity;

namespace LBApp
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<Reader> userManager, RoleManager<IdentityRole> roleManager)
{
string adminEmail = "admin@gmail.com";
string password = "A123@a";
if (await roleManager.FindByNameAsync("admin") == null)
{
await roleManager.CreateAsync(new IdentityRole("admin"));

}
if (await roleManager.FindByNameAsync("user") == null)
{
await roleManager.CreateAsync(new IdentityRole("user"));
}
if (await userManager.FindByNameAsync(adminEmail) == null)
{
    Reader admin = new Reader { Email = adminEmail, UserName = adminEmail };
    IdentityResult result = await userManager.CreateAsync(admin, password);
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(admin,"admin");
    }
}
}
    }
}
