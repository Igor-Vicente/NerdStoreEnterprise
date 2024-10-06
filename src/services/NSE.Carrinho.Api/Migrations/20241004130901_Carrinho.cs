using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NSE.Carrinho.Api.Migrations
{
    /// <inheritdoc />
    public partial class Carrinho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarrinhoCliente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrinhoCliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarrinhoItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeProduto = table.Column<string>(type: "varchar(100)", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Imagem = table.Column<string>(type: "varchar(100)", nullable: false),
                    CarrinhoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrinhoItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarrinhoItem_CarrinhoCliente_CarrinhoId",
                        column: x => x.CarrinhoId,
                        principalTable: "CarrinhoCliente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IDX_Cliente",
                table: "CarrinhoCliente",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_CarrinhoItem_CarrinhoId",
                table: "CarrinhoItem",
                column: "CarrinhoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarrinhoItem");

            migrationBuilder.DropTable(
                name: "CarrinhoCliente");
        }
    }
}
