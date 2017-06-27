using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading;

namespace TestProject1
{
    [TestFixture]
    public class Task14
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        ReadOnlyCollection<IWebElement> referencesEditCountry;
        ReadOnlyCollection<IWebElement> referencesNewWindow;
        Random rnd = new Random();
        int r;
        string mainWindow;
        string newWindow;
        IList<string> oldWindows;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void DoOpenNewWindows()
        {
            //Enter as admin
            driver.Url = "http://localhost/litecart/admin/";
            wait.Until(ExpectedConditions.TitleIs("My Store"));
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.Id("sidebar")));

            driver.FindElement(By.CssSelector("[href*=countries]")).Click();
            wait.Until(ExpectedConditions.TitleIs("Countries | My Store"));

            referencesEditCountry = driver.FindElements(By.CssSelector("[href*=edit_country]"));
            r = rnd.Next(0, referencesEditCountry.Count - 1);
            referencesEditCountry[r].Click();
            wait.Until(ExpectedConditions.TitleIs("Edit Country | My Store"));

            referencesNewWindow=driver.FindElements(By.CssSelector("i.fa-external-link"));

            //Was open new window?
            mainWindow = driver.CurrentWindowHandle;//id current window
            oldWindows = driver.WindowHandles;//list with id's of all windows were open
            referencesNewWindow[0].Click(); 
            newWindow = wait.Until(ThereIsWindowOtherThan(oldWindows));
            driver.SwitchTo().Window(newWindow);
            driver.Close();
            driver.SwitchTo().Window(mainWindow);
        }


        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }

        public Func<IWebDriver, string> ThereIsWindowOtherThan(IList<string> oldWindows)
        {
            IList<string> handles;

            return (driver) =>
            {
                for (int count = 0; ; count++)
                {
                    if (count >= 30)
                        throw new TimeoutException();
                    try
                    {
                        handles = driver.WindowHandles;
                        for (int i=0; i<oldWindows.Count; i++)
                        {
                            handles.Remove(oldWindows[i]);
                        }

                        if (handles.Count > 0)
                            return handles[0]; 
                        break;
                    }
                    catch (IndexOutOfRangeException e)
                    { }
                    Thread.Sleep(1000);
                }
                return null;
            };
        }
    }
}
