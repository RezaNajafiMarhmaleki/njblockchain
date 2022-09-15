using System;
using System.Collections.Generic;
using System.Text;

namespace njBlockChain
{
    class Trx
    {
        public string sender { set; get; }
        public string recipient { set; get; }
        public decimal amount { set; get; }
    }
}
