using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Admin.NETCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuType = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, comment: "菜单类型(catalog / menu / button)")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "菜单名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true, comment: "父类菜单id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentTitle = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, comment: "父类菜单名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "菜单编码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, comment: "菜单图标")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hidden = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Component = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "路由")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "DATETIME(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    UpdateDate = table.Column<DateTime>(type: "DATETIME(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CreateBy = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateBy = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDelete = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menu");
        }
    }
}
