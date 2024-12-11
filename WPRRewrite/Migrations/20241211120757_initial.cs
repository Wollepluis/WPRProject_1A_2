using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPRRewrite.Migrations
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
                    AbonnementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxVoertuigen = table.Column<int>(type: "int", nullable: false),
                    MaxMedewerkers = table.Column<int>(type: "int", nullable: false),
                    AbonnementType = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnementen", x => x.AbonnementId);
                });

            migrationBuilder.CreateTable(
                name: "Adressen",
                columns: table => new
                {
                    AdresId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Straatnaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Huisnummer = table.Column<int>(type: "int", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Woonplaats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gemeente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Provincie = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adressen", x => x.AdresId);
                });

            migrationBuilder.CreateTable(
                name: "Reparaties",
                columns: table => new
                {
                    ReparatieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Beschrijving = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReparatieDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Opmerkingen = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reparaties", x => x.ReparatieId);
                });

            migrationBuilder.CreateTable(
                name: "Voertuigen",
                columns: table => new
                {
                    VoertuigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kenteken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Merk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kleur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aanschafjaar = table.Column<int>(type: "int", nullable: false),
                    Prijs = table.Column<int>(type: "int", nullable: false),
                    VoertuigStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoertuigType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voertuigen", x => x.VoertuigId);
                });

            migrationBuilder.CreateTable(
                name: "Bedrijven",
                columns: table => new
                {
                    BedrijfId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bedrijfsnaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BedrijfAdres = table.Column<int>(type: "int", nullable: false),
                    Domeinnaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KvkNummer = table.Column<int>(type: "int", nullable: false),
                    AbonnementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bedrijven", x => x.BedrijfId);
                    table.ForeignKey(
                        name: "FK_Bedrijven_Abonnementen_AbonnementId",
                        column: x => x.AbonnementId,
                        principalTable: "Abonnementen",
                        principalColumn: "AbonnementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefoonnummer = table.Column<int>(type: "int", nullable: true),
                    AdresId = table.Column<int>(type: "int", nullable: true),
                    BedrijfId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_Adressen_AdresId",
                        column: x => x.AdresId,
                        principalTable: "Adressen",
                        principalColumn: "AdresId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_Bedrijven_BedrijfId",
                        column: x => x.BedrijfId,
                        principalTable: "Bedrijven",
                        principalColumn: "BedrijfId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reserveringen",
                columns: table => new
                {
                    ReserveringId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Begindatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Einddatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AardVanReis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersteBestemming = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerwachteHoeveelheidkm = table.Column<int>(type: "int", nullable: false),
                    Rijbewijsnummer = table.Column<int>(type: "int", nullable: false),
                    TotaalPrijs = table.Column<double>(type: "float", nullable: false),
                    IsBetaald = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    AccountZakelijkAccountId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserveringen", x => x.ReserveringId);
                    table.ForeignKey(
                        name: "FK_Reserveringen_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserveringen_Accounts_AccountZakelijkAccountId",
                        column: x => x.AccountZakelijkAccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "ReserveringVoertuig",
                columns: table => new
                {
                    GereserveerdeVoertuigenVoertuigId = table.Column<int>(type: "int", nullable: false),
                    ReserveringenReserveringId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReserveringVoertuig", x => new { x.GereserveerdeVoertuigenVoertuigId, x.ReserveringenReserveringId });
                    table.ForeignKey(
                        name: "FK_ReserveringVoertuig_Reserveringen_ReserveringenReserveringId",
                        column: x => x.ReserveringenReserveringId,
                        principalTable: "Reserveringen",
                        principalColumn: "ReserveringId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReserveringVoertuig_Voertuigen_GereserveerdeVoertuigenVoertuigId",
                        column: x => x.GereserveerdeVoertuigenVoertuigId,
                        principalTable: "Voertuigen",
                        principalColumn: "VoertuigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AdresId",
                table: "Accounts",
                column: "AdresId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BedrijfId",
                table: "Accounts",
                column: "BedrijfId");

            migrationBuilder.CreateIndex(
                name: "IX_Bedrijven_AbonnementId",
                table: "Bedrijven",
                column: "AbonnementId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserveringen_AccountId",
                table: "Reserveringen",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserveringen_AccountZakelijkAccountId",
                table: "Reserveringen",
                column: "AccountZakelijkAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ReserveringVoertuig_ReserveringenReserveringId",
                table: "ReserveringVoertuig",
                column: "ReserveringenReserveringId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reparaties");

            migrationBuilder.DropTable(
                name: "ReserveringVoertuig");

            migrationBuilder.DropTable(
                name: "Reserveringen");

            migrationBuilder.DropTable(
                name: "Voertuigen");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Adressen");

            migrationBuilder.DropTable(
                name: "Bedrijven");

            migrationBuilder.DropTable(
                name: "Abonnementen");
        }
    }
}
