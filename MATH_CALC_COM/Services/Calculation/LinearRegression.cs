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
                var retVals = LinearRegressionCalculator(graph.degree, x_vector, y_vector);

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

        private (double[] x_vector, double[] y_vector) LinearRegressionCalculator(int degree, double[] original_x_vector, double[] original_y_vector)
        {
            //degree 1: a0 + a1*t
            //degree 2: a0 + a1*t + a2*(t^2)

            degree = 1;

            Vector<double>[] a_row_array = new Vector[original_x_vector.Length];

            for (int i = 0; i < original_x_vector.Length; i++)
            {
                double[] values = new double[degree + 1];

                for (int j = 0; j < degree +1; j++)
                {
                    double value = 1.0;

                    for (int k = 0; k < j; k++)
                    {
                        value *= original_x_vector[i];
                    }

                    values[j] = value;
                }

                Vector<double> v = Vector<double>.Build.DenseOfArray(values);

                a_row_array[i] = v;
            }

            var A = Matrix<double>.Build.DenseOfRows(a_row_array);

            var QR = A.QR(MathNet.Numerics.LinearAlgebra.Factorization.QRMethod.Full);

            Vector<double> b = Vector<double>.Build.DenseOfArray(original_y_vector);

            var Q_transposed_b = QR.Q.Transpose().Multiply(b);

            var coefficients = QR.R.Solve(Q_transposed_b);

            int segments = 1;

            double[] x_vector = new double[segments + 1];

            double delta_x = (original_x_vector[original_x_vector.Length - 1] - original_x_vector[0]) / (double)segments;

            for (int i = 0; i < x_vector.Length; i++)
            {
                if(i == 0)
                {
                    x_vector[i] = original_x_vector[0];
                }
                else
                {
                    x_vector[i] = x_vector[i - 1] + delta_x;
                }
            }

            double[] y_vector = new double[x_vector.Length];

            for(int i = 0; i < x_vector.Length; i++)
            {
                y_vector[i] = coefficients[0] + coefficients[1] * x_vector[i]; 
            }

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
        public string name { get; set; }

        public int degree { get; set; }
    }
}
