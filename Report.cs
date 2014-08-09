using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerdepot
{
    public class Report
    {
        public virtual int ID { get; set; }
        public virtual double Count { get; set; }
        public virtual double IDCount { get; set; }
        public virtual double Frequency { get; set; }
        public virtual string Description { get; set;}

        public virtual double Price { get; set; }	
        public virtual DateTime TransactionDate	{ get; set; }
        public virtual double Total	{ get; set; }
        public virtual double SalesPerUnitTime { get; set; }
        public virtual double CountPerUnitTime { get; set; }
       
        public virtual int Create()
        {
            return 0;
        }

    }
}
