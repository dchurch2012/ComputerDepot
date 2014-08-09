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
    public partial class Order_Details : Form
    {
        protected Customer m_customer_info = null;
        protected List<Product> m_order_items = null;
        protected List<Discount>  m_discount_items = null;
        protected List<Order> m_orders = null;

        //For Services ordered
        protected List<Service> m_service_items = null;


        protected double total_charges_amt = 0;
        protected double total_order_amt = 0;
        protected double discount_percent = 0;
        protected double taxes_percent = 0;

        protected ErrorReporter errs = null;

        protected SqlConnection m_connection = null;

        public int EmployeeId { get; set; }

        public Order_Details(Customer customer_info, List<Product> order_items, List<Discount> discount_items, List<Service> service_items,SqlConnection connection, int employee_id)
        {
            InitializeComponent();
            m_customer_info = customer_info;
            m_order_items = order_items;
            m_discount_items = discount_items;
            m_connection = connection;

            m_service_items = service_items;

            EmployeeId = employee_id;
        }

        private void DisableRadioControls()
        {
            radiobtnStudent.Enabled = false;
            radiobtnTeacher.Enabled = false;
            radiobtnMilitary.Enabled = false;
        }

        private void EnableRadioControls()
        {
            radiobtnStudent.Enabled = true;
            radiobtnTeacher.Enabled = true;
            radiobtnMilitary.Enabled = true;
        }

        private void Order_Details_Load(object sender, EventArgs e)
        {
            DisableRadioControls();

            cbDiscount.Text = "YES";
            cbDiscount.Checked = false;
         
            txtFirstName.Text = m_customer_info.FirstName ;
            txtLastName.Text = m_customer_info.LastName;
            
            if (m_customer_info.Phone.Count > 0)
            {
                txtPhone.Text = m_customer_info.Phone[0].PhoneNumber;
            }
          
            calculate_total_charges();
            String str_total_order_amt = String.Format("{0:0.00}", total_order_amt);

            txtTotalListPrice.Text = str_total_order_amt;

        }

        private double calculate_total_charges()
        {
            total_order_amt = 0;

            foreach (Product prod in m_order_items)
            {
                total_order_amt += prod.ProductPrice;
            }

            foreach (Service services in m_service_items)
            {
                total_order_amt += services.ServiceCost;
            }


            total_charges_amt = total_order_amt - (total_order_amt * (discount_percent / 100));
            total_charges_amt = total_charges_amt * (1 + (taxes_percent / 100));

            return 0;
        }

        private void txtDiscountAmount_TextChanged(object sender, EventArgs e)
        {
            if (cbDiscount.Checked == true)
            {
                discount_percent = Convert.ToDouble(txtDiscountAmount.Text);
                calculate_total_charges();
                String charges = String.Format("{0:0.00}", total_charges_amt);

                txtOrderTotalPrice.Text = charges; 
            }
        }

        private void txtTax_TextChanged(object sender, EventArgs e)
        {
            taxes_percent = Convert.ToDouble(txtTax.Text);
            calculate_total_charges();

            String str_total_order_price = String.Format("{0:0.00}", total_charges_amt);

            txtOrderTotalPrice.Text = str_total_order_price;
        }

        private void cbDiscount_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDiscount.Checked == true)
            {
                EnableRadioControls();

                radiobtnMilitary.Checked = true;

                if (m_discount_items != null && m_discount_items.Count > 1)
                {
                    txtDiscountAmount.Text = m_discount_items[1].DiscountAmount.ToString();
                }

                if (txtDiscountAmount.Text != String.Empty)
                {
                    discount_percent = Convert.ToDouble(txtDiscountAmount.Text);
                    calculate_total_charges();

                    String charges = String.Format("{0:0.00}", total_charges_amt);
                    txtOrderTotalPrice.Text = charges;
                }
            }
            else
            {
                 DisableRadioControls();

                if (txtDiscountAmount.Text != String.Empty)
                {
                    discount_percent = 0;
                    calculate_total_charges();
                    txtDiscountAmount.Text = string.Empty;

                    String charges = String.Format("{0:0.00}", total_charges_amt);

                    txtOrderTotalPrice.Text = charges;
                }
            }
        }

        private void radiobtnStudent_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                discount_percent = m_discount_items[0].DiscountAmount;

                txtDiscountAmount.Text = discount_percent.ToString();
                calculate_total_charges();
            }
            catch (Exception except)
            {
                errs.ReportException(except);
            }


        }

        private void radiobtnMilitary_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_discount_items != null && m_discount_items.Count > 1)
                {
                    discount_percent = m_discount_items[1].DiscountAmount;
                }

                txtDiscountAmount.Text = discount_percent.ToString();
                calculate_total_charges();
            }
            catch (Exception except)
            {
                errs.ReportException(except);
            }
            
        }

        private void radiobtnTeacher_CheckedChanged(object sender, EventArgs e)
        {
            if (m_discount_items != null && m_discount_items.Count > 1)
            {
                discount_percent = m_discount_items[2].DiscountAmount;
                txtDiscountAmount.Text = discount_percent.ToString();
                calculate_total_charges();
            }
        }

        private void btnCompleteTrans_Click(object sender, EventArgs e)
        {
            try
            {
                Orders orders = new Orders(m_connection,m_order_items,m_service_items);

                m_orders = orders.Ord; //Ord is list of items
                Order order = new Order();

                order.CIN = m_customer_info.CIN;
                
                order.FirstName = txtFirstName.Text;
                order.LastName = txtLastName.Text;

                Phone ph = new Phone();

                order.PhoneNumber = txtPhone.Text;

                if (txtTotalListPrice.Text != null)
                {
                    order.ListPrice = Convert.ToDouble(txtTotalListPrice.Text);
                }

                if (txtDiscountAmount.Text != null)
                {
                    order.DiscountAmount = Convert.ToDouble(txtDiscountAmount.Text);
                }

                if (txtTax.Text != null)
                {
                    order.Tax = Convert.ToDouble(txtTax.Text);
                }

                if (txtOrderTotalPrice.Text != null)
                {
                    order.OrderTotalPrice = Convert.ToDouble(txtOrderTotalPrice.Text);
                }

                order.TransactionDate = Convert.ToDateTime(dtTransactionDate.Text);

                //Copy to the Order class so we can track which
                //employee made the sale on a particular order
                order.EmployeeID = EmployeeId;

                order.products_ordered = m_order_items; //list of products (products_ordered) passed in
                orders.Ord.Add(order); // adding an item in the order list to order

                if (orders.Create() == 0)
                {
                    MessageBox.Show("Thank you for shopping with Computer Depot - Your order has been placed");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("An Error has occurred - Please Call 888-555-1212 for assistance");
                    this.Close();
                }
            }
            catch (Exception except)
            {
                errs.ReportException(except);
            }
        }

    }
}
