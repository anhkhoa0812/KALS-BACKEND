using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KALS.DataAccess.Persistent.Migrations
{
    /// <inheritdoc />
    public partial class _9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("b48fcb14-8eb4-4f92-81dc-d918cf19d254"));

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "User",
                newName: "Username");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "FullName", "Password", "PhoneNumber", "Role", "Username" },
                values: new object[] { new Guid("4ab69f58-9720-4c7f-8f5d-951cc581e525"), "Admin", "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", "0123456789", "Manager", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("4ab69f58-9720-4c7f-8f5d-951cc581e525"));

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "User",
                newName: "UserName");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "FullName", "Password", "PhoneNumber", "Role", "UserName" },
                values: new object[] { new Guid("b48fcb14-8eb4-4f92-81dc-d918cf19d254"), "Admin", "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", "0123456789", "Manager", "admin" });
        }
    }
}
