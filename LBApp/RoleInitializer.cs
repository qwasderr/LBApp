using DocumentFormat.OpenXml.Spreadsheet;
using LBApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LBApp
{
    public class RoleInitializer
    {

        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, WebApplication app)
        {

            string adminEmail = "admin";
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
                User admin = new User { Email = adminEmail, UserName = adminEmail, EmailConfirmed = true };
                using (var scope = app.Services.CreateScope())
                {
                    var service = scope.ServiceProvider;
                    var context = service.GetService<DblibraryContext>();
                    var a = context.Readers.Where(c => c.ReaderName == adminEmail);
                    if (a.Count() == 0)
                    {
                        Reader rdr = new Reader { ReaderName = adminEmail };
                        context.Readers.Add(rdr);
                        context.SaveChanges();
                    }

                    IdentityResult result = await userManager.CreateAsync(admin, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "admin");
                    }
                }
            }
            
        }
    }
}
