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

    public void AddNewUser(User user, Contact contact)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.Users.Add(user);
            _context.Contacts.Add(contact);
            _context.SaveChanges(); // Save both user and contact

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new ApplicationException("Failed to add new user and contact.", ex);
        }
    }

    // ✅ Get User by ID (Guid)
    public User? GetUserById(Guid userId)
    {
        return _context.Users.FirstOrDefault(u => u.Id == userId && u.IsActive);
    }

    // ✅ Get Contact by ID (Guid)
    public Contact? GetContactById(Guid contactId)
    {
        return _context.Contacts.FirstOrDefault(c => c.Id == contactId);
    }
    
    public void UpdateUserContact(User user, Contact contact)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.Users.Update(user);
            _context.Contacts.Update(contact);
            _context.SaveChanges(); // Save both user and contact

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new ApplicationException("Failed to update  user and contact.", ex);
        }
    }

}
