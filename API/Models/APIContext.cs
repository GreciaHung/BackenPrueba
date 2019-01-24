
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using API.Core;
using EntityFramework.DynamicFilters;
using Api.Core.Helper;

namespace API.Models
{
    public class APIContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Policy> Policies { get; set; }

        private void AddMyFilters(ref DbModelBuilder modelBuilder)
        {
            modelBuilder.Filter("Deleted", (ISoftDeleted d) => d.Deleted, false);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<APIContext>(null);

            AddMyFilters(ref modelBuilder);

            base.OnModelCreating(modelBuilder);

        }

    }

}







