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


           

            //ОЩАДБАНК
            //WebRequest request;
            //request = WebRequest.Create(@"https://kurs.com.ua/bank/88-oshchadbank/");
            //using (var response = request.GetResponse())
            //{
            //    using (var stream = response.GetResponseStream())
            //    using (var reader = new StreamReader(stream))
            //    {
            //        string data = reader.ReadToEnd();
            //        string pattern = @"<span class='ipsKurs_rate'>(\d{2}.\d{4})</span>";
            //        Regex regex = new Regex(pattern);
            //        Match match = regex.Match(data);

            //        while (match.Success)
            //        {
            //            var a = match.Groups[1].Value;
            //            match = match.NextMatch();
            //        }
            //    }
            //}


            //УКРСИББАНК

            //WebRequest request;
            //request = WebRequest.Create(@"https://about.pumb.ua/ru/info/currency_converter");
            //using (var response = request.GetResponse())
            //{
            //    using (var stream = response.GetResponseStream())
            //    using (var reader = new StreamReader(stream))
            //    {
            //        string data = reader.ReadToEnd();
            //        string pattern = @"<td>(\d{2}.\d{2})</td>";
            //        Regex regex = new Regex(pattern);
            //        Match match = regex.Match(data);

            //        while (match.Success)
            //        {
            //            var a = match.Groups[1].Value;
            //            match = match.NextMatch();
            //        }
            //    }
            return View();
        }

        [HttpPost]
        public ActionResult Index(object o)
        {
            string firstCurrency = Request.Form["first-currency"];
            string secondCurrency = Request.Form["second-currency"];
            PrivatBankHandler pbh = new PrivatBankHandler();
            Currency cr1 = new Currency();
            cr1 = pbh.returnCourses(int.Parse(firstCurrency), int.Parse(secondCurrency));
           return PartialView("Result",cr1);
        }
    }
    
}