using Microsoft.AspNetCore.Mvc;
using StatisticalUMG.Models;

namespace StatisticalUMG.Controllers
{
    public class BayesController : Controller
    {
        public IActionResult InputBayes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CalculateBayes(double priorProbability, double likelihood, double marginalProbability)
        {
            if (priorProbability <= 0 || likelihood <= 0 || marginalProbability <= 0)
            {
                ModelState.AddModelError("", "Todos los valores deben ser mayores a 0.");
                return View("InputBayes");
            }

            // Crear modelo y calcular la probabilidad posterior
            var model = new BayesModel
            {
                PriorProbability = priorProbability,
                Likelihood = likelihood,
                MarginalProbability = marginalProbability
            };
            model.CalculatePosteriorProbability();

            return View("BayesResult", model);
        }
    }
}
