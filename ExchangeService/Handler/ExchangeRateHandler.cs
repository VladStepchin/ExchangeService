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
        public string data;

        public int firstCurrencyCode;

        public int secondCurrencyCode;
        
        public List<Currency> getCalculateExchangeRates(int firstCurrencyCode, int secondCurrencyCode) {
            var exchangeRates = new List<Currency>();
            exchangeRates.Add(returnOshadBankCourses(firstCurrencyCode,secondCurrencyCode, "https://api.privatbank.ua", @"(\d{2}.\d{4})","ПриватБанк")); 
            exchangeRates.Add(returnOshadBankCourses(firstCurrencyCode, secondCurrencyCode, @"https://kurs.com.ua/bank/88-oshchadbank/", @"<span class='ipsKurs_rate'>(\d{2}.\d{4})</span>", "Ощадбанк"));
            exchangeRates.Add(returnOshadBankCourses(firstCurrencyCode, secondCurrencyCode, @"https://about.pumb.ua/ru/info/currency_converter", @"<td>(\d{2}.\d{2})</td>", "ПУМБ"));
            return exchangeRates;
        }
        public Currency calculateExchangeRate(string data, int firstCurrencyCode, int secondCurrencyCode, string pattern)
        {
            if (firstCurrencyCode == secondCurrencyCode)
            {
                Currency eq = new Currency("1","1",null);
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
        public Currency returnOshadBankCourses(int firstCurrencyCode, int secondCurrencyCode,string URL, string pattern, string bank) {

            if (bank =="ПриватБанк") {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var responseFromAsync = client.GetAsync("/p24api/pubinfo?json&exchange&coursid=5").Result;
                string res = "";

                HttpContent content = responseFromAsync.Content;
                Task<string> result = content.ReadAsStringAsync();

                res = result.Result;
                return finilizeCurrencyExchange(res, firstCurrencyCode, secondCurrencyCode, pattern, bank);
            }

            WebRequest request;
            request = WebRequest.Create(URL);
            var response = request.GetResponse();


            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);

            string data = reader.ReadToEnd();
            
            return finilizeCurrencyExchange(data, firstCurrencyCode, secondCurrencyCode,pattern,bank);
        }
        public Currency finilizeCurrencyExchange(string data, int firstCurrencyCode, int secondCurrencyCode, string pattern, string bank) {
            Currency cur = new Currency();
            cur = calculateExchangeRate(data, firstCurrencyCode, secondCurrencyCode, pattern);
            cur.bankName = bank;
            return cur;
        }
    }
}