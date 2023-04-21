using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LBApp.ViewModel;
using LBApp.Models;
using DocumentFormat.OpenXml.Spreadsheet;

namespace LBApp.Controllers
{
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<Reader> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<Reader> userManager)
{
            _roleManager = roleManager;
            _userManager = userManager;
}
        public IActionResult Index() => View(_roleManager.Roles.ToList());
        public IActionResult UserList() => View(_userManager.Users.ToList());
        /*public IActionResult Index()
        {
            return View();
        }*/
        public async Task<IActionResult> Edit(string userId)
        {
            Reader user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
{
        Reader user = await _userManager.FindByIdAsync(userId);
if (user != null)
{
        var userRoles = await _userManager.GetRolesAsync(user);
        var allRoles = _roleManager.Roles.ToList();
        var addedRoles = roles.Except(userRoles);
        var removedRoles = userRoles.Except(roles);
        await _userManager.AddToRolesAsync(user, addedRoles);
        await _userManager.RemoveFromRolesAsync(user, removedRoles);
        return RedirectToAction("UserList");
}
return NotFound();
}
    }
}
