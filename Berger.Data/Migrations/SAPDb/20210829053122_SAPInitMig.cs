using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations.SAPDb
{
    public partial class SAPInitMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SAPSalesInfos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    bukrs = table.Column<string>(maxLength: 50, nullable: true),
                    spart = table.Column<string>(maxLength: 50, nullable: true),
                    matkl = table.Column<string>(maxLength: 50, nullable: true),
                    wgbez = table.Column<string>(maxLength: 200, nullable: true),
                    matnr = table.Column<string>(maxLength: 50, nullable: true),
                    vkorg = table.Column<string>(maxLength: 50, nullable: true),
                    kunrg = table.Column<string>(maxLength: 50, nullable: true),
                    kunnr_sh = table.Column<string>(maxLength: 50, nullable: true),
                    Payer_DL = table.Column<string>(maxLength: 50, nullable: true),
                    vbeln = table.Column<string>(maxLength: 200, nullable: true),
                    vkbur_c = table.Column<string>(maxLength: 50, nullable: true),
                    vkgrp_c = table.Column<string>(maxLength: 50, nullable: true),
                    kukla = table.Column<string>(maxLength: 50, nullable: true),
                    fkdat = table.Column<string>(maxLength: 50, nullable: true),
                    posnr = table.Column<string>(maxLength: 50, nullable: true),
                    arktx = table.Column<string>(maxLength: 200, nullable: true),
                    meins = table.Column<string>(maxLength: 50, nullable: true),
                    voleh = table.Column<string>(maxLength: 50, nullable: true),
                    Territory = table.Column<string>(maxLength: 50, nullable: true),
                    Szone = table.Column<string>(maxLength: 50, nullable: true),
                    cname = table.Column<string>(maxLength: 200, nullable: true),
                    spart_text = table.Column<string>(maxLength: 200, nullable: true),
                    Revenue = table.Column<string>(maxLength: 200, nullable: true),
                    gsber = table.Column<string>(maxLength: 50, nullable: true),
                    fkimg = table.Column<string>(maxLength: 50, nullable: true),
                    volum = table.Column<string>(maxLength: 200, nullable: true),
                    ktokd = table.Column<string>(maxLength: 50, nullable: true),
                    vtweg = table.Column<string>(maxLength: 50, nullable: true),
                    erzet_T = table.Column<string>(maxLength: 50, nullable: true),
                    kkber = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAPSalesInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SAPSalesInfos");
        }
    }
}
