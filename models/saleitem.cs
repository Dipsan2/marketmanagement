using System;
using System.Collections.Generic;
using System.Text;

namespace marketmanagement.models
{
    internal class saleitem
    {
        public int id { get; set; }
        public int saleid { get; set; }
        public int productid { get; set; }
        public int quantity { get; set; }
        public decimal unitprice { get; set; }
    }
}
