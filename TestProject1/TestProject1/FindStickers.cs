using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

namespace TestProject1
{
    [TestFixture]
    public class FindStickers
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
        public void TestStickers()
        {
            driver.Url = "http://localhost/litecart/";
            wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));

            ReadOnlyCollection<IWebElement> elements = driver.FindElements(By.ClassName("image-wrapper"));

            for (int i = 0; i < elements.Count; i++)
            {
                ReadOnlyCollection<IWebElement> stickers = elements[i].FindElements(By.CssSelector("[class*=sticker]"));
                if (stickers.Count!= 1)
                {
                    throw new Exception("Product has more then 1 stickers");
                }
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
