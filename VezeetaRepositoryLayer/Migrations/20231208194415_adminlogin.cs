using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VezeetaRepositoryLayer.Migrations
{
    public partial class adminlogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1a3b8460-06d5-44ba-9a62-a405d0fdaae8", "P@$sw0rd" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "af1b0dba-e38c-459e-bdc8-8611ec293732", "AQAAAAEAACcQAAAAEP7cvXhPiHNukZAGx7kVaq0aT8BiODvtCaXD+42dIRN00z1k1416tMycMo8/N4Tu6Q==" });
        }
    }
}
