using E_Commers.Data;
using E_Commers.Models;
using E_Commers.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace E_Commers.Repository
{
    public class CategoryRepository: Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        public  CategoryRepository(ApplicationDbContext dbContext): base(dbContext)
        {
            this.dbContext=dbContext;
        }
        //any additional logic
    }
}
