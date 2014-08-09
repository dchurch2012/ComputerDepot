using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;


namespace computerdepot
{
    public partial class OrderNew : Form
    {
        //Arrays to Store node "markers" 
        // 0 ==> PRODUCTS
        // AND
        // 1 ==> SERVICES
        protected int[] m_current_category = null;
        protected int[] m_current_sub_category = null;
        protected int[] m_current_product = null;
        
        protected ErrorReporter errs = null;
        protected Database db = null;
        
        protected TreeNode[] m_node = null;
        protected TreeNode m_root = null;
        protected TreeNode sub_cat_node = null;
        protected TreeNode cat_node = null;

        protected Order_Details m_order_details = null;
        protected Customer m_current_customer = null;

        protected List<Discount> m_discount_items = null;
        protected Discount m_current_discount = null;
        protected Discounts m_discount_details = null;
      
        protected Products m_products = null;
        protected List<Product> m_order_items = null;
        protected List<Service> m_service_items = null;
      
        protected SqlConnection m_sqlComputerDepotConnection = null;

        public int EmployeeId { get; set; }

        protected Services m_services = null;
   
        public OrderNew(SqlConnection sqlComputerDepotConnection, Customer cust, int employee_id)
        {
            try
            {
                InitializeComponent();
                InitializeLocalVars();

                txtCustomerName.Text = cust.FirstName + " " + cust.LastName;
                txtCustomerName.Enabled = false;

                
                EmployeeId = employee_id;
                
                errs = new ErrorReporter();

                m_sqlComputerDepotConnection = sqlComputerDepotConnection;

                m_order_items = new List<Product>();

                m_current_customer = cust;

                m_current_discount = new Discount();
                m_discount_items = new List<Discount>();

                //Create list of Service type
                m_service_items = new List<Service>();

                //Products Related Data
                m_order_items = new List<Product>();

                m_products = new Products(m_sqlComputerDepotConnection);
                m_products.Read();

                foreach (Product prod in m_products.Product)
                {
                    System.Diagnostics.Debug.WriteLine(prod.CategoryName);
                    AddAllNodes(prod);
                }

                m_discount_details = new Discounts(m_sqlComputerDepotConnection);
                m_discount_details.Read();

                m_discount_items = m_discount_details.Discount;
                
                foreach (Discount disc in m_discount_details.Discount)
                {
                    System.Diagnostics.Debug.WriteLine(disc.DiscountAmount);
                }

                //Get All Services
                m_services = new Services(m_sqlComputerDepotConnection);
                m_services.Read();


                AddAllServiceNodes();
             

            }
            catch (Exception except)
            {
                errs.ReportException(except); 
            }
        }

        protected int InitializeLocalVars()
        {
            m_node = new TreeNode[2];

            //Allocate storage space for int arrays
            m_current_category = new int[2];
            m_current_sub_category = new int[2];
            m_current_product = new int[2];

            //Category node bookmark
            for (int jindex = 0; jindex < m_current_category.Count(); jindex++)
            {
                m_current_category[jindex] = new int();
                m_current_category[jindex] = -1;
            }

            //SubCategory node bookmark
            for (int jindex = 0; jindex < m_current_sub_category.Count(); jindex++)
            {
                m_current_sub_category[jindex] = new int();
                m_current_sub_category[jindex] = -1;
            }

            //Product node bookmark
            for (int jindex = 0; jindex < m_current_product.Count(); jindex++)
            {
                m_current_product[jindex] = new int();
                m_current_product[jindex] = -1;
            }

            //Add Tree Nodes
            for (int jindex = 0; jindex < m_node.Count(); jindex++)
            {
                m_node[jindex] = new TreeNode();
            }

            tvProducts.Nodes.Clear();
            m_node[0] = tvProducts.Nodes.Add("PRODUCTS");
            m_root = m_node[0];

            return 0;
        }

        protected int AddAllNodes(Product prod)
        {
            switch( prod.DomainID )
            {
                case 1:
                    AddAllProductNodes(prod);
                    break;

                default:
                    break;
            }
            return 0;
        }

        protected int AddAllServiceNodes()
        {
            tvServices.Nodes.Clear();
            m_node[1] = tvServices.Nodes.Add("SERVICES");
            
            foreach (Service service in m_services.ServicesList)
            {
                AddServiceNode(service);
            }
            return 0;
        }

        protected int AddAllProductNodes(Product prod)
        {
            if (m_current_category[0] != prod.CategoryID)
            {
                AddProductCategory(prod);
            }

            if (m_current_sub_category[0] != prod.SubCategoryID)
            {
                m_node[0] = AddProductSubCategory(prod);
                m_current_category[0] = prod.CategoryID;
            }

            if (m_current_product[0]!= prod.ProductID)
            {
                AddProductNode(prod);
                m_current_product[0]= prod.ProductID;
            }
            return 0;
        }
        
