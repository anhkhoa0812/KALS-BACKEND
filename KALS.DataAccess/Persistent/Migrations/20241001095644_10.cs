using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KALS.DataAccess.Persistent.Migrations
{
    /// <inheritdoc />
    public partial class _10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("4ab69f58-9720-4c7f-8f5d-951cc581e525"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "Order",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Order",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "FullName", "Password", "PhoneNumber", "Role", "Username" },
                values: new object[] { new Guid("4985151f-abbc-47b3-96f4-a881fdf1af5f"), "Admin", "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", "0123456789", "Manager", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("4985151f-abbc-47b3-96f4-a881fdf1af5f"));

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedAt",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "FullName", "Password", "PhoneNumber", "Role", "Username" },
                values: new object[] { new Guid("4ab69f58-9720-4c7f-8f5d-951cc581e525"), "Admin", "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", "0123456789", "Manager", "admin" });
        }
    }
}
