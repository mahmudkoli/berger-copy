using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class addnotificationdatecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NotificationDate",
                table: "PaymentFollowup",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NotificationDate",
                table: "OccasionToCelebrate",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NotificationDate",
                table: "CreditLimitCrossNotifiction",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NotificationDate",
                table: "ChequeBounceNotification",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationDate",
                table: "PaymentFollowup");

            migrationBuilder.DropColumn(
                name: "NotificationDate",
                table: "OccasionToCelebrate");

            migrationBuilder.DropColumn(
                name: "NotificationDate",
                table: "CreditLimitCrossNotifiction");

            migrationBuilder.DropColumn(
                name: "NotificationDate",
                table: "ChequeBounceNotification");
        }
    }
}
