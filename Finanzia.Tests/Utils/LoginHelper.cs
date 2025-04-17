using OpenQA.Selenium;
using System;
using System.Threading;

namespace Finanzia.Tests.Utils
{
    public static class LoginHelper
    {
        public static void IniciarSesion(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://localhost:5267/Login");
            Thread.Sleep(1000);

            driver.FindElement(By.Name("correo")).SendKeys("Admin@gmail.com");
            driver.FindElement(By.Name("clave")).SendKeys("12345");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            Thread.Sleep(1500);
        }
    }
}