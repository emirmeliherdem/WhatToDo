namespace WhatToDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ToDoDescReqFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ToDoes", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ToDoes", "Description", c => c.String());
        }
    }
}
