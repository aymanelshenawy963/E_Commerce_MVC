using E_Commers.Data;
using E_Commers.Models;
using E_Commers.Utiltiy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commers.Controllers
{
    [Authorize(Roles = $"{SD.AdminRole}")]

    public class CompanyController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            var companines = dbContext.Companies.Include(e=>e.Products).ToList();
            return View(companines);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //Company company = new Company();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            //Company company = new Company() { Name = companyName };
            if (ModelState.IsValid)
            {
                dbContext.Companies.Add(company);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(company);

        }

        public IActionResult Edit(int companyId)
        {
            var company = dbContext.Companies.Find(companyId);
            return View(company);
        }

        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                //Company company = new Category() { Name = companyName };
                dbContext.Companies.Update(company);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(company);

        }

        public IActionResult Delete(int companyId)
        {
            var company = dbContext.Companies.Find(companyId);

            //Company company = new Category() { Name = companyName };
            dbContext.Companies.Remove(company);
                dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));


        }
    }
}
