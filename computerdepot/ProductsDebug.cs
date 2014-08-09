using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerdepot
{
    public class ProductsDebug : ProductMaster 
    {
        protected List<ProductDebug> m_products;

        public List<ProductDebug> Product
        {
            get
            {
                return this.m_products;
            }
            set
            {
                this.m_products = value;
            }
        }
     

        public ProductsDebug()
        {
            string[] s2 = new string[] { "John", "Paul", "Mary" };
            ProductDebug[] prod_array = new ProductDebug[] {
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",1,"PC Laptops",1,"Toshiba Satellite 15.6 AMD E2 Series","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",1,"PC Laptops",2,"Dell Inspiron 300 Series Intel Core i3","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",1,"PC Laptops",3,"HP Pavilion Slimline AMD A4 Series","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",1,"PC Laptops",4,"HP Pavilion AMD A8 Series","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",2,"PC Desktops",5,"Dell Inspiron 3000 Series Intel Pentium","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",2,"PC Desktops",6,"Dell Inspiron 300 Series Intel Core i3","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",2,"PC Desktops",7,"HP Pavilion Slimline AMD A4 Series","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",2,"PC Desktops",8,"HP Pavilion AMD A8 Series","None"),
                new ProductDebug(2,"SERVICES",1,"PC Hardware",2,"PC Desktops",8,"HP Pavilion AMD A8 Series","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",3,"LCD Monitors",10,"Dell P2210 Series 22","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",3,"LCD Monitors",11,"Acer Touch Series HD 27","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",4,"Keyboards",12,"Dell USB Keyboard w/ Smart Card Reader","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",4,"Keyboards",14,"Flexible USB Keyboard Waterproof","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",4,"Keyboards",15,"Alienware Tact-X Illuminated Gaming USB Keyboard","None"),
                new ProductDebug(1,"PRODUCTS",1,"PC Hardware",4,"Keyboards",16,"Microsoft Ergonomic 4000 USB Keyboard","None"),
                new ProductDebug(1,"PRODUCTS",2,"PC Software",5,"Operating Systems",17,"Microsoft OEM Windows 8.1 64-Bit","None"),
                new ProductDebug(1,"PRODUCTS",2,"PC Software",5,"Operating Systems",18,"Microsoft OEM Windows 8.1 32-Bit","None"),
                new ProductDebug(1,"PRODUCTS",2,"PC Software",5,"Operating Systems",19,"Microsoft OEM Windows 7 Premium 64-Bit","None"),
                new ProductDebug(1,"PRODUCTS",2,"PC Software",6,"Security Software",20,"Webroot SecureAnywhere Internet Security","None"),
                new ProductDebug(1,"PRODUCTS",2,"PC Software",6,"Security Software",21,"Kaspersky Internet Security 2014","None"),
                new ProductDebug(1,"PRODUCTS",2,"PC Software",6,"Security Software",22,"AVG Internet Security 2014 + PC Tune-Up","None"),
                new ProductDebug(1,"PRODUCTS",2,"PC Software",6,"Security Software",23,"Security Software","None")



            };
     
            m_products = new List<ProductDebug>();

            foreach (ProductDebug prod_debug in prod_array)
            {
                m_products.Add(prod_debug);
            }
        }
    }
}
