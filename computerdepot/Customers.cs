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
    public class Customers : Database 
    {
        private List<Customer> m_customers = null;

        public override Object GetList()
        {
            return (Object)m_customers;
        }

        public List<Customer> Cust
        {
            get
            {
                return this.m_customers;
            }
            set
            {
                this.m_customers = value;
            }
        }
        public int Create(String Firstname, String Lastname)
        {
            Console.WriteLine("Create Customer " + Firstname + " " + Lastname);
            return 0;
        }
     
        public override int Create()
        {
            Console.WriteLine("Called from Customers::Create())");
            return 0;
        }

        public override int Read()
        {
            // 'ALTER PROCEDURE [dbo].[ReadCustomers]

            try
            {
                command = new SqlCommand("ReadCustomers", sqlComputerDepot );
                command.CommandType = CommandType.StoredProcedure;
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Customer cust = new Customer();

                    if (reader.IsDBNull(reader.GetOrdinal("CIN")))
                    {
                        cust.CIN = -1;
                    }
                    else
                    {
                        cust.CIN= reader.GetInt32(reader.GetOrdinal("CIN"));
                    }

                    
                    //FirstName
                    cust.FirstName = (string)reader["FirstName"];

                    //LastName 
                    cust.LastName = (string)reader["LastName"];

           
                    //[Address].Street,
                    //IsDBNULL - MUST Check for NULLs in database -- otherwsie
                    //ADO.NET will crash (trigger exception) when you attenpt to assign
                    //a scalar value (an int for example) to NULL
                    //And strings too (I think!)
                    if (reader.IsDBNull(reader.GetOrdinal("Street")))
                    {
                        cust.Street = string.Empty;
                    }
                    else
                    {
                        //GetOrdinal will return the correct array index in the reader
                        //regardless of how the order is changed (if say, the order of returned
                        //items is changed in the stored procedure
                        cust.Street = reader.GetString(reader.GetOrdinal("Street"));
                    }

                    //[Address].StreetNumber,
                    if (reader.IsDBNull(reader.GetOrdinal("StreetNumber")))
                    {
                        cust.StreetNumber = string.Empty;
                    }
                    else
                    {
                        cust.StreetNumber = reader.GetString(reader.GetOrdinal("StreetNumber"));
                    }

                    //[Address].Apt,
                    if (reader.IsDBNull(reader.GetOrdinal("Apt")))
                    {
                        cust.Apt = string.Empty;
                    }
                    else
                    {
                        cust.Apt = reader.GetString(reader.GetOrdinal("Apt"));
                    }

                    //[Address].City, 
                    if (reader.IsDBNull(reader.GetOrdinal("City")))
                    {
                        cust.City = string.Empty;
                    }
                    else
                    {
                        cust.City = reader.GetString(reader.GetOrdinal("City"));
                    }

                    //[Address].[State],
                    if (reader.IsDBNull(reader.GetOrdinal("State")))
                    {
                        cust.State = string.Empty;
                    }
                    else
                    {
                        cust.State = reader.GetString(reader.GetOrdinal("State"));
                    }
                    
                    //[Address].[Zip],
                    if (reader.IsDBNull(reader.GetOrdinal("Zip")))
                    {
                        cust.Zip = string.Empty;
                    }
                    else
                    {
                        cust.Zip = reader.GetString(reader.GetOrdinal("Zip"));
                    }

                    
                    //[Customer].AddressID, 
                    if (reader.IsDBNull(reader.GetOrdinal("AddressID")))
                    {
                        cust.AddressID = -1;
                    }
                    else
                    {
                        cust.AddressID = (int)reader["AddressID"];
                    }

                    ////PaymentID
                    //if (reader.IsDBNull(reader.GetOrdinal("PaymentID")))
                    //{
                    //    cust.PaymentID = -1;
                    //}
                    //else
                    //{
                    //    cust.PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID"));
                    //}

                    
                    m_customers.Add(cust);
                }
                reader.Close();
                GetPhoneNumbers();
                GetEMailAddress();
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

        protected int GetPhoneNumbers()
        {
            try
            {
                command = null;

                command = new SqlCommand("GetCustomerPhoneNumber", sqlComputerDepot);
                command.CommandType = CommandType.StoredProcedure;
         
                for(int index = 0; index < m_customers.Count(); index++)
                {
                    command.Parameters.Clear(); 
                    command.Parameters.Add("@CustID", SqlDbType.Int).Value = m_customers[index].CIN;
                    reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        continue;
                    }

                    while (reader.Read())
                    {
                        Phone _phone = new Phone();

                        if (reader.IsDBNull(reader.GetOrdinal("PhoneID")))
                        {
                            _phone.PhoneID = -1;
                        }
                        else
                        {
                            _phone.PhoneID = reader.GetOrdinal("PhoneID");
                        }

                        if (reader.IsDBNull(reader.GetOrdinal("PhoneNumber")))
                        {
                            _phone.PhoneNumber = null;
                        }
                        else
                        {
                            _phone.PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"));
                        }

                        //[Phone].PhoneType   
                        if (reader.IsDBNull(reader.GetOrdinal("PhoneType")))
                        {
                            _phone.PhoneType = -1;
                        }
                        else
                        {
                            _phone.PhoneType = reader.GetInt32(reader.GetOrdinal("PhoneType"));
                        }
                        m_customers[index].Phone.Add(_phone);
                    }
                    reader.Close();

                }

            }
            catch(Exception except)
            {
                error_reporter .ReportException(except );
            }
            return 0;
        }


        protected int GetEMailAddress()
        {
            try
            {
                command = null;

                command = new SqlCommand("GetCustomerEmailAddress", sqlComputerDepot);
                command.CommandType = CommandType.StoredProcedure;

                foreach (Customer cust in m_customers)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add("@CustID", SqlDbType.Int).Value = cust.CIN;
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Email _email = new Email();


                        //[Email].EmailAddress,
                        if (reader.IsDBNull(reader.GetOrdinal("EmailAddress")))
                        {
                            _email.EmailAddress = string.Empty;
                        }
                        else
                        {
                            _email.EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress"));
                        }

                        //[Customer].EmailID, 
                        if (reader.IsDBNull(reader.GetOrdinal("EMailID")))
                        {
                            _email.EmailID = -1;
                        }
                        else
                        {
                            _email.EmailID = reader.GetInt32(reader.GetOrdinal("EMailID"));
                        }

                        cust.EMail.Add(_email);
                    }
                    reader.Close();
             
                }
            }
            catch (Exception except)
            {
                error_reporter.ReportException(except);
            }
            return 0;
        }
        public override int Update()
        {
            Console.WriteLine("Called from Customers::Update())");
            return 0;
        }

        public override int Delete()
        {
            Console.WriteLine("Called from Customers::Delete())");  
            return 0;
        }

        public Customers(SqlConnection sql_connection)  : base(sql_connection)
        {
           m_customers = new List<Customer>();
        }
        
        public Customers(String Firstname, String Lastname)
        {
            Create(Firstname, Lastname);
        }
    }
}











