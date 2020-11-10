using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class PainterEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Painters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    DepotName = table.Column<string>(nullable: true),
                    SaleGroupCd = table.Column<string>(nullable: true),
                    PainterName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    HasDbbl = table.Column<bool>(nullable: false),
                    AccDbblNumber = table.Column<string>(nullable: true),
                    AccDbblHolderName = table.Column<string>(nullable: true),
                    NationalIdNo = table.Column<string>(nullable: true),
                    PassportNo = table.Column<string>(nullable: true),
                    PainterImageUrl = table.Column<string>(nullable: true),
                    IsAppInstalled = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    AvgMonthlyVal = table.Column<string>(nullable: true),
                    Loyality = table.Column<float>(nullable: false),
                    AttachedDealerCd = table.Column<string>(nullable: true),
                    BrithCertificateNo = table.Column<string>(nullable: true),
                    PainterCatId = table.Column<int>(nullable: false),
                    TerritroyCd = table.Column<string>(nullable: false),
                    ZoneCd = table.Column<string>(nullable: false),
                    NoOfPainterAttached = table.Column<int>(nullable: false,defaultValue:0),
                    EmployeeId = table.Column<int>(nullable:false,defaultValue:0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Painters", x => x.Id);
                    //table.ForeignKey(
                    //    name: "FK_Painters_DropdownDetails_DealerId",
                    //    column: x => x.DealerId,
                    //    principalTable: "DropdownDetails",
                    //    principalColumn: "Id",
                    //    onDelete: ReferentialAction.NoAction);
                    //table.ForeignKey(
                    //    name: "FK_Painters_DropdownDetails_PainterCatId",
                    //    column: x => x.PainterCatId,
                    //    principalTable: "DropdownDetails",
                    //    principalColumn: "Id",
                    //    onDelete: ReferentialAction.NoAction);
                    //table.ForeignKey(
                    //    name: "FK_Painters_DropdownDetails_TerritoryId",
                    //    column: x => x.TerritoryId,
                    //    principalTable: "DropdownDetails",
                    //    principalColumn: "Id",
                    //    onDelete: ReferentialAction.NoAction);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Painters_EmployeeId",
            //    table: "Painters",
            //    column: "EmployeeId");

      

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(name: "IX_Painters_EmployeeId", table: "Painters");
            migrationBuilder.DropTable(
                name: "Painters");
          
            
        }
    }
}
