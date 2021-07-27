namespace WhatToDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HabitHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Habits", "History", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Habits", "History");
        }
    }
}
