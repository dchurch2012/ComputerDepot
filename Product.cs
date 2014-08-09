using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerdepot
{
    public class Product
    {
        public Product()
        {
            m_categories = new List<ProductCategory>(); 
        }
     
        public Product(int domainID, string domainName, int catID, string catName, int subcatID, string sub_catName, int prodID, string prodName, string prodDesc)
        {
            m_categories = new List<ProductCategory>();
            
            CategoryID = catID;
            CategoryName = catName; 
            SubCategoryName	= sub_catName; 
            SubCategoryID = subcatID ;
            ProductID 	= prodID; 
            ProductName = prodName;
            ProductDescription = prodDesc;
            DomainID = domainID;
            DomainName = domainName;
        }

        public int DomainID { get; set; }
        public string DomainName { get; set; }

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }
        public int SubCategoryID { get; set; }

        public DateTime StockedDate { get; set; }
        public int ReorderLevel { get; set; }
        public int Qty { get; set; }
        public string ProductType { get; set; }
        public double ProductPrice { get; set; }

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string CatalogID { get; set; }
        
        public string Mfr { get; set; }

        protected List<ProductCategory> m_categories = null;

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
