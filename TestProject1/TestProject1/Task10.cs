using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestProject1
{
    [TestFixture]
    public class Task10
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            //driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            driver = new InternetExplorerDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void CheckPage()
        {
            driver.Url = "http://localhost/litecart/";
            wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));
            IWebElement Good = driver.FindElement(By.Id("box-campaigns")).FindElement(By.ClassName("link"));

            string nameOnMain=Good.FindElement(By.ClassName("name")).Text;
            string priceOnMain = Good.FindElement(By.ClassName("regular-price")).Text;
            string salePriceOnMain = Good.FindElement(By.ClassName("campaign-price")).Text;
            
            Good.Click();
            wait.Until(ExpectedConditions.TitleContains(nameOnMain + " | Subcategory"));

            string nameGood = driver.FindElement(By.CssSelector("h1.title")).Text;
            string priceGood = driver.FindElement(By.ClassName("regular-price")).Text;
            string salePriceGood = driver.FindElement(By.ClassName("campaign-price")).Text;

            Assert.AreEqual(nameOnMain, nameGood);
            Assert.AreEqual(priceOnMain, priceGood);
            Assert.AreEqual(salePriceOnMain, salePriceGood);
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
