using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IPagedList<Categoria>> GetCategoriasPaginationAsync(CategoriasParameters categoriasParams);
    Task<IPagedList<Categoria>> GetFilterPaginationAsync(CategoriasFiltroNome categoriasParams);
}
