using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_LearningSystem.Data.Migrations
{
    public partial class AddRatingToTrainer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Trainers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Trainers");
        }
    }
}
