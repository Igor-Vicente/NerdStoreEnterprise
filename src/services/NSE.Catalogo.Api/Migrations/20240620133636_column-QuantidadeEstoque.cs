using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NSE.Catalogo.Api.Migrations
{
    /// <inheritdoc />
    public partial class columnQuantidadeEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuatidadeEstoque",
                table: "Produtos",
                newName: "QuantidadeEstoque");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantidadeEstoque",
                table: "Produtos",
                newName: "QuatidadeEstoque");
        }
    }
}
