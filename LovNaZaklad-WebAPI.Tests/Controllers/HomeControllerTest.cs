using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LovNaZaklad_WebAPI;
using LovNaZaklad_WebAPI.Controllers;

namespace LovNaZaklad_WebAPI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Lov na zaklad", result.ViewBag.Title);
        }
    }
}
