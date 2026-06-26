using System;
using System.Collections.Generic;
using System.Text;

namespace marketmanagement.models
{
    internal class stock
    {
        public int id { get; set; }
        public int productid { get; set; }
        public int lowstockthreshold { get; set; }
        public string status { get; set; }
    }
}
