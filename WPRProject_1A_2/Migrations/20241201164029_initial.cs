using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPRProject_1A_2.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abonnementen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Maxvoertuigen = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxMedewerkers = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnementen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Adressen",
                columns: table => new
                {
                    AdresId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Straatnaam = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Huisnummer = table.Column<int>(type: "INTEGER", nullable: false),
                    Postcode = table.Column<string>(type: "TEXT", maxLength: 6, nullable: false),
                    Woonplaats = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Gemeente = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Provincie = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adressen", x => x.AdresId);
                });

            migrationBuilder.CreateTable(
                name: "Betalingen",
                columns: table => new
                {
                    BetalingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Bedrag = table.Column<double>(type: "REAL", nullable: false),
                    IsBetaald = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Betalingen", x => x.BetalingId);
                });

            migrationBuilder.CreateTable(
                name: "Voertuigen",
                columns: table => new
                {
                    VoertuigId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Kenteken = table.Column<string>(type: "TEXT", nullable: false),
                    Merk = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Kleur = table.Column<string>(type: "TEXT", nullable: false),
                    Aanschafjaar = table.Column<int>(type: "INTEGER", nullable: false),
                    Voertuigstatus = table.Column<int>(type: "INTEGER", nullable: false),
                    ReserveringId = table.Column<int>(type: "INTEGER", nullable: false),
                    SchadeclaimId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voertuigen", x => x.VoertuigId);
                });

            migrationBuilder.CreateTable(
                name: "Bedrijf",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Bedrijfsnaam = table.Column<string>(type: "TEXT", nullable: false),
                    Domeinnaam = table.Column<string>(type: "TEXT", nullable: false),
                    AdresId = table.Column<int>(type: "INTEGER", nullable: false),
                    KvkNummer = table.Column<int>(type: "INTEGER", nullable: false),
                    AbonnementId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bedrijf", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bedrijf_Abonnementen_AbonnementId",
                        column: x => x.AbonnementId,
                        principalTable: "Abonnementen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bedrijf_Adressen_AdresId",
                        column: x => x.AdresId,
                        principalTable: "Adressen",
                        principalColumn: "AdresId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schadeclaim",
                columns: table => new
                {
                    SchadeclaimId = table.Column<int>(type: "INTEGER", nullable: false),
                    Beschrijving = table.Column<string>(type: "TEXT", nullable: true),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VoertuigId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReparatieId = table.Column<int>(type: "INTEGER", nullable: false),
                    SchadeclaimStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schadeclaim", x => x.SchadeclaimId);
                    table.ForeignKey(
                        name: "FK_Schadeclaim_Voertuigen_SchadeclaimId",
                        column: x => x.SchadeclaimId,
                        principalTable: "Voertuigen",
                        principalColumn: "VoertuigId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schadeclaim_Voertuigen_VoertuigId",
                        column: x => x.VoertuigId,
                        principalTable: "Voertuigen",
                        principalColumn: "VoertuigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Wachtwoord = table.Column<string>(type: "TEXT", nullable: false),
                    BetaalgeschiedenisId = table.Column<int>(type: "INTEGER", nullable: false),
                    BedrijfId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Bedrijf_BedrijfId",
                        column: x => x.BedrijfId,
                        principalTable: "Bedrijf",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reparatie",
                columns: table => new
                {
                    ReparatieId = table.Column<int>(type: "INTEGER", nullable: false),
                    Beschrijving = table.Column<string>(type: "TEXT", nullable: true),
                    Reparatiedatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Opmerkingen = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reparatie", x => x.ReparatieId);
                    table.ForeignKey(
                        name: "FK_Reparatie_Schadeclaim_ReparatieId",
                        column: x => x.ReparatieId,
                        principalTable: "Schadeclaim",
                        principalColumn: "SchadeclaimId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reserveringen",
                columns: table => new
                {
                    ReserveringId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Begindatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Einddatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AardVanReis = table.Column<string>(type: "TEXT", nullable: false),
                    VersteBestemming = table.Column<string>(type: "TEXT", nullable: false),
                    VerwachteHoeveelheidKm = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    RijbewijsDocumentnummer = table.Column<long>(type: "INTEGER", nullable: false),
                    Totaalprijs = table.Column<double>(type: "REAL", nullable: false),
                    Huuraanvraag = table.Column<int>(type: "INTEGER", nullable: false),
                    IsBetaald = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserveringen", x => x.ReserveringId);
                    table.ForeignKey(
                        name: "FK_Reserveringen_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factuur",
                columns: table => new
                {
                    FactuurId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReserveringId = table.Column<int>(type: "INTEGER", nullable: false),
                    Prijs = table.Column<double>(type: "REAL", nullable: false),
                    BedrijfId = table.Column<int>(type: "INTEGER", nullable: false),
                    BetaalgeschiedenisId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factuur", x => x.FactuurId);
                    table.ForeignKey(
                        name: "FK_Factuur_Accounts_BetaalgeschiedenisId",
                        column: x => x.BetaalgeschiedenisId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Factuur_Bedrijf_BedrijfId",
                        column: x => x.BedrijfId,
                        principalTable: "Bedrijf",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factuur_Reserveringen_ReserveringId",
                        column: x => x.ReserveringId,
                        principalTable: "Reserveringen",
                        principalColumn: "ReserveringId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReserveringVoertuig",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReserveringId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReserveringVoertuig", x => new { x.AccountId, x.ReserveringId });
                    table.ForeignKey(
                        name: "FK_ReserveringVoertuig_Reserveringen_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Reserveringen",
                        principalColumn: "ReserveringId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReserveringVoertuig_Voertuigen_ReserveringId",
                        column: x => x.ReserveringId,
                        principalTable: "Voertuigen",
                        principalColumn: "VoertuigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BedrijfId",
                table: "Accounts",
                column: "BedrijfId");

            migrationBuilder.CreateIndex(
                name: "IX_Bedrijf_AbonnementId",
                table: "Bedrijf",
                column: "AbonnementId");

            migrationBuilder.CreateIndex(
                name: "IX_Bedrijf_AdresId",
                table: "Bedrijf",
                column: "AdresId");

            migrationBuilder.CreateIndex(
                name: "IX_Factuur_BedrijfId",
                table: "Factuur",
                column: "BedrijfId");

            migrationBuilder.CreateIndex(
                name: "IX_Factuur_BetaalgeschiedenisId",
                table: "Factuur",
                column: "BetaalgeschiedenisId");

            migrationBuilder.CreateIndex(
                name: "IX_Factuur_ReserveringId",
                table: "Factuur",
                column: "ReserveringId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserveringen_AccountId",
                table: "Reserveringen",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ReserveringVoertuig_ReserveringId",
                table: "ReserveringVoertuig",
                column: "ReserveringId");

            migrationBuilder.CreateIndex(
                name: "IX_Schadeclaim_VoertuigId",
                table: "Schadeclaim",
                column: "VoertuigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Betalingen");

            migrationBuilder.DropTable(
                name: "Factuur");

            migrationBuilder.DropTable(
                name: "Reparatie");

            migrationBuilder.DropTable(
                name: "ReserveringVoertuig");

            migrationBuilder.DropTable(
                name: "Schadeclaim");

            migrationBuilder.DropTable(
                name: "Reserveringen");

            migrationBuilder.DropTable(
                name: "Voertuigen");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Bedrijf");

            migrationBuilder.DropTable(
                name: "Abonnementen");

            migrationBuilder.DropTable(
                name: "Adressen");
        }
    }
}
