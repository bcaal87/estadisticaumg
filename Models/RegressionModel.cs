using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalCalculator.Models
{
    public class RegressionModel
    {
        public List<double> X { get; set; }
        public List<double> Y { get; set; }
        public double Slope { get; set; } // Pendiente
        public double Intercept { get; set; } // Intersección
        public double CorrelationCoefficient { get; set; } // Coeficiente de correlación
        public double DeterminationCoefficient { get; set; } // Coeficiente de determinación
        public string Procedure { get; set; } // Descripción del procedimiento

        // Método para calcular la regresión por mínimos cuadrados
        public void CalculateRegression()
        {
            if (X.Count != Y.Count || X.Count == 0)
            {
                throw new InvalidOperationException("Las listas X e Y deben tener el mismo tamaño y no deben estar vacías.");
            }

            double meanX = X.Average();
            double meanY = Y.Average();

            double sumXY = X.Zip(Y, (x, y) => (x - meanX) * (y - meanY)).Sum();
            double sumXX = X.Select(x => (x - meanX) * (x - meanX)).Sum();

            // Evitar división por cero
            if (sumXX == 0)
            {
                Slope = double.NaN;
                Intercept = double.NaN;
                CorrelationCoefficient = 0;
                DeterminationCoefficient = 0;
                return;
            }

            Slope = sumXY / sumXX;
            Intercept = meanY - Slope * meanX;

            // Calcular el coeficiente de correlación
            CorrelationCoefficient = CalculateCorrelationCoefficient();
            DeterminationCoefficient = CorrelationCoefficient * CorrelationCoefficient;
        }

        // Método para calcular el coeficiente de correlación
        public double CalculateCorrelationCoefficient()
        {
            double meanX = X.Average();
            double meanY = Y.Average();

            double numerator = X.Zip(Y, (x, y) => (x - meanX) * (y - meanY)).Sum();
            double denominatorX = Math.Sqrt(X.Sum(x => (x - meanX) * (x - meanX)));
            double denominatorY = Math.Sqrt(Y.Sum(y => (y - meanY) * (y - meanY)));

            if (denominatorX == 0 || denominatorY == 0)
            {
                return 0;
            }

            return numerator / (denominatorX * denominatorY);
        }
    }
}





