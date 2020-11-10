using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class PainterEntity_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                 name: "AvgMonthlyVal",
                 table: "Painters",
                 nullable: false,
                 oldClrType: typeof(string),
                 oldType: "string",
                 oldNullable: false, defaultValue: 0); ;
  
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Painters_DropdownDetails_DealerId",
            //    table: "Painters");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Painters_DropdownDetails_PainterCatId",
            //    table: "Painters");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Painters_DropdownDetails_TerritoryId",
            //    table: "Painters");

            //migrationBuilder.DropIndex(
            //    name: "IX_Painters_DealerId",
            //    table: "Painters");

            //migrationBuilder.DropIndex(
            //    name: "IX_Painters_PainterCatId",
            //    table: "Painters");

            //migrationBuilder.DropIndex(
            //    name: "IX_Painters_TerritoryId",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "AccHolderName",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "AccNumber",
            //    table: "Painters");

            ////migrationBuilder.DropColumn(
            ////    name: "DealerId",
            ////    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "Loality",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "Name",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "PainterImage",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "PersonlIdentityNo",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "SaleGroup",
            //    table: "Painters");

            ////migrationBuilder.DropColumn(
            ////    name: "TerritoryId",
            ////    table: "Painters");

            //migrationBuilder.AlterColumn<decimal>(
            //    name: "AvgMonthlyVal",
            //    table: "Painters",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "AccDbblHolderName",
            //    table: "Painters",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "AccDbblNumber",
            //    table: "Painters",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "AttachedDealerCd",
            //    table: "Painters",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "BrithCertificateNo",
            //    table: "Painters",
            //    nullable: true);

            //migrationBuilder.AddColumn<float>(
            //    name: "Loyality",
            //    table: "Painters",
            //    nullable: false,
            //    defaultValue: 0f);

            //migrationBuilder.AddColumn<string>(
            //    name: "NationalIdNo",
            //    table: "Painters",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "NoOfPainterAttached",
            //    table: "Painters",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "PainterImageUrl",
            //    table: "Painters",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "PainterName",
            //    table: "Painters",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "PassportNo",
            //    table: "Painters",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "SaleGroupCd",
            //    table: "Painters",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "TerritroyCd",
            //    table: "Painters",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "ZoneCd",
            //    table: "Painters",
            //    nullable: true);


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AvgMonthlyVal",
                table: "Painters",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal",
                oldNullable: false);

            //migrationBuilder.DropTable(
            //    name: "PainterCompanyMTDValue");


            //migrationBuilder.DropColumn(
            //    name: "AccDbblHolderName",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "AccDbblNumber",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "AttachedDealerCd",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "BrithCertificateNo",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "Loyality",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "NationalIdNo",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "NoOfPainterAttached",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "PainterImageUrl",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "PainterName",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "PassportNo",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "SaleGroupCd",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "TerritroyCd",
            //    table: "Painters");

            //migrationBuilder.DropColumn(
            //    name: "ZoneCd",
            //    table: "Painters");

            //migrationBuilder.AlterColumn<string>(
            //    name: "AvgMonthlyVal",
            //    table: "Painters",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(decimal));

            //migrationBuilder.AddColumn<string>(
            //    name: "AccHolderName",
            //    table: "Painters",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "AccNumber",
            //    table: "Painters",
            //    type: "nvarchar(max)",
            //    nullable: true);

            ////migrationBuilder.AddColumn<int>(
            ////    name: "DealerId",
            ////    table: "Painters",
            ////    type: "int",
            ////    nullable: false,
            ////    defaultValue: 0);

            //migrationBuilder.AddColumn<float>(
            //    name: "Loality",
            //    table: "Painters",
            //    type: "real",
            //    nullable: false,
            //    defaultValue: 0f);

            //migrationBuilder.AddColumn<string>(
            //    name: "Name",
            //    table: "Painters",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "PainterImage",
            //    table: "Painters",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "PersonlIdentityNo",
            //    table: "Painters",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "SaleGroup",
            //    table: "Painters",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "TerritoryId",
            //    table: "Painters",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Painters_DealerId",
            //    table: "Painters",
            //    column: "DealerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Painters_PainterCatId",
            //    table: "Painters",
            //    column: "PainterCatId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Painters_TerritoryId",
            //    table: "Painters",
            //    column: "TerritoryId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Painters_DropdownDetails_DealerId",
            //    table: "Painters",
            //    column: "DealerId",
            //    principalTable: "DropdownDetails",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Painters_DropdownDetails_PainterCatId",
            //    table: "Painters",
            //    column: "PainterCatId",
            //    principalTable: "DropdownDetails",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Painters_DropdownDetails_TerritoryId",
            //    table: "Painters",
            //    column: "TerritoryId",
            //    principalTable: "DropdownDetails",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
