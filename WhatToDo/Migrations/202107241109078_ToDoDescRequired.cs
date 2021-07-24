namespace WhatToDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ToDoDescRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Habits", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Habits", "Description", c => c.String());
        }
    }
}
