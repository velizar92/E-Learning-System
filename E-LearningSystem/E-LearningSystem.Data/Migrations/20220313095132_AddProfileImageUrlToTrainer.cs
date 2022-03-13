using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_LearningSystem.Data.Migrations
{
    public partial class AddProfileImageUrlToTrainer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "Trainers");
        }
    }
}
