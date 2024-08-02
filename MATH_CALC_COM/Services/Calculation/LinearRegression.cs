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

        private double[] LinearRegressionCalculator(int degree, double[] original_x_vector, double[] original_y_vector)
        {
            //degree 1: x1 + x2*t
            //degree 2: x1 + x2*t + x3

            Vector<double>[] a_column_array = new Vector[degree + 1];

            for (int i = 0; i <= degree; i++)
            {
                if(i == 0)
                {
                    double[] values = new double[original_x_vector.Length];

                    for(int j = 0; j < original_x_vector.Length; j++)
                    {
                        values[j] = 1.0;
                    }

                    Vector<double> v = Vector<double>.Build.DenseOfArray(values);

                    a_column_array[i] = v;
                }
                else
                {
                    double[] values = new double[original_x_vector.Length];

                    for (int j = 0; j < original_x_vector.Length; j++)
                    {
                        double value = 0.0;

                        for(int k = 0; k < degree; k++)
                        {
                            value *= original_x_vector[j];
                        }

                        values[j] = value;
                    }

                    Vector<double> v = Vector<double>.Build.DenseOfArray(values);

                    a_column_array[i] = v;
                }
            }

            var A = Matrix<double>.Build.DenseOfColumnVectors(a_column_array);

            var QR = A.QR(MathNet.Numerics.LinearAlgebra.Factorization.QRMethod.Thin);

            Vector<double> b = Vector<double>.Build.DenseOfArray(original_y_vector);

            var Q_transposed_b = QR.Q.Transpose().Multiply(b);

            var coefficients = QR.R.Solve(Q_transposed_b);

            double[] y_vector = new double[original_x_vector.Length];

            for(int i = 0;  i < original_x_vector.Length; i++)
            {
                double value = 0.0;

                for(int j = 0; j < degree; j++)
                {
                    


                }
            }

            return y_vector;
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
        public string name;

        public int degree;
    }
}
