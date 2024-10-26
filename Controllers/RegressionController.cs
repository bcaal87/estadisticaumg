using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StatisticalCalculator.Models;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalCalculator.Controllers
{
    public class RegressionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult InputRegression()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessRegressionData(string xValues, string yValues, string method)
        {
            if (string.IsNullOrWhiteSpace(xValues) || string.IsNullOrWhiteSpace(yValues))
            {
                ModelState.AddModelError("", "Los valores no pueden estar vacíos.");
                return View("InputRegression");
            }

            List<double> xList;
            List<double> yList;

            // Intentar convertir las cadenas a listas de doubles
            try
            {
                xList = xValues.Split(',')
                               .Select(v => double.Parse(v.Trim()))
                               .ToList();
                yList = yValues.Split(',')
                               .Select(v => double.Parse(v.Trim()))
                               .ToList();
            }
            catch (FormatException)
            {
                ModelState.AddModelError("", "Por favor, asegúrese de que los valores sean numéricos.");
                return View("InputRegression");
            }

            if (xList.Count != yList.Count)
            {
                ModelState.AddModelError("", "Las listas de valores X e Y deben tener el mismo tamaño.");
                return View("InputRegression");
            }

            // Inicializar el modelo de regresión
            RegressionModel regressionModel = new RegressionModel
            {
                X = xList,
                Y = yList
            };

            // Calcular la regresión según el método elegido
            if (method == "LeastSquares")
            {
                regressionModel.CalculateRegression();
                regressionModel.Procedure = "Cálculo realizado con la fórmula de mínimos cuadrados.";
            }
            else if (method == "Covariance")
            {
                CalculateCovariance(regressionModel);
                regressionModel.Procedure = "Cálculo realizado con la fórmula de covarianza.";
            }
            else
            {
                ModelState.AddModelError("", "Método no válido.");
                return View("InputRegression");
            }

            // Convertir los resultados a JSON para pasarlos a la vista
            TempData["RegressionResults"] = JsonConvert.SerializeObject(regressionModel);
            return RedirectToAction("DisplayRegressionResults");
        }

        public IActionResult DisplayRegressionResults()
        {
            var resultsJson = TempData["RegressionResults"] as string;
            var results = JsonConvert.DeserializeObject<RegressionModel>(resultsJson);

            return View(results);
        }

        // Método de cálculo usando Covarianza
        private void CalculateCovariance(RegressionModel model)
        {
            double meanX = model.X.Average();
            double meanY = model.Y.Average();

            // Calcular la covarianza
            double covariance = model.X.Zip(model.Y, (x, y) => (x - meanX) * (y - meanY)).Sum();
            double varianceX = model.X.Select(x => (x - meanX) * (x - meanX)).Sum();

            // Validar si hay división por cero
            if (varianceX == 0)
            {
                model.Slope = double.NaN;
                model.Intercept = double.NaN;
                model.CorrelationCoefficient = 0;
                model.DeterminationCoefficient = 0;
                model.Procedure += " - No se puede calcular la pendiente ya que la varianza de X es 0.";
                return;
            }

            model.Slope = covariance / varianceX;
            model.Intercept = meanY - model.Slope * meanX;

            // Calcular el coeficiente de correlación
            model.CorrelationCoefficient = model.CalculateCorrelationCoefficient();
            model.DeterminationCoefficient = model.CorrelationCoefficient * model.CorrelationCoefficient;
        }
    }
}

















