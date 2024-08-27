using MATH_CALC_COM.Models;
using MATH_CALC_COM.Services.DatabaseContext;
using MATH_CALC_COM.Services.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace MATH_CALC_COM.Controllers
{
    public class TestController : Controller
    {
        private readonly RequestDataContext _context;

        public TestController(RequestDataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Test testObject = new Test() { Teststring = "MKO2", Testdate = DateTime.Now };
            return View(testObject);
        }

        public IActionResult MKO1()
        {
            Test testObject = new Test() { Teststring = "MKO1", Testdate = DateTime.Now };
            return View("MKO1", testObject);
        }

        ////https://localhost:{PORT}/Test/WelcomeParam?name=Rick&numTimes=4
        //public IActionResult WelcomeParam(string name, int numTimes = 1)
        //{
        //    ViewData["Message"] = "Hello " + name;
        //    ViewData["NumTimes"] = numTimes;
        //    return View();
        //}

        //[Route("/Test/{article_name}")]
        //public IActionResult Article(string article_name)
        //{
        //    Test testObject = new Test() { Teststring = article_name, Testdate = DateTime.Now };
        //    return View("Article", testObject);
        //}

        //public IActionResult Article()
        //{
        //    return View();
        //}

        //// GET: /Test/Custom/{id}
        //[Route("/Test/Custom/{id}")]
        //public string Custom(string id)
        //{
        //    //test mko
        //    return $"This is the Custom action method with custom string: {id}";
        //}

        public void IP()
        {
            IPAddress clientIpAddress = HttpContext.Connection.RemoteIpAddress;

            string ipAddressString = clientIpAddress?.ToString();

            RequestData requestData = new RequestData() { datetime = DateTime.Now, url = Request.Path, ip_adress = ipAddressString };

            _context.RequestData.Add(requestData);

            _context.SaveChanges();
        }

    }
}
