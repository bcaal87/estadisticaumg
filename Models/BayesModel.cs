using System;
using System.Collections.Generic;

namespace StatisticalUMG.Models
{
    public class BayesModel
    {
        public double PriorProbability { get; set; }         // P(A)
        public double Likelihood { get; set; }               // P(B|A)
        public double MarginalProbability { get; set; }      // P(B)
        public double PosteriorProbability { get; set; }     // P(A|B)
        public List<string> CalculationSteps { get; set; }   // Procedimiento paso a paso
        public List<string> ProbabilityTree { get; set; }    // Representación del árbol

        public void CalculatePosteriorProbability()
        {
            // Calcular la probabilidad posterior usando el Teorema de Bayes
            PosteriorProbability = (Likelihood * PriorProbability) / MarginalProbability;

            // Definir los pasos del cálculo
            CalculationSteps = new List<string>
            {
                $"Paso 1: Probabilidad Previa (P(A)) = {PriorProbability}",
                $"Paso 2: Probabilidad Condicional (P(B|A)) = {Likelihood}",
                $"Paso 3: Probabilidad Marginal (P(B)) = {MarginalProbability}",
                $"Paso 4: Probabilidad Posterior (P(A|B)) = (P(B|A) * P(A)) / P(B)",
                $"Resultado: P(A|B) = {PosteriorProbability}"
            };

            // Crear el árbol de probabilidades
            ProbabilityTree = new List<string>
            {
                "Nodo raíz: Probabilidad inicial",
                $" ├── P(A) = {PriorProbability}",
                $" ├── P(B|A) = {Likelihood}",
                $" └── P(B) = {MarginalProbability}"
            };
        }
    }
}

