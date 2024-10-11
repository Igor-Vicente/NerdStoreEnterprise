using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NSE.Carrinho.Api.Migrations
{
    /// <inheritdoc />
    public partial class CarrinhoCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItem_CarrinhoCliente_CarrinhoId",
                table: "CarrinhoItem");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItem_CarrinhoCliente_CarrinhoId",
                table: "CarrinhoItem",
                column: "CarrinhoId",
                principalTable: "CarrinhoCliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItem_CarrinhoCliente_CarrinhoId",
                table: "CarrinhoItem");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItem_CarrinhoCliente_CarrinhoId",
                table: "CarrinhoItem",
                column: "CarrinhoId",
                principalTable: "CarrinhoCliente",
                principalColumn: "Id");
        }
    }
}
