using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using X.PagedList;
using X.PagedList.EF;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Categoria>> GetCategoriasPaginationAsync(CategoriasParameters categoriasParams)
    {
        var query = _context.Set<Categoria>().AsNoTracking().OrderBy(c => c.CategoriaID);

        var resultado = await query.ToPagedListAsync(categoriasParams.PageNumber,
                                                            categoriasParams.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Categoria>> GetFilterPaginationAsync(CategoriasFiltroNome categoriasParams)
    {
        var query = _context.Set<Categoria>().AsNoTracking();

        if (categoriasParams.Id.HasValue)
        {
            query = query.Where(c => c.CategoriaID == categoriasParams.Id);
        }
        if (!string.IsNullOrWhiteSpace(categoriasParams.Nome))
        {
            query = query.Where(c => c.Nome.ToLower().Contains(categoriasParams.Nome.ToLower()));
        }

        query = query.OrderBy(c => c.CategoriaID);

        var resultado = await query.ToPagedListAsync(categoriasParams.PageNumber,
                                                            categoriasParams.PageSize);

        return resultado;
    }
}
