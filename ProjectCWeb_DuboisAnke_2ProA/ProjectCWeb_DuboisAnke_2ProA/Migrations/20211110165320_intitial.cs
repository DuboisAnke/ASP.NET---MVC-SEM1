using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectCWeb_DuboisAnke_2ProA.Migrations
{
    public partial class intitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademieJaar",
                columns: table => new
                {
                    AcademieJaarId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDatum = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademieJaar", x => x.AcademieJaarId);
                });

            migrationBuilder.CreateTable(
                name: "Gebruikers",
                columns: table => new
                {
                    GebruikerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(nullable: false),
                    Voornaam = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gebruikers", x => x.GebruikerId);
                });

            migrationBuilder.CreateTable(
                name: "Handboeken",
                columns: table => new
                {
                    HandboekId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(nullable: false),
                    Kostprijs = table.Column<double>(nullable: false),
                    UitgifteDatum = table.Column<DateTime>(nullable: false),
                    Afbeelding = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Handboeken", x => x.HandboekId);
                });

            migrationBuilder.CreateTable(
                name: "Lectors",
                columns: table => new
                {
                    LectorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GebruikerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectors", x => x.LectorId);
                    table.ForeignKey(
                        name: "FK_Lectors_Gebruikers_GebruikerId",
                        column: x => x.GebruikerId,
                        principalTable: "Gebruikers",
                        principalColumn: "GebruikerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Studenten",
                columns: table => new
                {
                    StudentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GebruikerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenten", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Studenten_Gebruikers_GebruikerId",
                        column: x => x.GebruikerId,
                        principalTable: "Gebruikers",
                        principalColumn: "GebruikerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vakken",
                columns: table => new
                {
                    VakId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VakNaam = table.Column<string>(nullable: false),
                    StudiePunten = table.Column<int>(nullable: false),
                    HandBoekId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vakken", x => x.VakId);
                    table.ForeignKey(
                        name: "FK_Vakken_Handboeken_HandBoekId",
                        column: x => x.HandBoekId,
                        principalTable: "Handboeken",
                        principalColumn: "HandboekId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VakLectoren",
                columns: table => new
                {
                    VakLectorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LectorId = table.Column<int>(nullable: true),
                    VakId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VakLectoren", x => x.VakLectorId);
                    table.ForeignKey(
                        name: "FK_VakLectoren_Lectors_LectorId",
                        column: x => x.LectorId,
                        principalTable: "Lectors",
                        principalColumn: "LectorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VakLectoren_Vakken_VakId",
                        column: x => x.VakId,
                        principalTable: "Vakken",
                        principalColumn: "VakId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inschrijvingen",
                columns: table => new
                {
                    InschrijvingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(nullable: false),
                    AcademieJaarID = table.Column<int>(nullable: false),
                    VakLectorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inschrijvingen", x => x.InschrijvingId);
                    table.ForeignKey(
                        name: "FK_Inschrijvingen_AcademieJaar_AcademieJaarID",
                        column: x => x.AcademieJaarID,
                        principalTable: "AcademieJaar",
                        principalColumn: "AcademieJaarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inschrijvingen_Studenten_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Studenten",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inschrijvingen_VakLectoren_VakLectorId",
                        column: x => x.VakLectorId,
                        principalTable: "VakLectoren",
                        principalColumn: "VakLectorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inschrijvingen_AcademieJaarID",
                table: "Inschrijvingen",
                column: "AcademieJaarID");

            migrationBuilder.CreateIndex(
                name: "IX_Inschrijvingen_StudentID",
                table: "Inschrijvingen",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Inschrijvingen_VakLectorId",
                table: "Inschrijvingen",
                column: "VakLectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectors_GebruikerId",
                table: "Lectors",
                column: "GebruikerId");

            migrationBuilder.CreateIndex(
                name: "IX_Studenten_GebruikerId",
                table: "Studenten",
                column: "GebruikerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vakken_HandBoekId",
                table: "Vakken",
                column: "HandBoekId");

            migrationBuilder.CreateIndex(
                name: "IX_VakLectoren_LectorId",
                table: "VakLectoren",
                column: "LectorId");

            migrationBuilder.CreateIndex(
                name: "IX_VakLectoren_VakId",
                table: "VakLectoren",
                column: "VakId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inschrijvingen");

            migrationBuilder.DropTable(
                name: "AcademieJaar");

            migrationBuilder.DropTable(
                name: "Studenten");

            migrationBuilder.DropTable(
                name: "VakLectoren");

            migrationBuilder.DropTable(
                name: "Lectors");

            migrationBuilder.DropTable(
                name: "Vakken");

            migrationBuilder.DropTable(
                name: "Gebruikers");

            migrationBuilder.DropTable(
                name: "Handboeken");
        }
    }
}
