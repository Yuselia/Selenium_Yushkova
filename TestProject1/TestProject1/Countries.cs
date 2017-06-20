using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace TestProject1
{
    [TestFixture]
    public class Countries
    {
        string url1 = "http://localhost/litecart/admin/?app=countries&doc=countries";
        string url2 = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";

        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            //driver = new InternetExplorerDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Sorted()
        {
            ///Open countries
            ClickAndWait(driver, url1, "My Store");
           
            ///Login, go to Countries
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            IWebElement buttonLog = driver.FindElement(By.Name("login"));
            ClickAndWait(driver, buttonLog, By.Name("countries_form"));
            Assert.AreEqual("Countries", driver.FindElement(By.CssSelector("h1")).Text);
            ReadOnlyCollection<IWebElement> rows = FindRows(driver);

            ///Find countries
            List<String> countries = new List<String>();
            countries = GetList(rows, By.XPath("./td[5]"));

            ///Are countries sorted?
            Assert.AreEqual(countries, Sorting(countries));

            ///Find zones>0
            int countZone = 0;
            for (int i=0; i< countries.Count; i++)
            {
                countZone=Convert.ToInt32(rows[i].FindElement(By.XPath("./td[6]")).Text);
                if (countZone>0)
                {
                    ///Go to zones
                    IWebElement buttonEdit = rows[i].FindElement(By.XPath("./td[7]"));
                    ClickAndWait(driver, buttonEdit, By.CssSelector("form[method=post]"));
                    Assert.AreEqual("Edit Country", driver.FindElement(By.CssSelector("h1")).Text);
                    rows = FindRows(driver);

                    ///Find zones
                    List<String> zones = new List<String>();
                    zones = GetList(rows, By.XPath("./td[3]"));

                    ///Are zones sorted?
                    Assert.AreEqual(zones, Sorting(zones));

                    ///Go to selected menu (coutries)
                    IWebElement selectedMenu = driver.FindElement(By.CssSelector(".selected"));
                    ClickAndWait(driver, selectedMenu, By.Name("countries_form"));
                    Assert.AreEqual("Countries", driver.FindElement(By.CssSelector("h1")).Text);
                    rows = FindRows(driver);
                }
            }

            ///Open geo zones
            ClickAndWait(driver, url2, "Geo Zones | My Store");
            Assert.AreEqual("Geo Zones", driver.FindElement(By.CssSelector("h1")).Text);
            rows = FindRows(driver);
            ReadOnlyCollection<IWebElement> geozones = FindRows(driver);
            ///Find zones
            for (int i = 0; i < geozones.Count; i++)
            {
                IWebElement buttonEdit = geozones[i].FindElement(By.XPath("./td[5]"));
                ClickAndWait(driver, buttonEdit, "Edit Geo Zone | My Store");
                Assert.AreEqual("Edit Geo Zone", driver.FindElement(By.CssSelector("h1")).Text);
                List<String> zones = new List<String>();
                rows = FindRows(driver);
                for (int j = 0; j < rows.Count - 1; j++)
                {
                    IWebElement cell = rows[j].FindElement(By.XPath("./td[3]"));
                    IWebElement zone = cell.FindElement(By.XPath("./select/option[@selected]"));
                    zones.Add(zone.Text);
                }
                Assert.AreEqual(zones, Sorting(zones));
                ///Go to selected menu (geo zones)
                IWebElement selectedMenu = driver.FindElement(By.CssSelector(".selected"));
                ClickAndWait(driver, selectedMenu, "Geo Zones | My Store");
                Assert.AreEqual("Geo Zones", driver.FindElement(By.CssSelector("h1")).Text);
                geozones = FindRows(driver);
            }
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }

        private void ClickAndWait(IWebDriver driver, string url, string title)
        {
            driver.Url = url;
            wait.Until(ExpectedConditions.TitleIs(title));
        }

        private void ClickAndWait(IWebDriver driver, IWebElement element, string title)
        {
            element.Click();
            wait.Until(ExpectedConditions.TitleIs(title));
        }

        private void ClickAndWait(IWebDriver driver, string url, By locatorWait)
        {
            driver.Url = url;
            wait.Until(ExpectedConditions.ElementExists(locatorWait));
        }

        private void ClickAndWait(IWebDriver driver, IWebElement element, By locatorWait)
        {
            element.Click();
            wait.Until(ExpectedConditions.ElementExists(locatorWait));
        }

        private List<String> GetList(ReadOnlyCollection<IWebElement> rows, By locatorTd)
        {
            List<String> list = new List<String>();
            foreach (IWebElement row in rows)
            {
                list.Add(row.FindElement(locatorTd).Text);
            }
            return list;
        }

        private List<String> GetList(ReadOnlyCollection<IWebElement> rows, By locatorTd, By locatorSelected)
        {
            List<String> list = new List<String>();
            foreach (IWebElement row in rows)
            {
                list.Add(row.FindElement(locatorTd).FindElement(locatorSelected).Text);
            }
            return list;
        }

        private List<String> Sorting(List<String> elements)
        {
            elements.Sort();
            return elements;
        }

        private ReadOnlyCollection<IWebElement> FindRows(IWebDriver driver)
        {
            IWebElement table = driver.FindElement(By.ClassName("dataTable"));
            ReadOnlyCollection<IWebElement> rows = table.FindElements(By.CssSelector("tr:not(.header):not(.footer)"));
            return rows;
        }
    }
}
