using E_Commers.Data;
using E_Commers.Models;
using E_Commers.Repository;
using E_Commers.Repository.IRepository;
using E_Commers.Utiltiy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commers.Controllers
{
    [Authorize(Roles = $"{SD.AdminRole},{SD.CompanyRole}")]

    public class CategoryController : Controller
    {
        //ApplicationDbContext dbContext=new ApplicationDbContext();
        //CategoryRepository categoryRepository=new CategoryRepository(); 

        ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            //var categores = dbContext.Categories. Include(e=>e.Products).ToList();
            var categories = categoryRepository.GetAll([e=>e.Products]);
            return View(categories.ToList());
        }

        [HttpGet]
            public IActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            //Category category = new Category() { Name = categoryName };
            if (ModelState.IsValid)
            {
                categoryRepository.Add(category);
                categoryRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
           
        }
        public IActionResult Edit(int categoryId)
        {
            var category = categoryRepository.GetOne(null,e=>e.Id==categoryId);
            return View(category);
        } 

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                //Category category = new Category() { Name = categoryName };
            categoryRepository.Edit(category);
             categoryRepository.Commit();
            return RedirectToAction(nameof(Index));
            }
            return View(category);
       
        }
        public IActionResult Delete(int categoryId)
        {
            Category category = new Category() { Id = categoryId };
            categoryRepository.Delete(category);
            categoryRepository.Commit();
             return RedirectToAction(nameof(Index));
        }
    }
}
