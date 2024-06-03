using Project.Core.Models;
using Project.Infrastructure.DAL;
using Project.Infrastructure.Repositories.Abstraction;

namespace Project.Infrastructure.Repositories.Implementation
{
    public class RectangleRepository 
        : GenericRepository<RectangleModel>
        , IRectangleRepository
    {
        public RectangleRepository(AppDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
