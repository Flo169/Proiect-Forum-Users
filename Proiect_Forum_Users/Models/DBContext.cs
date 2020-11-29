using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Proiect_Forum.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DBConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DBContext, Proiect_Forum.Migrations.Configuration>("DBConnectionString"));
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}