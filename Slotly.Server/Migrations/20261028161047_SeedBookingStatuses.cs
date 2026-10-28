using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Slotly.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedBookingStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "ID", "Category", "Description", "Name", "createdOn", "updatedOn" },
                values: new object[,]
                {
                    { new Guid("d1a1b2c0-0001-4a10-9000-000000000001"), 1, "The booking is awaiting confirmation.", "Pending", new DateTimeOffset(new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d1a1b2c0-0001-4a10-9000-000000000002"), 1, "The booking has been confirmed.", "Confirmed", new DateTimeOffset(new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d1a1b2c0-0001-4a10-9000-000000000003"), 1, "The booking has been completed.", "Completed", new DateTimeOffset(new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d1a1b2c0-0001-4a10-9000-000000000004"), 1, "The booking has been cancelled.", "Cancelled", new DateTimeOffset(new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "ID",
                keyValue: new Guid("d1a1b2c0-0001-4a10-9000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "ID",
                keyValue: new Guid("d1a1b2c0-0001-4a10-9000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "ID",
                keyValue: new Guid("d1a1b2c0-0001-4a10-9000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "ID",
                keyValue: new Guid("d1a1b2c0-0001-4a10-9000-000000000004"));
        }
    }
}
