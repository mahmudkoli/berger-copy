
﻿using System;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.CollectionEntry;
using Berger.Data.MsfaEntity.Examples;
using Berger.Data.MsfaEntity.Menus;
using Berger.Data.MsfaEntity.Organizations;
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
    }
}
