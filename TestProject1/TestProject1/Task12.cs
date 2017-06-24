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
    public class Task12
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        string name = "";
        string locator = "";
        Random rnd = new Random();

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void AddNewProduct()
        {
            string s = TestContext.CurrentContext.TestDirectory + "\\img.jpeg";

            driver.Url = "http://localhost/litecart/admin/";
            wait.Until(ExpectedConditions.TitleIs("My Store"));
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.ClassName("list-vertical")));

            //Open Catalog
            driver.FindElement(By.XPath("//a[contains(.,'Catalog')]")).Click();
            wait.Until(ExpectedConditions.TitleIs("Catalog | My Store"));

            //General
            driver.FindElement(By.XPath("//a[contains(.,' Add New Product')]")).Click();
            wait.Until(ExpectedConditions.TitleIs("Add New Product | My Store"));

            name = GetRandom();
            driver.FindElement(By.CssSelector("input[name *= name]")).SendKeys(name);
            driver.FindElement(By.Name("code")).SendKeys(GetRandom());

            ReadOnlyCollection<IWebElement> productGroups = driver.FindElements(By.CssSelector("input[name*=product_groups]"));
            productGroups[rnd.Next(0, 2)].Click();

            driver.FindElement(By.CssSelector("input[name = quantity]")).SendKeys(Keys.Up);
            driver.FindElement(By.CssSelector("input[type = file]")).SendKeys(TestContext.CurrentContext.TestDirectory+"\\img.jpeg");

            driver.FindElement(By.Name("date_valid_from")).SendKeys("01022017");
            driver.FindElement(By.Name("date_valid_to")).SendKeys("01022018");

            //Information
            driver.FindElement(By.XPath("//a[contains(.,'Information')]")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("tab-information")));

            SelectElement selectManufacturer = new SelectElement(driver.FindElement(By.Name("manufacturer_id")));
            selectManufacturer.SelectByText("ACME Corp.");

            driver.FindElement(By.Name("keywords")).SendKeys(name.ToLower());
            driver.FindElement(By.CssSelector("[name*=short_description]")).SendKeys(name+" "+name);
            driver.FindElement(By.ClassName("trumbowyg-editor")).SendKeys(name + "\n" + name+"\n" + name);
            driver.FindElement(By.CssSelector("[name*=head_title]")).SendKeys(name.ToUpper());
            driver.FindElement(By.CssSelector("[name*=meta_description]")).SendKeys(name+name);

            //Prices
            driver.FindElement(By.XPath("//a[contains(.,'Prices')]")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("tab-prices")));

            driver.FindElement(By.Name("purchase_price")).Clear();
            driver.FindElement(By.Name("purchase_price")).SendKeys("2.00");
            SelectElement purchasePrice = new SelectElement(driver.FindElement(By.Name("purchase_price_currency_code")));
            purchasePrice.SelectByText("US Dollars");

            driver.FindElement(By.CssSelector("[name*=prices][name*=USD][data-type=currency]")).SendKeys("2");
            Assert.AreEqual(driver.FindElement(By.CssSelector("[name*=gross_prices][name*=USD]")).GetAttribute("value"), "2.00");
            driver.FindElement(By.CssSelector("[name*=gross_prices][name*=EUR]")).Clear();
            driver.FindElement(By.CssSelector("[name*=gross_prices][name*=EUR]")).SendKeys("1.75");
            Assert.AreEqual(driver.FindElement(By.CssSelector("[name*=prices][name*=EUR][data-type=currency]")).GetAttribute("value"), "1.75");

            //Save
            driver.FindElement(By.CssSelector("button[name=save]")).Click();
            wait.Until(ExpectedConditions.TitleIs("Catalog | My Store"));
            locator = "//a[contains(.,'" + name + "')]";
            driver.FindElement(By.XPath(locator));
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }

        private string GetRandom()
        {
            string s = "";
            string s0 = "";
            int n;
            string st = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (int j = 0; j < 6; j++)
            {
                n = rnd.Next(0, 61);
                s0 = st.Substring(n, 1);
                s += s0;
            }
            return s;
        }
    }
}
