using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DepotEntityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "UserZoneAreaMappings");

            migrationBuilder.AddColumn<string>(
                name: "PlantId",
                table: "UserZoneAreaMappings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Depots",
                columns: table => new
                {
                    Mandt = table.Column<string>(nullable: true),
                    Werks = table.Column<string>(nullable: true),
                    Name1 = table.Column<string>(nullable: true),
                    Bwkey = table.Column<string>(nullable: true),
                    Kunnr = table.Column<string>(nullable: true),
                    Lifnr = table.Column<string>(nullable: true),
                    Fabkl = table.Column<string>(nullable: true),
                    Name2 = table.Column<string>(nullable: true),
                    Stras = table.Column<string>(nullable: true),
                    Pfach = table.Column<string>(nullable: true),
                    Pstlz = table.Column<string>(nullable: true),
                    Ort01 = table.Column<string>(nullable: true),
                    Ekorg = table.Column<string>(nullable: true),
                    Vkorg = table.Column<string>(nullable: true),
                    Chazv = table.Column<string>(nullable: true),
                    Kkowk = table.Column<string>(nullable: true),
                    Kordb = table.Column<string>(nullable: true),
                    Bedpl = table.Column<string>(nullable: true),
                    Land1 = table.Column<string>(nullable: true),
                    Regio = table.Column<string>(nullable: true),
                    Counc = table.Column<string>(nullable: true),
                    Cityc = table.Column<string>(nullable: true),
                    Adrnr = table.Column<string>(nullable: true),
                    Iwerk = table.Column<string>(nullable: true),
                    Txjcd = table.Column<string>(nullable: true),
                    Vtweg = table.Column<string>(nullable: true),
                    Spart = table.Column<string>(nullable: true),
                    Spras = table.Column<string>(nullable: true),
                    Wksop = table.Column<string>(nullable: true),
                    Awsls = table.Column<string>(nullable: true),
                    ChazvOld = table.Column<string>(nullable: true),
                    Vlfkz = table.Column<string>(nullable: true),
                    Bzirk = table.Column<string>(nullable: true),
                    Zone1 = table.Column<string>(nullable: true),
                    Taxiw = table.Column<string>(nullable: true),
                    Bzqhl = table.Column<string>(nullable: true),
                    Let01 = table.Column<double>(nullable: true),
                    Let02 = table.Column<double>(nullable: true),
                    Let03 = table.Column<double>(nullable: true),
                    TxnamMa1 = table.Column<string>(nullable: true),
                    TxnamMa2 = table.Column<string>(nullable: true),
                    TxnamMa3 = table.Column<string>(nullable: true),
                    Betol = table.Column<string>(nullable: true),
                    J1bbranch = table.Column<string>(nullable: true),
                    Vtbfi = table.Column<string>(nullable: true),
                    Fprfw = table.Column<string>(nullable: true),
                    Achvm = table.Column<string>(nullable: true),
                    Dvsart = table.Column<string>(nullable: true),
                    Nodetype = table.Column<string>(nullable: true),
                    Nschema = table.Column<string>(nullable: true),
                    Pkosa = table.Column<string>(nullable: true),
                    Misch = table.Column<string>(nullable: true),
                    Mgvupd = table.Column<string>(nullable: true),
                    Vstel = table.Column<string>(nullable: true),
                    Mgvlaupd = table.Column<string>(nullable: true),
                    Mgvlareval = table.Column<string>(nullable: true),
                    Sourcing = table.Column<string>(nullable: true),
                    FshMgArunReq = table.Column<string>(nullable: true),
                    FshSeaim = table.Column<string>(nullable: true),
                    FshBomMaintenance = table.Column<string>(nullable: true),
                    Oilival = table.Column<string>(nullable: true),
                    Oihvtype = table.Column<string>(nullable: true),
                    Oihcredipi = table.Column<string>(nullable: true),
                    Storetype = table.Column<string>(nullable: true),
                    DepStore = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Depots");

            migrationBuilder.DropColumn(
                name: "PlantId",
                table: "UserZoneAreaMappings");

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "UserZoneAreaMappings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
