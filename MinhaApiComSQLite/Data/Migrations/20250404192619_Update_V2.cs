using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaApiComSQLite.Migrations
{
    /// <inheritdoc />
    public partial class Update_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "Produtos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "Produtos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