        protected int AddProductCategory(Product prod)
        {
            cat_node = m_root.Nodes.Add(prod.CategoryName);
      
            if (m_current_sub_category[0] != prod.SubCategoryID) 
            {
                m_node[0] = AddProductSubCategory(prod);
                m_current_category[0] = prod.CategoryID;
            }

            if (m_current_category[0] != prod.CategoryID)
            {
                AddProductCategory(prod);
            }

            if (m_current_product[0]!= prod.ProductID)
            {
                AddProductNode(prod);
                m_current_product[0]= prod.ProductID;
            } 
            return m_current_category[0];
        }

        protected TreeNode AddProductSubCategory(Product prod)
        {
            m_node[0] = cat_node.Nodes.Add(prod.SubCategoryName);
            m_node[0].Name = prod.ProductID.ToString();
 
            sub_cat_node = m_node[0];
            
            m_current_sub_category[0] = prod.SubCategoryID;

            return cat_node;
        }

        protected int AddProductNode(Product prod)
        {
            TreeNode product_node = sub_cat_node.Nodes.Add(prod.ProductDescription);
            product_node.Name = prod.ProductID.ToString(); 
            return 0;
        }

        protected int AddServiceNode(Service service)
        {
            TreeNode service_node = m_node[1].Nodes.Add(service.ServiceDescrip);
            return 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        protected Service FindService(string ServiceName)
        {
            foreach (Service serv in m_services.ServicesList )
            {
                if (serv.ServiceDescrip == ServiceName)
                    return serv;
            }
            
            return null;
        }

        private void tvServices_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                // Get the node that was clicked
                TreeNode selectedNode = tvServices.HitTest(e.Location).Node;

                Service service = null;

                if (selectedNode != null)
                {
                    // Do something with the selected node here...
                    System.Diagnostics.Debug.WriteLine(selectedNode.Text);

                    ListViewItem[] LVItem = null;
                    LVItem = new ListViewItem[1];

                    LVItem[0] = new ListViewItem();

                    service = FindService(selectedNode.Text);

                    if (service != null)
                    {
                        m_service_items.Add(service);

                        // Add Text To First ListView Item - ServiceID
                        LVItem[0].SubItems[0].Text = service.ServiceID.ToString();
                        LVItem[0].SubItems[0].Name = "Service";

                        //Add Product Name
                        LVItem[0].SubItems.Add(selectedNode.Text);
 
                        //Add Product price
                        LVItem[0].SubItems.Add(service.ServiceCost.ToString());

                        lvOrderItems.Items.AddRange(LVItem);
                    }
                }
            }
            catch (Exception except)
            {
                errs.ReportException(except);
            }

        }

        private void tvProducts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                // Get the node that was clicked
                TreeNode selectedNode = tvProducts.HitTest(e.Location).Node;

                Product prod = null;

                if (selectedNode != null)
                {
                    // Do something with the selected node here...
                    System.Diagnostics.Debug.WriteLine(selectedNode.Text);

                    ListViewItem[] LVItem = null;
                    LVItem = new ListViewItem[1];
 
                    LVItem[0] = new ListViewItem();
                    //LVItem[1] = new ListViewItem();
                    //LVItem[2] = new ListViewItem();

                    prod = FindProduct(Convert.ToInt32(selectedNode.Name));

                    if (prod != null)
                    {
                        m_order_items.Add(prod); 

                        // Add Text To First ListView Item - ProductID
                        LVItem[0].SubItems[0].Text = prod.ProductID.ToString();

                        //Add Product Name
                        LVItem[0].SubItems.Add(selectedNode.Text);

                        //Add Product price
                        LVItem[0].SubItems.Add(prod.ProductPrice.ToString());

                        lvOrderItems.Items.AddRange(LVItem);
                    }
                }
            }
            catch (Exception except)
            {
                errs.ReportException(except); 
            }
        }

        protected Product FindProduct(int ProductID)
        {
            foreach (Product prod in m_products.Product)
            {
                if (prod.ProductID == ProductID)
                    return prod;
            }
            return null;
        }

        private void OnOrderDetailsClose(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btnContinue_Click(object sender, EventArgs e)
        {
            m_order_details = new Order_Details(m_current_customer, m_order_items, m_discount_items, m_service_items,m_sqlComputerDepotConnection,EmployeeId);

            m_order_details.FormClosing += OnOrderDetailsClose;
            
            m_order_details.Show();
        }

        private void tvProducts_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(tvProducts, "Double click to add products.");
        }

        private void tvServices_MouseHover(object sender, EventArgs e)
        {
            toolTip2.SetToolTip(tvServices, "Double click to add services.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvOrderItems.SelectedItems)
            {
                item.Remove();
            }

        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
