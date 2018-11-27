using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using ExchangeService.Handler;

namespace ExchangeService.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(object o)
        {
            string firstCurrency = Request.Form["first-currency"];
            string secondCurrency = Request.Form["second-currency"];
            
            ExchangeRateHandler pbh = new ExchangeRateHandler();
            var list = new List<Currency>();
            list = pbh.getCalculateExchangeRates(int.Parse(firstCurrency), int.Parse(secondCurrency));
            return View("Results",list);
        }
    }
    
}