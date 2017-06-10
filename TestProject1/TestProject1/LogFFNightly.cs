using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;

namespace TestProject1
{
    /// <summary>
    /// Summary description for LogFFNightly
    /// </summary>
    [TestFixture]
    public class FFNightly
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.UseLegacyImplementation = false;
            options.BrowserExecutableLocation = @"C:\Program Files\Nightly\firefox.exe";
            driver = new FirefoxDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void LoginFFNightly()
        {
            driver.Url = "http://localhost/litecart/admin/";
            wait.Until(ExpectedConditions.TitleIs("My Store"));
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.XPath("(//li[@id='app-']/a/span[2])[3]")));
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
