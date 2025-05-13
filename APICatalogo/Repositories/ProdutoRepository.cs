using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EF;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto> , IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    //public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
    //{
    //    return GetAll()
    //        .OrderBy(p => p.Nome)
    //        .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
    //        .Take(produtosParameters.PageSize).ToList();
    //}

    public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
    {
        var query = _context.Set<Produto>().AsNoTracking().OrderBy(p => p.ProdutoId);

        var resultado = await query.ToPagedListAsync(produtosParams.PageNumber, produtosParams.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
    {
        var query = _context.Set<Produto>().AsNoTracking();

        if (produtosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.Preco.ToString()))
        {
            if(produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
            else if (produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
            else if (produtosFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
        }

        var produtosFiltrados = await query.ToPagedListAsync(produtosFiltroParams.PageNumber, produtosFiltroParams.PageSize);
        
        return produtosFiltrados;
    }

    public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
    {
        return await _context.Set<Produto>().AsNoTracking().Where(c => c.CategoriaID == id).ToListAsync();      
    }
}
