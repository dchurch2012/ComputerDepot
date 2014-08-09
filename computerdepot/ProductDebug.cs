using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerdepot
{
    public class ProductDebug
    {
        public ProductDebug(int domainID, string domainName, int catID, string catName, int subcatID, string sub_catName, int prodID, string prodName, string prodDesc)
        {
            CategoryID 	= catID ;
            CategoryName = catName; 
            SubCategory	= sub_catName; 
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
        
        public string SubCategory { get; set; }
        public int SubCategoryID { get; set; }
        
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
    }
}
