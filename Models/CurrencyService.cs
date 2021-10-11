using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace web_development_course.Models
{
    public class CurrencyService
    {
        private readonly Dictionary<int, string> Currencies;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrencyService(IHttpContextAccessor httpContextAccessor)
        {
            Currencies = new Dictionary<int, string>();
            Currencies.Add(1, "$");
            Currencies.Add(2, "₪");
            Currencies.Add(3, "€");
            Currencies.Add(4, "£");
            _httpContextAccessor = httpContextAccessor;
        }

        public Dictionary<int, string> GetCurrencies()
        {
            return Currencies;
        }

        public float GetCurrentCurrencyValue()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies["currency"] != null)
            {
                return float.Parse(_httpContextAccessor.HttpContext.Request.Cookies["currency"]);
            }
            // USD
            return 1;
        }

        public string GetCurrencySign()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies["currencySign"] != null)
            {
                return Currencies[int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["CurrencySign"])];
            }

            // USD
            return "$";
        }
    }
}
