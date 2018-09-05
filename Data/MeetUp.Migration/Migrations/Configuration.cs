namespace MeetUp.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    using Common;

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

            context.AspNetUsers.AddOrUpdate(new AspNetUser
            {
                Id = SystemUsers.Migrator,
                FirstName = nameof(SystemUsers.Migrator),
                LastName = nameof(SystemUsers),
                Email = $"{nameof(SystemUsers.Migrator).ToLower()}@sysusers.internal",
                InternalEmail = null, // not possible to email him
                IsSystemUser = true,
                LanguageCode = "en-US",
                CreatedBy = SystemUsers.Migrator,
                CreatedDateTimeUtc = DateTime.UtcNow
            });
        }
    }
}
