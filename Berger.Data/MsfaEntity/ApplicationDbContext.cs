
﻿using System;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.CollectionEntry;
using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.Examples;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.Menus;
using Berger.Data.MsfaEntity.Organizations;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.Setup;
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


      


        #region Users
        public DbSet<CMUser> CMUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }

        //public DbSet<Delegation> Delegations { get; set; }

        public DbSet<UserTerritoryMapping> UserTerritoryMapping { get; set; }

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
        public DbSet<JourneyPlan> JourneyPlans { get; set; }
        #endregion

        #region Painter
        public DbSet<Painter> Painters { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<PainterCall> PainterCalls { get; set; }
        public DbSet<PainterCompanyMTDValue> PainterCompanyMTDValues { get; set; }

        public DbSet<DealerOpening> DealerOpenings { get; set; }

        #endregion
        public virtual DbSet<CreditControlArea> CreditControlArea { get; set; }
        public virtual DbSet<CustomerGroup> CustomerGroup { get; set; }
        public virtual DbSet<Depot> Depot { get; set; }
        public virtual DbSet<DistributionChannel> DistributionChannel { get; set; }
        public virtual DbSet<Division> Division { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CreditControlArea>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreditControlArea1).HasColumnName("Credit control area");

                entity.Property(e => e.Description)
                    .HasColumnName("Description ")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<CustomerGroup>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CustomerAccountGroup)
                    .HasColumnName("Customer Account Group ")
                    .HasMaxLength(255);

                entity.Property(e => e.Description)
                    .HasColumnName("Description ")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Depot>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Achvm)
                    .HasColumnName("ACHVM")
                    .HasMaxLength(255);

                entity.Property(e => e.Adrnr)
                    .HasColumnName("ADRNR")
                    .HasMaxLength(255);

                entity.Property(e => e.Awsls)
                    .HasColumnName("AWSLS")
                    .HasMaxLength(255);

                entity.Property(e => e.Bedpl)
                    .HasColumnName("BEDPL")
                    .HasMaxLength(255);

                entity.Property(e => e.Betol)
                    .HasColumnName("BETOL")
                    .HasMaxLength(255);

                entity.Property(e => e.Bwkey)
                    .HasColumnName("BWKEY")
                    .HasMaxLength(255);

                entity.Property(e => e.Bzirk)
                    .HasColumnName("BZIRK")
                    .HasMaxLength(255);

                entity.Property(e => e.Bzqhl)
                    .HasColumnName("BZQHL")
                    .HasMaxLength(255);

                entity.Property(e => e.Chazv)
                    .HasColumnName("CHAZV")
                    .HasMaxLength(255);

                entity.Property(e => e.ChazvOld)
                    .HasColumnName("CHAZV_OLD")
                    .HasMaxLength(255);

                entity.Property(e => e.Cityc)
                    .HasColumnName("CITYC")
                    .HasMaxLength(255);

                entity.Property(e => e.Counc)
                    .HasColumnName("COUNC")
                    .HasMaxLength(255);

                entity.Property(e => e.DepStore)
                    .HasColumnName("DEP_STORE")
                    .HasMaxLength(255);

                entity.Property(e => e.Dvsart)
                    .HasColumnName("DVSART")
                    .HasMaxLength(255);

                entity.Property(e => e.Ekorg)
                    .HasColumnName("EKORG")
                    .HasMaxLength(255);

                entity.Property(e => e.Fabkl)
                    .HasColumnName("FABKL")
                    .HasMaxLength(255);

                entity.Property(e => e.Fprfw)
                    .HasColumnName("FPRFW")
                    .HasMaxLength(255);

                entity.Property(e => e.FshBomMaintenance)
                    .HasColumnName("FSH_BOM_MAINTENANCE")
                    .HasMaxLength(255);

                entity.Property(e => e.FshMgArunReq)
                    .HasColumnName("FSH_MG_ARUN_REQ")
                    .HasMaxLength(255);

                entity.Property(e => e.FshSeaim)
                    .HasColumnName("FSH_SEAIM")
                    .HasMaxLength(255);

                entity.Property(e => e.Iwerk)
                    .HasColumnName("IWERK")
                    .HasMaxLength(255);

                entity.Property(e => e.J1bbranch)
                    .HasColumnName("J_1BBRANCH")
                    .HasMaxLength(255);

                entity.Property(e => e.Kkowk)
                    .HasColumnName("KKOWK")
                    .HasMaxLength(255);

                entity.Property(e => e.Kordb)
                    .HasColumnName("KORDB")
                    .HasMaxLength(255);

                entity.Property(e => e.Kunnr)
                    .HasColumnName("KUNNR")
                    .HasMaxLength(255);

                entity.Property(e => e.Land1)
                    .HasColumnName("LAND1")
                    .HasMaxLength(255);

                entity.Property(e => e.Let01).HasColumnName("LET01");

                entity.Property(e => e.Let02).HasColumnName("LET02");

                entity.Property(e => e.Let03).HasColumnName("LET03");

                entity.Property(e => e.Lifnr)
                    .HasColumnName("LIFNR")
                    .HasMaxLength(255);

                entity.Property(e => e.Mandt)
                    .HasColumnName("MANDT")
                    .HasMaxLength(255);

                entity.Property(e => e.Mgvlareval)
                    .HasColumnName("MGVLAREVAL")
                    .HasMaxLength(255);

                entity.Property(e => e.Mgvlaupd)
                    .HasColumnName("MGVLAUPD")
                    .HasMaxLength(255);

                entity.Property(e => e.Mgvupd)
                    .HasColumnName("MGVUPD")
                    .HasMaxLength(255);

                entity.Property(e => e.Misch)
                    .HasColumnName("MISCH")
                    .HasMaxLength(255);

                entity.Property(e => e.Name1)
                    .HasColumnName("NAME1")
                    .HasMaxLength(255);

                entity.Property(e => e.Name2)
                    .HasColumnName("NAME2")
                    .HasMaxLength(255);

                entity.Property(e => e.Nodetype)
                    .HasColumnName("NODETYPE")
                    .HasMaxLength(255);

                entity.Property(e => e.Nschema)
                    .HasColumnName("NSCHEMA")
                    .HasMaxLength(255);

                entity.Property(e => e.Oihcredipi)
                    .HasColumnName("OIHCREDIPI")
                    .HasMaxLength(255);

                entity.Property(e => e.Oihvtype)
                    .HasColumnName("OIHVTYPE")
                    .HasMaxLength(255);

                entity.Property(e => e.Oilival)
                    .HasColumnName("OILIVAL")
                    .HasMaxLength(255);

                entity.Property(e => e.Ort01)
                    .HasColumnName("ORT01")
                    .HasMaxLength(255);

                entity.Property(e => e.Pfach)
                    .HasColumnName("PFACH")
                    .HasMaxLength(255);

                entity.Property(e => e.Pkosa)
                    .HasColumnName("PKOSA")
                    .HasMaxLength(255);

                entity.Property(e => e.Pstlz)
                    .HasColumnName("PSTLZ")
                    .HasMaxLength(255);

                entity.Property(e => e.Regio)
                    .HasColumnName("REGIO")
                    .HasMaxLength(255);

                entity.Property(e => e.Sourcing)
                    .HasColumnName("SOURCING")
                    .HasMaxLength(255);

                entity.Property(e => e.Spart)
                    .HasColumnName("SPART")
                    .HasMaxLength(255);

                entity.Property(e => e.Spras)
                    .HasColumnName("SPRAS")
                    .HasMaxLength(255);

                entity.Property(e => e.Storetype)
                    .HasColumnName("STORETYPE")
                    .HasMaxLength(255);

                entity.Property(e => e.Stras)
                    .HasColumnName("STRAS")
                    .HasMaxLength(255);

                entity.Property(e => e.Taxiw)
                    .HasColumnName("TAXIW")
                    .HasMaxLength(255);

                entity.Property(e => e.Txjcd)
                    .HasColumnName("TXJCD")
                    .HasMaxLength(255);

                entity.Property(e => e.TxnamMa1)
                    .HasColumnName("TXNAM_MA1")
                    .HasMaxLength(255);

                entity.Property(e => e.TxnamMa2)
                    .HasColumnName("TXNAM_MA2")
                    .HasMaxLength(255);

                entity.Property(e => e.TxnamMa3)
                    .HasColumnName("TXNAM_MA3")
                    .HasMaxLength(255);

                entity.Property(e => e.Vkorg)
                    .HasColumnName("VKORG")
                    .HasMaxLength(255);

                entity.Property(e => e.Vlfkz)
                    .HasColumnName("VLFKZ")
                    .HasMaxLength(255);

                entity.Property(e => e.Vstel)
                    .HasColumnName("VSTEL")
                    .HasMaxLength(255);

                entity.Property(e => e.Vtbfi)
                    .HasColumnName("VTBFI")
                    .HasMaxLength(255);

                entity.Property(e => e.Vtweg)
                    .HasColumnName("VTWEG")
                    .HasMaxLength(255);

                entity.Property(e => e.Werks)
                    .HasColumnName("WERKS")
                    .HasMaxLength(255);

                entity.Property(e => e.Wksop)
                    .HasColumnName("WKSOP")
                    .HasMaxLength(255);

                entity.Property(e => e.Zone1)
                    .HasColumnName("ZONE1")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<DistributionChannel>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Discription)
                    .HasColumnName("Discription ")
                    .HasMaxLength(255);

                entity.Property(e => e.DistribtnChannel).HasColumnName("Distribtn Channel");
            });

            modelBuilder.Entity<Division>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Description)
                    .HasColumnName("Description ")
                    .HasMaxLength(255);

                entity.Property(e => e.DivisionCode).HasColumnName("Division Code ");
            });

            //OnModelCreatingPartial(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
