using System;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.SAPReports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Berger.Data.MsfaEntity
{
    public class SAPDbContext : DbContext, IUnitOfWork
    {
        public SAPDbContext(DbContextOptions<SAPDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public static SAPDbContext Create(DbContextOptions<SAPDbContext> options)
        {
            return new SAPDbContext(options);
        }

        public static SAPDbContext Create()
        {
            DbContextOptionsBuilder<SAPDbContext> builder = new DbContextOptionsBuilder<SAPDbContext>();
            builder.UseSqlServer(AppSettingsJson.SAPConnectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
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
        ~SAPDbContext()
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

        public DbSet<SAPSalesInfo> SAPSalesInfos { get; set; }
        public DbSet<QuarterlyPerformanceReport> QuarterlyPerformanceReports { get; set; }
        public DbSet<SummaryPerformanceReport> SummaryPerformanceReports { get; set; }
        public DbSet<CustomerPerformanceReport> CustomerPerformanceReports { get; set; }
        public DbSet<ColorBankPerformanceReport> ColorBankPerformanceReports { get; set; }
        public DbSet<KPIPerformanceReport> KpiPerformanceReports { get; set; }
        public DbSet<CategoryWisePerformanceReport> CategoryWisePerformanceReports { get; set; }
        public DbSet<CustomerInvoiceReport> CustomerInvoiceReports { get; set; }
    }
}
