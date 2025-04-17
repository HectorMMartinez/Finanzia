using OpenQA.Selenium;
using System;
using System.IO;

namespace Finanzia.Tests.Utils
{
    public static class CapturaHelper
    {
        public static void GuardarCaptura(IWebDriver driver, string carpeta, string nombreArchivo)
        {
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\", carpeta);
            Directory.CreateDirectory(folderPath);
            string path = Path.Combine(folderPath, $"{nombreArchivo}.png");
            ss.SaveAsFile(path);
            Console.WriteLine($"📸 Captura guardada: {path}");
        }
    }
}
