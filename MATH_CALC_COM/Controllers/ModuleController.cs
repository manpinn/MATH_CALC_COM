using Microsoft.AspNetCore.Mvc;

namespace MATH_CALC_COM.Controllers
{
    public class ModuleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
