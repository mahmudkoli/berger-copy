using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class UpdateAddedAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<long>(
            //    name: "Size",
            //    table: "Attachments",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Attachments",
                nullable: false
               );

            //migrationBuilder.AddColumn<string>(
            //    name: "Path",
            //    table: "Attachments",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "TableName",
            //    table: "Attachments",
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Attachments");

            //migrationBuilder.DropColumn(
            //    name: "Path",
            //    table: "Attachments");

            //migrationBuilder.DropColumn(
            //    name: "TableName",
            //    table: "Attachments");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Size",
            //    table: "Attachments",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(long));
        }
    }
}
