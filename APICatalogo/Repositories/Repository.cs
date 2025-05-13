using APICatalogo.Context;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using X.PagedList;
using X.PagedList.EF;

namespace APICatalogo.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }
    
    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public async Task<IPagedList<T>> GetFilterPagedAsync(Expression<Func<T, bool>>? predicate,
            int pageNumber, int PageSize)
    {
        var query = _context.Set<T>().AsNoTracking();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.ToPagedListAsync(pageNumber, PageSize);
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        
        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        // _context.Entry(entity).State = EntityState.Modified;
        
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        
        return entity;
    }

}
