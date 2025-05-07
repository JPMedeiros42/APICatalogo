using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO;

public class CategoriaDTO
{
    public int CategoriaID { get; set; }

    [Required]
    [StringLength(60)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }
}
