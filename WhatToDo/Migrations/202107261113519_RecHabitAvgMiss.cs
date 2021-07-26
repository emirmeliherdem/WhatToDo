namespace WhatToDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecHabitAvgMiss : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecommendedHabits", "AvgMissPercentage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecommendedHabits", "AvgMissPercentage");
        }
    }
}
