
using ServiceLayer.User.Dto;
using ServiceLayer.Models;
using ServiceLayer.Shared.Interfaces;
using ServiceLayer.Shared.Services;
namespace ServiceLayer.Interface.Reposiory;
public interface IUserRepository:IRepositoryBase<ServiceLayer.Models.User>
{
    IQueryable<Models.User> GetAllUsers();
}