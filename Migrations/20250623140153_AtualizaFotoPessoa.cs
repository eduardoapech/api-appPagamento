using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PagamentosApp.Migrations
{
    public partial class AtualizaFotoPessoa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValorMensal",
                table: "Pessoas",
                newName: "Ramo");

            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Pessoas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Idade",
                table: "Pessoas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Mes",
                table: "Pagamentos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "Ano",
                table: "Pagamentos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Idade",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Ano",
                table: "Pagamentos");

            migrationBuilder.RenameColumn(
                name: "Ramo",
                table: "Pessoas",
                newName: "ValorMensal");

            migrationBuilder.AlterColumn<string>(
                name: "Mes",
                table: "Pagamentos",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
