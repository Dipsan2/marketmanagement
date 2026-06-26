using System;
using System.Collections.Generic;
using System.Text;

namespace marketmanagement.datastructures
{
    public class productnode
    {
        public marketmanagement.models.product productdata { get; set; }
        public productnode rightchild { get; set; }
        public productnode leftchild { get; set; }
        public productnode (marketmanagement.models.product item)
        {
            productdata =item;
            leftchild = null;
            rightchild =null;
        }

    }
}
