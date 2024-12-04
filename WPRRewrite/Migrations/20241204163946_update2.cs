using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPRRewrite.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeVoertuig",
                table: "Voertuigen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    AccountMedewerkerBackOfficeId = table.Column<int>(type: "int", nullable: true),
                    Account = table.Column<int>(type: "int", nullable: true),
                    AccountMedewerkerFrontofficeId = table.Column<int>(type: "int", nullable: true),
                    AccountMedewerkerFrontoffice_Account = table.Column<int>(type: "int", nullable: true),
                    ParticulierAccountId = table.Column<int>(type: "int", nullable: true),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParticulierAdres = table.Column<int>(type: "int", nullable: true),
                    Telefoonnummer = table.Column<int>(type: "int", nullable: true),
                    AccountParticulier_Account = table.Column<int>(type: "int", nullable: true),
                    BedrijfsId = table.Column<int>(type: "int", nullable: true),
                    AccountZakelijkBeheerderId = table.Column<int>(type: "int", nullable: true),
                    AccountZakelijkBeheerder_Account = table.Column<int>(type: "int", nullable: true),
                    AccountZakelijkHuurderId = table.Column<int>(type: "int", nullable: true),
                    AccountZakelijkHuurder_Account = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
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
                name: "Bedrijven",
                columns: table => new
                {
                    BedrijfId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bedrijfsnaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BedrijfAdres = table.Column<int>(type: "int", nullable: false),
                    Domeinnaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KvkNummer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bedrijven", x => x.BedrijfId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Adressen");

            migrationBuilder.DropTable(
                name: "Bedrijven");

            migrationBuilder.DropColumn(
                name: "TypeVoertuig",
                table: "Voertuigen");
        }
    }
}
