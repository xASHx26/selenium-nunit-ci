using OpenQA.Selenium;
using NUnit.Framework;
using System;
using System.IO;

namespace Day19.Utils
{
    public static class ScreenshotHelper
    {
        public static string Capture(IWebDriver driver)
        {
            string folder = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Reports",
                "Screenshots"
            );

            Directory.CreateDirectory(folder);

            string filePath = Path.Combine(
                folder,
                $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            );

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(filePath);

            return filePath;
        }
    }
}
