using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace computerdepot
{
    public partial class Order_Details : Form
    {
        protected String m_FirstName = String.Empty;
        protected String m_LastName = String.Empty;
        protected String m_Phone = String.Empty;
        
        public Order_Details(String FirstName, String LastName, String Phone)
        {
            InitializeComponent();

            m_FirstName = FirstName;
            m_LastName = LastName;
            m_Phone = Phone;
        }

        private void Order_Details_Load(object sender, EventArgs e)
        {
            txtFirstName.Text = m_FirstName;
            txtLastName.Text = m_LastName;
            txtPhone.Text = m_Phone;    
        }
    }
}
