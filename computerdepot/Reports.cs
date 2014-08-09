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
    public partial class Reports : Form
    {
        protected SqlConnection m_SqlConnection = null;

        public Reports(SqlConnection SqlConnection)
        {
            InitializeComponent();
            m_SqlConnection = SqlConnection; 
        }
    }
}
