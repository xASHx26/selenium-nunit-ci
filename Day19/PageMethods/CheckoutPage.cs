using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace Day19.PageMethods
{
    public class CheckoutPage
    {
        private IWebDriver driver;

        public CheckoutPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void FillInfo(string first, string last, string zip)
        {
            driver.FindElement(By.Id("first-name")).SendKeys(first);
            driver.FindElement(By.Id("last-name")).SendKeys(last);
            driver.FindElement(By.Id("postal-code")).SendKeys(zip);
            driver.FindElement(By.Id("continue")).Click();
        }

        public void Finish()
        {
            driver.FindElement(By.Id("finish")).Click();
        }

        public void BackToProducts()
        {
            driver.FindElement(By.Id("back-to-products")).Click();
        }

        public void Logout()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Open burger menu
            IWebElement menuBtn = wait.Until(
                ExpectedConditions.ElementToBeClickable(By.Id("react-burger-menu-btn"))
            );
            menuBtn.Click();

            // Wait until logout is clickable
            IWebElement logoutBtn = wait.Until(
                ExpectedConditions.ElementToBeClickable(By.Id("logout_sidebar_link"))
            );
            logoutBtn.Click();
        }
    }
}
