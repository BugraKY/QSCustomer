/*using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Extensions
{

    public interface IWebDriverExtension
    {/*
        private static bool firstAction = true;
        private static ChromeDriver driver;
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable IDE0060 // Remove unused parameter
        public static void CheckProcess(string? Url = default)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            Process[] DriverProcesses = Process.GetProcessesByName("chromedriver");
            Process[] Chrome = Process.GetProcessesByName("chrome");
            if (DriverProcesses.Count() > 0 && firstAction == true)
            {
                firstAction = false;
                foreach (var DriverProcess in DriverProcesses)
                {
                    DriverProcess.Kill();
                }
                foreach (var _chrome in Chrome)
                {
                    _chrome.Kill();
                }
                LaunchDriver(Url);
            }
            else if (DriverProcesses.Count() == 0)
                LaunchDriver(Url);
            else if (DriverProcesses.Count() == 1)
                Execute(Url);
        }
#pragma warning disable IDE0060 // Remove unused parameter
        private static void LaunchDriver(string Url)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            var opts = new ChromeOptions();
            opts.AddArguments("--headless", "--disable-gpu", "--log-level=3", "--window-size=1920,1080",
                    "--no-sandbox", "--disable-crash-reporter", "--disable-extensions", "--disable-in-process-stack-traces",
                    "--disable-logging", "--disable-dev-shm-usage", "--output=/dev/null");

            driver = new ChromeDriver("ExtensionLibs/ChromeDriver/", opts);
            //driver = new ChromeDriver("ExtensionLibs/ChromeDriver/");
            Execute(Url);
        }
#pragma warning disable IDE0060 // Remove unused parameter
        private static void Execute(string Url)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            driver.Url = Url;
            // wait for jQuery to load
            while (true)
            {
                Boolean ajaxIsComplete = (Boolean)((IJavaScriptExecutor)driver).ExecuteScript("return jQuery.active == 0");
                if (ajaxIsComplete)
                {
                    WebDriverModel.PageSource = driver.PageSource;
                    break;
                }
            }
        }
        public static class WebDriverModel
        {
            public static string Url { get; set; }
            public static string PageSource { get; set; }
        }*/
    }
}
