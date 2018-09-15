namespace MeetUp.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    using Common;

    using Enumerations;

    using Model;

    internal sealed class Configuration : DbMigrationsConfiguration<MeetUp.Model.MeetUpDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MeetUp.Model.MeetUpDbContext context)
        {
            // Used Seed only to add core Migrator user to the system, no need to run this check every update. 
            // Stupid, should be per migration.

            // https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/migrations-and-deployment-with-the-entity-framework-in-an-asp-net-mvc-application#set-up-the-seed-method

            CreateSystemUser(context, SystemUsers.Migrator, nameof(SystemUsers.Migrator)); // used to determine whether entity already exists, without overwriting it. Good if structure changes.
            CreateSystemUser(context, SystemUsers.OrderService, nameof(SystemUsers.OrderService));
            CreateSystemUser(context, SystemUsers.EmailingService, nameof(SystemUsers.EmailingService));
            CreateSystemUser(context, SystemUsers.OrderFormService, nameof(SystemUsers.OrderFormService));
            CreateSystemUser(context, SystemUsers.CustomerCaseService, nameof(SystemUsers.CustomerCaseService));
        }

        private static void CreateSystemUser(MeetUpDbContext context, Guid serviceId, string serviceName)
        {
            context.AspNetUsers.AddOrUpdate(
                m => m.Id,
                new AspNetUser
                {
                    Id = serviceId,
                    FirstName = serviceName,
                    LastName = nameof(SystemUsers),
                    Email = $"{serviceName.ToLower()}@sysusers.internal", // must be unique, and better conform to email format
                    InternalEmail = $"#ERR_{serviceName}#", // not possible to email sys-users, yet must be unique, conform or not?
                    IsSystemUser = true,
                    LanguageCode = LanguageCode.English,
                    Tracking = EntityTracker.StartTracking(SystemUsers.Migrator)
                });
        }
    }
}
