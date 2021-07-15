using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class editpostingandcleardatetimeformate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "InvoiceDate",
                table: "PaymentFollowup",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostingDate",
                table: "PaymentFollowup",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ClearDate",
                table: "ChequeBounceNotification",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostingDate",
                table: "ChequeBounceNotification",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostingDate",
                table: "PaymentFollowup");

            migrationBuilder.DropColumn(
                name: "PostingDate",
                table: "ChequeBounceNotification");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceDate",
                table: "PaymentFollowup",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClearDate",
                table: "ChequeBounceNotification",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
