using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.dbContext;
using ServiceLayer.Shared.Interfaces;

namespace ServiceLayer.Shared.Services;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly AppDbContext _context;

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> FindAll() =>
        _context.Set<T>().AsNoTracking();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
        _context.Set<T>().Where(expression).AsNoTracking();

    public void Create(T entity) =>
        _context.Set<T>().Add(entity);

    public void Update(T entity) =>
        _context.Set<T>().Update(entity);

    public void Delete(T entity) =>
        _context.Set<T>().Remove(entity);

    public async Task SaveAsync() =>
        await _context.SaveChangesAsync();
}
