using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace WebDoAn2.Models
{
    class DataContext : DbContext
    {
        public DataContext() : base()
        {
            string databasename = "BookRoofLibraryContext";
            string datasource = "GOLAPTOP\\SQLEXPRESS";
            this.Database.Connection.ConnectionString = "Data Source=" + datasource + ";Initial Catalog=" + databasename + ";Trusted_Connection=Yes";
            Database.SetInitializer<DataContext>(new DropCreateDatabaseIfModelChanges<DataContext>());
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Caterogy> Caterogies { get; set; }
        public DbSet<Book_Caterogy> Book_Caterogies { get; set; }
    }
}