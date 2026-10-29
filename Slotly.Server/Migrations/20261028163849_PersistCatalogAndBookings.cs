using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Slotly.Server.Migrations
{
    /// <inheritdoc />
    public partial class PersistCatalogAndBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Popular",
                table: "Services",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "ServiceAddOns",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClientEmail",
                table: "Bookings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Bookings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientPhone",
                table: "Bookings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Bookings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Bookings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Bookings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "StaffID",
                table: "Bookings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StaffName",
                table: "Bookings",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookingAddOns",
                columns: table => new
                {
                    AddOnsID = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingsID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingAddOns", x => new { x.AddOnsID, x.BookingsID });
                    table.ForeignKey(
                        name: "FK_BookingAddOns_Bookings_BookingsID",
                        column: x => x.BookingsID,
                        principalTable: "Bookings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingAddOns_ServiceAddOns_AddOnsID",
                        column: x => x.AddOnsID,
                        principalTable: "ServiceAddOns",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingAddOns_BookingsID",
                table: "BookingAddOns",
                column: "BookingsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingAddOns");

            migrationBuilder.DropColumn(
                name: "Popular",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ServiceAddOns");

            migrationBuilder.DropColumn(
                name: "ClientEmail",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ClientPhone",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StaffID",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StaffName",
                table: "Bookings");
        }
    }
}
