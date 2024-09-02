using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01e0d4f9-9f2b-4ca8-aade-bfb1fe4f2589");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40457a67-47d2-495b-bf69-82ca99b61c2a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5de34562-8448-413a-a4a6-ba9df411bd49", null, "Admin", "ADMIN" },
                    { "cf44502a-885e-491b-abf6-c98901a48c34", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5de34562-8448-413a-a4a6-ba9df411bd49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf44502a-885e-491b-abf6-c98901a48c34");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01e0d4f9-9f2b-4ca8-aade-bfb1fe4f2589", null, "Customer", "CUSTOMER" },
                    { "40457a67-47d2-495b-bf69-82ca99b61c2a", null, "Admin", "ADMIN" }
                });
        }
    }
}
