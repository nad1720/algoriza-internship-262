using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VezeetaRepositoryLayer.Migrations
{
    public partial class finalmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Admins_AdminId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "46585eb4-4a67-40e6-9896-6f02cc433ba5");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Admins_AdminId",
                table: "Doctors",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Admins_AdminId",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Doctors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "acc84c1e-20d5-46e7-9079-abddbb2ec39f");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Admins_AdminId",
                table: "Doctors",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");
        }
    }
}
