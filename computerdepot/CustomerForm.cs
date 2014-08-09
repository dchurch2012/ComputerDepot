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
    public partial class CustomerForm : Form
    {
        protected Database db = null;
        protected ErrorReporter errs = null;
        protected List<Customer> m_customers = null;
        protected OrderNew ordNew = null;
        protected SqlConnection m_sqlComputerDepotConnection = null;
            
        public CustomerForm(SqlConnection sqlComputerDepotConnection)
        {
            InitializeComponent();
            errs = new ErrorReporter();

            try
            {
                db = new Customers(sqlComputerDepotConnection);
                m_sqlComputerDepotConnection = sqlComputerDepotConnection;
            }
            catch (Exception except)
            {
                errs.ReportException(except); 
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                db.Read();
                m_customers = (List<Customer>)db.GetList();

                if (m_customers != null)
                {
                    for (int index = 0; index < m_customers.Count; index++)
                    {
                        listAcc.Items.Add(m_customers[index].CIN);
                    }
                    dgvCustomers.DataSource = m_customers;  
                }
            }
            catch (Exception except)
            {
                errs.ReportException(except);
            }
        }

        private void listAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(listAcc.SelectedItem.ToString());

                Customer cust = FindCustomerRecord(value);

                if (cust != null)
                {
                    txtFirstName.Text = cust.FirstName;
                    txtLastName.Text = cust.LastName;
                    txtStreetNo.Text = cust.StreetNumber;
                    txtStreetName.Text = cust.Street;
                    txtApt.Text = cust.Apt;
                    txtCity.Text = cust.City;
                    txtState.Text = cust.State;
                    txtZipCode.Text = cust.Zip;

                    cbContactNo.Items.Clear();

                    foreach (Phone ph in cust.Phone)
                    {
                        cbContactNo.Items.Add(ph.PhoneNumber);
                    }

                    cbContactNo.SelectedIndex = 0;

                    cbEmail.Items.Clear();

                    foreach (Email email in cust.EMail)
                    {
                        cbEmail.Items.Add(email.EmailAddress);
                    }

                    cbEmail.SelectedIndex = 0;

                }
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            catch (Exception except)
            {
                errs.ReportException(except);
            }

        }

        private Customer FindCustomerRecord(int value)
        {
            foreach (Customer cust in m_customers)
            {
                if (cust.CIN == value)
                    return cust;
            }
            return null;
        }

        private void dgvCustomers_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            Customer cust = new Customer();

            m_customers.Add(cust);
            dgvCustomers.DataSource = m_customers; 
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            String Phone = cbContactNo.Items[0].ToString();
            try
            {
#if _USE_
                ordNew = new OrderNew();
#else
                ordNew = new OrderNew(m_sqlComputerDepotConnection);
#endif
            }
            catch (Exception except)
            {
                errs.ReportException(except); 
            }
            ordNew.Show(); 
        }
    }
}
