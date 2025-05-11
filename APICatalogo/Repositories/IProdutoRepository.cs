using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
    PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroParams);
    IEnumerable<Produto> GetProdutosPorCategoria(int id);

}
