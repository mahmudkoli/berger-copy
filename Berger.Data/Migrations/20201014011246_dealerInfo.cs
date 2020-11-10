using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class dealerInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "DealerInfos",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CreatedBy = table.Column<int>(nullable: false),
            //        CreatedTime = table.Column<DateTime>(nullable: false),
            //        ModifiedBy = table.Column<int>(nullable: true),
            //        ModifiedTime = table.Column<DateTime>(nullable: true),
            //        Status = table.Column<int>(nullable: false),
            //        WorkflowId = table.Column<int>(nullable: true),
            //        WFStatus = table.Column<int>(nullable: false),
            //        CustomerNo = table.Column<int>(nullable: false),
            //        DivisionId = table.Column<int>(nullable: false),
            //        DayLimit = table.Column<string>(nullable: true),
            //        CreditLimit = table.Column<decimal>(nullable: false),
            //        TotalDue = table.Column<decimal>(nullable: false),
            //        CustomerName = table.Column<string>(nullable: true),
            //        CustomerZone = table.Column<string>(nullable: true),
            //        BusinessArea = table.Column<string>(nullable: true),
            //        Address = table.Column<string>(nullable: true),
            //        ContactNo = table.Column<string>(nullable: true),
            //        AccountGroup = table.Column<string>(nullable: true),
            //        Territory = table.Column<string>(nullable: true),
            //        CreditControlArea = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_DealerInfos", x => x.Id);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "DealerInfos");
        }
    }
}
