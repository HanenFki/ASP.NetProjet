﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet2.Migrations
{
    /// <inheritdoc />
    public partial class marq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Marque",
                table: "Produits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Marque",
                table: "Produits");
        }
    }
}
