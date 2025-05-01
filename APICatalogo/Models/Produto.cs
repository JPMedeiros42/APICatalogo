using APICatalogo.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

public class Produto : IValidatableObject
{
    public int ProdutoId { get; set; }
    [Required(ErrorMessage ="O nome é obrigatório")]
    [StringLength(60, ErrorMessage ="O nome deve ter entre 5 e 60 caracteres",
        MinimumLength = 5)]
    public string? Nome { get; set; }

    [StringLength(300, ErrorMessage = "A descrição deve ter no maximo {1} caracteres")]
    public string? Descricao { get; set; }

    [Required]
    [Range(1, 10000, ErrorMessage = "O preço deve ser entre {1} e {2}")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300, MinimumLength = 10)]
    public string? ImagemUrl { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
    public int CategoriaID { get; set; }
    [JsonIgnore]
    public Categoria? Categoria { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(this.Nome))
        {
            var primeiraLetra = this.Nome[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                yield return new ValidationResult("A primeira letra deve ser maiúscula(ValidationResult).", 
                    new[] { nameof(this.Nome) });
            }
        }

        if (this.Estoque <= 0)
        {
            yield return new ValidationResult("O estoque deve ser maior que zero(ValidationResult).",
                    new[] { nameof(this.Estoque) });
        }
    }
}
