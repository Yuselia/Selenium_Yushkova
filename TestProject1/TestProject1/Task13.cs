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

        int howMuchAdd = 3;
        int countInCart=0;
        IWebElement countInCartText;
        string nameOfProduct = "";
        string size = "Small";
        ReadOnlyCollection<IWebElement> removeButtons;
        IWebElement table;
        ReadOnlyCollection<IWebElement> trInTable;
        int trCount;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void CheckCart()
        {
            driver.Url = "http://localhost/litecart";
            wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));

            for (int i=0; i<howMuchAdd; i++)
            {
                AddProductInCart();
                driver.FindElement(By.CssSelector("i.fa-home")).Click();
                wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));
            }

            driver.FindElement(By.CssSelector("a.link[href*=checkout]")).Click();
            wait.Until(ExpectedConditions.TitleIs("Checkout | My Store"));

            table = driver.FindElement(By.ClassName("dataTable"));
            trInTable = table.FindElements(By.CssSelector("tr"));
            trCount = trInTable.Count;
            removeButtons = driver.FindElements(By.Name("remove_cart_item"));

            for (int i=0; i<removeButtons.Count;)
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(removeButtons[i]));
                table = driver.FindElement(By.ClassName("dataTable"));
                IWebElement el = table.FindElement(By.CssSelector("tr:nth-of-type("+trCount+")"));
                removeButtons[i].Click();
                wait.Until(ExpectedConditions.StalenessOf(el));
                removeButtons = driver.FindElements(By.Name("remove_cart_item"));
                if (removeButtons.Count!=0)
                {
                    trInTable = driver.FindElement(By.ClassName("dataTable")).FindElements(By.CssSelector("tr"));
                    trCount = trInTable.Count;
                }
            }
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }

        private void AddProductInCart()
        {

            IWebElement product = driver.FindElement(By.CssSelector(".content .link"));
            nameOfProduct = product.FindElement(By.ClassName("name")).Text;
            product.Click();
            wait.Until(ExpectedConditions.TitleContains(nameOfProduct));

            //If have to select size
            try
            {
                driver.FindElement(By.CssSelector("[name*=options]"));
                SelectElement selectSize = new SelectElement(driver.FindElement(By.CssSelector("[name*=options]")));
                selectSize.SelectByText(size);
            }
            catch (NoSuchElementException)
            {

            }

            countInCartText = driver.FindElement(By.CssSelector("span.quantity"));
            countInCart = Convert.ToInt32(countInCartText.Text);
            driver.FindElement(By.Name("add_cart_product")).Click();
            countInCart++;
            wait.Until(ExpectedConditions.TextToBePresentInElement(countInCartText, Convert.ToString(countInCart)));
        }
    }
}
