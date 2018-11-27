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

namespace ExchangeService.Handler
{
    public class ExchangeRateHandler
    {
        public Currency calculateExchangeRate(string data, int firstCurrencyCode, int secondCurrencyCode, string pattern)
        {
            if (firstCurrencyCode == secondCurrencyCode)
            {
                Currency eq = new Currency();
                eq.buying = "1";
                eq.selling = "1";
                return eq;
            }

            Currency cur = new Currency();
            string curPattern = pattern;
            Regex regex = new Regex(curPattern);
            Match match = regex.Match(data);

            if (firstCurrencyCode == 0)
            {
                if (secondCurrencyCode == 1)
                {
                    double tmp1 = 0, tmp2 = 0;
                    string buf = match.Groups[1].Value.Replace(".", ",");
                    double.TryParse(buf, out tmp1);
                    cur.buying = (1 / tmp1).ToString();
                    match = match.NextMatch();
                    buf = match.Groups[1].Value.Replace(".", ",");
                    double.TryParse(buf, out tmp2);
                    cur.selling = (1 / tmp2).ToString();
                    return cur;
                }
                if (secondCurrencyCode == 2)
                {
                    match = match.NextMatch();
                    match = match.NextMatch();
                    double tmp1 = 0, tmp2 = 0;
                    string buf = match.Groups[1].Value.Replace(".", ",");
                    double.TryParse(buf, out tmp1);
                    cur.buying = (1 / tmp1).ToString();
                    match = match.NextMatch();
                    buf = match.Groups[1].Value.Replace(".", ",");
                    double.TryParse(buf, out tmp2);
                    cur.selling = (1 / tmp2).ToString();
                }
            }

            if (firstCurrencyCode == 1)
            {
                if (secondCurrencyCode == 0)
                {
                    cur.buying = match.Groups[1].Value;
                    match = match.NextMatch();
                    cur.selling = match.Groups[1].Value;
                    return cur;
                }
                if (secondCurrencyCode == 2)
                {
                    double tmp1 = 0, tmp2 = 0;
                    string buf = match.Groups[1].Value.Replace(".", ",");
                    double dollarBuy = 1;
                    double.TryParse(buf, out dollarBuy);
                    match = match.NextMatch();
                    buf = match.Groups[1].Value.Replace(".", ",");
                    double dollarSell = 1;
                    double.TryParse(buf, out dollarSell);
                    match = match.NextMatch();
                    buf = match.Groups[1].Value.Replace(".", ",");
                    double.TryParse(buf, out tmp1);
                    cur.buying = (dollarBuy / tmp1).ToString();
                    match = match.NextMatch();
                    buf = match.Groups[1].Value.Replace(".", ",");
                    double.TryParse(buf, out tmp2);
                    cur.selling = (dollarSell / tmp2).ToString();
                    return cur;
                }
            }
            if (firstCurrencyCode == 2)
            {
                if (secondCurrencyCode == 0)
                {
                    match = match.NextMatch();
                    match = match.NextMatch();
                    cur.buying = match.Groups[1].Value;
                    match = match.NextMatch();
                    cur.selling = match.Groups[1].Value;
                    return cur;
                }
                if (secondCurrencyCode == 1)
                {
                    double dt = 0, dt2 = 0, et = 0, et2 = 0;
                    string buf = match.Groups[1].Value.Replace(".", ",");
                    double.TryParse(buf, out dt);
                    match = match.NextMatch();
                    buf = match.Groups[1].Value.Replace(".", ",");
                    double.TryParse(buf, out dt2);
                    match = match.NextMatch();
                    buf = match.Groups[1].Value.Replace(".", ",");
                    double.TryParse(buf, out et);
                    match = match.NextMatch();
                    buf = match.Groups[1].Value.Replace(".", ",");
                    double.TryParse(buf, out et2);
                    cur.buying = (et / dt).ToString();
                    cur.selling = (et2 / dt2).ToString();
                    return cur;
                }
            }
            return cur;
        }
        public Currency returnPrivatBankCourses(int firstCurrencyCode, int secondCurrencyCode, string bank) {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.privatbank.ua");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync("/p24api/pubinfo?json&exchange&coursid=5").Result;
            string res = "";

            HttpContent content = response.Content;
            Task<string> result = content.ReadAsStringAsync();

            res = result.Result;
            Currency cur = new Currency();
            cur = calculateExchangeRate(res, firstCurrencyCode, secondCurrencyCode, @"(\d{2}.\d{4})");
            cur.bankName = bank;
            return cur;
        }

        public Currency returnOshadBankCourses(int firstCurrencyCode, int secondCurrencyCode, string bank)
        {
            WebRequest request;
            request = WebRequest.Create(@"https://kurs.com.ua/bank/88-oshchadbank/");
            var response = request.GetResponse();


            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);

            string data = reader.ReadToEnd();
            Currency cur = new Currency();
            cur = calculateExchangeRate(data, firstCurrencyCode, secondCurrencyCode, @"<span class='ipsKurs_rate'>(\d{2}.\d{4})</span>");
            cur.bankName = bank;
            return cur;
        }

        public Currency returnPUMBCourses(int firstCurrencyCode, int secondCurrencyCode, string bank)
        {
            WebRequest request;
            request = WebRequest.Create(@"https://about.pumb.ua/ru/info/currency_converter");
            var response = request.GetResponse();


            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);

            string data = reader.ReadToEnd();
            Currency cur = new Currency();
            cur = calculateExchangeRate(data, firstCurrencyCode, secondCurrencyCode, @"<td>(\d{2}.\d{2})</td>");
            cur.bankName = bank;
            return cur;
        }


    }
}