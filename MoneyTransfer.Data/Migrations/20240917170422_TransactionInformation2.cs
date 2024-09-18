using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyTransfer.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransactionInformation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Transaction_Information",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Information_UserId",
                table: "Transaction_Information",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Information_Users_UserId",
                table: "Transaction_Information",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Information_Users_UserId",
                table: "Transaction_Information");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_Information_UserId",
                table: "Transaction_Information");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Transaction_Information",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Transaction_Information",
                type: "nvarchar(450)",
                nullable: true);

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
    }
}
