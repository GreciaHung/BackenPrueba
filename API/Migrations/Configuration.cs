namespace API.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<API.Models.APIContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(API.Models.APIContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            var salt = PasswordHasherService.GenerateSalt();

            context.Users.AddOrUpdate(
               p => p.UserName,
               new Core.User
               {
                   EmailAddress = "test@test.com",
                   FirstName = "Admin",
                   LastName = "Test",
                   Hash = PasswordHasherService.HashPassword("test", salt),
                   Salt = salt,
                   UserName = "admintest"
               }
           );

            context.Policies.AddOrUpdate(p => p.Id,
             new Core.Policy
             {
                 Id = 1,
                 CustomerName = "Grecia Hung",
                 Description = "Indernizacion del patrimonio",
                 TypeCover = Core.TypeCover.Earthquake,
                 TypeRisk = Core.TypeRisk.Low,
                 PercentageCoverage = 20,
                 Price = 1000,
                 StartDate = DateTimeOffset.Now,
                 EndDate = DateTimeOffset.Now.AddYears(1)
             },
             new Core.Policy
             {
                 Id = 2,
                 CustomerName = "Osner Sanchez",
                 Description = "N/A",
                 TypeCover = Core.TypeCover.Stole,
                 TypeRisk = Core.TypeRisk.Low,
                 PercentageCoverage = 100,
                 Price = 5000,
                 StartDate = DateTimeOffset.Now,
                 EndDate = DateTimeOffset.Now.AddYears(1)
             },
              new Core.Policy
              {
                  Id = 3,
                  CustomerName = "Juan Guaidó",
                  Description = "Aplica para perdida total",
                  TypeCover = Core.TypeCover.Fire,
                  TypeRisk = Core.TypeRisk.High,
                  PercentageCoverage = 10,
                  Price = 10000,
                  StartDate = DateTimeOffset.Now.AddYears(-1),
                  EndDate = DateTimeOffset.Now.AddDays(-1)
              },
              new Core.Policy
              {
                  Id = 4,
                  CustomerName = "Darwin Ruiz",
                  Description = "Aplica para perdida total",
                  TypeCover = Core.TypeCover.Others,
                  TypeRisk = Core.TypeRisk.Medium,
                  PercentageCoverage = 10,
                  Price = 3000,
                  StartDate = DateTimeOffset.Now,
                  EndDate = DateTimeOffset.Now.AddDays(50)
              },
              new Core.Policy
              {
                  Id = 5,
                  CustomerName = "Ambar Urbaez",
                  Description = "N/A",
                  TypeCover = Core.TypeCover.Earthquake,
                  TypeRisk = Core.TypeRisk.High,
                  PercentageCoverage = 20,
                  Price = 10000,
                  StartDate = DateTimeOffset.Now,
                  EndDate = DateTimeOffset.Now.AddYears(1)
              },
             new Core.Policy
             {
                 Id = 6,
                 CustomerName = "Davis Mejias",
                 Description = "N/A",
                 TypeCover = Core.TypeCover.Stole,
                 TypeRisk = Core.TypeRisk.Low,
                 PercentageCoverage = 100,
                 Price = 5000,
                 StartDate = DateTimeOffset.Now,
                 EndDate = DateTimeOffset.Now.AddYears(1)
             },
              new Core.Policy
              {
                  Id = 7,
                  CustomerName = "Andres Padilla",
                  Description = "N/A",
                  TypeCover = Core.TypeCover.Fire,
                  TypeRisk = Core.TypeRisk.Low,
                  PercentageCoverage = 60,
                  Price = 10000,
                  StartDate = DateTimeOffset.Now.AddYears(-1),
                  EndDate = DateTimeOffset.Now.AddDays(-1)
              },
              new Core.Policy
              {
                  Id = 8,
                  CustomerName = "Ibsen Rios",
                  Description = "Aplica para perdida total",
                  TypeCover = Core.TypeCover.Lost,
                  TypeRisk = Core.TypeRisk.MediumHigh,
                  PercentageCoverage = 35,
                  Price = 9500,
                  StartDate = DateTimeOffset.Now,
                  EndDate = DateTimeOffset.Now.AddDays(100)
              }
         );

        }
    }
}
