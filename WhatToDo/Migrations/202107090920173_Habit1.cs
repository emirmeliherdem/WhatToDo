namespace WhatToDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Habit1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Habits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        IsDone = c.Boolean(nullable: false),
                        AimedDayCount = c.Int(nullable: false),
                        MissedDayCount = c.Int(nullable: false),
                        CompDayCount = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Habits", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Habits", new[] { "User_Id" });
            DropTable("dbo.Habits");
        }
    }
}
