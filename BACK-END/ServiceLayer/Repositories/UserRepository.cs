using ServiceLayer.dbContext;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using ServiceLayer.Shared.Services;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
        // No need to redefine _context — already handled in base class
    }

    public IQueryable<User> GetAllUsers()
    {
        return FindByCondition(user => user.IsActive);
    }
    
    public IQueryable<Contact> GetAllContacts()
    {
         return _context.Contacts;
    }
}
