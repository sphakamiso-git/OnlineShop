using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private ApplicationDbContext _db; 

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //var data = _db.ProductTypes.ToList();
            return View(_db.ProductTypes.ToList());
        }

        //GET:Create
        public ActionResult Create()
        {
            return View();
        }

        //POST:Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes producttypes)
        {
            if (ModelState.IsValid)
            {
                _db.ProductTypes.Add(producttypes);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product type has been saved";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(producttypes);
        }

        //GET:Edit
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var productType = _db.ProductTypes.Find(id);
            if(productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //POST:Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes producttypes)
        {
            if (ModelState.IsValid)
            {
                _db.Update(producttypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(producttypes);
        }

        //GET:Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //POST:Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Details(ProductTypes producttypes)
        { 
            return View(producttypes);
        }
        //GET:Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //POST:Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, ProductTypes producttypes)
        {

            if (id == null)
            {
                return NotFound();
            }
            if (id != producttypes.Id)
            {
                return NotFound();
            }
            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(productType);
                await _db.SaveChangesAsync();
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(producttypes);
        }

    

    }
}
