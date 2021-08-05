using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class BillingAddressOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BillingAddresses_BillingAddressId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "BillingAddressId",
                table: "AspNetUsers",
                newName: "BillingAddressModel");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_BillingAddressId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_BillingAddressModel");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "BillingAddresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BillingAddresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillingAddresses_User",
                table: "BillingAddresses",
                column: "User");

            migrationBuilder.CreateIndex(
                name: "IX_BillingAddresses_UserId",
                table: "BillingAddresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BillingAddresses_BillingAddressModel",
                table: "AspNetUsers",
                column: "BillingAddressModel",
                principalTable: "BillingAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BillingAddresses_AspNetUsers_User",
                table: "BillingAddresses",
                column: "User",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BillingAddresses_AspNetUsers_UserId",
                table: "BillingAddresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BillingAddresses_BillingAddressModel",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BillingAddresses_AspNetUsers_User",
                table: "BillingAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_BillingAddresses_AspNetUsers_UserId",
                table: "BillingAddresses");

            migrationBuilder.DropIndex(
                name: "IX_BillingAddresses_User",
                table: "BillingAddresses");

            migrationBuilder.DropIndex(
                name: "IX_BillingAddresses_UserId",
                table: "BillingAddresses");

            migrationBuilder.DropColumn(
                name: "User",
                table: "BillingAddresses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BillingAddresses");

            migrationBuilder.RenameColumn(
                name: "BillingAddressModel",
                table: "AspNetUsers",
                newName: "BillingAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_BillingAddressModel",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_BillingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BillingAddresses_BillingAddressId",
                table: "AspNetUsers",
                column: "BillingAddressId",
                principalTable: "BillingAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
