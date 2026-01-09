using OpenQA.Selenium;

namespace Day19.PageMethods
{
    public class CartPage
    {
        private IWebDriver driver;

        public CartPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void Checkout()
        {
            driver.FindElement(By.Id("checkout")).Click();
        }
    }
}
