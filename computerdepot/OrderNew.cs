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
        protected Products m_products = null;

        protected TreeNode[] m_node = null;
        protected TreeNode m_root = null;
        protected TreeNode sub_cat_node = null;
        protected TreeNode cat_node = null;

        protected SqlConnection m_sqlComputerDepotConnection = null;

        public OrderNew()
        {
            InitializeComponent();
            InitializeLocalVars();

            errs = new ErrorReporter();

            m_products = new Products(m_sqlComputerDepotConnection);

            foreach (Product prod in m_products.Product)
            {
                System.Diagnostics.Debug.WriteLine(prod.CategoryName);
                AddAllNodes(prod);
            }
        }

        public OrderNew(SqlConnection sqlComputerDepotConnection)
        {
            try
            {
                InitializeComponent();
                InitializeLocalVars();

                errs = new ErrorReporter();

                m_sqlComputerDepotConnection = sqlComputerDepotConnection;

                m_products = new Products(m_sqlComputerDepotConnection);
                m_products.Read();

                foreach (Product prod in m_products.Product)
                {
                    System.Diagnostics.Debug.WriteLine(prod.CategoryName);
                    AddAllNodes(prod);
                }
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

                case 2:
                    AddAllServiceNodes(prod);
                    break;

                default:
                    break;
            }
            return 0;
        }

        protected int AddAllServiceNodes(Product prod)
        {
            AddServiceNode(prod);
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

        protected int AddServiceNode(Product prod)
        {
            TreeNode service_node = m_node[1].Nodes.Add(prod.DomainName);
            service_node.Name = prod.ProductID.ToString(); 
            return 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {

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
    }
}
