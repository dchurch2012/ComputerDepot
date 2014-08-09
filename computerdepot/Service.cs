using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerdepot
{
    public class Service
    {
        public int ServiceID{ get; set; }     
        public double ServiceCost { get; set; }
        public string ServiceDescrip { get; set; }
        public int ServiceOrderID { get; set; }
        public int DomainID { get; set; }
        public string DomainName { get; set; }

    }
}
