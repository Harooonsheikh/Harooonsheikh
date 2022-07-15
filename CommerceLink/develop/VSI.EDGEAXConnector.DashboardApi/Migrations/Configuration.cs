namespace VSI.EDGEAXConnector.DashboardApi.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VSI.EDGEAXConnector.DashboardApi.Infrastructure;

    internal sealed class Configuration : DbMigrationsConfiguration<VSI.EDGEAXConnector.DashboardApi.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VSI.EDGEAXConnector.DashboardApi.Infrastructure.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var integrationStore = context.Stores.Where(m => m.Name.ToLower().Equals("Contoso")).FirstOrDefault();
            if (integrationStore == null)
            {
                ApplicationStore store = new ApplicationStore();
                store.Name = "Contoso";
                store.DBServer = "localhost";
                store.IsActive = true;
                store.MongoConnection = "mongodb://localhost:27017";
                store.MongoDbName = store.Name.ToLower();
                store.CreatedAt = System.DateTime.UtcNow;
                store.DBName = "VSIEdgeAXCommerceLink";
                store.APIKey = "E550E995-1D34-4E65-9222-FA4C15712ADA";
                store.APIURL = "http://localhost:6321/api/v1/";
                store.Enabled = true;
                

                StoreAction channelPublish = new StoreAction();
                channelPublish.ActionName = "Channel Publish";
                channelPublish.ActionRoute = "Channel/Publish";
                channelPublish.RequestType = "GET";
                channelPublish.Store = store;

                StoreAction getCustomer = new StoreAction();
                getCustomer.ActionName = "Get Customer";
                getCustomer.ActionRoute = "Customer/GetCustomer";
                getCustomer.RequestType = "POST";
                getCustomer.Store = store;

                ActionParam param = new ActionParam();
                param.Key = "customerRequest";
                param.Value = "{\"Status\": \"string\",\"CustomerInfo\": { },\"Message\": \"string\"}";
                param.Action = getCustomer;

                getCustomer.ActionParams = new List<ActionParam>();
                getCustomer.ActionParams.Add(param);

                store.Actions = new List<StoreAction>();
                store.Actions.Add(channelPublish);
                store.Actions.Add(getCustomer);

                context.ActionParams.Add(param);
                context.StoreActions.Add(channelPublish);
                context.StoreActions.Add(getCustomer);
                context.Stores.Add(store);

                context.SaveChanges();
            }
            var integrationStore2 = context.Stores.Where(m => m.Name.ToLower().Equals("Fabrikam")).FirstOrDefault();
            if (integrationStore2 == null)
            {
                ApplicationStore store = new ApplicationStore();
                store.Name = "Fabrikam";
                store.DBServer = "localhost";
                store.IsActive = true;
                store.MongoConnection = "mongodb://localhost:27017";
                store.MongoDbName = store.Name.ToLower();
                store.CreatedAt = System.DateTime.UtcNow;
                store.DBName = "VSIEdgeAXCommerceLinkStore2";
                store.APIKey = "E550E995-1D34-4E65-9222-FA4C15712ADA";
                store.APIURL = "http://localhost:6322/api/v1/";
                store.Enabled = true;
                context.Stores.Add(store);
                context.SaveChanges();


                StoreAction channelPublish = new StoreAction();
                channelPublish.ActionName = "Channel Publish";
                channelPublish.ActionRoute = "Channel/Publish";
                channelPublish.RequestType = "GET";
                channelPublish.Store = store;

                StoreAction getCustomer = new StoreAction();
                getCustomer.ActionName = "Get Customer";
                getCustomer.ActionRoute = "Customer/GetCustomer";
                getCustomer.RequestType = "POST";
                getCustomer.Store = store;

                ActionParam param = new ActionParam();
                param.Key = "customerRequest";
                param.Value = "{\"Status\": \"string\",\"CustomerInfo\": { },\"Message\": \"string\"}";
                param.Action = getCustomer;

                getCustomer.ActionParams = new List<ActionParam>();
                getCustomer.ActionParams.Add(param);

                store.Actions = new List<StoreAction>();
                store.Actions.Add(channelPublish);
                store.Actions.Add(getCustomer);

                context.ActionParams.Add(param);
                context.StoreActions.Add(channelPublish);
                context.StoreActions.Add(getCustomer);
                context.Stores.Add(store);

                context.SaveChanges();


            }
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }


            var shanUser = manager.FindByName("shan");
            if (shanUser == null)
            {
                shanUser = new ApplicationUser()
                {
                    UserName = "shan",
                    Email = "shan.ali@systemsltd.com",
                    EmailConfirmed = true,
                    FirstName = "Shan",
                    LastName = "Khan",
                    //StoreId = 1
                };
                manager.Create(shanUser, "ShanAli!");
                manager.AddToRoles(shanUser.Id, new string[] { "SuperAdmin" });
            }


            var qaUser = manager.FindByName("qauser");
            if (qaUser == null)
            {
                qaUser = new ApplicationUser()
                {
                    UserName = "qauser",
                    Email = "asim.noaman@systemsltd.com",
                    EmailConfirmed = true,
                    FirstName = "Asim",
                    LastName = "Noaman",
                    //StoreId = 1
                };
                manager.Create(qaUser, "QaUser!");
                manager.AddToRoles(qaUser.Id, new string[] { "SuperAdmin" });
            }

            var demoUser = manager.FindByName("demo");
            if (demoUser == null)
            {
                demoUser = new ApplicationUser()
                {
                    UserName = "demo",
                    Email = "babar.fraz@systemsltd.com",
                    EmailConfirmed = true,
                    FirstName = "Demo",
                    LastName = "User",
                    //StoreId = 1
                };
                manager.Create(demoUser, "Demo123");
                manager.AddToRoles(demoUser.Id, new string[] { "SuperAdmin" });
            }
        }
    }
}
