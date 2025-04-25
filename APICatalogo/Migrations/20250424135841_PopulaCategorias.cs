using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mB)
        {
            mB.Sql("INSERT INTO Categorias (Nome, ImagemUrl) VALUES ('Bebidas','bebidas.jpg')");
            mB.Sql("INSERT INTO Categorias (Nome, ImagemUrl) VALUES ('Lanches','lanches.jpg')");
            mB.Sql("INSERT INTO Categorias (Nome, ImagemUrl) VALUES ('Sobremesas','sobremesas.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mB)
        {
            mB.Sql("DELETE FROM Categorias");
        }
    }
}
