using System.Linq.Expressions;
using X.PagedList;

namespace APICatalogo.Repositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T>? GetAsync(Expression<Func<T, bool>> predicate);
    Task<IPagedList<T>> GetFilterPagedAsync(Expression<Func<T, bool>>? predicate,
                                                        int pageNumber, int pageSize);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}
