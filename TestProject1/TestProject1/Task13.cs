using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace TestProject1
{
    [TestFixture]
    public class Task13
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Checkout()
        {
            driver.Url = "http://localhost/litecart";
            wait.Until(ExpectedConditions.TitleContains("My Store"));
            driver.FindElement(By.Name("email")).SendKeys("yuselia@yandex.ru");
            driver.FindElement(By.Name("password")).SendKeys("1");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//a[contains(@href, 'logout')]")));

            driver.FindElement(By.CssSelector(".content .link")).Click();
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
