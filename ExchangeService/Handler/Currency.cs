using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExchangeService.Handler
{
    public class Currency
    {
        public string selling;
        public string buying;
        public string bankName;
        public Currency()
        {

        }
        public Currency(string selling, string buying, string bankName)
        {
            this.buying = buying;
            this.selling = selling;
            this.bankName = bankName;
        }
    }
}