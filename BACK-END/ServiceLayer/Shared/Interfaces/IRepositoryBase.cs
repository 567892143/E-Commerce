using System.Linq.Expressions;
namespace ServiceLayer.Shared.Interfaces;
public interface IRepositoryBase<T> where T : class
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveAsync(); // Optional if you're not using a Unit of Work
}
