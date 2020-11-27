using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreRelationshipsPractice.Migrations
{
    public partial class AddForeignKeyForEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Profiles_ProfileId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies_CompanyEntityId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CompanyEntityId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ProfileId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CompanyEntityId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Profiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Employees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_CompanyId",
                table: "Profiles",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                table: "Employees",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Companies_CompanyId",
                table: "Profiles",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Companies_CompanyId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_CompanyId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "CompanyEntityId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyEntityId",
                table: "Employees",
                column: "CompanyEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ProfileId",
                table: "Companies",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Profiles_ProfileId",
                table: "Companies",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies_CompanyEntityId",
                table: "Employees",
                column: "CompanyEntityId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
