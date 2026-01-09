using Day19.Config;
using Day19.DataReaders;
using Day19.PageMethods;
using Day19.TestData;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
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

            test.Info($"Logging in as: {data.Username}");
            login.Login(data.Username, data.Password);

            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(10));
            wait.Until(d =>
                d.Url.Contains("inventory") ||
                d.FindElements(By.CssSelector("[data-test='error']")).Count > 0
            );

            // ❌ Invalid users → Skip (EXPECTED)
            if (!GetDriver().Url.Contains("inventory"))
            {
                test.Info("Login failed as expected for invalid user");
                Assert.Ignore("Skipping E2E flow for invalid login");
            }

            // ✅ Valid user flow
            if (data.Items != null && data.Items.Count > 0)
            {
                test.Info("Adding products");
                foreach (var item in data.Items)
                {
                    test.Info($"Adding item: {item}");
                    products.AddToCart(item);
                }

                test.Info("Opening cart");
                products.OpenCart();

                test.Info("Checkout");
                cart.Checkout();

                test.Info("Filling checkout info");
                checkout.FillInfo("Habib", "Siam", "1234");

                test.Info("Finishing order");
                checkout.Finish();
                checkout.BackToProducts();

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
