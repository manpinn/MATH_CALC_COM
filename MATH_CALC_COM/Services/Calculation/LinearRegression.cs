using Plotly.NET;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MATH_CALC_COM.Services.Calculation
{
    public class LinearRegression
    {
        public string LinearRegressionTest() 
        {
            
            List<int> xValues = Enumerable.Range(1, 500).ToList();
            List<float> y1Values = getY1Values(); 

            
            var chartY1 = Chart2D.Chart.Line<int, float, string>(xValues, y1Values, true, "MKO");

            
            var chartList = new List<GenericChart>();
            chartList.Add(chartY1);

            
            var combinedChart = Chart.Combine(chartList);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(combinedChart, new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });

            return json;
        }

        public List<float> getY1Values()
        {
            List<float> y1Values = new List<float>();
            for (int x = 1; x <= 500; x++)
            {
                float y = x * x; // y = x^2
                y1Values.Add(y);
            }
            return y1Values;
        }

    }
}
