
using System;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.CollectionEntry;
using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.DealerSalesCall;
using Berger.Data.MsfaEntity.DemandGeneration;
using Berger.Data.MsfaEntity.Examples;
using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.Menus;
using Berger.Data.MsfaEntity.Organizations;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Scheme;
using Berger.Data.MsfaEntity.Setup;
using Berger.Data.MsfaEntity.Tinting;
using Berger.Data.MsfaEntity.Users;
using Berger.Data.MsfaEntity.WorkFlows;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Berger.Data.MsfaEntity
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SaleOffice>(e =>{e.HasNoKey();});
            modelBuilder.Entity<SaleGroup>(e =>{e.HasNoKey();});
            modelBuilder.Entity<Territory>(e =>{e.HasNoKey();});
            modelBuilder.Entity<Zone>(e =>{e.HasNoKey();});
            modelBuilder.Entity<Depot>(e =>{e.HasNoKey();});
            modelBuilder.Entity<CustomerGroup>(e => { e.HasNoKey(); });

        }
        public static ApplicationDbContext Create(DbContextOptions<ApplicationDbContext> options)
        {
            return new ApplicationDbContext(options);
        }
        public static ApplicationDbContext Create()
        {
            DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(AppSettingsJson.ConnectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            return Create(builder.Options);
        }

        private IDbContextTransaction _transaction;



        public void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                SaveChanges();
                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

        #region Dispose
        ~ApplicationDbContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction?.Dispose();
            }
        }
        #endregion


        #region Examples
        public DbSet<Example> Examples { get; set; }

        #endregion



        #region Menus
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuActivity> MenuActivities { get; set; }
        public DbSet<MenuActivityPermission> MenuActivityPermissions { get; set; }
        public DbSet<MenuPermission> MenuPermissions { get; set; }

        #endregion

        #region Organizations
        public DbSet<OrganizationRole> OrganizationRoles { get; set; }
        public DbSet<OrganizationUserRole> OrganizationUserRoles { get; set; }

        #endregion


        #region Master
        public DbSet<Depot> Depots { get; set; }
        public DbSet<CustomerGroup> CustomerGroups { get; set; }
        #endregion


        #region Users
        public DbSet<CMUser> CMUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }

        //public DbSet<Delegation> Delegations { get; set; }
        public DbSet<UserHirearchyInfo> UserHirearchyInfos { get; set; }
        public DbSet<UserTerritoryMapping> UserTerritoryMapping { get; set; }
        public DbSet<UserZoneAreaMapping> UserZoneAreaMappings { get; set; }

        #endregion
        #region Setup
        public DbSet<DropdownType> DropdownTypes { get; set; }
        public DbSet<DropdownDetail> DropdownDetails { get; set; }
        #endregion
        #region CollectionEntry
        public DbSet<Payment> Payments { get; set; }
        #endregion
        #region WorkFlows
        public DbSet<WorkFlow> WorkFlows { get; set; }
        public DbSet<WorkFlowConfiguration> WorkFlowConfigurations { get; set; }
        public DbSet<WorkflowLog> WorkflowLogs { get; set; }
        public DbSet<WorkflowLogHistory> WorkflowLogHistories { get; set; }
        public DbSet<WorkFlowType> WorkFlowTypes { get; set; }
        #endregion

        #region JourneyPlan&FocusDealer
        public DbSet<FocusDealer> FocusDealers { get; set; }
        public DbSet<DealerOpeningAttachment> DealerOpeningAttachments { get; set; }
        public DbSet<JourneyPlan> JourneyPlans { get; set; }
        public DbSet<JourneyPlanMaster> JourneyPlanMasters { get; set; }
        public DbSet<JourneyPlanDetail> JourneyPlanDetails { get; set; }

        #endregion

        #region Painter
        public DbSet<PainterCompanyMTDValue> PainterCompanyMTDValues { get; set; }
        public DbSet<Painter> Painters { get; set; }
        public DbSet<PainterAttachment> PainterAttachments { get; set; }
        public DbSet<PainterCall> PainterCalls { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        #endregion
      
        #region SAP Tables

        public DbSet<DealerInfo> DealerInfos { get; set; }


        #endregion
        #region Scheme
        public DbSet<SchemeMaster> SchemeMasters { get; set; }
        public DbSet<SchemeDetail> SchemeDetails { get; set; }
        public DbSet<SaleOffice> SaleOffice { get; set; }
        public DbSet<SaleGroup> SaleGroup { get; set; }
        public DbSet<Territory> Territory { get; set; }
        public DbSet<Zone> Zone { get; set; }

        #endregion
        public DbSet<DealerOpening> DealerOpenings { get; set; }
        public DbSet<AttachedDealerPainter> AttachedDealerPainters { get; set; }
        #region Dealer Sales Call
        public DbSet<Berger.Data.MsfaEntity.DealerSalesCall.DealerSalesCall> DealerSalesCalls { get; set; }
        public DbSet<DealerCompetitionSales> DealerCompetitionSales { get; set; }
        public DbSet<DealerSalesIssue> DealerSalesIssues { get; set; }
        #endregion

        #region Dealer Sales Call
        public DbSet<LeadGeneration> LeadGenerations { get; set; }
        public DbSet<LeadFollowUp> LeadFollowUps { get; set; }
        public DbSet<LeadBusinessAchievement> LeadBusinessAchievements { get; set; }
        #endregion

        #region Tinting
        public DbSet<TintiningMachine> TintiningMachines { get; set; }
        #endregion

    }
}
