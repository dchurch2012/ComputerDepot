using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace computerdepot
{
    enum ReportErrors
    {
        NoProductsSoldLastMonth = -1
    }

    public class Reports : Database
    {
        public List<Report> report_list { get; set; }
        protected Report m_report = null;
        protected OrdersReport m_orders_report = null;
        protected SqlConnection m_connection = null;

        private delegate int fReadPointer();
        private fReadPointer[] f_read_pointers = null;

   
        public Reports(SqlConnection connection)
        {
            m_orders_report = new OrdersReport();
            m_connection = connection;
            report_list = new List<Report>();
            f_read_pointers = new fReadPointer[10];

            if(f_read_pointers != null)
            {
                f_read_pointers[0] = RunSalesStatsReport;
                f_read_pointers[1] = RunTopSellersPerMonthReport;
                f_read_pointers[2] = RunSalesStatsPerMonthPerEmployeeReport;
                f_read_pointers[3] = RunOrderReport;
            }

            error_reporter = new ErrorReporter();

        }

        // Stored Procedure - GetTopSellersPerMonth
        public int RunTopSellersPerMonthReport()
        {
            int error_code = 0;

            try
            {
          
                command = new SqlCommand("GetTopSellersPerMonth", m_connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Clear();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Report report = (OrdersReport)new OrdersReport();

                    //[Product].ProductID

                    if (reader.IsDBNull(reader.GetOrdinal("ProductID")))
                    {
                        report.ID = -1;
                    }
                    else
                    {
                        report.ID = reader.GetInt32(reader.GetOrdinal("ProductID"));
                    }


                    //TransactionDate
                    if (reader.IsDBNull(reader.GetOrdinal("TransactionDate")))
                    {
                        report.TransactionDate = DateTime.Now;
                    }
                    else
                    {
                        report.TransactionDate = reader.GetDateTime(reader.GetOrdinal("TransactionDate"));
                    }

                     if (reader.IsDBNull(reader.GetOrdinal("ProductDescription")))
                    {
                        report.Description = string.Empty;
                    }
                    else
                    {
                        report.Description = reader.GetString(reader.GetOrdinal("ProductDescription"));
                    }
                   //ProductDescription
                    if (reader.IsDBNull(reader.GetOrdinal("ProductDescription")))
                    {
                        report.Description = string.Empty;
                    }
                    else
                    {
                        report.Description = reader.GetString(reader.GetOrdinal("ProductDescription"));
                    }
               
                    //ProductFrequency
                    if (reader.IsDBNull(reader.GetOrdinal("ProductFrequency")))
                    {
                        report.Frequency = -1;
                    }
                    else
                    {
                        report.Frequency = (double)reader.GetDecimal(reader.GetOrdinal("ProductFrequency"));
                    }
               

                    report_list.Add(report);
                }
            }
            catch (Exception except)
            {
                error_reporter.ReportException(except);
                error_code = -2;

            }

            if (!reader.IsClosed)
            {
                reader.Close();
            }

            return error_code;
        }


        public int RunSalesStatsPerMonthPerEmployeeReport()
        {
            int error_code = 0;

            try
            {
                command = new SqlCommand("GetSalesStatsPerMonthPerEmployee", m_connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Clear();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Report report = (OrdersReport)new OrdersReport();

                    //EmployeeId 
                    if (reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                    {
                        report.ID = -1;
                    }
                    else
                    {
                        report.ID = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
                    }


                    //TotalPerMonth
                    if (reader.IsDBNull(reader.GetOrdinal("TotalPerMonth")))
                    {
                        report.Total = -1;
                    }
                    else
                    {
                        report.Total = (double)reader.GetDecimal(reader.GetOrdinal("TotalPerMonth"));
                    }
                    report_list.Add(report);
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string str;
                str = "Source:" + ex.Source;
                str += "\n" + "Message:" + ex.Message;
                System.Diagnostics.Debug.WriteLine(str, "Database Exception");
                error_reporter.ReportSqlException(ex);
                error_code = -3;
            }
            catch (Exception except)
            {
                error_reporter.ReportException(except);
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

            return error_code; 
        }


        public int RunOrderReport()
        {
            try
            {
            command = new SqlCommand("GetOrderReport", m_connection);
            //command = new SqlCommand("GetSalesStatsPerMonth", m_connection);
            command.CommandType = CommandType.StoredProcedure;
       
            command.Parameters.Clear();
            command.Parameters.Add("@ReportType", SqlDbType.Int).Value = 1;

            reader = command.ExecuteReader();
                
            while (reader.Read())
            {
                Report report = (OrdersReport) new OrdersReport();
                    
                //ProductID
                if (reader.IsDBNull(reader.GetOrdinal("ProductID")))
                {
                    report.ID = -1;
                }
                else
                {
                    report.ID = reader.GetInt32(reader.GetOrdinal("ProductID"));
                }

                //ProductCount
                if (reader.IsDBNull(reader.GetOrdinal("ProductCount")))
                {
                    report.Count = -1;
                }
                else
                {
                    report.Count = (double)reader.GetDecimal(reader.GetOrdinal("ProductCount"));
                }

                //ProductIDCount
                if (reader.IsDBNull(reader.GetOrdinal("ProductIDCount")))
                {
                    report.IDCount = -1;
                }
                else
                {
                    report.IDCount =  (double)reader.GetDecimal((reader.GetOrdinal("ProductCount")));
                }

                //ProductFrequency
                if (reader.IsDBNull(reader.GetOrdinal("ProductFrequency")))
                {
                    report.Frequency = -1;
                }
                else
                {
                    report.Frequency = (double)reader.GetDecimal((reader.GetOrdinal("ProductFrequency")));
                }

                //ProductFrequency
                if (reader.IsDBNull(reader.GetOrdinal("ProductFrequency")))
                {
                    report.Frequency = -1;
                }
                else
                {
                    report.Frequency = (double)reader.GetDecimal((reader.GetOrdinal("ProductFrequency")));
                }

                //ProductDescription"
                if (reader.IsDBNull(reader.GetOrdinal("ProductDescription")))
                {
                    report.Description = string.Empty;
                }
                else
                {
                    report.Description = reader.GetString((reader.GetOrdinal("ProductDescription")));
                }
                    
                report_list.Add(report);
            }
            reader.Close();
        }
        catch (Exception ex)
        {
            error_reporter.ReportException(ex);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        return 0;
    }

        public int RunSalesStatsReport()
        {
            try
            {
                command = new SqlCommand("GetSalesStatsPerMonth", m_connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
      
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Report report = (OrdersReport)new OrdersReport();

                    //ProductCount
                    if (reader.IsDBNull(reader.GetOrdinal("ProductCount")))
                    {
                        report.Count = -1;
                    }
                    else
                    {
                        report.Count = (double)reader.GetDecimal(reader.GetOrdinal("ProductCount"));
                    }

                    //ListPrice
                    if (reader.IsDBNull(reader.GetOrdinal("ListPrice")))
                    {
                        report.Price = -1;
                    }
                    else
                    {
                        report.Price = (double)reader.GetDecimal((reader.GetOrdinal("ListPrice")));
                    }

                    //TransactionDate
                    if (reader.IsDBNull(reader.GetOrdinal("TransactionDate")))
                    {
                        //report.TPrice = -1;
                    }
                    else
                    {
                       // report.Price = (double)reader.GetDecimal((reader.GetOrdinal("ListPrice")));
                    }

                    //ProductDescription
                    if (reader.IsDBNull(reader.GetOrdinal("ProductDescription")))
                    {
                        report.Description = string.Empty;
                    }
                    else
                    {
                        report.Description  = reader.GetString((reader.GetOrdinal("ProductDescription")));
                    }

                    //ProductSalesTotal
                    if (reader.IsDBNull(reader.GetOrdinal("ProductSalesTotal")))
                    {
                        report.Total = -1;
                    }
                    else
                    {
                        report.Total = (double)reader.GetDecimal((reader.GetOrdinal("ProductSalesTotal")));
                    }

                    
                    //ProductIDCount
                    if (reader.IsDBNull(reader.GetOrdinal("ProductIDCount")))
                    {
                        report.IDCount = -1;
                    }
                    else
                    {
                        report.IDCount = reader.GetInt32((reader.GetOrdinal("ProductIDCount")));
                    }
                    
                    //ProductFrequency
                    if (reader.IsDBNull(reader.GetOrdinal("ProductFrequency")))
                    {
                        report.Frequency = -1;
                    }
                    else
                    {
                        report.Frequency = (double)reader.GetDecimal((reader.GetOrdinal("ProductFrequency")));
                    }
                  
                    //ProductCountPerMonth
                    if (reader.IsDBNull(reader.GetOrdinal("ProductCountPerMonth")))
                    {
                        report.CountPerUnitTime = -1;
                    }
                    else
                    {
                        report.CountPerUnitTime = (double)reader.GetDecimal((reader.GetOrdinal("ProductCountPerMonth")));
                    }


                    //ProductSalesPerMonth
                    if (reader.IsDBNull(reader.GetOrdinal("ProductSalesPerMonth")))
                    {
                        report.SalesPerUnitTime = -1;
                    }
                    else
                    {
                        report.SalesPerUnitTime = (double)reader.GetDecimal((reader.GetOrdinal("ProductSalesPerMonth")));
                    }


                    //ProductSalesPerMonth
                    if (reader.IsDBNull(reader.GetOrdinal("ProductSalesPerMonth")))
                    {
                        report.SalesPerUnitTime = -1;
                    }
                    else
                    {
                        report.SalesPerUnitTime = (double)reader.GetDecimal((reader.GetOrdinal("ProductSalesPerMonth")));
                    }
      

                    report_list.Add(report);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                error_reporter.ReportException(ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

 
            return 0;
        }
              
        public override int Read(int type)
        {
            int error_code = f_read_pointers[type]();

            return error_code;
        }


        public override int Read()
        {
            return 0;
        }

    }
}
