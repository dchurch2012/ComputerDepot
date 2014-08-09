using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerdepot
{
    public class ProductMaster
    {
        public int ProductID { get; set; }
        public double ProductPrice { get; set; }
        public int Qty { get; set; }
        public string ProductDescrip { get; set; }
        public DateTime StockedDate { get; set; }
        public int ReorderLevel { get; set; }
        public int ProductType { get; set; }
        public int CatalogID { get; set; }
        public string Mfr { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public string SubCategoryName { get; set; }

        public int ProductTypeID { get; set; }
        public string ProductTypeDescription { get; set; }

        protected List<ProductCategory> m_categories;

        public List<ProductCategory> Categories
        {
            get
            {
                return this.m_categories;
            }
            set
            {
                this.m_categories = value;
            }
        }

    }
}
