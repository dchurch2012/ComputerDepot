using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerdepot
{
    public class Inventory
    {
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string ProdDescrip { get; set;}
        public double ProdPrice { get; set; }
        public string Mfr { get; set; }
        public int Qty { get; set; }
        public DateTime StockedDate { get; set; }
        public int ReorderLevel { get; set; }
        public string ProductType { get; set; }
        public string CatalogID { get; set; }
        public int ProductTypeID { get; set; }

    }
}
