using BergerMsfaApi.Domain.CollectionEntry;
using BergerMsfaApi.Domain.Examples;
using BergerMsfaApi.Domain.Menus;
using BergerMsfaApi.Domain.Organizations;
using BergerMsfaApi.Domain.Setup;
using BergerMsfaApi.Domain.Users;
using BergerMsfaApi.Domain.WorkFlows;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BergerMsfaApi.Domain
{
    public class ApplicationDbContext : DbContext
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
            builder.UseSqlServer(AppSettingsJson.ConnectionString);
            return Create(builder.Options);
        }


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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.BuildUniqueKey();

        }



    }
}
