using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blogger.Context.Migrations
{
    public partial class InitCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Visible = table.Column<bool>(nullable: false),
                    InsertedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    EditedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Name = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Visible = table.Column<bool>(nullable: false),
                    InsertedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    EditedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    BlogParentID = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    PublishFrom = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    BlogId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true),
                    TagId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Visible = table.Column<bool>(nullable: false),
                    InsertedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    EditedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Name = table.Column<string>(nullable: true),
                    PostId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Visible = table.Column<bool>(nullable: false),
                    InsertedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    EditedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Name = table.Column<string>(nullable: true),
                    PostId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "PostId", "Visible" },
                values: new object[,]
                {
                    { 1, "C#", null, true },
                    { 2, "SQL", null, true },
                    { 3, "Entity Framework", null, true },
                    { 4, "MVC", null, true },
                    { 5, "SignalR", null, true },
                    { 6, "HideMe", null, false }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name", "PostId", "Visible" },
                values: new object[,]
                {
                    { 1, "MVC", null, true },
                    { 2, "C#", null, true },
                    { 3, "EF", null, true },
                    { 4, "CSharp", null, true },
                    { 5, "SQL", null, true },
                    { 6, "SQL Managements", null, true },
                    { 7, "Signalr", null, true },
                    { 8, "HideMe", null, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_PostId",
                table: "Categories",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogId",
                table: "Posts",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TagId",
                table: "Posts",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_PostId",
                table: "Tags",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Categories_CategoryId",
                table: "Posts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Tags_TagId",
                table: "Posts",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Posts_PostId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
