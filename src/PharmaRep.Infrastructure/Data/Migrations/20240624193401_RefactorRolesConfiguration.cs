using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PharmaRep.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorRolesConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "3dad34c7-1574-49a0-8500-84f6851cd295");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "6521685b-5e33-43d0-872b-e1873940fe29");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { "13c90e8a-b6df-403d-a0f3-be6c2576a0fc", "c4762ffe-86e7-44b7-8cea-e459f06e13fd", "Admin", null },
                    { "ee6f7539-69cd-46e7-9068-50b3ffc17db4", "e3fcc74b-2fa8-4f2b-a2da-9bf8b7ef7748", "PharmaceuticalRepresentative", null },
                    { "fc5fb185-e1e6-42b3-b51e-8dc6eb22196e", "b0711fc0-76fe-455b-80e4-ffe306b19af3", "Midwife", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "13c90e8a-b6df-403d-a0f3-be6c2576a0fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "ee6f7539-69cd-46e7-9068-50b3ffc17db4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "fc5fb185-e1e6-42b3-b51e-8dc6eb22196e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { "3dad34c7-1574-49a0-8500-84f6851cd295", "", "PharmaceuticalRepresentative", null },
                    { "6521685b-5e33-43d0-872b-e1873940fe29", "", "Midwife", null }
                });
        }
    }
}
