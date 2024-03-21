using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiClientMarket.Migrations
{
    /// <inheritdoc />
    public partial class secondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "product_pkey",
                table: "client_products");

            migrationBuilder.AlterColumn<Guid>(
                name: "product_id",
                table: "client_products",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "product_id",
                table: "client_products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "product_pkey",
                table: "client_products",
                column: "product_id");
        }
    }
}
