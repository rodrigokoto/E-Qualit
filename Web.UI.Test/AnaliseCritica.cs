using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Web.UI.Test
{
    [TestClass]
    public class AnaliseCritica
    {
        [TestMethod]
        public void Index()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://localhost:54907/AnaliseCritica/Index");
            driver.Manage().Window.Maximize();

            //Busca o elemento
            IWebElement responsavel = driver.FindElement(By.Id("responsavel"));

            //Define o valor do elemento
            responsavel.SendKeys("Bruno Santos");


            //Busca botão enter da tela de login
            var enter = driver.FindElement(By.Id("btn_analiseCriticaEnviar"));

            enter.Click();

            driver.Quit();
        }
    }
}
