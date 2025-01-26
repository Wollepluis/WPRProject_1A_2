using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WPRRewrite;
using WPRRewrite.Controllers;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewriteTests.Tests.VoertuigTests
{
    [TestFixture]
    public class VoertuigControllerTests
    {
        private CarAndAllContext _context;
        private VoertuigController _controller;

        [SetUp]
        public void Setup()
        {
            // Verbinden met SQL Server database voor testen
            var options = new DbContextOptionsBuilder<CarAndAllContext>()
                .UseSqlServer(@"Server=LaptopMorris\SQLEXPRESS;Database=CarandallTest;Trusted_Connection=True;TrustServerCertificate=True")
                .Options;

            _context = new CarAndAllContext(options);
            _controller = new VoertuigController(_context);

            // Verwijder eerst alle schadeclaims die aan voertuigen gekoppeld zijn
            _context.Schadeclaim.RemoveRange(_context.Schadeclaim);
            _context.SaveChanges();

            // Zorg ervoor dat de database leeg is voor deze test
            _context.Voertuigen.RemoveRange(_context.Voertuigen);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAll_VoertuigenInDatabase_RetourneertVoertuigenLijst()
        {
            // Arrange
            var voertuig1 = new Auto
            {
                Kenteken = "AB-123-CD",  // Kenteken toegevoegd
                Merk = "Merk1",          // Merk toegevoegd
                Model = "Model1",        // Model toegevoegd
                Kleur = "Rood",          // Kleur toegevoegd
                Aanschafjaar = 2020,     // Aanschafjaar toegevoegd
                Prijs = 15000,           // Prijs toegevoegd
                VoertuigStatus = "Beschikbaar", // VoertuigStatus toegevoegd
                VoertuigType = "Auto",  // VoertuigType toegevoegd
                AantalZitPlaatsen = 4,   // AantalZitplaatsen toegevoegd
                BrandstofType = "Benzine" // BrandstofType toegevoegd
            };

            var voertuig2 = new Auto
            {
                Kenteken = "EF-456-GH",  // Kenteken toegevoegd
                Merk = "Merk2",          // Merk toegevoegd
                Model = "Model2",        // Model toegevoegd
                Kleur = "Blauw",         // Kleur toegevoegd
                Aanschafjaar = 2021,     // Aanschafjaar toegevoegd
                Prijs = 20000,           // Prijs toegevoegd
                VoertuigStatus = "Beschikbaar", // VoertuigStatus toegevoegd
                VoertuigType = "Auto",    // VoertuigType toegevoegd
                AantalZitPlaatsen = 4,   // AantalZitplaatsen toegevoegd
                BrandstofType = "Diesel" // BrandstofType toegevoegd
            };

            // Voeg voertuigen toe aan de database
            _context.Voertuigen.Add(voertuig1);
            _context.Voertuigen.Add(voertuig2);
            await _context.SaveChangesAsync();

            // Act
            var resultaat = await _controller.GetAlleVoertuigen();

            // Assert
            Assert.That(resultaat.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = resultaat.Result as OkObjectResult;
            var voertuigen = (List<Voertuig>)okResult.Value;
            Assert.That(voertuigen.Count, Is.EqualTo(2));
            Assert.That(voertuigen[0].Merk, Is.EqualTo("Merk1"));
            Assert.That(voertuigen[1].Merk, Is.EqualTo("Merk2"));
        }


        [Test]
        public async Task GetAll_GeenVoertuigen_RetourneertLegeLijst()
        {
            // Act
            var resultaat = await _controller.GetAlleVoertuigen();

            // Assert
            Assert.That(resultaat.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = resultaat.Result as OkObjectResult;
            Assert.That(((List<Voertuig>)okResult.Value).Count, Is.EqualTo(0));
        }
    }
}
