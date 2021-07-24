namespace WhatToDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReqFix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RecommendedHabits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        UserCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Habits", "RecId", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Habits", "RecId");
            DropTable("dbo.RecommendedHabits");
        }
    }
}
