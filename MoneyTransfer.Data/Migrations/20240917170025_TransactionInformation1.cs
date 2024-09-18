using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyTransfer.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransactionInformation1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Transaction_Information",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Transaction_Information",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Information_AppUserId",
                table: "Transaction_Information",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Information_Users_AppUserId",
                table: "Transaction_Information",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Information_Users_AppUserId",
                table: "Transaction_Information");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_Information_AppUserId",
                table: "Transaction_Information");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Transaction_Information");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Transaction_Information");
        }
    }
}
