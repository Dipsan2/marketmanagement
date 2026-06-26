using System;
using System.Collections.Generic;
using System.Text;

namespace marketmanagement.models
{
    public class product
    {
        public int id { get; set; }
        public string title { get; set; }
        public string barcode { get; set; }
        public decimal price { get; set; }
        public int quantityinstock { get; set; }
        public DateTime expiryorrestockdate { get; set; }
        public int categoryid { get; set; }
        public int supplierid { get; set; }
    }
}
