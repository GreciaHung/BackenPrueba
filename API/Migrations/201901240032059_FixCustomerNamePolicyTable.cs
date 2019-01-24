namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCustomerNamePolicyTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Policies", "CustomerName", c => c.String());
            DropColumn("dbo.Policies", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Policies", "Name", c => c.String());
            DropColumn("dbo.Policies", "CustomerName");
        }
    }
}
