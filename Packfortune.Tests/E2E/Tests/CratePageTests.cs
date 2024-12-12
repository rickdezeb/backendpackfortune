using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Packfortune.Tests.E2E.Pages;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Packfortune.Tests.E2E.Tests
{
    [TestClass]
    public class CratePageTests
    {
        private IWebDriver driver;
        private CratesPage page;

        [TestInitialize]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://localhost:5173/crates");
            page = new CratesPage(driver);
        }

        [TestCleanup]
        public void Teardown()
        {
           driver.Quit();
        }

        [TestMethod]
        public void CratePage_CreateCrate()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string testImagePath = Path.Combine(projectDirectory, "E2E", "Tests", "TestFiles", "test.jpg");

            page.ClickAddCrateButton();
            page.EnterName("Test Crate");
            page.EnterPrice("100");
            page.EnterPicture(testImagePath);

            page.ClickSubmit();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.PageSource.Contains("Test Crate"));

            Assert.IsTrue(driver.PageSource.Contains("Test Crate"));

            Task.Delay(500).Wait();
        }

        [TestMethod]
        public void CratePage_DeleteCrate()
        {
            CratePage_CreateCrate();

            Task.Delay(500).Wait();

            driver.Navigate().Refresh();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.PageSource.Contains("Test Crate"));

            page.ClickDeleteCrateButton();

            wait.Until(d =>
            {
                try
                {
                    IAlert alert = driver.SwitchTo().Alert();
                    string alertText = alert.Text;
                    alert.Accept();
                    Assert.AreEqual("Crate succesvol verwijderd!", alertText);
                    return true;
                }
                catch (NoAlertPresentException)
                {
                    return false;
                }
            });

            bool noCrateFoundVisible = wait.Until(d => d.FindElement(By.CssSelector("[data-test='NoCrateFound']")).Displayed);
            Assert.IsTrue(noCrateFoundVisible, "The 'Geen crates gevonden' message was not found on the page.");
        }
    }
}