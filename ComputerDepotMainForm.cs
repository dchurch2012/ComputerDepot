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
        public SqlConnection sqlComputerDepotConnection = null;
        protected Customers cust = null;
        ErrorReporter errs = null;
        
        //External Forms to be launched
        protected SupplierDetails m_Suppliers = null;
        protected EmployeeInformation m_Employees = null;
        protected InventoryForm m_Inventory = null;
        protected ReportingForm m_Reports = null;
        
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
        
        protected bool LogOnUser(int UserID)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            bool error = true;
            int EIN = -1;

            try
            {
                //CREATE PROCEDURE [dbo].[CheckForEmployeeExistence]
                command = new SqlCommand("CheckForEmployeeExistence", sqlComputerDepotConnection);
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.Add("@EIN", SqlDbType.Int).Value = UserID;


                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.IsDBNull(reader.GetOrdinal("EIN")))
                    {
                        EIN = -1;
                    }
                    else
                    {
                        EIN =  reader.GetInt32(reader.GetOrdinal("EIN"));
                    }
                }

                if (EIN != -1)
                {
                    error = true;
                }
                else
                {
                   error = false;
                }
            }
            catch (Exception except)
            {
                errs.ReportException (except); 
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
            return error;
        }

           
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 )
            {
                if (LogOnUser(Convert.ToInt32(txtEmployeeID.Text)))
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
            int employee_id = Convert.ToInt32(txtEmployeeID.Text);

            CustomerForm CustomerInfoForm = new CustomerForm(sqlComputerDepotConnection, employee_id);
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
            m_Inventory = new InventoryForm(sqlComputerDepotConnection);
            m_Inventory.Show();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            int report_id = 0;

            m_Reports = new ReportingForm(sqlComputerDepotConnection,report_id);
            m_Reports.Show();
        }
    }
}
