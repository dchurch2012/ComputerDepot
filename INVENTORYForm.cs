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
    public partial class InventoryForm : Form
    {
        protected SqlConnection m_sqlConnection = null;
        protected List<Inventory> m_new_products = null;
        protected ErrorReporter errs = null;
        protected Database db = null;

        protected string m_last_category_name = string.Empty;

        protected int LoadData()
        {
            try
            {
                ClearAddProductControls();

                //DisableAllControls();
                db.Read();
                m_new_products = (List<Inventory>)db.GetList();

                if (m_new_products != null)
                {
                    for (int index = 0; index < m_new_products.Count; index++)
                    {
                        if (m_last_category_name != m_new_products[index].CategoryName)
                        {
                            cbCategory.Items.Add(m_new_products[index].CategoryName);
                            m_last_category_name = m_new_products[index].CategoryName;
                        }
                    }
                }
            }
            catch (Exception except)
            {
                errs.ReportException(except);
            }
            return 0;
        }


        public InventoryForm(SqlConnection sqlConnection)
        {
            InitializeComponent();
            
            try
            {
                errs = new ErrorReporter();
                m_sqlConnection = sqlConnection;
                db = new Inventories(m_sqlConnection);

                LoadData();
     
            }
            catch (Exception except)
            {
                errs.ReportException(except);
            }
       
        }

        protected int FindSubCategoryID(string SubCategoryName)
        {
            foreach (Inventory invent in m_new_products)
            {
                if (SubCategoryName == invent.SubCategoryName)
                {
                    return invent.SubCategoryID;
                }
            }
            return -1;
        }


        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            Inventory invent = new Inventory ();

            invent.ProdDescrip = txtInventoryProdDescrip.Text;
            invent.ProdPrice = Convert.ToDouble (txtInventoryPrice.Text);
            invent.Mfr = txtMfr.Text;
            invent.CategoryName = cbCategory.SelectedText;
            //invent.SubCategoryName = cbSubCategory.SelectedText;

            //Get the selected combobox entry
            int nIndex = cbSubCategory.SelectedIndex;

            //Get the string in the combox that corresponds to the index retrieved
            invent.SubCategoryName = cbSubCategory.Items[nIndex].ToString();


            int sub_cat_id = FindSubCategoryID(invent.SubCategoryName);

            if (sub_cat_id != -1)
            {
                invent.SubCategoryID = sub_cat_id;
            }

            invent.ReorderLevel = 1;
            invent.ProductType = "1";
            invent.CatalogID = "QXR2";
            invent.ProductTypeID = 1;
            invent.StockedDate = dtStockedDate.Value.Date;
            invent.Qty = Convert.ToInt32(txtQty.Text);

            db.Create(invent);

            MessageBox.Show("1 new product has been sucessfull added to Inventory.");
        }
        
        private void ClearAddProductControls()
        {
            cbCategory.Items.Clear();
            cbSubCategory.Items.Clear();

            txtInventoryProdDescrip.Clear();
            txtInventoryPrice.Clear();

            txtMfr.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearAddProductControls();
            txtInventoryProdDescrip.Focus();
        }

        private void InventoryForm_Load(object sender, EventArgs e)
        {
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Initialize to empty string
            String category_name = String.Empty;

            //Get the selected combobox entry
            int nIndex = cbCategory.SelectedIndex;

            //Get the string in the combox that corresponds to the index retrieved
            category_name = cbCategory.Items[nIndex].ToString();
            PopulateSubCategory(category_name);
        }

        protected void PopulateSubCategory(string category_name)
        {
            cbSubCategory.Items.Clear();

            foreach (Inventory invent in m_new_products)
            {
                if (category_name == invent.CategoryName)
                {
                    cbSubCategory.Items.Add(invent.SubCategoryName);
                }
            }
        }
    }
}
