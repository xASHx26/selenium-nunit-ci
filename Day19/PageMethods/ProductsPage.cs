using OpenQA.Selenium;

namespace Day19.PageMethods
{
    public class ProductsPage
    {
        private IWebDriver driver;

        public ProductsPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void AddToCart(string productId)
        {
            driver.FindElement(By.Id(productId)).Click();
        }

        public void RemoveFromCart(string productId)
        {
            driver.FindElement(By.Id(productId)).Click();
        }

        public void OpenCart()
        {
            driver.FindElement(By.ClassName("shopping_cart_link")).Click();
        }
    }
}
