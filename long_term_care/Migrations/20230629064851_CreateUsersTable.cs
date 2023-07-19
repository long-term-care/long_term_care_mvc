using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace long_term_care.Migrations
{
    public partial class CreateUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__car_pick__Case_C__45F365D3",
                table: "car_pick");

            migrationBuilder.DropForeignKey(
                name: "FK_case_daily_registration_Member_Information_MemSid",
                table: "case_daily_registration");

            migrationBuilder.DropIndex(
                name: "IX_case_daily_registration_MemSid",
                table: "case_daily_registration");

            migrationBuilder.DropIndex(
                name: "IX_car_pick_Case_ContID",
                table: "car_pick");

            migrationBuilder.DropColumn(
                name: "MemSid",
                table: "case_daily_registration");

            migrationBuilder.DropColumn(
                name: "Car_Search_M",
                table: "car_pick");

            migrationBuilder.DropColumn(
                name: "Car_Search_Y",
                table: "car_pick");

            migrationBuilder.DropColumn(
                name: "Case_ContID",
                table: "car_pick");

            migrationBuilder.RenameColumn(
                name: "Case_dailyTime1",
                table: "case_daily_registration",
                newName: "Case_date");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Member_Information",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lec_Date",
                table: "lecture_table",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Sch_E",
                table: "lecture_class",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Case_Temp",
                table: "case_daily_registration",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<string>(
                name: "Case_Pluse",
                table: "case_daily_registration",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Case_date",
                table: "case_daily_registration",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Case_Diastolic",
                table: "case_daily_registration",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Case_Systolic",
                table: "case_daily_registration",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Act_Date",
                table: "case_act",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<string>(
                name: "Car_AgencyLoc",
                table: "car_pick",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Car_Search",
                table: "car_pick",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Case_No",
                table: "car_pick",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Member_Information");

            migrationBuilder.DropColumn(
                name: "Sch_E",
                table: "lecture_class");

            migrationBuilder.DropColumn(
                name: "Case_Diastolic",
                table: "case_daily_registration");

            migrationBuilder.DropColumn(
                name: "Case_Systolic",
                table: "case_daily_registration");

            migrationBuilder.DropColumn(
                name: "Car_AgencyLoc",
                table: "car_pick");

            migrationBuilder.DropColumn(
                name: "Car_Search",
                table: "car_pick");

            migrationBuilder.DropColumn(
                name: "Case_No",
                table: "car_pick");

            migrationBuilder.RenameColumn(
                name: "Case_date",
                table: "case_daily_registration",
                newName: "Case_dailyTime1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Lec_Date",
                table: "lecture_table",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "Case_Temp",
                table: "case_daily_registration",
                type: "int",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<int>(
                name: "Case_Pluse",
                table: "case_daily_registration",
                type: "int",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Case_dailyTime1",
                table: "case_daily_registration",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<string>(
                name: "MemSid",
                table: "case_daily_registration",
                type: "nvarchar(8)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Act_Date",
                table: "case_act",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<byte>(
                name: "Car_Search_M",
                table: "car_pick",
                type: "tinyInt",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Car_Search_Y",
                table: "car_pick",
                type: "tinyInt",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Case_ContID",
                table: "car_pick",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_case_daily_registration_MemSid",
                table: "case_daily_registration",
                column: "MemSid");

            migrationBuilder.CreateIndex(
                name: "IX_car_pick_Case_ContID",
                table: "car_pick",
                column: "Case_ContID");

            migrationBuilder.AddForeignKey(
                name: "FK__car_pick__Case_C__45F365D3",
                table: "car_pick",
                column: "Case_ContID",
                principalTable: "case_daily_registration",
                principalColumn: "Case_ContID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_case_daily_registration_Member_Information_MemSid",
                table: "case_daily_registration",
                column: "MemSid",
                principalTable: "Member_Information",
                principalColumn: "Mem_SID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
