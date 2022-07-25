using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectCWeb_DuboisAnke_2ProA.Migrations
{
    public partial class identity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tempRoleName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoginViewModel",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginViewModel", x => x.Email);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginViewModel");

            migrationBuilder.DropColumn(
                name: "tempRoleName",
                table: "AspNetUsers");
        }
    }
}
