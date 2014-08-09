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
    public partial class EmployeeInformation : Form
    {
        protected SqlConnection m_sqlConnection = null;

        public EmployeeInformation(SqlConnection sqlConnection)
        {
            InitializeComponent();
            m_sqlConnection = sqlConnection; 
        }
    }
}
