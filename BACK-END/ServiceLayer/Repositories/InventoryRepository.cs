using ServiceLayer.dbContext;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using ServiceLayer.Shared.Services;

namespace ServiceLayer.Repository
{
    public class InventoryRepository : RepositoryBase<Models.Inventory>, IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Models.Inventory? GetByVariantId(Guid variantId)
        {
            return _context.Inventories.FirstOrDefault(i => i.VariantId == variantId);
        }

        public Models.Inventory? GetById(Guid id)
        {
            return _context.Inventories.FirstOrDefault(i => i.Id == id);
        }

        public void Create(Models.Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            _context.SaveChanges();
        }

        public bool Update(Models.Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            return _context.SaveChanges() > 0;
        }
    }
}
