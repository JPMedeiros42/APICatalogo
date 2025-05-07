using APICatalogo.Models;

namespace APICatalogo.DTO.Mappings;

public static class CategoriaDTOMappingExtensions
{
    public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
    {
        if (categoria is null)
            return null;

        return new CategoriaDTO
        {
            CategoriaID = categoria.CategoriaID,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        };
    }

    public static Categoria? ToCategoria(this CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
            return null;

        return new Categoria
        {
            CategoriaID = categoriaDto.CategoriaID,
            Nome = categoriaDto.Nome,
            ImagemUrl = categoriaDto.ImagemUrl
        };
    }

    public static IEnumerable<CategoriaDTO> ToCategoriaDTOList(this IEnumerable<Categoria> categorias)
    {
        if (categorias is null || !categorias.Any())
            return new List<CategoriaDTO>();

        return categorias.Select(c => new CategoriaDTO(){
            CategoriaID = c.CategoriaID,
            Nome = c.Nome,
            ImagemUrl = c.ImagemUrl
        }).ToList();
    }
}
