using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.IO;
using Day19.Utils;


namespace Day19.Config
{
    public abstract class ReportsGenerationClass
    {
        protected static ExtentReports extent;
        protected ExtentTest test;
        protected IWebDriver driver;

        private static string reportDir =
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

            extent.AddSystemInfo("Browser", "Chrome");
            extent.AddSystemInfo("Framework", "NUnit + Selenium");
        }

        // ================= BEFORE EACH TEST =================
        [SetUp]
        public void Setup()
        {
            driver = new EdgeDriver();
            driver.Manage().Window.Maximize();

            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        // ================= AFTER EACH TEST =================
        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;

            if (status == TestStatus.Failed)
            {
                string screenshotPath = ScreenshotHelper.Capture(driver);

                test.Fail(TestContext.CurrentContext.Result.Message)
                    .AddScreenCaptureFromPath(screenshotPath);
            }
            else if (status == TestStatus.Passed)
            {
                test.Pass("Test Passed");
            }
            else
            {
                test.Skip("Test Skipped");
            }

            driver.Quit();
            driver.Dispose();
        }

        // ================= ONE TIME CLEANUP =================
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            extent.Flush();
        }

        // ================= SCREENSHOT METHOD =================
        private string CaptureScreenshot()
        {
            string screenshotDir = Path.Combine(reportDir, "Screenshots");
            Directory.CreateDirectory(screenshotDir);

            string filePath = Path.Combine(
                screenshotDir,
                $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            );

            ITakesScreenshot ts = (ITakesScreenshot)driver;
            ts.GetScreenshot().SaveAsFile(filePath);

            return filePath;
        }

        // ================= DRIVER ACCESS =================
        protected IWebDriver GetDriver()
        {
            return driver;
        }
    }
}
