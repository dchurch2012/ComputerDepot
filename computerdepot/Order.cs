using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerdepot
{
    public class Order
    {
        public int OrderID { get; set; }
        public double ListPrice { get; set; }
        public double DiscountAmount { get; set;}
        public double Tax { get; set; }
        public double OrderTotalPrice { get; set;}
        public DateTime TransactionDate { get; set; }
        public int CIN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        
        public int EmployeeID{ get; set; }

        public List<Product> products_ordered { get; set;  }

        public Order()
        {
            products_ordered = new List<Product>();
        }
    }
}
