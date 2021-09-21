using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_development_course.Models;

namespace web_development_course.WebServices
{
    public class CurrencyConverter
    {
        #region Constants
        // using currencyfreaks api 
        private const string START_URL = @"https://api.currencyfreaks.com";
        private string ALL_CURRENCY = "" + CurrencyTypes.ILS + "," + CurrencyTypes.EUR + "," + CurrencyTypes.GBP;
        private const string API_KEY = "618cc1d3ccf445fda4e621a37b480e66"; // remeber to hide it from the github

        #endregion

        #region Properties

        public double Ils { get; set; }

        public double Eur { get; set; }

        public double Gbp { get; set; }

        #endregion

        public void initCurrencyRates()
        {
            // make sure to ask only for ILS, EUR, GBP the default of the api and the web site is value is USD
            string url = $"{START_URL}/latest?apikey={API_KEY}&symbols={ALL_CURRENCY}";
            var client = new RestClient(url);

            client.Timeout = -1;
            IRestRequest request = new RestRequest(Method.GET);
            var respone = client.Execute(request);

            parseRates(JObject.Parse(respone.Content)["rates"]);

        }

        // parse the return format from Json to doubles
        private void parseRates(JToken obj)
        {
           this.Ils = JsonConvert.DeserializeObject<double>(obj[CurrencyTypes.ILS.ToString()].ToString());
           this.Eur = JsonConvert.DeserializeObject<double>(obj[CurrencyTypes.EUR.ToString()].ToString());
           this.Gbp = JsonConvert.DeserializeObject<double>(obj[CurrencyTypes.GBP.ToString()].ToString());
        }

    }   
}

