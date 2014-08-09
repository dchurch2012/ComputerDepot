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
    public class Orders : Database
    {
        enum ProducType
        {
            PRODUCT,
            SERVICE
        };

        private List<Order> m_orders = null;
        private List<Service> m_services = null;

        public override Object GetList()
        {
            return (Object)m_orders;
        }

        public List<Order> Ord
        {
            get
            {
                return this.m_orders;
            }
            set
            {
                this.m_orders = value;
            }
        }

        public List<Service> Services
        {
            get
            {
                return this.m_services;
            }
            set
            {
                this.m_services = value;
            }
        }
     
        public Orders(SqlConnection sqlConnection, List<Product> product_list, List<Service> services_list) : base(sqlConnection )
        {
            m_orders = new List<Order>();
            m_services = services_list;
        }
        
        public override int Create()
        {
            int order_id = -1;

            // 'ALTER PROCEDURE [dbo].[ReadCustomers]

            try
            {
                command = new SqlCommand("CreateOrder", sqlComputerDepot);

                SqlCommand special_command = new SqlCommand("GetLastOrderID", sqlComputerDepot);
                special_command.Parameters.Clear();
                special_command.CommandType = CommandType.StoredProcedure;
                reader = special_command.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("LastOrderID")))
                        {
                            order_id = 0;
                        }
                        else
                        {
                            order_id = reader.GetInt32(reader.GetOrdinal("LastOrderID")) + 1;
                        }
                    }

                    reader.Close();
                }


                for (int index = 0; index < m_orders[0].products_ordered.Count(); index++)
                {
                    command.Parameters.Clear();
                    command.CommandType = CommandType.StoredProcedure;

                    //@OrderID    
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = order_id;
                    
                    //@OrderItem    
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = m_orders[0].products_ordered[index].ProductID;

                    //@CIN int
                    command.Parameters.Add("@CIN", SqlDbType.Int).Value = m_orders[0].CIN;
             
                    //@FirstName nchar(64)    
                    command.Parameters.Add("@FirstName", SqlDbType.NChar).Value = m_orders[0].FirstName;
                
                    //,@LastName nchar(64)      
                    command.Parameters.Add("@LastName", SqlDbType.NChar).Value = m_orders[0].LastName;
            
                    //,@PhoneNumber nchar(32)      
                    command.Parameters.Add("@PhoneNumber", SqlDbType.NChar).Value = m_orders[0].PhoneNumber;
            
                    //,@ListPrice decimal      
                    command.Parameters.Add("@ListPrice", SqlDbType.Decimal).Value = m_orders[0].products_ordered[index].ProductPrice;
                
                    //,@DiscountAmount decimal
                    command.Parameters.Add("@DiscountAmount", SqlDbType.Decimal).Value = m_orders[0].DiscountAmount;
             
                    //Tax
                    command.Parameters.Add("@Tax", SqlDbType.Decimal).Value = m_orders[0].Tax;
            
                    //,@OrderTotalPrice decimal
                    command.Parameters.Add("@OrderTotalPrice", SqlDbType.Decimal).Value = m_orders[0].OrderTotalPrice;
            
                    //,@TransactionDate datetime
                    command.Parameters.Add("@TransactionDate", SqlDbType.DateTime).Value = m_orders[0].TransactionDate;

                    //EmployeeId
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = m_orders[0].EmployeeID;

                    //OrderId
                    command.Parameters.Add("@ProductType", SqlDbType.Int).Value = ProducType.PRODUCT;
                    command.ExecuteNonQuery();
                }
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

            //INSERET Service Orders (If any)
           try
           {
               command = new SqlCommand("CreateOrder", sqlComputerDepot);

                SqlCommand special_command = new SqlCommand("GetLastOrderID", sqlComputerDepot);
                special_command.Parameters.Clear();
                special_command.CommandType = CommandType.StoredProcedure;
                reader = special_command.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("LastOrderID")))
                        {
                            order_id = 0;
                        }
                        else
                        {
                            order_id = reader.GetInt32(reader.GetOrdinal("LastOrderID")) + 1;
                        }
                    }

                    reader.Close();
                }
      
                for (int index = 0; index  < m_services.Count(); index++)
                {
                    command.Parameters.Clear();
                    command.CommandType = CommandType.StoredProcedure;

                    //@OrderID    
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = order_id;
                    
                    //@OrderItem    
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = m_services[index].ServiceID;

                    //@CIN int
                    command.Parameters.Add("@CIN", SqlDbType.Int).Value = m_orders[0].CIN;
             
                    //@FirstName nchar(64)    
                    command.Parameters.Add("@FirstName", SqlDbType.NChar).Value = m_orders[0].FirstName;
                
                    //,@LastName nchar(64)      
                    command.Parameters.Add("@LastName", SqlDbType.NChar).Value = m_orders[0].LastName;
            
                    //,@PhoneNumber nchar(32)      
                    command.Parameters.Add("@PhoneNumber", SqlDbType.NChar).Value = m_orders[0].PhoneNumber;
            
                    //,@ListPrice decimal      
                    command.Parameters.Add("@ListPrice", SqlDbType.Decimal).Value = m_services[index].ServiceCost;
                
                    //,@DiscountAmount decimal
                    command.Parameters.Add("@DiscountAmount", SqlDbType.Decimal).Value = m_orders[0].DiscountAmount;
             
                    //Tax
                    command.Parameters.Add("@Tax", SqlDbType.Decimal).Value = m_orders[0].Tax;
            
                    //,@OrderTotalPrice decimal
                    command.Parameters.Add("@OrderTotalPrice", SqlDbType.Decimal).Value = m_orders[0].OrderTotalPrice;
            
                    //,@TransactionDate datetime
                    command.Parameters.Add("@TransactionDate", SqlDbType.DateTime).Value = m_orders[0].TransactionDate;

                    //EmployeeId
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = m_orders[0].EmployeeID;

                    //OrderId
                    command.Parameters.Add("@ProductType", SqlDbType.Int).Value = ProducType.SERVICE;
                    command.ExecuteNonQuery();
                }
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

        public override int Read()
        {
            // 'ALTER PROCEDURE [dbo].[ReadCustomers]

            try
            {
                command = new SqlCommand("ReadOrder", sqlComputerDepot);
                
                for(int index = 0; index < m_orders.Count(); index++)
                {

                    command.Parameters.Clear();
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = m_orders[index].OrderID;

                    command.CommandType = CommandType.StoredProcedure;
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Order order = new Order();

                        if (reader.IsDBNull(reader.GetOrdinal("OrderID")))
                        {
                            order.OrderID = -1;
                        }
                        else
                        {
                            order.OrderID = reader.GetInt32(reader.GetOrdinal("OrderID"));
                        }

                        m_orders.Add(order);
                    }
                    reader.Close();
                }
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
    }
}
