using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PagamentosApp.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirNomeTabelaPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagamentos_Pessoas_PessoaId",
                table: "Pagamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pagamentos",
                table: "Pagamentos");

            migrationBuilder.RenameTable(
                name: "Pagamentos",
                newName: "pagamentos");

            migrationBuilder.RenameIndex(
                name: "IX_Pagamentos_PessoaId",
                table: "pagamentos",
                newName: "IX_pagamentos_PessoaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pagamentos",
                table: "pagamentos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_pagamentos_Pessoas_PessoaId",
                table: "pagamentos",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pagamentos_Pessoas_PessoaId",
                table: "pagamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pagamentos",
                table: "pagamentos");

            migrationBuilder.RenameTable(
                name: "pagamentos",
                newName: "Pagamentos");

            migrationBuilder.RenameIndex(
                name: "IX_pagamentos_PessoaId",
                table: "Pagamentos",
                newName: "IX_Pagamentos_PessoaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pagamentos",
                table: "Pagamentos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamentos_Pessoas_PessoaId",
                table: "Pagamentos",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
