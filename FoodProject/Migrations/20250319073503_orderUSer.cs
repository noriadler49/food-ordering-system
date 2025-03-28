using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodProject.Migrations
{
    /// <inheritdoc />
    public partial class orderUSer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AccountId",
                table: "Orders",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Accounts_AccountId",
                table: "Orders",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Accounts_AccountId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AccountId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");
        }
    }
}
