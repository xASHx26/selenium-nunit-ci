using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.IO;
using Day19.Utils;

namespace Day19.Config
{
    public abstract class ReportsGenerationClass
    {
        protected static ExtentReports? extent;
        protected ExtentTest? test;
        protected IWebDriver? driver;

        private static readonly string reportDir =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");

        // ================= ONE TIME =================
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Directory.CreateDirectory(reportDir);

            var reporter = new ExtentSparkReporter(
                Path.Combine(reportDir, "ExtentReport.html")
            );

            reporter.Config.ReportName = "SauceDemo Automation Report";
            reporter.Config.DocumentTitle = "NUnit Selenium Results";

            extent = new ExtentReports();
            extent.AttachReporter(reporter);

            extent.AddSystemInfo("Browser", "Edge (Headless)");
            extent.AddSystemInfo("Framework", "NUnit + Selenium");
            extent.AddSystemInfo("CI", "GitHub Actions");
        }

        // ================= BEFORE EACH TEST =================
        [SetUp]
        public void Setup()
        {
            var options = new EdgeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");

            driver = new EdgeDriver(options);
            driver.Manage().Window.Maximize();

            test = extent!.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        // ================= AFTER EACH TEST =================
        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;

            if (status == TestStatus.Failed)
            {
                string screenshotPath = ScreenshotHelper.Capture(driver!);
                test!.Fail(TestContext.CurrentContext.Result.Message)
                     .AddScreenCaptureFromPath(screenshotPath);
            }
            else if (status == TestStatus.Passed)
            {
                test!.Pass("Test Passed");
            }
            else
            {
                test!.Skip("Test Skipped");
            }

            driver?.Quit();
            driver?.Dispose();
        }

        // ================= ONE TIME CLEANUP =================
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            extent?.Flush();
        }

        // ================= DRIVER ACCESS =================
        protected IWebDriver GetDriver()
        {
            return driver!;
        }
    }
}
