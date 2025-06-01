using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointments.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialAppointmentsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "appointments");

            migrationBuilder.CreateTable(
                name: "Appointments",
                schema: "appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Address_Number = table.Column<int>(type: "integer", nullable: false),
                    Address_Street = table.Column<string>(type: "text", nullable: true),
                    Address_ZipCode = table.Column<long>(type: "bigint", nullable: false),
                    Date_EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Date_StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attendees",
                schema: "appointments",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendees", x => new { x.UserId, x.AppointmentId });
                    table.ForeignKey(
                        name: "FK_Attendees_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalSchema: "appointments",
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CreatedBy",
                schema: "appointments",
                table: "Appointments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Id",
                schema: "appointments",
                table: "Appointments",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_AppointmentId",
                schema: "appointments",
                table: "Attendees",
                column: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendees",
                schema: "appointments");

            migrationBuilder.DropTable(
                name: "Appointments",
                schema: "appointments");
        }
    }
}
