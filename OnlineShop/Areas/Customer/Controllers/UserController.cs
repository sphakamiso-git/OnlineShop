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

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUser.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            userInfo.FirstName = user.FirstName;
            userInfo.LastName = user.LastName;
            var result =await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
            {
                TempData["save"] = "User has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        
        public async Task<IActionResult> Details(string id)
        {
            var user = _db.ApplicationUser.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.ApplicationUser.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUser.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            _db.ApplicationUser.Remove(userInfo);
            int rowAffected =  _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["save"] = "User has been successfully deleted out";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        public async Task<IActionResult> Active(string id)
        {
            var user = _db.ApplicationUser.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Active(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUser.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            userInfo.LockoutEnd = DateTime.Now.AddDays(-1);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["save"] = "user has been activated successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(userInfo);
        }
    }
}
