using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Simple_API_Assessment.Migrations
{
  public partial class InitialCreate : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Applicants",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Applicants", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Skills",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: false),
            ApplicantId = table.Column<int>(type: "integer", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Skills", x => x.Id);
            table.ForeignKey(
                      name: "FK_Skills_Applicants_ApplicantId",
                      column: x => x.ApplicantId,
                      principalTable: "Applicants",
                      principalColumn: "Id");
          });

      migrationBuilder.CreateIndex(
          name: "IX_Skills_ApplicantId",
          table: "Skills",
          column: "ApplicantId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Skills");

      migrationBuilder.DropTable(
          name: "Applicants");
    }
  }
}
