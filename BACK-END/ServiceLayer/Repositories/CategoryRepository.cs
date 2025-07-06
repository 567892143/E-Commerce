using ServiceLayer.dbContext;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using ServiceLayer.Shared.Services;
using System.Linq;

namespace ServiceLayer.Repository
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) {}

        
    }
}
