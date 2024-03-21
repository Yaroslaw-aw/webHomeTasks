using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiClientMarket.Migrations
{
    /// <inheritdoc />
    public partial class fourthMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "client_products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "order_pkey",
                table: "client_products",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "order_pkey",
                table: "client_products");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "client_products");
        }
    }
}
