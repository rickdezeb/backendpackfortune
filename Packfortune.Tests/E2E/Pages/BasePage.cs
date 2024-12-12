using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packfortune.Tests.E2E.Pages
{
    public class BasePage
    {
        protected IWebDriver _driver;
        public WebDriverWait Wait => new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        public BasePage(IWebDriver driver)
        {
            _driver = driver;
        }
    }
}
