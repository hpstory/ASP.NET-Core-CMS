using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Position = table.Column<int>(nullable: true),
                    PublishTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    Title = table.Column<string>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Cover = table.Column<string>(nullable: true),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false),
                    IsHot = table.Column<bool>(nullable: false),
                    LikeCount = table.Column<int>(nullable: false),
                    DislikeCount = table.Column<int>(nullable: false),
                    CollectCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: false),
                    PublishTime = table.Column<DateTime>(nullable: false),
                    ArticlesID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticlesID",
                        column: x => x.ArticlesID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryID",
                table: "Articles",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticlesID",
                table: "Comments",
                column: "ArticlesID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
