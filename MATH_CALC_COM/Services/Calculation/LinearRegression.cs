using DevExpress.Utils;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.FSharp.Core;
using Plotly.NET;
using Plotly.NET.LayoutObjects;
using Plotly.NET.TraceObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using static Plotly.NET.StyleParam.Range;
using static Plotly.NET.StyleParam.TextAngle;


namespace MATH_CALC_COM.Services.Calculation
{
    public class LinearRegression
    {
        public string LinearRegressionPlotter(LinearRegressionGraph[] graphs, double[] x_vector, double[] y_vector)
        {
            var chartList = new List<GenericChart>();

            chartList.Add(Chart2D.Chart.Scatter<double, double, string>(X: x_vector, Y: y_vector, Name: "Values", Mode: StyleParam.Mode.Markers_Text));

            foreach (LinearRegressionGraph graph in graphs)
            {
                var retVals = LinearRegressionCalculator(graph.degree ?? 1, x_vector, y_vector);

                var chartY = Chart2D.Chart.Scatter<double, double, string>(X: retVals.x_vector, Y: retVals.y_vector, Name: graph.name, Mode: StyleParam.Mode.Lines_Markers_Text);

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
            //degree etc

            Vector<double>[] a_row_array = new Vector[original_x_vector.Length];

            for (int i = 0; i < original_x_vector.Length; i++)
            {
                double[] values = new double[degree + 1];

                for (int j = 0; j < degree + 1; j++)
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

            int segments = 20;

            double[] x_vector = new double[segments + 1];

            double delta_x = (original_x_vector[original_x_vector.Length - 1] - original_x_vector[0]) / (double)segments;

            for (int i = 0; i < x_vector.Length; i++)
            {
                if (i == 0)
                {
                    x_vector[i] = original_x_vector[0];
                }
                else
                {
                    x_vector[i] = x_vector[i - 1] + delta_x;
                }
            }

            //degree 1: a0 + a1*t
            //degree 2: a0 + a1*t + a2*(t^2)
            //degree etc

            double[] y_vector = new double[x_vector.Length];

            for (int i = 0; i < x_vector.Length; i++)
            {
                for (int j = 0; j <= degree; j++)
                {
                    double term = coefficients[j];

                    for (int k = 0; k < j; k++)
                    {
                        term *= x_vector[i];
                    }

                    y_vector[i] += term;
                }
            }

            return (x_vector, y_vector);
        }

        private (double a, double b, double c) LinearRegressioPlaneCalculator(
            double offset,
            double[] original_x_vector,
            double[] original_y_vector,
            double[] original_z_vector)
        {
            //z = ax + by + c

            Vector<double>[] a_row_array = new Vector[3];

            Vector<double> v_x = Vector<double>.Build.DenseOfArray(original_x_vector);

            a_row_array[0] = v_x;

            Vector<double> v_y = Vector<double>.Build.DenseOfArray(original_y_vector);

            a_row_array[1] = v_y;

            double[] values_1 = new double[original_x_vector.Length];

            for (int j = 0; j < original_x_vector.Length; j++)
            {
                values_1[j] = 1.0;
            }

            Vector<double> v_1 = Vector<double>.Build.DenseOfArray(values_1);

            a_row_array[2] = v_1;

            var A = Matrix<double>.Build.DenseOfColumns(a_row_array);

            var QR = A.QR(MathNet.Numerics.LinearAlgebra.Factorization.QRMethod.Full);

            Vector<double> b = Vector<double>.Build.DenseOfArray(original_z_vector);

            var Q_transposed_b = QR.Q.Transpose().Multiply(b);

            var coefficients = QR.R.Solve(Q_transposed_b);

            double a = coefficients[0];
            double b_val = coefficients[1];
            double c = coefficients[2] + offset;

            return (a, b_val, c);
        }

        private (double[] xs, double[] ys, double[] zs) CreatePlaneMesh(
    double a, double b, double c,
    double[] x_vector,
    double[] y_vector,
    int resolution = 20)
        {
            double x_min = x_vector.Min();
            double x_max = x_vector.Max();
            double y_min = y_vector.Min();
            double y_max = y_vector.Max();

            double[] xs = new double[resolution * resolution];
            double[] ys = new double[resolution * resolution];
            double[] zs = new double[resolution * resolution];

            int index = 0;

            for (int i = 0; i < resolution; i++)
            {
                double x = x_min + (x_max - x_min) * (i / (double)(resolution - 1));

                for (int j = 0; j < resolution; j++)
                {
                    double y = y_min + (y_max - y_min) * (j / (double)(resolution - 1));

                    xs[index] = x;
                    ys[index] = y;
                    zs[index] = a * x + b * y + c;

                    index++;
                }
            }

            return (xs, ys, zs);
        }


        public string LinearRegressionPlanePlotter(
            double offset,
            LinearRegressionGraph[] graphs,
            double[] x_vector,
            double[] y_vector,
            double[] z_vector
        )
        {
            var chartList = new List<GenericChart>();

            // 1. Add Original 3D Points to the chart list
            var scatter3D = Chart3D.Chart.Scatter3D<double, double, double, string>(
               x_vector,
               y_vector,
               z_vector,
               StyleParam.Mode.Markers,
               Name: "Original Points"
            );
            chartList.Add(scatter3D);

            // 2. Calculate Plane
            var (a, b, c) = LinearRegressioPlaneCalculator(offset, x_vector, y_vector, z_vector);

            // 3. Generate Mesh
            var (xs, ys, zs) = CreatePlaneMesh(a, b, c, x_vector, y_vector);

            // Convert 1D coordinate arrays to 2D matrices for the Surface plot
            int resolution = 20; // Matches default resolution in CreatePlaneMesh
            double[][] zMatrix = Convert1DTo2DArray(zs, resolution, resolution);
            double[] xUnique = xs.Distinct().OrderBy(val => val).ToArray();
            double[] yUnique = ys.Distinct().OrderBy(val => val).ToArray();

            // 4. Plot Surface using base Trace
            var surfaceTrace = new Plotly.NET.Trace("surface");
            surfaceTrace.SetValue("z", zMatrix);
            surfaceTrace.SetValue("x", xUnique);
            surfaceTrace.SetValue("y", yUnique);
            surfaceTrace.SetValue("name", "Regression Plane");

            var surfaceChart = GenericChart.ofTraceObject(true, surfaceTrace);
            chartList.Add(surfaceChart);

            // 5. Combine and Serialize
            var combinedChart = Chart.Combine(chartList);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(
                combinedChart,
                new Newtonsoft.Json.JsonSerializerSettings
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }
            );

            return json;
        }

        private double[][] Convert1DTo2DArray(double[] flatArray, int rows, int cols)
        {
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                result[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                {
                    result[i][j] = flatArray[i * cols + j];
                }
            }
            return result;
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

        public int? degree { get; set; }
    }
}
