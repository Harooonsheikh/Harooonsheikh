using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace VSI.EDGEAXConnector.DashboardApi.Infrastructure
{
    /// <summary>
    /// Database context class responsible to communicate with database
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<ApplicationStore> Stores { get; set; }
        public DbSet<StoreAction> StoreActions { get; set; }
        public DbSet<ActionParam> ActionParams { get; set; }


        /// <summary>
        /// Default constructor takes the connection string name “DefaultConnection” as an argument, 
        /// this connection string will be used point to the right server and database name to connect to.
        /// </summary>
        /// 
        public ApplicationDbContext()
           
            : base("name=AspNetIdentity", throwIfV1Schema: false)
        // : base("Data Source=localhost;Initial Catalog=CLStores;integrated security=True", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;

        }

        /// <summary>
        /// Method to create Application Db context.
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {

            return new ApplicationDbContext();

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationStore>().HasKey(m => m.StoreId);
            modelBuilder.Entity<ApplicationStore>().Property(t => t.StoreId).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<ApplicationStore>().Property(m => m.Name).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            modelBuilder.Entity<StoreAction>().HasKey(m => m.ActionId);
            modelBuilder.Entity<StoreAction>().Property(m => m.ActionId).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ActionParam>().HasKey(m => m.ParamId);
            modelBuilder.Entity<ActionParam>().Property(m => m.ParamId).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            // configures one-to-many relationship
            modelBuilder.Entity<ApplicationStore>().HasMany(a => a.Actions).WithRequired(s => s.Store).HasForeignKey(s => s.StoreId);
            modelBuilder.Entity<StoreAction>().HasMany(m => m.ActionParams).WithRequired(m=> m.Action).HasForeignKey(m => m.ActionId);
            //modelBuilder.Entity<ApplicationStore>().HasMany(a => a.ApplicationUsers).WithRequired(s => s.Store).HasForeignKey(s => s.StoreId);

            base.OnModelCreating(modelBuilder);
        }

    }
}