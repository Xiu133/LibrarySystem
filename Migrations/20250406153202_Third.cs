using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRecords_users_UserName1",
                table: "BorrowRecords");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.RenameColumn(
                name: "UserName1",
                table: "BorrowRecords",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BorrowRecords_UserName1",
                table: "BorrowRecords",
                newName: "IX_BorrowRecords_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "fines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FineAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fines", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRecords_AspNetUsers_UserId",
                table: "BorrowRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRecords_AspNetUsers_UserId",
                table: "BorrowRecords");

            migrationBuilder.DropTable(
                name: "fines");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BorrowRecords",
                newName: "UserName1");

            migrationBuilder.RenameIndex(
                name: "IX_BorrowRecords_UserId",
                table: "BorrowRecords",
                newName: "IX_BorrowRecords_UserName1");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserName);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRecords_users_UserName1",
                table: "BorrowRecords",
                column: "UserName1",
                principalTable: "users",
                principalColumn: "UserName");
        }
    }
}
