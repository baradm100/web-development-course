using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_development_course.WebServices;

namespace web_development_course.Controllers
{
    public class UtilsController : Controller
    {

        private CurrencyConverter ct;

        public UtilsController(CurrencyConverter currencyConverter) {
            ct = currencyConverter;
        }

        // GET: /currency/json
        [HttpGet]
        [Route("currency/json")]
        public IActionResult GetCurrency(string currency)
        {
            // dollar is equal to 1, its the default value in app
            double currencyValue = 1;

            if (currency == "GBP")
                currencyValue = ct.Gbp;
            else if (currency == "EUR")
                currencyValue = ct.Eur;
            else if (currency == "ILS")
                currencyValue = ct.Ils;
            return Json(new { success = true, value = currencyValue });
        }
    }
}
