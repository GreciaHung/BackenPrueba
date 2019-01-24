namespace API.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Policies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        TypeCover = c.Int(nullable: false),
                        TypeRisk = c.Int(nullable: false),
                        PercentageCoverage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserName = c.String(),
                        EmailAddress = c.String(nullable: false),
                        Hash = c.String(),
                        Salt = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_User_Deleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_User_Deleted", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Policies");
        }
    }
}
