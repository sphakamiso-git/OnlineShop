using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;
        public ProductController (ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {

            return View(_db.products.Include(c => c.ProductTypes)
                .Include( f => f.SpecialTag).ToList());
        }

        //POST Index action Method
        [HttpPost]
        public IActionResult Index(double? lowAmount, double? largeAmount)
        {
            var products = _db.products.Include(c => c.ProductTypes)
                .Include(c => c.SpecialTag)
                .Where(c => c.Price >= lowAmount && c.Price <= largeAmount)
                .ToList();

            if (lowAmount == null || largeAmount == null)
            {
                products = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();

            }
            
            return View(products);
        }

        //Get: Create
        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id","Name");
            return View();
        }

        //Post: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.products.FirstOrDefault(c => c.Name == product.Name);
                if(searchProduct != null)
                {
                    ViewBag.message = "This product already exists";
                    ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                    ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
                    return View(product);
                }
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Images/" + image.FileName;
                }

                if (image == null)
                {
                    product.Image = "Images/noimage.JPEG";
                }

                _db.products.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        //Get : Edit Action Method
        public IActionResult Edit(int? id)
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");

            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
                .FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //POST Edit Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Images/" + image.FileName;
                }

                if (image == null)
                {
                    product.Image = "Images/noimage.JPEG";
                }
                _db.products.Update(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);  
        }

        //Get Details Action Method
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
               .FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //GET Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
                .Where(c => c.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //POST Delete
        [HttpPost]
        [ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _db.products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
