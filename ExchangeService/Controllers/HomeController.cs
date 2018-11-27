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

        // GET: Home
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
            Currency cr1 = new Currency();
            Currency cr2 = new Currency();
            Currency cr3 = new Currency();
            cr1 = pbh.returnPrivatBankCourses(int.Parse(firstCurrency), int.Parse(secondCurrency),"ПриватБанк");
            cr2 = pbh.returnOshadBankCourses(int.Parse(firstCurrency), int.Parse(secondCurrency),"Ощадбанк");
            cr3 = pbh.returnPUMBCourses(int.Parse(firstCurrency), int.Parse(secondCurrency),"ПУМб");
            var list = new List<Currency>();
            list.Add(cr1);
            list.Add(cr2);
            list.Add(cr3);

            return View("Results",list);
        }
    }
    
}