using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        private ApplicationDbContext _db;
        UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> usermanager, ApplicationDbContext db)
        {
            _userManager = usermanager;
            _db = db;
        }

        public IActionResult Index()
        {    
            return View(_db.ApplicationUser.ToList());
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var results = await _userManager.CreateAsync(user, user.PasswordHash);
                if (results.Succeeded)
                {
                    TempData["Save"] = "User created successfully";
                    return RedirectToAction(nameof(Index));

                }

                foreach (var error in results.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
          
         
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = _db.ApplicationUser.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}
