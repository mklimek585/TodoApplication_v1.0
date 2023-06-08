using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace TodoApplication_v1._0
{
    public class AppContext : DbContext
    {
        public AppContext()
            : base("name=AppContext")
        {
                this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Day> Days { get; set; }
    }
}