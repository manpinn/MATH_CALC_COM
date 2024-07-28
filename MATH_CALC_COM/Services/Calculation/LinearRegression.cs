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
            // Assuming you have two sets of y values and corresponding x values
            List<int> xValues = Enumerable.Range(1, 500).ToList();
            List<float> y1Values = getY1Values(); // Replace with your function or data

            // Create a chart for each line plot
            var chartY1 = Chart2D.Chart.Line<int, float, string>(xValues, y1Values);

            // Add them to a list
            var chartList = new List<GenericChart>();
            chartList.Add(chartY1);

            // Combine the charts
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
