namespace Music.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                {
                    AlbumID = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    GenreID = c.Int(nullable: false),
                    Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    ArtistID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.AlbumID)
                .ForeignKey("dbo.Artists", t => t.ArtistID, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.GenreID, cascadeDelete: true)
                .Index(t => t.GenreID)
                .Index(t => t.ArtistID);

            CreateTable(
                "dbo.Artists",
                c => new
                {
                    ArtistID = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.ArtistID);

            CreateTable(
                "dbo.Genres",
                c => new
                {
                    GenreID = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.GenreID);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Albums", "GenreID", "dbo.Genres");
            DropForeignKey("dbo.Albums", "ArtistID", "dbo.Artists");
            DropIndex("dbo.Albums", new[] { "ArtistID" });
            DropIndex("dbo.Albums", new[] { "GenreID" });
            DropTable("dbo.Genres");
            DropTable("dbo.Artists");
            DropTable("dbo.Albums");
        }
    }
}