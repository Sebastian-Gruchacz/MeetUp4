namespace MeetUp.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    using Common;

    using Enumerations;

    using Model;

    internal sealed class Configuration : DbMigrationsConfiguration<MeetUp.Model.MeetupDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MeetUp.Model.MeetupDbContext context)
        {
            // Used Seed only to add core Migrator user to the system, no need to run this check every update. 
            // Stupid, should be per migration.

            // https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/migrations-and-deployment-with-the-entity-framework-in-an-asp-net-mvc-application#set-up-the-seed-method

            context.AspNetUsers.AddOrUpdate(m => m.Id, // used to determine whether entity already exists, without overwriting it. Good if structure changes.
                new AspNetUser
                {
                    Id = SystemUsers.Migrator,
                    FirstName = nameof(SystemUsers.Migrator),
                    LastName = nameof(SystemUsers),
                    Email = $"{nameof(SystemUsers.Migrator).ToLower()}@sysusers.internal",
                    InternalEmail = null, // not possible to email him
                    IsSystemUser = true,
                    LanguageCode = LanguageCode.English,
                    //CreatedBy = SystemUsers.Migrator,
                    //CreatedDateTimeUtc = DateTime.UtcNow
                });
            }
    }
}
