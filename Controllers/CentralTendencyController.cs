using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; // Para serialización/deserialización de JSON
using StatisticalCalculator.Models;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalCalculator.Controllers
{
    public class CentralTendencyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Acción para datos nominales
        public IActionResult InputNominal()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessNominalData(int count)
        {
            if (count <= 0)
            {
                ModelState.AddModelError("", "La cantidad debe ser mayor a 0.");
                return View("InputNominal");
            }

            TempData["Count"] = count;
            return RedirectToAction("EnterData");
        }

        public IActionResult EnterData()
        {
            var count = TempData["Count"] as int?;
            if (count == null)
            {
                return RedirectToAction("InputNominal");
            }
            return View(count.Value);
        }

        [HttpPost]
        public IActionResult SubmitData(List<double> data, string distributionType)
        {
            if (data == null || !data.Any())
            {
                ModelState.AddModelError("", "Los datos no pueden estar vacíos.");
                return View("EnterData");
            }

            if (string.IsNullOrEmpty(distributionType))
            {
                ModelState.AddModelError("", "Debes seleccionar un tipo de distribución.");
                return View("EnterData");
            }

            // Guardar datos y tipo de distribución en TempData
            TempData["Data"] = JsonConvert.SerializeObject(data);
            TempData["DistributionType"] = distributionType;

            return RedirectToAction("CalculateCentralTendency");
        }

        // Acción para calcular las medidas de tendencia central con datos nominales
        public IActionResult CalculateCentralTendency()
        {
            var dataJson = TempData["Data"] as string;
            var data = JsonConvert.DeserializeObject<List<double>>(dataJson);
            var distributionType = TempData["DistributionType"] as string;

            var results = new DistributionResultsModel
            {
                DistributionType = distributionType,
                Mean = CalculateMean(data),
                StdDev = CalculateStdDev(data),
                Median = CalculateMedian(data),
                Mode = CalculateMode(data),
                Q1 = CalculateQuartile(data, 1),
                Q3 = CalculateQuartile(data, 3),
                Interpretation = "Resultados interpretativos según el cálculo.",
                Data = data // Guardar los datos para el histograma
            };

            return View("DistributionResults", results);
        }

        // Nueva acción para datos consolidados
        public IActionResult InputConsolidated()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessConsolidatedData(string consolidatedData, string distributionType)
        {
            if (string.IsNullOrEmpty(consolidatedData))
            {
                ModelState.AddModelError("", "Los datos consolidados no pueden estar vacíos.");
                return View("InputConsolidated");
            }

            if (string.IsNullOrEmpty(distributionType))
            {
                ModelState.AddModelError("", "Debes seleccionar un tipo de distribución.");
                return View("InputConsolidated");
            }

            // Convertir los datos consolidados en una lista de números
            var data = consolidatedData.Split(',')
                .Select(d => double.TryParse(d.Trim(), out var num) ? num : (double?)null)
                .Where(d => d.HasValue)
                .Select(d => d.Value)
                .ToList();

            if (!data.Any())
            {
                ModelState.AddModelError("", "No se pudieron procesar los datos.");
                return View("InputConsolidated");
            }

            // Guardar datos y tipo de distribución en TempData
            TempData["Data"] = JsonConvert.SerializeObject(data);
            TempData["DistributionType"] = distributionType;

            return RedirectToAction("CalculateCentralTendency");
        }

        // Métodos de cálculo
        private double CalculateMean(List<double> data)
        {
            return data.Average();
        }

        private double CalculateStdDev(List<double> data)
        {
            double mean = CalculateMean(data);
            return Math.Sqrt(data.Sum(d => Math.Pow(d - mean, 2)) / data.Count);
        }

        private double CalculateMedian(List<double> data)
        {
            var sortedData = data.OrderBy(d => d).ToList();
            int count = sortedData.Count;
            if (count % 2 == 0)
            {
                return (sortedData[count / 2 - 1] + sortedData[count / 2]) / 2;
            }
            return sortedData[count / 2];
        }

        private double CalculateMode(List<double> data)
        {
            return data.GroupBy(x => x)
                       .OrderByDescending(g => g.Count())
                       .First().Key;
        }

        private double CalculateQuartile(List<double> data, int quartile)
        {
            var sortedData = data.OrderBy(d => d).ToList();
            int n = sortedData.Count;
            if (quartile == 1)
            {
                return sortedData[(int)(n * 0.25)];
            }
            else if (quartile == 3)
            {
                return sortedData[(int)(n * 0.75)];
            }
            return 0;
        }
    }
}




