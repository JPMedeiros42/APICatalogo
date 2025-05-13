namespace APICatalogo.Pagination;

public class CategoriasFiltroNome : QueryStringParameters
{
    public int? Id { get; set; }
    public string? Nome { get; set; }
}
