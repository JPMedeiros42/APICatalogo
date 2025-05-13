using APICatalogo.Context;

namespace APICatalogo.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IProdutoRepository? _produtoRepo;
    private ICategoriaRepository? _categoriaRepo;

    public AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    //Lazy Loading
    public IProdutoRepository ProdutoRepository
    {
        get
        {
            // Se não existe uma instância de ProdutoRepository, cria uma nova
            return _produtoRepo ??= new ProdutoRepository(_context);
        }
    }

    public ICategoriaRepository CategoriaRepository
    {
        get
        {
            // Se não existe uma instância de CategoriaRepository, cria uma nova
            return _categoriaRepo ??= new CategoriaRepository(_context);
        }
    }

    public async Task CommitAsync()
    {
       await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
