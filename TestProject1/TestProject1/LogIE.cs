using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.IE;

namespace TestProject1
{
    /// <summary>
    /// Summary description for UnitTest3
    /// </summary>
    [TestFixture]
    public class LogIE
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new InternetExplorerDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void LoginIE()
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
