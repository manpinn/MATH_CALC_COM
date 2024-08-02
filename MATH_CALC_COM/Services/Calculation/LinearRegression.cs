using Plotly.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using static Plotly.NET.StyleParam.Range;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace MATH_CALC_COM.Services.Calculation
{
    public class LinearRegression
    {
        public string LinearRegressionPlotter(LinearRegressionGraph[] graphs, double[] x_vector, double[] y_vector)
        {
            var chartList = new List<GenericChart>();

            foreach (LinearRegressionGraph graph in graphs)
            {
                var retVals = LinearRegressionCalculator(graph.coefficients, x_vector, y_vector);

                var chartY = Chart2D.Chart.Line<double, double, string>(retVals.x_vector, retVals.y_vector, true, graph.name);

                chartList.Add(chartY);
            }

            var combinedChart = Chart.Combine(chartList);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(combinedChart, new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });

            return json;
        }

        private (double[] x_vector, double[] y_vector) LinearRegressionCalculator(double[] coefficients, double[] original_x_vector, double[] original_y_vector)
        {
            Vector<double>[] a_column_array = new Vector[coefficients.Length];

            for (int i = 0; i < coefficients.Length; i++)
            {
                Vector<double> v = Vector<double>.Build.Dense(10);

                a_column_array[i] = v;
            }

            var M = Matrix<double>.Build.DenseOfColumnVectors(v);

            return (x_vector, y_vector);
        }

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

    public class LinearRegressionGraph 
    {
        public double[] coefficients;

        public string name;
    }
}
