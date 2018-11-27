using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExchangeService.Handler
{
    public class Currency
    {
        public string selling;
        public string buying;
        public Currency()
        {

        }
        public Currency(string selling, string buying)
        {
            this.buying = buying;
            this.selling = selling;
        }
    }
}