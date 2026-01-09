using Day19.Config;
using Day19.DataReaders;
using Day19.PageMethods;
using Day19.TestData;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace Day19.TestCases
{
    [TestFixture]
    public class SauceDemoE2ETest : ReportsGenerationClass
    {
        [TestCaseSource(nameof(LoginDataFromJson))]
        public void Complete_Order_Flow(LoginData data)
        {
            var login = new LoginPage(GetDriver());
            var products = new ProductsPage(GetDriver());
            var cart = new CartPage(GetDriver());
            var checkout = new CheckoutPage(GetDriver());

            test.Info("Navigating to SauceDemo");
            login.Navigate();

            test.Info($"Logging in as  username:{data.Username}, password: {data.Password}");
            login.Login(data.Username, data.Password);

            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(10));
            wait.Until(d =>
            {
 
                return d.Url.Contains("inventory") ||
                       d.FindElements(By.CssSelector("[data-test='error']")).Count > 0;
            });

            if (!driver.Url.Contains("inventory"))
            {
                test.Fail("Login failed");
                Assert.Fail("Login failed");
            }



            if (data.Items.Count > 0)
            {
                test.Info("Adding products");
                for (int i = 0; i < data.Items.Count; i++)
                {
                    test.Info();
                    products.AddToCart(data.Items[i]);
                }



                //Thread.Sleep(2000);
                test.Info("Opening cart");
                products.OpenCart();
                //Thread.Sleep(2000);
                test.Info("Checkout");
                cart.Checkout();
                //Thread.Sleep(2000);
                test.Info("Filling checkout info");
                checkout.FillInfo("Habib", "Siam", "1234");
                //Thread.Sleep(2000);
                test.Info("Finishing order");
                checkout.Finish();
                checkout.BackToProducts();
                //Thread.Sleep(2000);
                test.Info("Logging out");
                checkout.Logout();
            }
        }

        public static IEnumerable<LoginData> LoginDataFromJson()
        {
            return JsonDataReader.ReadLoginData();
        }
    }
}
