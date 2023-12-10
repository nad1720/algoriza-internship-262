using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VezeetaRepositoryLayer.Migrations
{
    public partial class thelastISA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "Requests");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5e344d9a-08ae-4956-af78-28ea78b536b0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "46585eb4-4a67-40e6-9896-6f02cc433ba5");
        }
    }
}
