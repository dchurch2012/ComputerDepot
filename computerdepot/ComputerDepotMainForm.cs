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
using System.Configuration;


namespace computerdepot
{
    public partial class ComputerDepotMainForm : Form
    {
        private String magic_value = "12345678";
        public SqlConnection sqlComputerDepotConnection = null;
        protected Customers cust = null;
        ErrorReporter errs = null;
        
        //External Forms to be launched
        protected SupplierDetails m_Suppliers = null;
        protected EmployeeInformation m_Employees = null;
        protected Inventory m_Inventory = null;
        protected Reports m_Reports = null;
        
        public ComputerDepotMainForm()
        {
            InitializeComponent();
            errs = new ErrorReporter(); 

            try
            {
                //Define an object from SqlConnection
                sqlComputerDepotConnection = new SqlConnection();
                sqlComputerDepotConnection.ConnectionString = Properties.Settings.Default.DBConnection;

                try
                {
                    sqlComputerDepotConnection.Open();
                }
                catch (Exception except)
                {
                    errs.ReportException(except); 
                }
            }
            catch (Exception except)
            {
                errs.ReportException(except); 
            }
            DisableControls();
        }

        protected void EnableControls()
        {
            btnCustomers.Enabled = true;
            btnEmployees.Enabled = true;
            btnInventory.Enabled = true;
            btnReports.Enabled = true;
            btnSuppliers.Enabled  = true;
        }
   
        protected void DisableControls()
        {
            btnCustomers.Enabled = false;
            btnEmployees.Enabled = false;
            btnInventory.Enabled = false;
            btnReports.Enabled = false;
            btnSuppliers.Enabled = false;
        }
        
        protected bool LogOnUser(String UserID)
        {
            if (UserID == magic_value)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

           
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 )
            {
                if (LogOnUser(textBox1.Text))
                {
                    EnableControls();
                }
                else
                {
                    MessageBox.Show("Invalid User ID entered please try again");
                }

            }

        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            CustomerForm CustomerInfoForm = new CustomerForm(sqlComputerDepotConnection);
            CustomerInfoForm.Show();
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            m_Suppliers = new SupplierDetails();
            m_Suppliers.Show();
        }

        private void ComputerDepotMainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            m_Employees = new EmployeeInformation(sqlComputerDepotConnection);
            m_Employees.Show(); 
 
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            m_Inventory = new Inventory(sqlComputerDepotConnection);
            m_Inventory.Show();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            m_Reports = new Reports(sqlComputerDepotConnection);
            m_Reports.Show();
        }
    }
}
