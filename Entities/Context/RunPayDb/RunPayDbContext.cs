using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Context.RunPayDb
{
    public class RunPayDbContext : DbContext
    {
        public RunPayDbContext(DbContextOptions<RunPayDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) { }

        //Sector Dbset
        #region DbSet

        #endregion
    }
}
