using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mB)
        {
            mB.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaID) " +
                "VALUES ('Coca-Cola Diet', 'Refrigerante de Cola 350ml', 5.45, 'cocacoladiet.jpg', 50, GETDATE(), 1)");

            mB.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaID) " +
                "VALUES ('Lanche de Atumt', 'Lanche de Atum com Maionese', 8.50, 'lancheatum.jpg', 10, GETDATE(), 2)");

            mB.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaID) " +
                "VALUES ('Pudim', 'Pudim de Leite Condensado 100g', 6.75, 'pudim.jpg', 20, GETDATE(), 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mB)
        {
            mB.Sql("DELETE FROM Produtos");
        }
    }
}
