using MATH_CALC_COM.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace MATH_CALC_COM.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            Test testObject = new Test() { Teststring = "MKO Schon" };
            return View(testObject);
        }

        public IActionResult MKO1()
        {
            Test testObject = new Test() { Teststring = "MKO Schon" };
            return View("MKO1", testObject);
        }
    }
}
