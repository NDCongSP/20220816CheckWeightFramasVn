using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking.Models.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(string connectionString) : base(connectionString)
        {
        }

        public string GetConnectionString()
        {
            return this.Database.Connection.ConnectionString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        public virtual DbSet<tblApprovedPrintLabel> TblApprovedPrintLabels { get; set; }

        public virtual DbSet<tblConfig> TblConfigs { get; set; }
        public virtual DbSet<tblCoreDataCodeItemSize> TblCoreDataCodeItemSizes { get; set; }
        public virtual DbSet<tblItemMissingInfo> TblItemMissingInfos { get; set; }
        public virtual DbSet<tblLog> TblLogs { get; set; }
        public virtual DbSet<tblScanData> TblScanDatas { get; set; }
        public virtual DbSet<tblSpecialCase> TblSpecialCases { get; set; }
        public virtual DbSet<tblSystemOC> TblSystemOCs { get; set; }
        public virtual DbSet<tblUser> TblUsers { get; set; }
        public virtual DbSet<tblWinlineProductsInfo> TblWinlineProductsInfos { get; set; }
    }
}
