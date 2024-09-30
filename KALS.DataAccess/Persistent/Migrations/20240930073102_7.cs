using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KALS.DataAccess.Persistent.Migrations
{
    /// <inheritdoc />
    public partial class _7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("1d4495da-f924-43e7-8175-d4926f6472ed"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Lab",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Lab",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "FullName", "Password", "PhoneNumber", "Role", "UserName" },
                values: new object[] { new Guid("141559e8-5bf5-480f-91bd-39e70085a8da"), "Admin", "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", "0123456789", "Manager", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("141559e8-5bf5-480f-91bd-39e70085a8da"));

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Lab");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Lab");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "FullName", "Password", "PhoneNumber", "Role", "UserName" },
                values: new object[] { new Guid("1d4495da-f924-43e7-8175-d4926f6472ed"), "Admin", "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", "0123456789", "Manager", "admin" });
        }
    }
}
