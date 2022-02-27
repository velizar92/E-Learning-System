using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_LearningSystem.Data.Migrations
{
    public partial class AddUserIdToIssueModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Issues");
        }
    }
}
