using E_Commers.Data;
using E_Commers.Models;
using E_Commers.Repository.IRepository;

namespace E_Commers.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        //any additional logic
    }
}
