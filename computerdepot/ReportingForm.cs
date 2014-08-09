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
    public partial class ReportingForm : Form
    {
        public ReportingForm()
        {
            InitializeComponent();
        }
        protected SqlConnection m_SqlConnection = null;

        protected int report_type = -1;

        protected int RunTopSellersStatsReport()
        {
            // GetTopSellersPerMonth]
            //[Product].ProductID
            //,TransactionDate
            //,ProductDescription
            //,ProductFrequency


            Reports reports = new Reports(m_SqlConnection);
            reports.Read(1);

            dgvReports.Rows.Clear();

            dgvReports.ColumnCount = 4;

            dgvReports.Columns[0].Width = 80;

            dgvReports.Columns[0].Name = "Product ID";
            dgvReports.Columns[1].Name = "Transaction Date";
            dgvReports.Columns[2].Name = "Product Description";
            dgvReports.Columns[3].Name = "Sales Per Month";

            string[] data_array = null;
            data_array = new string[4];

            //dgvReports.DataSource = reports.report_list;
            foreach (Report report in reports.report_list)
            {
                //EmployeeId TotalPerMonth

                data_array[0] = report.ID.ToString();
                data_array[1] = report.TransactionDate.ToString();
                data_array[2] = report.Description;
                data_array[3] = report.Frequency.ToString();

                dgvReports.Rows.Add(data_array);

            }

            return 0;
        }
        
        // RunSalesStatsReport;
        protected int RunSalesStatsPerEmployeeReport()
        {
            Reports reports = new Reports(m_SqlConnection);
            reports.Read(2);

            dgvReports.Rows.Clear();

            dgvReports.ColumnCount = 2;

            dgvReports.Columns[0].Width = 400;

            dgvReports.Columns[0].Name = "EmployeeId";
            dgvReports.Columns[1].Name = "TotalPerMonth";
         
            string[] data_array = null;
            data_array = new string[5];

            double[] report_total = null;
            report_total = new double[3];

            report_total[0] = 0;
            report_total[1] = 0;
            report_total[2] = 0;


            string[] total_array = null;
            total_array = new string[4];

            //dgvReports.DataSource = reports.report_list;
            foreach (Report report in reports.report_list)
            {
                //EmployeeId TotalPerMonth

                data_array[0] = report.ID.ToString();
                data_array[1] = report.Total.ToString();
          
                report_total[0] += report.Total;
          
                dgvReports.Rows.Add(data_array);

            }

            //Blank Row for eye relief
            total_array[0] = "";
            total_array[1] = "";

            dgvReports.Rows.Add(total_array);

            //Summary Row
            total_array[0] = "Summary Totals";
            total_array[1] = report_total[0].ToString();
          

            dgvReports.Rows.Add(total_array);
       
            return 0;
        }


        protected int RunProductSalesStatsReport()
        {
            Reports reports = new Reports(m_SqlConnection);
            reports.Read(0);

            dgvReports.Rows.Clear();

            dgvReports.ColumnCount = 4;

            dgvReports.Columns[0].Width = 400;

            dgvReports.Columns[0].Name = "Product Description";
            dgvReports.Columns[1].Name = "Total Sales Price";
            dgvReports.Columns[2].Name = "Unit Qty";
            dgvReports.Columns[3].Name = "Product Sales per Month";

            string[] data_array = null;
            data_array = new string[5];

            double[] report_total = null;
            report_total = new double[3];

            report_total[0] = 0;
            report_total[1] = 0;
            report_total[2] = 0;


            string[] total_array = null;
            total_array = new string[4];

            //dgvReports.DataSource = reports.report_list;
            foreach (Report report in reports.report_list)
            {
                data_array[0] = report.Description;
                data_array[1] = report.Total.ToString();
                data_array[2] = report.IDCount.ToString();
                data_array[3] = report.SalesPerUnitTime.ToString();

                report_total[0] += report.Total;
                report_total[1] += report.IDCount;
                report_total[2] += report.SalesPerUnitTime;

                dgvReports.Rows.Add(data_array);

            }

            //Blank Row for eye relief
            total_array[0] = "";
            total_array[1] = "";

            dgvReports.Rows.Add(total_array);

            //Summary Row
            total_array[0] = "Summary Totals";
            total_array[1] = report_total[0].ToString();
            total_array[2] = report_total[1].ToString();
            total_array[3] = report_total[2].ToString();

            dgvReports.Rows.Add(total_array);
       
            return 0;
        }

        
        
        // RunOrderReport;


        public ReportingForm(SqlConnection SqlConnection, int report_id)
        {
            InitializeComponent();
            m_SqlConnection = SqlConnection;
                        
        
        }

        private void rbSales_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSales.Checked == true)
            {
                RunProductSalesStatsReport();
            }
        }

        private void rbEmployeeSales_CheckedChanged(object sender, EventArgs e)
        {
            if(rbEmployeeSales.Checked == true)
            {
                RunSalesStatsPerEmployeeReport();
            }
        }

        private void rbTopSellers_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTopSellers.Checked == true)
            {
                RunTopSellersStatsReport();
            }
        }

        private void ReportingForm_Load(object sender, EventArgs e)
        {
            dgvReports.ReadOnly = true;
    
        }
    }
}
