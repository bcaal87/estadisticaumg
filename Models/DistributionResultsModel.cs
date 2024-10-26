using System.Collections.Generic;

namespace StatisticalCalculator.Models
{
    public class DistributionResultsModel
    {
        public double Mean { get; set; } // Media
        public double StdDev { get; set; } // Desviación estándar
        public double Median { get; set; } // Mediana
        public double Mode { get; set; } // Moda
        public double Q1 { get; set; } // Primer cuartil
        public double Q3 { get; set; } // Tercer cuartil
        public string Interpretation { get; set; } // Interpretación de los resultados
        public List<double> Data { get; set; } // Lista de datos para el histograma
        public string DistributionType { get; set; } // Tipo de distribución
    }
}



