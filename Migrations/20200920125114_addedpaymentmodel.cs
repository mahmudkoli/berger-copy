using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BergerMsfaApi.Migrations
{
    public partial class addedpaymentmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    PaymentForm = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    SAPID = table.Column<int>(nullable: false),
                    BankName = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Account = table.Column<string>(nullable: true),
                    ManualNumber = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    PaymentMethodId = table.Column<int>(nullable: true),
                    CreditControlAreaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_DropdownDetails_CreditControlAreaId",
                        column: x => x.CreditControlAreaId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_DropdownDetails_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreditControlAreaId",
                table: "Payments",
                column: "CreditControlAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethodId",
                table: "Payments",
                column: "PaymentMethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
