
using ServiceLayer.User.Dto;
using ServiceLayer.Models;
using ServiceLayer.Shared.Interfaces;
using ServiceLayer.Shared.Services;
namespace ServiceLayer.Interface.Reposiory;

public interface IUserRepository : IRepositoryBase<ServiceLayer.Models.User>
{
    IQueryable<Models.User> GetAllUsers();
    IQueryable<Models.Contact> GetAllContacts();
    void AddNewUser(Models.User user, Contact contact);
    Models.User? GetUserById(Guid userId);
    Contact? GetContactById(Guid contactId);
    void UpdateUserContact(Models.User user, Contact contact);
}