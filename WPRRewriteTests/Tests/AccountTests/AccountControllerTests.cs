﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.Controllers;

namespace WPRRewriteTests.Tests.AccountTests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private CarAndAllContext _mockContext;
        private AccountMedewerkerBackofficeController _controller;

        [SetUp]
        public void Setup()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

            var options = new DbContextOptionsBuilder<CarAndAllContext>()
                .UseSqlite("DataSource=:memory:")  // Gebruik SQLite als in-memory database
                .Options;

            _mockContext = new CarAndAllContext(options);
            _mockContext.Database.OpenConnection();
            _mockContext.Database.EnsureCreated();

            _controller = new AccountMedewerkerBackofficeController(_mockContext);
        }

        [TearDown]
        public void TearDown()
        {
            _mockContext.Database.CloseConnection();
            _mockContext.Dispose();
        }

        [Test]
        public async Task GetAll_GeenAccounts_RetourneertLegeLijst()
        {
            // Act
            var result = await _controller.GetAllAccounts();

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());  
        }

        [Test]
        public async Task GetAll_AccountsAanwezig_RetourneertOk()
        {
            // Arrange
            _mockContext.Accounts.Add(new AccountMedewerkerBackoffice 
            { 
                Email = "test@test.com", 
                Wachtwoord = "test123" 
            });
            await _mockContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetAllAccounts();

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.Not.Null);
        }
    }
}