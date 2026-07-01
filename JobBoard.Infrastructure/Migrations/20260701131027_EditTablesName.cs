using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditTablesName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfile_AspNetUsers_ApplicationUserId",
                table: "EmployerProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_SeekerProfile_SeekerProfileId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPosts_EmployerProfile_EmployerProfileId",
                table: "JobPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_SeekerProfile_AspNetUsers_ApplicationUserId",
                table: "SeekerProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeekerProfile",
                table: "SeekerProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployerProfile",
                table: "EmployerProfile");

            migrationBuilder.RenameTable(
                name: "SeekerProfile",
                newName: "SeekerProfiles");

            migrationBuilder.RenameTable(
                name: "EmployerProfile",
                newName: "EmployerProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_SeekerProfile_ApplicationUserId",
                table: "SeekerProfiles",
                newName: "IX_SeekerProfiles_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployerProfile_ApplicationUserId",
                table: "EmployerProfiles",
                newName: "IX_EmployerProfiles_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeekerProfiles",
                table: "SeekerProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployerProfiles",
                table: "EmployerProfiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_AspNetUsers_ApplicationUserId",
                table: "EmployerProfiles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_SeekerProfiles_SeekerProfileId",
                table: "JobApplications",
                column: "SeekerProfileId",
                principalTable: "SeekerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosts_EmployerProfiles_EmployerProfileId",
                table: "JobPosts",
                column: "EmployerProfileId",
                principalTable: "EmployerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeekerProfiles_AspNetUsers_ApplicationUserId",
                table: "SeekerProfiles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_AspNetUsers_ApplicationUserId",
                table: "EmployerProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_SeekerProfiles_SeekerProfileId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPosts_EmployerProfiles_EmployerProfileId",
                table: "JobPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_SeekerProfiles_AspNetUsers_ApplicationUserId",
                table: "SeekerProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeekerProfiles",
                table: "SeekerProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployerProfiles",
                table: "EmployerProfiles");

            migrationBuilder.RenameTable(
                name: "SeekerProfiles",
                newName: "SeekerProfile");

            migrationBuilder.RenameTable(
                name: "EmployerProfiles",
                newName: "EmployerProfile");

            migrationBuilder.RenameIndex(
                name: "IX_SeekerProfiles_ApplicationUserId",
                table: "SeekerProfile",
                newName: "IX_SeekerProfile_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployerProfiles_ApplicationUserId",
                table: "EmployerProfile",
                newName: "IX_EmployerProfile_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeekerProfile",
                table: "SeekerProfile",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployerProfile",
                table: "EmployerProfile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfile_AspNetUsers_ApplicationUserId",
                table: "EmployerProfile",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_SeekerProfile_SeekerProfileId",
                table: "JobApplications",
                column: "SeekerProfileId",
                principalTable: "SeekerProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosts_EmployerProfile_EmployerProfileId",
                table: "JobPosts",
                column: "EmployerProfileId",
                principalTable: "EmployerProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeekerProfile_AspNetUsers_ApplicationUserId",
                table: "SeekerProfile",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
