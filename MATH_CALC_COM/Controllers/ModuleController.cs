using MATH_CALC_COM.Models;
using MATH_CALC_COM.Services.Calculation;
using Microsoft.AspNetCore.Mvc;

namespace MATH_CALC_COM.Controllers
{
    public class ModuleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Module/{module_name:regex(^(?!AJAX$).*)}")]
        public IActionResult Module(string module_name)
        {
            string view_name = string.Empty;

            if(module_name == "LinearRegression")
            {
                view_name = "1";

                //LinearRegression calculator = new LinearRegression();

                //string json = calculator.LinearRegressionTest();

                //ViewData["ChartJson"] = json;
            }

            return View(view_name);
        }

        [HttpPost]
        [Route("/Module/AJAX/LinearRegression")]
        public ActionResult AJAX_LinearRegression([FromBody] AJAX_LinearRegression_Model model)
        {
            LinearRegression calculator = new LinearRegression();

            string json = calculator.LinearRegressionPlotter(model.graphs, model.x_vector, model.y_vector);

            return Json(json);
        }

        public class AJAX_LinearRegression_Model 
        {
            public LinearRegressionGraph[] graphs { get; set; }

            public double[] x_vector { get; set; }

            public double[] y_vector { get; set; }
        }

        
    }
}
