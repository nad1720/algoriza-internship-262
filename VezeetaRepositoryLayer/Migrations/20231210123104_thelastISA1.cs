using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VezeetaRepositoryLayer.Migrations
{
    public partial class thelastISA1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Discounts",
                newName: "IsDeactivated");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "af24c0d5-35ef-4512-b7d0-2ddfaba21912");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeactivated",
                table: "Discounts",
                newName: "IsActive");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5e344d9a-08ae-4956-af78-28ea78b536b0");
        }
    }
}
