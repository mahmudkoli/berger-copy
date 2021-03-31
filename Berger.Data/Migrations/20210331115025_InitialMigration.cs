using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Format = table.Column<string>(nullable: true),
                    TableName = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrandInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    MaterialCode = table.Column<string>(nullable: true),
                    MaterialDescription = table.Column<string>(nullable: true),
                    MaterialType = table.Column<string>(nullable: true),
                    MaterialGroupOrBrand = table.Column<string>(nullable: true),
                    PackSize = table.Column<string>(nullable: true),
                    Division = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsCBInstalled = table.Column<bool>(nullable: false),
                    IsMTS = table.Column<bool>(nullable: false),
                    IsPremium = table.Column<bool>(nullable: false),
                    IsEnamel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditControlAreas",
                columns: table => new
                {
                    CreditControlAreaId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditControlAreas", x => x.CreditControlAreaId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerGroups",
                columns: table => new
                {
                    CustomerAccountGroup = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DealerInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CustomerNo = table.Column<int>(nullable: false),
                    Division = table.Column<int>(nullable: false),
                    SalesOffice = table.Column<string>(nullable: true),
                    SalesGroup = table.Column<string>(nullable: true),
                    DayLimit = table.Column<string>(nullable: true),
                    CreditLimit = table.Column<decimal>(nullable: false),
                    TotalDue = table.Column<decimal>(nullable: false),
                    CustomerName = table.Column<string>(nullable: true),
                    CustZone = table.Column<string>(nullable: true),
                    BusinessArea = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    ContactNo = table.Column<string>(nullable: true),
                    AccountGroup = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    CreditControlArea = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsExclusive = table.Column<bool>(nullable: false),
                    IsCBInstalled = table.Column<bool>(nullable: false),
                    IsLastYearAppointed = table.Column<bool>(nullable: false),
                    IsClubSupreme = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerInfos", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    DivisionCode = table.Column<double>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DropdownTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TypeName = table.Column<string>(maxLength: 128, nullable: true),
                    TypeCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropdownTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailConfigForDealerOppenings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Designation = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfigForDealerOppenings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailConfigForDealerSalesCalls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DealerSalesIssueCategoryId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfigForDealerSalesCalls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    To = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Attachmenturl = table.Column<string>(nullable: true),
                    LogStatus = table.Column<int>(nullable: false),
                    FailResoan = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Examples",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examples", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FocusDealers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Code = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "Date", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FocusDealers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JourneyPlanMasters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: true),
                    LineManagerId = table.Column<string>(nullable: true),
                    PlanDate = table.Column<DateTime>(type: "Date", nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    ApprovedById = table.Column<int>(nullable: false),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    PlanStatus = table.Column<int>(nullable: false),
                    RejectedBy = table.Column<int>(nullable: false),
                    EditCount = table.Column<int>(nullable: false),
                    RejectedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyPlanMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JourneyPlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    EmployeeRegId = table.Column<int>(nullable: false),
                    EditCount = table.Column<int>(nullable: false),
                    VisitDate = table.Column<DateTime>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeadBusinessAchievements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BergerValueSales = table.Column<decimal>(nullable: false),
                    BergerPremiumBrandSalesValue = table.Column<decimal>(nullable: false),
                    CompetitionValueSales = table.Column<decimal>(nullable: false),
                    ProductSourcing = table.Column<string>(nullable: true),
                    IsColorSchemeGiven = table.Column<bool>(nullable: false),
                    IsProductSampling = table.Column<bool>(nullable: false),
                    ProductSamplingBrandName = table.Column<string>(nullable: true),
                    NextVisitDate = table.Column<DateTime>(nullable: false),
                    RemarksOrOutcome = table.Column<string>(nullable: true),
                    PhotoCaptureUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadBusinessAchievements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Controller = table.Column<string>(maxLength: 128, nullable: true),
                    Action = table.Column<string>(maxLength: 128, nullable: true),
                    Url = table.Column<string>(maxLength: 256, nullable: true),
                    IconClass = table.Column<string>(maxLength: 128, nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    IsParent = table.Column<bool>(nullable: false),
                    IsTitle = table.Column<bool>(nullable: false),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RPRSPolicies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    FromDaysLimit = table.Column<int>(nullable: false),
                    ToDaysLimit = table.Column<int>(nullable: false),
                    RPRSDays = table.Column<int>(nullable: false),
                    NotificationDays = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RPRSPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleGroup",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SaleOffice",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SchemeMasters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    SchemeName = table.Column<string>(nullable: true),
                    Condition = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchemeMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Territory",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<string>(nullable: true),
                    EmployeeRole = table.Column<int>(nullable: false),
                    Department = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    ManagerName = table.Column<string>(nullable: true),
                    ManagerId = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DropdownDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DropdownName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropdownDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropdownDetails_DropdownTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DropdownTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JourneyPlanDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DealerId = table.Column<int>(nullable: false),
                    VisitDate = table.Column<DateTime>(type: "Date", nullable: false),
                    PlanId = table.Column<int>(nullable: false),
                    JourneyPlanMasterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyPlanDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JourneyPlanDetails_JourneyPlanMasters_JourneyPlanMasterId",
                        column: x => x.JourneyPlanMasterId,
                        principalTable: "JourneyPlanMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuActivities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    ActivityCode = table.Column<string>(nullable: true),
                    MenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuActivities_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    MenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuPermissions_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuPermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchemeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    RateInLtrOrKg = table.Column<string>(nullable: true),
                    RateInDrum = table.Column<string>(nullable: true),
                    Slab = table.Column<string>(nullable: true),
                    Condition = table.Column<string>(nullable: true),
                    BenefitDate = table.Column<string>(nullable: true),
                    SchemeId = table.Column<string>(nullable: true),
                    Material = table.Column<string>(nullable: true),
                    TargetVolume = table.Column<string>(nullable: true),
                    Benefit = table.Column<string>(nullable: true),
                    SchemeMasterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchemeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchemeDetails_SchemeMasters_SchemeMasterId",
                        column: x => x.SchemeMasterId,
                        principalTable: "SchemeMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandInfoStatusLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    BrandInfoId = table.Column<int>(nullable: false),
                    PropertyValue = table.Column<string>(nullable: true),
                    PropertyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandInfoStatusLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandInfoStatusLogs_BrandInfos_BrandInfoId",
                        column: x => x.BrandInfoId,
                        principalTable: "BrandInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandInfoStatusLogs_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerInfoStatusLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    DealerInfoId = table.Column<int>(nullable: false),
                    PropertyValue = table.Column<string>(nullable: true),
                    PropertyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerInfoStatusLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerInfoStatusLogs_DealerInfos_DealerInfoId",
                        column: x => x.DealerInfoId,
                        principalTable: "DealerInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerInfoStatusLogs_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerOpenings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    BusinessArea = table.Column<string>(nullable: true),
                    SaleOffice = table.Column<string>(nullable: true),
                    SaleGroup = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<string>(nullable: true),
                    CurrentApprovarId = table.Column<int>(nullable: true),
                    NextApprovarId = table.Column<int>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    DealerOpeningStatus = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerOpenings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerOpenings_UserInfos_CurrentApprovarId",
                        column: x => x.CurrentApprovarId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealerOpenings_UserInfos_NextApprovarId",
                        column: x => x.NextApprovarId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    UserInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoleMapping_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleMapping_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserZoneAreaMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    PlantId = table.Column<string>(nullable: true),
                    SalesOfficeId = table.Column<string>(nullable: true),
                    AreaId = table.Column<string>(nullable: true),
                    TerritoryId = table.Column<string>(nullable: true),
                    ZoneId = table.Column<string>(nullable: true),
                    UserInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserZoneAreaMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserZoneAreaMappings_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerSalesCalls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DealerId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    JourneyPlanId = table.Column<int>(nullable: true),
                    IsTargetPromotionCommunicated = table.Column<bool>(nullable: false),
                    IsTargetCommunicated = table.Column<bool>(nullable: false),
                    SecondarySalesRatingsId = table.Column<int>(nullable: false),
                    SecondarySalesReasonTitle = table.Column<string>(nullable: true),
                    SecondarySalesReasonRemarks = table.Column<string>(nullable: true),
                    HasOS = table.Column<bool>(nullable: false),
                    IsOSCommunicated = table.Column<bool>(nullable: false),
                    HasSlippage = table.Column<bool>(nullable: false),
                    IsSlippageCommunicated = table.Column<bool>(nullable: false),
                    IsPremiumProductCommunicated = table.Column<bool>(nullable: false),
                    IsPremiumProductLifting = table.Column<bool>(nullable: false),
                    PremiumProductLiftingId = table.Column<int>(nullable: true),
                    PremiumProductLiftingOthers = table.Column<string>(nullable: true),
                    IsCBInstalled = table.Column<bool>(nullable: false),
                    IsCBProductivityCommunicated = table.Column<bool>(nullable: false),
                    MerchendisingId = table.Column<int>(nullable: true),
                    HasSubDealerInfluence = table.Column<bool>(nullable: false),
                    SubDealerInfluenceId = table.Column<int>(nullable: true),
                    HasPainterInfluence = table.Column<bool>(nullable: false),
                    PainterInfluenceId = table.Column<int>(nullable: true),
                    IsShopManProductKnowledgeDiscussed = table.Column<bool>(nullable: false),
                    IsShopManSalesTechniquesDiscussed = table.Column<bool>(nullable: false),
                    IsShopManMerchendizingImprovementDiscussed = table.Column<bool>(nullable: false),
                    HasCompetitionPresence = table.Column<bool>(nullable: false),
                    IsCompetitionServiceBetterThanBPBL = table.Column<bool>(nullable: false),
                    CompetitionServiceBetterThanBPBLRemarks = table.Column<string>(nullable: true),
                    IsCompetitionProductDisplayBetterThanBPBL = table.Column<bool>(nullable: false),
                    CompetitionProductDisplayBetterThanBPBLRemarks = table.Column<string>(nullable: true),
                    CompetitionProductDisplayImageUrl = table.Column<string>(nullable: true),
                    CompetitionSchemeModalityComments = table.Column<string>(nullable: true),
                    CompetitionSchemeModalityImageUrl = table.Column<string>(nullable: true),
                    CompetitionShopBoysComments = table.Column<string>(nullable: true),
                    HasDealerSalesIssue = table.Column<bool>(nullable: false),
                    DealerSatisfactionId = table.Column<int>(nullable: false),
                    DealerSatisfactionReason = table.Column<string>(nullable: true),
                    IsSubDealerCall = table.Column<bool>(nullable: false),
                    HasBPBLSales = table.Column<bool>(nullable: false),
                    BPBLAverageMonthlySales = table.Column<decimal>(nullable: false),
                    BPBLActualMTDSales = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerSalesCalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerSalesCalls_DealerInfos_DealerId",
                        column: x => x.DealerId,
                        principalTable: "DealerInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerSalesCalls_DropdownDetails_DealerSatisfactionId",
                        column: x => x.DealerSatisfactionId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerSalesCalls_JourneyPlanMasters_JourneyPlanId",
                        column: x => x.JourneyPlanId,
                        principalTable: "JourneyPlanMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealerSalesCalls_DropdownDetails_MerchendisingId",
                        column: x => x.MerchendisingId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealerSalesCalls_DropdownDetails_PainterInfluenceId",
                        column: x => x.PainterInfluenceId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealerSalesCalls_DropdownDetails_PremiumProductLiftingId",
                        column: x => x.PremiumProductLiftingId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealerSalesCalls_DropdownDetails_SecondarySalesRatingsId",
                        column: x => x.SecondarySalesRatingsId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealerSalesCalls_DropdownDetails_SubDealerInfluenceId",
                        column: x => x.SubDealerInfluenceId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealerSalesCalls_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ELearningDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ELearningDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ELearningDocuments_DropdownDetails_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeadGenerations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Depot = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    TypeOfClientId = table.Column<int>(nullable: false),
                    ProjectName = table.Column<string>(nullable: true),
                    ProjectAddress = table.Column<string>(nullable: true),
                    KeyContactPersonName = table.Column<string>(nullable: true),
                    KeyContactPersonMobile = table.Column<string>(nullable: true),
                    PaintContractorName = table.Column<string>(nullable: true),
                    PaintContractorMobile = table.Column<string>(nullable: true),
                    PaintingStageId = table.Column<int>(nullable: false),
                    VisitDate = table.Column<DateTime>(nullable: false),
                    ExpectedDateOfPainting = table.Column<DateTime>(nullable: false),
                    NumberOfStoriedBuilding = table.Column<int>(nullable: false),
                    TotalPaintingAreaSqftInterior = table.Column<int>(nullable: false),
                    TotalPaintingAreaSqftInteriorChangeCount = table.Column<int>(nullable: false),
                    TotalPaintingAreaSqftExterior = table.Column<int>(nullable: false),
                    TotalPaintingAreaSqftExteriorChangeCount = table.Column<int>(nullable: false),
                    ExpectedValue = table.Column<decimal>(nullable: false),
                    ExpectedValueChangeCount = table.Column<int>(nullable: false),
                    ExpectedMonthlyBusinessValue = table.Column<decimal>(nullable: false),
                    ExpectedMonthlyBusinessValueChangeCount = table.Column<int>(nullable: false),
                    RequirementOfColorScheme = table.Column<bool>(nullable: false),
                    ProductSamplingRequired = table.Column<bool>(nullable: false),
                    NextFollowUpDate = table.Column<DateTime>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    PhotoCaptureUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadGenerations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadGenerations_DropdownDetails_PaintingStageId",
                        column: x => x.PaintingStageId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadGenerations_DropdownDetails_TypeOfClientId",
                        column: x => x.TypeOfClientId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadGenerations_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    Depot = table.Column<string>(nullable: true),
                    SaleGroup = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    PainterCatId = table.Column<int>(nullable: false),
                    PainterName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    NoOfPainterAttached = table.Column<int>(nullable: false),
                    HasDbbl = table.Column<bool>(nullable: false),
                    AccDbblNumber = table.Column<string>(nullable: true),
                    AccDbblHolderName = table.Column<string>(nullable: true),
                    PassportNo = table.Column<string>(nullable: true),
                    NationalIdNo = table.Column<string>(nullable: true),
                    BrithCertificateNo = table.Column<string>(nullable: true),
                    PainterImageUrl = table.Column<string>(nullable: true),
                    AttachedDealerCd = table.Column<string>(nullable: true),
                    IsAppInstalled = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    AvgMonthlyVal = table.Column<decimal>(nullable: false),
                    Loyality = table.Column<float>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Painters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Painters_DropdownDetails_PainterCatId",
                        column: x => x.PainterCatId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    CustomerTypeId = table.Column<int>(nullable: false),
                    CollectionDate = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    SapId = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    ManualNumber = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    PaymentMethodId = table.Column<int>(nullable: true),
                    CreditControlAreaId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_CreditControlAreas_CreditControlAreaId",
                        column: x => x.CreditControlAreaId,
                        principalTable: "CreditControlAreas",
                        principalColumn: "CreditControlAreaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_DropdownDetails_CustomerTypeId",
                        column: x => x.CustomerTypeId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_DropdownDetails_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TintingMachines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Territory = table.Column<string>(nullable: true),
                    UserInfoId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    NoOfActiveMachine = table.Column<int>(nullable: false),
                    NoOfInactiveMachine = table.Column<int>(nullable: false),
                    No = table.Column<int>(nullable: false),
                    Contribution = table.Column<decimal>(nullable: false),
                    NoOfCorrection = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TintingMachines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TintingMachines_DropdownDetails_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TintingMachines_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuActivityPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    CanView = table.Column<bool>(nullable: false),
                    CanUpdate = table.Column<bool>(nullable: false),
                    CanInsert = table.Column<bool>(nullable: false),
                    CanDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuActivityPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuActivityPermissions_MenuActivities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "MenuActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuActivityPermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerOpeningAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Format = table.Column<string>(nullable: true),
                    DealerOpeningId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerOpeningAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerOpeningAttachments_DealerOpenings_DealerOpeningId",
                        column: x => x.DealerOpeningId,
                        principalTable: "DealerOpenings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerOpeningLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    DealerOpeningId = table.Column<int>(nullable: false),
                    PropertyValue = table.Column<string>(nullable: true),
                    PropertyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerOpeningLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerOpeningLogs_DealerOpenings_DealerOpeningId",
                        column: x => x.DealerOpeningId,
                        principalTable: "DealerOpenings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerOpeningLogs_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerCompetitionSales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerSalesCallId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    AverageMonthlySales = table.Column<decimal>(nullable: false),
                    ActualMTDSales = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerCompetitionSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerCompetitionSales_DropdownDetails_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerCompetitionSales_DealerSalesCalls_DealerSalesCallId",
                        column: x => x.DealerSalesCallId,
                        principalTable: "DealerSalesCalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DealerSalesIssues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerSalesCallId = table.Column<int>(nullable: false),
                    DealerSalesIssueCategoryId = table.Column<int>(nullable: false),
                    MaterialName = table.Column<string>(nullable: true),
                    MaterialGroup = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    BatchNumber = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    PriorityId = table.Column<int>(nullable: false),
                    HasCBMachineMantainance = table.Column<bool>(nullable: false),
                    CBMachineMantainanceId = table.Column<int>(nullable: true),
                    CBMachineMantainanceRegularReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerSalesIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerSalesIssues_DropdownDetails_CBMachineMantainanceId",
                        column: x => x.CBMachineMantainanceId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealerSalesIssues_DealerSalesCalls_DealerSalesCallId",
                        column: x => x.DealerSalesCallId,
                        principalTable: "DealerSalesCalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerSalesIssues_DropdownDetails_DealerSalesIssueCategoryId",
                        column: x => x.DealerSalesIssueCategoryId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealerSalesIssues_DropdownDetails_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ELearningAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Format = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    ELearningDocumentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ELearningAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ELearningAttachments_ELearningDocuments_ELearningDocumentId",
                        column: x => x.ELearningDocumentId,
                        principalTable: "ELearningDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Mark = table.Column<int>(nullable: false),
                    ELearningDocumentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_ELearningDocuments_ELearningDocumentId",
                        column: x => x.ELearningDocumentId,
                        principalTable: "ELearningDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionSets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    TotalMark = table.Column<int>(nullable: false),
                    PassMark = table.Column<int>(nullable: false),
                    ELearningDocumentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionSets_ELearningDocuments_ELearningDocumentId",
                        column: x => x.ELearningDocumentId,
                        principalTable: "ELearningDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeadFollowUps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    LeadGenerationId = table.Column<int>(nullable: false),
                    LastVisitedDate = table.Column<DateTime>(nullable: false),
                    NextVisitDatePlan = table.Column<DateTime>(nullable: false),
                    ActualVisitDate = table.Column<DateTime>(nullable: false),
                    TypeOfClientId = table.Column<int>(nullable: false),
                    KeyContactPersonName = table.Column<string>(nullable: true),
                    KeyContactPersonNameChangeReason = table.Column<string>(nullable: true),
                    KeyContactPersonMobile = table.Column<string>(nullable: true),
                    KeyContactPersonMobileChangeReason = table.Column<string>(nullable: true),
                    PaintContractorName = table.Column<string>(nullable: true),
                    PaintContractorNameChangeReason = table.Column<string>(nullable: true),
                    PaintContractorMobile = table.Column<string>(nullable: true),
                    PaintContractorMobileChangeReason = table.Column<string>(nullable: true),
                    NumberOfStoriedBuilding = table.Column<int>(nullable: false),
                    NumberOfStoriedBuildingChangeReason = table.Column<string>(nullable: true),
                    ExpectedValue = table.Column<decimal>(nullable: false),
                    ExpectedValueChangeReason = table.Column<string>(nullable: true),
                    ExpectedMonthlyBusinessValue = table.Column<decimal>(nullable: false),
                    ExpectedMonthlyBusinessValueChangeReason = table.Column<string>(nullable: true),
                    ProjectStatusId = table.Column<int>(nullable: false),
                    ProjectStatusLeadCompletedId = table.Column<int>(nullable: true),
                    ProjectStatusTotalLossRemarks = table.Column<string>(nullable: true),
                    ProjectStatusPartialBusinessPercentage = table.Column<decimal>(nullable: false),
                    HasSwappingCompetition = table.Column<bool>(nullable: false),
                    SwappingCompetitionId = table.Column<int>(nullable: true),
                    SwappingCompetitionAnotherCompetitorName = table.Column<string>(nullable: true),
                    TotalPaintingAreaSqftInterior = table.Column<int>(nullable: false),
                    TotalPaintingAreaSqftInteriorChangeReason = table.Column<string>(nullable: true),
                    TotalPaintingAreaSqftExterior = table.Column<int>(nullable: false),
                    TotalPaintingAreaSqftExteriorChangeReason = table.Column<string>(nullable: true),
                    UpTradingFromBrandName = table.Column<string>(nullable: true),
                    UpTradingToBrandName = table.Column<string>(nullable: true),
                    BrandUsedInteriorBrandName = table.Column<string>(nullable: true),
                    BrandUsedExteriorBrandName = table.Column<string>(nullable: true),
                    BrandUsedUnderCoatBrandName = table.Column<string>(nullable: true),
                    BrandUsedTopCoatBrandName = table.Column<string>(nullable: true),
                    ActualPaintJobCompletedInteriorPercentage = table.Column<decimal>(nullable: false),
                    ActualPaintJobCompletedExteriorPercentage = table.Column<decimal>(nullable: false),
                    ActualVolumeSoldInteriorGallon = table.Column<decimal>(nullable: false),
                    ActualVolumeSoldInteriorKg = table.Column<decimal>(nullable: false),
                    ActualVolumeSoldExteriorGallon = table.Column<decimal>(nullable: false),
                    ActualVolumeSoldExteriorKg = table.Column<decimal>(nullable: false),
                    ActualVolumeSoldUnderCoatGallon = table.Column<decimal>(nullable: false),
                    ActualVolumeSoldTopCoatGallon = table.Column<decimal>(nullable: false),
                    BusinessAchievementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadFollowUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_LeadBusinessAchievements_BusinessAchievementId",
                        column: x => x.BusinessAchievementId,
                        principalTable: "LeadBusinessAchievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_LeadGenerations_LeadGenerationId",
                        column: x => x.LeadGenerationId,
                        principalTable: "LeadGenerations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_DropdownDetails_ProjectStatusId",
                        column: x => x.ProjectStatusId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_DropdownDetails_ProjectStatusLeadCompletedId",
                        column: x => x.ProjectStatusLeadCompletedId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_DropdownDetails_SwappingCompetitionId",
                        column: x => x.SwappingCompetitionId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_DropdownDetails_TypeOfClientId",
                        column: x => x.TypeOfClientId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttachedDealerPainters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Dealer = table.Column<int>(nullable: false),
                    PainterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachedDealerPainters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachedDealerPainters_Painters_PainterId",
                        column: x => x.PainterId,
                        principalTable: "Painters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PainterAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Format = table.Column<string>(nullable: true),
                    PainterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PainterAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PainterAttachments_Painters_PainterId",
                        column: x => x.PainterId,
                        principalTable: "Painters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PainterCalls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    HasSchemeComnunaction = table.Column<bool>(nullable: false),
                    HasPremiumProtBriefing = table.Column<bool>(nullable: false),
                    HasNewProBriefing = table.Column<bool>(nullable: false),
                    HasUsageEftTools = table.Column<bool>(nullable: false),
                    HasAppUsage = table.Column<bool>(nullable: false),
                    WorkInHandNumber = table.Column<decimal>(nullable: false),
                    HasDbblIssue = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    PainterId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PainterCalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PainterCalls_Painters_PainterId",
                        column: x => x.PainterId,
                        principalTable: "Painters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false),
                    IsCorrectAnswer = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionSetCollections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    QuestionSetId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Mark = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionSetCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionSetCollections_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionSetCollections_QuestionSets_QuestionSetId",
                        column: x => x.QuestionSetId,
                        principalTable: "QuestionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserQuestionAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserInfoId = table.Column<int>(nullable: false),
                    QuestionSetId = table.Column<int>(nullable: false),
                    TotalMark = table.Column<int>(nullable: false),
                    TotalCorrectAnswer = table.Column<int>(nullable: false),
                    Passed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuestionAnswers_QuestionSets_QuestionSetId",
                        column: x => x.QuestionSetId,
                        principalTable: "QuestionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuestionAnswers_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PainterCompanyMTDValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    CountInPercent = table.Column<float>(nullable: false),
                    PainterCallId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PainterCompanyMTDValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PainterCompanyMTDValues_DropdownDetails_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PainterCompanyMTDValues_PainterCalls_PainterCallId",
                        column: x => x.PainterCallId,
                        principalTable: "PainterCalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserQuestionAnswerCollections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserQuestionAnswerId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Mark = table.Column<int>(nullable: false),
                    IsCorrectAnswer = table.Column<bool>(nullable: false),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestionAnswerCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuestionAnswerCollections_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuestionAnswerCollections_UserQuestionAnswers_UserQuestionAnswerId",
                        column: x => x.UserQuestionAnswerId,
                        principalTable: "UserQuestionAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachedDealerPainters_PainterId",
                table: "AttachedDealerPainters",
                column: "PainterId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandInfoStatusLogs_BrandInfoId",
                table: "BrandInfoStatusLogs",
                column: "BrandInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandInfoStatusLogs_UserId",
                table: "BrandInfoStatusLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerCompetitionSales_CompanyId",
                table: "DealerCompetitionSales",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerCompetitionSales_DealerSalesCallId",
                table: "DealerCompetitionSales",
                column: "DealerSalesCallId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerInfoStatusLogs_DealerInfoId",
                table: "DealerInfoStatusLogs",
                column: "DealerInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerInfoStatusLogs_UserId",
                table: "DealerInfoStatusLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerOpeningAttachments_DealerOpeningId",
                table: "DealerOpeningAttachments",
                column: "DealerOpeningId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerOpeningLogs_DealerOpeningId",
                table: "DealerOpeningLogs",
                column: "DealerOpeningId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerOpeningLogs_UserId",
                table: "DealerOpeningLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerOpenings_CurrentApprovarId",
                table: "DealerOpenings",
                column: "CurrentApprovarId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerOpenings_NextApprovarId",
                table: "DealerOpenings",
                column: "NextApprovarId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_DealerId",
                table: "DealerSalesCalls",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_DealerSatisfactionId",
                table: "DealerSalesCalls",
                column: "DealerSatisfactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_JourneyPlanId",
                table: "DealerSalesCalls",
                column: "JourneyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_MerchendisingId",
                table: "DealerSalesCalls",
                column: "MerchendisingId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_PainterInfluenceId",
                table: "DealerSalesCalls",
                column: "PainterInfluenceId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_PremiumProductLiftingId",
                table: "DealerSalesCalls",
                column: "PremiumProductLiftingId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_SecondarySalesRatingsId",
                table: "DealerSalesCalls",
                column: "SecondarySalesRatingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_SubDealerInfluenceId",
                table: "DealerSalesCalls",
                column: "SubDealerInfluenceId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_UserId",
                table: "DealerSalesCalls",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesIssues_CBMachineMantainanceId",
                table: "DealerSalesIssues",
                column: "CBMachineMantainanceId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesIssues_DealerSalesCallId",
                table: "DealerSalesIssues",
                column: "DealerSalesCallId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesIssues_DealerSalesIssueCategoryId",
                table: "DealerSalesIssues",
                column: "DealerSalesIssueCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesIssues_PriorityId",
                table: "DealerSalesIssues",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_DropdownDetails_TypeId",
                table: "DropdownDetails",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ELearningAttachments_ELearningDocumentId",
                table: "ELearningAttachments",
                column: "ELearningDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ELearningDocuments_CategoryId",
                table: "ELearningDocuments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyPlanDetails_JourneyPlanMasterId",
                table: "JourneyPlanDetails",
                column: "JourneyPlanMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_BusinessAchievementId",
                table: "LeadFollowUps",
                column: "BusinessAchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_LeadGenerationId",
                table: "LeadFollowUps",
                column: "LeadGenerationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_ProjectStatusId",
                table: "LeadFollowUps",
                column: "ProjectStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_ProjectStatusLeadCompletedId",
                table: "LeadFollowUps",
                column: "ProjectStatusLeadCompletedId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_SwappingCompetitionId",
                table: "LeadFollowUps",
                column: "SwappingCompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_TypeOfClientId",
                table: "LeadFollowUps",
                column: "TypeOfClientId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadGenerations_PaintingStageId",
                table: "LeadGenerations",
                column: "PaintingStageId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadGenerations_TypeOfClientId",
                table: "LeadGenerations",
                column: "TypeOfClientId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadGenerations_UserId",
                table: "LeadGenerations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuActivities_MenuId",
                table: "MenuActivities",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuActivityPermissions_ActivityId",
                table: "MenuActivityPermissions",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuActivityPermissions_RoleId",
                table: "MenuActivityPermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermissions_MenuId",
                table: "MenuPermissions",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermissions_RoleId",
                table: "MenuPermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PainterAttachments_PainterId",
                table: "PainterAttachments",
                column: "PainterId");

            migrationBuilder.CreateIndex(
                name: "IX_PainterCalls_PainterId",
                table: "PainterCalls",
                column: "PainterId");

            migrationBuilder.CreateIndex(
                name: "IX_PainterCompanyMTDValues_CompanyId",
                table: "PainterCompanyMTDValues",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PainterCompanyMTDValues_PainterCallId",
                table: "PainterCompanyMTDValues",
                column: "PainterCallId");

            migrationBuilder.CreateIndex(
                name: "IX_Painters_PainterCatId",
                table: "Painters",
                column: "PainterCatId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreditControlAreaId",
                table: "Payments",
                column: "CreditControlAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CustomerTypeId",
                table: "Payments",
                column: "CustomerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethodId",
                table: "Payments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ELearningDocumentId",
                table: "Questions",
                column: "ELearningDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionSetCollections_QuestionId",
                table: "QuestionSetCollections",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionSetCollections_QuestionSetId",
                table: "QuestionSetCollections",
                column: "QuestionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionSets_ELearningDocumentId",
                table: "QuestionSets",
                column: "ELearningDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchemeDetails_SchemeMasterId",
                table: "SchemeDetails",
                column: "SchemeMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_TintingMachines_CompanyId",
                table: "TintingMachines",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TintingMachines_UserInfoId",
                table: "TintingMachines",
                column: "UserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionAnswerCollections_QuestionId",
                table: "UserQuestionAnswerCollections",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionAnswerCollections_UserQuestionAnswerId",
                table: "UserQuestionAnswerCollections",
                column: "UserQuestionAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionAnswers_QuestionSetId",
                table: "UserQuestionAnswers",
                column: "QuestionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionAnswers_UserInfoId",
                table: "UserQuestionAnswers",
                column: "UserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMapping_RoleId",
                table: "UserRoleMapping",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMapping_UserInfoId",
                table: "UserRoleMapping",
                column: "UserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserZoneAreaMappings_UserInfoId",
                table: "UserZoneAreaMappings",
                column: "UserInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachedDealerPainters");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "BrandInfoStatusLogs");

            migrationBuilder.DropTable(
                name: "CustomerGroups");

            migrationBuilder.DropTable(
                name: "DealerCompetitionSales");

            migrationBuilder.DropTable(
                name: "DealerInfoStatusLogs");

            migrationBuilder.DropTable(
                name: "DealerOpeningAttachments");

            migrationBuilder.DropTable(
                name: "DealerOpeningLogs");

            migrationBuilder.DropTable(
                name: "DealerSalesIssues");

            migrationBuilder.DropTable(
                name: "Depots");

            migrationBuilder.DropTable(
                name: "Divisions");

            migrationBuilder.DropTable(
                name: "ELearningAttachments");

            migrationBuilder.DropTable(
                name: "EmailConfigForDealerOppenings");

            migrationBuilder.DropTable(
                name: "EmailConfigForDealerSalesCalls");

            migrationBuilder.DropTable(
                name: "EmailLogs");

            migrationBuilder.DropTable(
                name: "Examples");

            migrationBuilder.DropTable(
                name: "FocusDealers");

            migrationBuilder.DropTable(
                name: "JourneyPlanDetails");

            migrationBuilder.DropTable(
                name: "JourneyPlans");

            migrationBuilder.DropTable(
                name: "LeadFollowUps");

            migrationBuilder.DropTable(
                name: "MenuActivityPermissions");

            migrationBuilder.DropTable(
                name: "MenuPermissions");

            migrationBuilder.DropTable(
                name: "PainterAttachments");

            migrationBuilder.DropTable(
                name: "PainterCompanyMTDValues");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "QuestionOptions");

            migrationBuilder.DropTable(
                name: "QuestionSetCollections");

            migrationBuilder.DropTable(
                name: "RPRSPolicies");

            migrationBuilder.DropTable(
                name: "SaleGroup");

            migrationBuilder.DropTable(
                name: "SaleOffice");

            migrationBuilder.DropTable(
                name: "SchemeDetails");

            migrationBuilder.DropTable(
                name: "Territory");

            migrationBuilder.DropTable(
                name: "TintingMachines");

            migrationBuilder.DropTable(
                name: "UserQuestionAnswerCollections");

            migrationBuilder.DropTable(
                name: "UserRoleMapping");

            migrationBuilder.DropTable(
                name: "UserZoneAreaMappings");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropTable(
                name: "BrandInfos");

            migrationBuilder.DropTable(
                name: "DealerOpenings");

            migrationBuilder.DropTable(
                name: "DealerSalesCalls");

            migrationBuilder.DropTable(
                name: "LeadBusinessAchievements");

            migrationBuilder.DropTable(
                name: "LeadGenerations");

            migrationBuilder.DropTable(
                name: "MenuActivities");

            migrationBuilder.DropTable(
                name: "PainterCalls");

            migrationBuilder.DropTable(
                name: "CreditControlAreas");

            migrationBuilder.DropTable(
                name: "SchemeMasters");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "UserQuestionAnswers");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "DealerInfos");

            migrationBuilder.DropTable(
                name: "JourneyPlanMasters");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Painters");

            migrationBuilder.DropTable(
                name: "QuestionSets");

            migrationBuilder.DropTable(
                name: "UserInfos");

            migrationBuilder.DropTable(
                name: "ELearningDocuments");

            migrationBuilder.DropTable(
                name: "DropdownDetails");

            migrationBuilder.DropTable(
                name: "DropdownTypes");
        }
    }
}
