using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestProject1
{
    [TestFixture]
    public class Task11
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        string firstPartEmail = "yuselia";
        string secondPartEmail= "@yandex.ru";
        string randomPartEmail = "";
        string email="";

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            //driver = new InternetExplorerDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void SignUp()
        {
            driver.Url = "http://localhost/litecart/";
            wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));
            
            driver.FindElement(By.XPath("//*[@id='box-account-login']//a[contains(@href,'create_account')]")).Click();
            wait.Until(ExpectedConditions.TitleIs("Create Account | My Store"));
           
            IWebElement box = driver.FindElement(By.CssSelector("#create-account"));
            box.FindElement(By.Name("firstname")).SendKeys("Ivan");
            box.FindElement(By.Name("lastname")).SendKeys("Ivanov");
            box.FindElement(By.Name("address1")).SendKeys("1556 Broadway, suite 416");
            box.FindElement(By.Name("postcode")).SendKeys("10120"); 
            box.FindElement(By.Name("city")).SendKeys("New York"+Keys.Tab+Keys.Enter+"United S"+ Keys.Enter + Keys.Tab + Keys.Tab);
            randomPartEmail = GetRandomPart();
            email = firstPartEmail + "+" + randomPartEmail + secondPartEmail;
            box.FindElement(By.Name("email")).SendKeys(email);
            box.FindElement(By.Name("phone")).SendKeys("12345");
            box.FindElement(By.Name("password")).SendKeys("1");
            box.FindElement(By.Name("confirmed_password")).SendKeys("1");

            box.FindElement(By.Name("create_account")).Click();
           
            try
            {
                wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));
            }
            catch (WebDriverTimeoutException)
            {
                    IWebElement notice = driver.FindElement(By.CssSelector("[class*=notice][class*=errors]"));
                    if (!DisplayNotice(notice))
                    {
                        wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));
                    }
                    else DisplayNotice(notice);
            }

            driver.FindElement(By.XPath("//a[contains(@href, 'logout')]")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.Name("login")));

            driver.FindElement(By.Name("email")).SendKeys(email);
            driver.FindElement(By.Name("password")).SendKeys("1");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//a[contains(@href, 'logout')]")));
            driver.FindElement(By.XPath("//a[contains(@href, 'logout')]")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.Name("login")));
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }

        private bool DisplayNotice(IWebElement notice)
        {
            if (notice.Displayed)
            {
                randomPartEmail = GetRandomPart();
                driver.FindElement(By.Name("email")).Clear();
                email = firstPartEmail + "+" + randomPartEmail + secondPartEmail;
                driver.FindElement(By.Name("email")).SendKeys(email);
                driver.FindElement(By.Name("password")).SendKeys("1");
                driver.FindElement(By.Name("confirmed_password")).SendKeys("1");
                driver.FindElement(By.Name("create_account")).Click();
                notice = driver.FindElement(By.CssSelector("[class*=notice][class*=errors]"));
                return DisplayNotice(notice);
            }
            else return false;
        }

        private string GetRandomPart()
        {
            string s = "";
            string s0 = "";
            Random rnd = new Random();
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
