using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusEats.Migrations
{
    /// <inheritdoc />
    public partial class CampusEatsNotificationsPreferencesAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacije_Korisnici_KorisnikId",
                table: "Rezervacije");

            migrationBuilder.AddColumn<int>(
                name: "KurirId",
                table: "Rezervacije",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alergije",
                table: "Korisnici",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OmiljenaHrana",
                table: "Korisnici",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Vegetarijanac",
                table: "Korisnici",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Aktivnosti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikEmail = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Akcija = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Entitet = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    EntitetId = table.Column<int>(type: "int", nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: false),
                    Vrijeme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aktivnosti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistorijaIzmjena",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Entitet = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    EntitetId = table.Column<int>(type: "int", nullable: false),
                    TipIzmjene = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    KorisnikEmail = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: false),
                    Vrijeme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorijaIzmjena", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Obavijesti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<int>(type: "int", nullable: true),
                    RezervacijaId = table.Column<int>(type: "int", nullable: true),
                    Naslov = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Poruka = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    DatumSlanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Procitana = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obavijesti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Obavijesti_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Obavijesti_Rezervacije_RezervacijaId",
                        column: x => x.RezervacijaId,
                        principalTable: "Rezervacije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_KurirId",
                table: "Rezervacije",
                column: "KurirId");

            migrationBuilder.CreateIndex(
                name: "IX_Obavijesti_KorisnikId",
                table: "Obavijesti",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Obavijesti_RezervacijaId",
                table: "Obavijesti",
                column: "RezervacijaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacije_Korisnici_KorisnikId",
                table: "Rezervacije",
                column: "KorisnikId",
                principalTable: "Korisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacije_Korisnici_KurirId",
                table: "Rezervacije",
                column: "KurirId",
                principalTable: "Korisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacije_Korisnici_KorisnikId",
                table: "Rezervacije");

            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacije_Korisnici_KurirId",
                table: "Rezervacije");

            migrationBuilder.DropTable(
                name: "Aktivnosti");

            migrationBuilder.DropTable(
                name: "HistorijaIzmjena");

            migrationBuilder.DropTable(
                name: "Obavijesti");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacije_KurirId",
                table: "Rezervacije");

            migrationBuilder.DropColumn(
                name: "KurirId",
                table: "Rezervacije");

            migrationBuilder.DropColumn(
                name: "Alergije",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "OmiljenaHrana",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "Vegetarijanac",
                table: "Korisnici");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacije_Korisnici_KorisnikId",
                table: "Rezervacije",
                column: "KorisnikId",
                principalTable: "Korisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
