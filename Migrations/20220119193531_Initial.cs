using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TokenBasedAuth_NetCore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Temporaries",
                table: "Temporaries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_Temporaries",
                table: "Temporaries",
                column: "Id");
        }
    }
}
