using APICatalogo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO;

public class ProdutoDTO
{
    public int ProdutoId { get; set; }
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(60, ErrorMessage = "O nome deve ter entre 5 e 60 caracteres",
        MinimumLength = 5)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "A descrição deve ter no maximo {1} caracteres")]
    public string? Descricao { get; set; }

    [Required]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300, MinimumLength = 10)]
    public string? ImagemUrl { get; set; }
    public int CategoriaID { get; set; }
}
