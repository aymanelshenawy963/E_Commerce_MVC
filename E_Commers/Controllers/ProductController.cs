using E_Commers.Data;
using E_Commers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace E_Commers.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext dbContext = new();
        public IActionResult Index(int Page=1,string? search=null)
        {
            if(Page <= 0)
                Page = 1;

            IQueryable<Product> products = dbContext.Products.Include(e => e.Category);
            if(search != null)
            {
                search = search.Trim();
                ////search = search.TrimStart();
                ////search = search.TrimEnd();


                products = products.Where(e=>e.Name.Contains(search)|| e.Description.Contains(search));
            }
              products= products.Skip((Page-1)*5).Take(5);
            //ViewBag.name = TempData["test"];
            //ViewBag.name=Request.Cookies["Success"];
            if (products.Any())
            {
                return View(products.ToList());

            }
            return RedirectToAction("NotFoundPage","Home");
        }

        public IActionResult Create()
        {

            var categories = dbContext.Categories.ToList().Select(e=>new SelectListItem { Text = e.Name, Value = e.Id.ToString() } );
            ViewBag.Categories = categories;
            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile ImgUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImgUrl.Length > 0)//99656
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);// 1.png
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ImgUrl.CopyTo(stream);
                    }
                    product.ImgUrl = fileName;
                }
                dbContext.Products.Add(product);
                dbContext.SaveChanges();
                TempData["Success"] = "Add Product Successfully";
                //CookieOptions cookieOptions = new();
                //cookieOptions.Expires = DateTime.Now.AddMinutes(1);
                //Response.Cookies.Append("Success", "Add Product Successfully"/*, cookieOptions*/);
                //HttpContext.Session.SetString("Success", "Add Product Successfully");
                return RedirectToAction(nameof(Index));
            }
            var categories = dbContext.Categories.ToList();
            ViewBag.Categories = categories;
            return View(product);

        }

        public IActionResult Edit(int productId)
        {
            var product = dbContext.Products.Find(productId);
            var categories = dbContext.Categories.ToList();
            //ViewData["categories"] = categories;
            ViewBag.categories = categories;
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product, IFormFile ImgUrl)
        {
            var oldProduct = dbContext.Products.AsNoTracking().FirstOrDefault(e => e.Id == product.Id);

            if (ModelState.IsValid)
            {

                if (ImgUrl != null && ImgUrl.Length > 0)//99656
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);// 1.png
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", oldProduct.ImgUrl);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ImgUrl.CopyTo(stream);
                    }
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    product.ImgUrl = fileName;
                }
                else
                {
                    product.ImgUrl = oldProduct.ImgUrl;
                }
                dbContext.Products.Update(product);
                dbContext.SaveChanges();
                TempData["Success"] = "Update Product Successfully";
                //Response.Cookies.Append("Success", "Update Product Successfully");
                return RedirectToAction(nameof(Index));
            }
            var categories = dbContext.Categories.ToList();
            ViewBag.categories = categories;
            product.ImgUrl = oldProduct.ImgUrl;
            return View(product);

        }



        public IActionResult Delete(int productId)
        {
            var product = dbContext.Products.Find(productId);
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", product.ImgUrl);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            dbContext.Products.Remove(product);
            dbContext.SaveChanges();
            TempData["Success"] = "Delete Product Successfully";
            //Response.Cookies.Append("Success", "Delete Product Successfully");
            return RedirectToAction(nameof(Index));
            
        }
    }
}
 