using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System.Collections.ObjectModel;

namespace TestProject1
{
    [TestFixture]
    public class AdminMenu
    {
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
        public void TestAdminMenu()
        {
            driver.Url = "http://localhost/litecart/admin/";
            wait.Until(ExpectedConditions.TitleIs("My Store"));
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.Id("sidebar")));

            ReadOnlyCollection <IWebElement> elements = driver.FindElements(By.Id("app-"));

            for (int i=0; i < elements.Count; i++)
            {
                elements[i].Click();
                driver.FindElement(By.CssSelector("h1"));
                elements = driver.FindElements(By.Id("app-"));
                ReadOnlyCollection<IWebElement> children = elements[i].FindElements(By.ClassName("name"));
                if (children!=null)
                {
                    for (int j=0; j < children.Count; j++)
                    {
                        children[j].Click();
                        driver.FindElement(By.CssSelector("h1"));
                        elements = driver.FindElements(By.Id("app-"));
                        children = elements[i].FindElements(By.ClassName("name"));
                    }
                }
                elements = driver.FindElements(By.Id("app-"));
            }
            
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
