using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.RegularExpressions;

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
        protected Customer m_selected_customer = null;
        public int EmployeeId { get; set; }
        protected bool m_EditMode = false;
    
        //To Get Last Selected Email
        int email_text_changed_count = 0;
        protected string selected_email = string.Empty;

        //To Get Last Selected Phone
        int phone_text_changed_count = 0;
        protected string selected_phone = string.Empty;

        private bool bSet = false;

        private int ReLoadData()
        {
            try
            {
                listAcc.Items.Clear();
                listAcc.Refresh();

                DisableAllControls();
                btnSave.Enabled = false;
                dgvCustomers.ReadOnly = true;

                db.Read();
                m_customers = (List<Customer>)db.GetList();

                dgvCustomers.DataSource = null;

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

            return 0;
        }
        public CustomerForm(SqlConnection sqlComputerDepotConnection, int employee_id)
        {
            InitializeComponent();
            errs = new ErrorReporter();
            EmployeeId = employee_id;

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

        protected int LoadData()
        {
            try
            {
                listAcc.Items.Clear();
                listAcc.Refresh();

                DisableAllControls();
                btnSave.Enabled = false;
                dgvCustomers.ReadOnly = true;

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
            return 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void listAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(listAcc.SelectedItem.ToString());

                btnEdit.Enabled = true;
                btnDeleteCustomer.Enabled = true;
                btnPlaceOrder.Enabled = true;

                Customer cust = FindCustomerRecord(value);
                m_selected_customer = cust;

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
                        cbContactNo.SelectedIndex = 0;
                    }


                    cbEmail.Items.Clear();

                    foreach (Email email in cust.EMail)
                    {
                        cbEmail.Items.Add(email.EmailAddress);
                        cbEmail.SelectedIndex = 0;
                    }


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

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbContactNo.Items.Count > 0)
                {
                    String Phone = cbContactNo.Items[0].ToString();
                }
                ordNew = new OrderNew(m_sqlComputerDepotConnection, m_selected_customer, EmployeeId);
            }
            catch (Exception except)
            {
                errs.ReportException(except);
            }
            ordNew.Show();
        }


        private Phone FindPhoneNumber(string phone_number)
        {
            foreach (Customer cust in m_customers)
            {
                foreach (Phone phone in cust.Phone)
                {
                    if (phone.PhoneNumber == phone_number)
                    {
                        return phone;
                    }
                }
            }

            return null;
        }

        private void cbEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string email = cbEmail.Text;

                foreach (string item in cbEmail.Items)
                {
                    if (item == email)
                        return;
                }

                cbEmail.Items.Add(email);
            }
        }

        private void cbContactNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string phone = cbContactNo.Text;
                foreach (string item in cbContactNo.Items)
                {
                    if (phone == item)
                        return;
                }
                cbContactNo.Items.Add(phone);
            }
        }

        protected int ClearAllFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtStreetNo.Text = "";
            txtStreetName.Text = "";
            txtApt.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtZipCode.Text = "";

            cbContactNo.Items.Clear();

            cbContactNo.Text = "";

            cbEmail.Text = "";
            cbEmail.Items.Clear();

            listAcc.Items.Clear();

            return 0;
        }

        protected int EnableAllControls()
        {
            txtFirstName.Enabled = true;
            txtLastName.Enabled = true;
            txtStreetNo.Enabled = true;
            txtStreetName.Enabled = true;
            txtApt.Enabled = true;
            txtCity.Enabled = true;
            txtState.Enabled = true;
            txtZipCode.Enabled = true;
            cbContactNo.Enabled = true;
            cbEmail.Enabled = true;

            return 0;
        }

        protected int DisableAllControls()
        {
            txtFirstName.Enabled = false;
            txtLastName.Enabled = false;
            txtStreetNo.Enabled = false;
            txtStreetName.Enabled = false;
            txtApt.Enabled = false;
            txtCity.Enabled = false;
            txtState.Enabled = false;
            txtZipCode.Enabled = false;
            cbContactNo.Enabled = false;
            cbEmail.Enabled = false;

            btnSave.Enabled = false;
            btnEdit.Enabled = false;
            btnPlaceOrder.Enabled = false;
            btnDeleteCustomer.Enabled = false;

            return 0;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            m_EditMode = true;

            EnableAllControls();
            btnSave.Enabled = true;
            btnCreateNewAccount.Enabled = false;
            btnDeleteCustomer.Enabled = false;
            btnPlaceOrder.Enabled = false;
        }

        private bool PhoneExists(Customer cust, string phone_number)
        {
            foreach (Customer customer in m_customers)
            {
                foreach (Phone phone in customer.Phone)
                {
                    if (phone_number == phone.PhoneNumber)
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        private bool EMailExists(Customer cust, string email_address)
        {
            return false;
        }

        protected void GetBasicCustomerInfo(ref Customer cust)
        {
            cust.FirstName = txtFirstName.Text;
            cust.LastName = txtLastName.Text;
            cust.StreetNumber = txtStreetNo.Text;
            cust.Street = txtStreetName.Text;
            cust.Apt = txtApt.Text;
            cust.City = txtCity.Text;
            cust.State = txtState.Text;
            cust.Zip = txtZipCode.Text;
        }


        protected Phone InPhoneList(string PhoneText, Customer cust)
        {
            foreach (Phone phone in cust.Phone)
            {
                if (phone.PhoneNumber == PhoneText)
                {
                    return phone;
                }
            }
            return null;
        }

        protected Email InEmailList(string EmailText, Customer cust)
        {
            foreach (Email email in cust.EMail)
            {
                if (email.EmailAddress == EmailText)
                {
                    return email;
                }
            }
            return null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Customer cust = null;

            string curItem = listAcc.SelectedItem.ToString();
            cust = FindCustomerRecord(Convert.ToInt32(curItem));

            if (cust == null)
            {
                //ERROR
                return;
            }

            //Use ref -- because we don't want a copy of the data; we want to pass in
            //the address of cust so we can modify THAT particular instance
            GetBasicCustomerInfo(ref cust);
            cust.CIN = Convert.ToInt32(curItem);

            // Special case for situation where user enters text 
            // but does not press ENTER

            //Get All Email Addresses and Phone Numbers

            //if(current cbEmail.Text <> any item in email list
            //THEN
            //Find LAST selected Text in email list
            //AND
            //Replace with THIS cbEmail.Text

            //////////////////////////////////////////////////////
            //Check For Email Edit
            //////////////////////////////////////////////////////
            Email em = InEmailList(selected_email, cust);

            //If selected_email WAS NOT found in email listnew EM
            if (em != null)
            {
                em.EmailAddress = cbEmail.Text;
            }
            else
            {
                em = new Email();
                em.ID = cust.CIN;
                em.EmailAddress = cbEmail.Text;
                em.EmailID = -1;

                cust.EMail.Add(em);
            }

            //////////////////////////////////////////////////////
            //Check for Phone (Contact No) Edit
            //////////////////////////////////////////////////////
            Phone ph = InPhoneList(cbContactNo.Text, cust);

            //If selected_phone WAS found in phone list
            if (ph != null)
            {
                ph.PhoneNumber = cbContactNo.Text;
            }
            else
            {
                ph = new Phone();
                ph.ID = cust.AddressID;
                ph.PhoneNumber = cbContactNo.Text;
                ph.PhoneType = 1;
                ph.PhoneID = -1;

                cust.Phone.Add(ph);
            }

            //Reset Edit Control State
            m_EditMode = false;

            db.Update(cust);
            
          
            DisableAllControls();

            MessageBox.Show("Update Complete");
            ReLoadData();

            btnCreateNewAccount.Enabled = true;
            btnDeleteCustomer.Enabled = true;
            btnPlaceOrder.Enabled = true;

        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            int error_code = db.Delete(m_selected_customer.CIN);
            string message = string.Empty;

            if (error_code == 0)
            {
                message = "Customer Account " + m_selected_customer.CIN.ToString() + " Deleted";
                LoadData();
            }
            else
            {
                message = "Error Occurrred Customer Account " + m_selected_customer.CIN.ToString() + " Not Deleted";
            }
            MessageBox.Show(message);
        }

        private void GetEmailAddresses(ref Customer cust)
        {
            if (cbEmail.Text != string.Empty)
            {
                if (!EMailExists(cust, cbEmail.Text))
                {
                    Email em = new Email();
                    em.EmailAddress = cbEmail.Text;
                    cust.EMail.Add(em);
                }
            }

            foreach (string sci in cbEmail.Items)
            {
                if (!EMailExists(cust, sci))
                {
                    Email em = new Email();
                    em.EmailAddress = sci;
                    cust.EMail.Add(em);
                }
            }
        }
        
        private void GetPhoneNumbers(ref Customer cust )
        {
            if (cbContactNo.Text != string.Empty)
            {
                if (!PhoneExists(cust, cbContactNo.Text))
                {
                    Phone phone = new Phone();
                    phone.PhoneNumber = cbContactNo.Text;
                    phone.PhoneID = cust.PhoneID;

                    cust.Phone.Add(phone);
                }
            }

            foreach (string sci in cbContactNo.Items)
            {
                if (!PhoneExists(cust, sci))
                {
                    Phone phone = new Phone();
                    phone.PhoneNumber = sci;
                    phone.PhoneID = cust.PhoneID;

                    cust.Phone.Add(phone);
                }
            }
        }
        
        private void btnCreateNewAccount_Click(object sender, EventArgs e)
        {
            Customer cust = new Customer();
      
            if (bSet == false)
            {
                bSet = true;
                ClearAllFields();
                EnableAllControls();
                
                btnSave.Enabled = false;
                btnDeleteCustomer.Enabled = false;
                btnPlaceOrder.Enabled = false;
                btnEdit.Enabled = false;

                btnCreateNewAccount.Text = "Commit New Customer Account";
                return;
            }
            else
            {
                bSet = false;
                btnCreateNewAccount.Text = "Create New Account";

                GetBasicCustomerInfo(ref cust);
                GetPhoneNumbers(ref cust);
                GetEmailAddresses(ref cust);

                
                db.Create(cust);
                MessageBox.Show("Customer Successfully added.");
                ReLoadData();


                btnSave.Enabled = false;
                btnDeleteCustomer.Enabled = true;
                btnPlaceOrder.Enabled = true;

                btnEdit.Enabled = true;



            }

        }

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        public bool ValidPhoneNumber(string phoneNumber, out string errorMessage)
        {
            Regex regexObj = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");

            if (regexObj.IsMatch(phoneNumber))
            {
                string formattedPhoneNumber = regexObj.Replace(phoneNumber, "($1) $2-$3");
                errorMessage = ""; 
                return true;
            }
            else
            {
                // Invalid phone number
                errorMessage = "Phone number format is invalid.";
                return false;
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////

        public bool ValidEmailAddress(string emailAddress, out string errorMessage)
        {
            // Confirm that the e-mail address string is not empty. 
            if (emailAddress.Length == 0)
            {
                errorMessage = "e-mail address is required.";
                return false;
            }

            // Confirm that there is an "@" and a "." in the e-mail address, and in the correct order.
            if (emailAddress.IndexOf("@") > -1)
            {
                if (emailAddress.IndexOf(".", emailAddress.IndexOf("@")) > emailAddress.IndexOf("@"))
                {
                    errorMessage = "";
                    return true;
                }
            }

            errorMessage = "e-mail address must be valid e-mail address format.\n" +
                            "For example 'someone@example.com' ";
            return false;
        }

        private void cbEmail_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg;
            if (!ValidEmailAddress(cbEmail.Text, out errorMsg))
            {
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                cbEmail.Select(0, cbEmail.Text.Length);

                // Set the ErrorProvider error with the text to display.  
                this.errorProvider.SetError(cbEmail, errorMsg);
            }
            else
            {
                errorMsg = "format sample: someone@aol.com";
                this.errorProvider.SetError(cbEmail, errorMsg);
            }

        }

        private void cbContactNo_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg;

            if (!ValidPhoneNumber(cbContactNo.Text, out errorMsg))
            {
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                cbContactNo.Select(0, cbContactNo.Text.Length);

                // Set the ErrorProvider error with the text to display.  
                this.errorProvider.SetError(cbContactNo, errorMsg);
            }
            else
            {
                errorMsg = "Format is NNNXXXXXXX";
                this.errorProvider.SetError(cbContactNo, errorMsg);
            }
        }

        private void cbEmail_Validated(object sender, EventArgs e)
        {
            // If all conditions have been met, clear the ErrorProvider of errors.
            errorProvider.SetError(cbEmail, "");
        }

        private void cbContactNo_Validated(object sender, EventArgs e)
        {
            // If all conditions have been met, clear the ErrorProvider of errors.
            errorProvider.SetError(cbContactNo, "");
        }

        private void cbEmail_TextChanged(object sender, EventArgs e)
        {
            if (cbEmail.Text != null)
            {
                if (cbEmail.Text != string.Empty)
                {
                    //if(email_text_changed_count == 0)
                    {
                        //selected_email = cbEmail.Text;
                        //email_text_changed_count = 1;
                    }
                }
            }
        }

        private void cbContactNo_TextChanged(object sender, EventArgs e)
        {
            if (cbContactNo.Text != null)
            {
                if (cbContactNo.Text != string.Empty)
                {
                    if (phone_text_changed_count == 0)
                    {
                        //selected_phone = cbContactNo.Text;
                        // phone_text_changed_count = 1;
                    }
                }
            }
        }

        private void cbEmail_TextUpdate(object sender, EventArgs e)
        {
        }

        private void cbEmail_Enter(object sender, EventArgs e)
        {
            selected_email = cbEmail.Text;
        }

        private void cbContactNo_Enter(object sender, EventArgs e)
        {
            selected_phone = cbContactNo.Text;
        }

    
    }
}