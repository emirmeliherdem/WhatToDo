namespace WhatToDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecHabitAimedDayCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecommendedHabits", "AimedDayCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecommendedHabits", "AimedDayCount");
        }
    }
}
