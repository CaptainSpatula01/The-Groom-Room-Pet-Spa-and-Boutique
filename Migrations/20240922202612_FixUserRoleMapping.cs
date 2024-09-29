using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace groomroom.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRoleMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRoleRoleId",
                table: "AspNetUserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserRoleUserId",
                table: "AspNetUserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserRoleUserId_UserRoleRoleId",
                table: "AspNetUserRoles",
                columns: new[] { "UserRoleUserId", "UserRoleRoleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUserRoles_UserRoleUserId_UserRoleRoleId",
                table: "AspNetUserRoles",
                columns: new[] { "UserRoleUserId", "UserRoleRoleId" },
                principalTable: "AspNetUserRoles",
                principalColumns: new[] { "UserId", "RoleId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUserRoles_UserRoleUserId_UserRoleRoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserRoleUserId_UserRoleRoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "UserRoleRoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "UserRoleUserId",
                table: "AspNetUserRoles");
        }
    }
}
