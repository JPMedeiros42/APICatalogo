using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;
public class Categoria
{
    //Boa prática: sempre inicializar as coleções
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }

    public int CategoriaID { get; set; }
    [Required]
    [StringLength(60)]
    public string? Nome { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }
    [JsonIgnore]
    public ICollection<Produto>? Produtos { get; set; }
}
