using Microsoft.EntityFrameworkCore.Migrations;

namespace DevArena.IdentityServer4.Migrations
{
    public partial class dausers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Provider",
                table: "Users");
        }
    }
}
