using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VezeetaRepositoryLayer.Migrations
{
    public partial class thelastISA2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Requests");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4207a37b-52e6-4777-80c0-0181f72d4fdd");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "af24c0d5-35ef-4512-b7d0-2ddfaba21912");
        }
    }
}
