using OpenQA.Selenium;

namespace Day19.PageMethods
{
    public class LoginPage
    {
        private IWebDriver driver;

        private By username = By.Id("user-name");
        private By password = By.Id("password");
        private By loginBtn = By.Id("login-button");

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void Navigate()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        public void Login(string user, string pass)
        {
            driver.FindElement(username).SendKeys(user);
            driver.FindElement(password).SendKeys(pass);
            driver.FindElement(loginBtn).Click();
        }
    }
}
