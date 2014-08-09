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
        
             
        public override int Create()
        {
            Console.WriteLine("Called from Customers::Create())");
            return 0;
        }


        public override int Create(Object customer)
        {
            Console.WriteLine("Called from Customers::Create())");

            Customer cust = (Customer)customer;

            SqlCommand[] extra_command = new SqlCommand[4];

            extra_command[0] = null;
            extra_command[1] = null;
            extra_command[2] = null;
            extra_command[3] = null;

            try
            {
                //ALTER PROCEDURE [dbo].[CreateCustomer]
                command = new SqlCommand("CreateCustomer", sqlComputerDepot);

                command.Parameters.Clear();
                command.CommandType = CommandType.StoredProcedure;

                //////////////////////////////////////////////////////////////////////////////////////////
                //INPUT PARAMETERS
                //////////////////////////////////////////////////////////////////////////////////////////
                
                //@FirstName nvarchar(50)  
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = cust.FirstName;

                //,@LastName nvarchar(50)
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = cust.LastName;

                //@Street [nvarchar](50)
                command.Parameters.Add("@Street", SqlDbType.NVarChar).Value = cust.Street;

                //@StreetNumber [nvarchar](50)
                command.Parameters.Add("@StreetNumber", SqlDbType.NVarChar).Value = cust.StreetNumber;

                //@Apt		[nvarchar](50)
                command.Parameters.Add("@Apt", SqlDbType.NVarChar).Value = cust.Apt;

                //@City		[nvarchar](50)
                command.Parameters.Add("@City", SqlDbType.NVarChar).Value = cust.City;

                //@State		[nvarchar](50)
                command.Parameters.Add("@State", SqlDbType.NVarChar).Value = cust.State;

                //@Zip		[nvarchar](10)
                command.Parameters.Add("@Zip", SqlDbType.NVarChar).Value = cust.Zip;

                //////////////////////////////////////////////////////////////////////////////////////////
                //OUTPUT PARAMETERS
                //////////////////////////////////////////////////////////////////////////////////////////
                
                //,@EmailID int OUT
                //,@PaymentID int OUT
                //,@AddressID int OUT
                //,@PhoneID int OUT

                SqlParameter[] param = new SqlParameter[5];
                
                param[0] = new SqlParameter("@CIN", SqlDbType.Int);
                param[0].Direction = ParameterDirection.Output; // This is important!
                command.Parameters.Add(param[0]);

                param[2] = new SqlParameter("@PaymentID", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output; // This is important!
                command.Parameters.Add(param[2]);

                try
                {
                    command.ExecuteNonQuery();

                    cust.CIN = (int)param[0].Value;
                    cust.PaymentID = (int)param[2].Value;

                    //////////////////////////////////////////////////////////////////////
                    //ADD EACH PHONE
                    //////////////////////////////////////////////////////////////////////
                    command = null;
                    command = new SqlCommand("InsertPhone", sqlComputerDepot);
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (Phone ph in cust.Phone)
                    {
                        ph.ID = cust.CIN;

                        if (ph.PhoneNumber == null)
                            continue;

                        command = new SqlCommand("InsertPhone", sqlComputerDepot);
                        command.CommandType = CommandType.StoredProcedure;

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //INPUT PARAMETERS
                        //////////////////////////////////////////////////////////////////////////////////////////

                        //CREATE PROCEDURE [dbo].[InsertPhone]
                        //@PhoneNumber [nvarchar](50)
                        //,@PhoneType [int]
                        //,@ID [int]

                        //@PhoneNumber nvarchar(50)  
                        //extra_command[0].Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = phone.PhoneNumber;
                        command.Parameters.AddWithValue("@PhoneNumber", ph.PhoneNumber);

                        //@PhoneType  
                        //extra_command[0].Parameters.Add("@PhoneType", SqlDbType.Int).Value = 1;
                        command.Parameters.AddWithValue("@PhoneType", 1);

                        //@ID int
                        //extra_command[0].Parameters.Add("@ID", SqlDbType.Int).Value = cust.CIN;
                        command.Parameters.AddWithValue("@ID", cust.CIN);
                        //////////////////////////////////////////////////////////////////////////////////////////
                        //OUTPUT PARAMETER FROM STORED PROCEDURE
                        //////////////////////////////////////////////////////////////////////////////////////////
                        param[0] = new SqlParameter("@PhoneID", SqlDbType.Int);
                        param[0].Direction = ParameterDirection.Output; // This is important!
                        command.Parameters.Add(param[0]);
                        //////////////////////////////////////////////////////////////////////////////////////////
                        try
                        {
                            command.ExecuteNonQuery();
                            ph.PhoneID = (int)param[0].Value;
                        }
                        catch (Exception except)
                        {
                            error_reporter.ReportException(except);
                        }
                        command.Parameters.Clear();
               
                    }
                }
                catch (Exception except)
                {
                    error_reporter.ReportException(except);
                }

                //////////////////////////////////////////////////////////////////////////////
                //ADD EMAIL
                //////////////////////////////////////////////////////////////////////////////
                command = null;
                command = new SqlCommand("CreateCustomerEMail", sqlComputerDepot);

                foreach (Email email in cust.EMail)
                {
                    if (email.EmailAddress == null)
                        continue;

                    command.Parameters.Clear();
                    command.CommandType = CommandType.StoredProcedure;

                    param[0] = new SqlParameter("@EmailID", SqlDbType.Int);
                    param[0].Direction = ParameterDirection.Output; // This is important!
                    command.Parameters.Add(param[0]);

                    //@EmailAddress nvarchar(50)  
                    //extra_command[1].Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = email.EmailAddress ;
                    command.Parameters.AddWithValue("@EmailAddress", email.EmailAddress);
                   
                    //@ID  
                    //extra_command[1].Parameters.Add("@ID", SqlDbType.Int).Value = cust.CIN;
                    command.Parameters.AddWithValue("@ID", cust.CIN);

                    try
                    {
                        command.ExecuteNonQuery();
                        email.EmailID = (int)param[0].Value;
                        email.ID = cust.CIN;
                    }
                    catch (Exception except)
                    {
                        error_reporter.ReportException(except);
                    }

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
                m_customers.Clear();
      
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

                    //PaymentID
                    if (reader.IsDBNull(reader.GetOrdinal("PaymentID")))
                    {
                        cust.PaymentID = -1;
                    }
                    else
                    {
                        cust.PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID"));
                    }

                    //EMailID
                    if (reader.IsDBNull(reader.GetOrdinal("EmailID")))
                    {
                        cust.EmailID = -1;
                    }
                    else
                    {
                        cust.EmailID = reader.GetInt32(reader.GetOrdinal("EmailID"));
                    }

                    //PhoneID
                    if (reader.IsDBNull(reader.GetOrdinal("PhoneID")))
                    {
                        cust.PhoneID = -1;
                    }
                    else
                    {
                        cust.PhoneID = reader.GetInt32(reader.GetOrdinal("PhoneID"));
                    }
              
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

                    m_customers[index].Phone.Clear();

                    if (!reader.HasRows)
                    {
                        reader.Close();
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
                            _phone.PhoneID = reader.GetInt32(reader.GetOrdinal("PhoneID"));
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

                    cust.EMail.Clear();
                    
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        continue;
                    }
                    
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

        public override int Update(Object customer)
        {
            Customer cust = (Customer)customer;

            SqlCommand[] extra_command = new SqlCommand[4];

            extra_command[0] = null;
            extra_command[1] = null;
            extra_command[2] = null;
            extra_command[3] = null;

            try
            {
                command = new SqlCommand("UpdateCustomer", sqlComputerDepot);

                command.Parameters.Clear();
                command.CommandType = CommandType.StoredProcedure;

                //@FirstName nvarchar(50)  
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = cust.FirstName;

                //,@LastName nvarchar(50)
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = cust.LastName;

                //,@CIN int      
                command.Parameters.Add("@CIN", SqlDbType.Int).Value = cust.CIN;

                //,@PaymentID int    
                command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = cust.PaymentID;

                //,@AddressID int 
                command.Parameters.Add("@AddressID", SqlDbType.Int).Value = cust.AddressID;

                command.ExecuteNonQuery();

                extra_command[0] = new SqlCommand("UpdateCustomerPhone", sqlComputerDepot);

                ////////////////////////////////////////////////////////////////////////////////
                //UPDATE Customer Phone(s)
                ////////////////////////////////////////////////////////////////////////////////

                foreach (Phone ph in cust.Phone)
                {
                    if (ph.PhoneID == -1)
                        continue;

                    extra_command[0].Parameters.Clear();
                    extra_command[0].CommandType = CommandType.StoredProcedure;

                    //@PhoneID int
                    extra_command[0].Parameters.Add("@PhoneID", SqlDbType.Int).Value = ph.PhoneID;

                    //@PhoneNumber nvarchar(50)  
                    extra_command[0].Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = ph.PhoneNumber;

                    //,@PhoneType
                    extra_command[0].Parameters.Add("@PhoneType", SqlDbType.Int).Value = ph.PhoneType;

                    //@ID
                    extra_command[0].Parameters.Add("@ID", SqlDbType.Int).Value = cust.CIN;
                    
                    extra_command[0].ExecuteNonQuery();

                }

                ////////////////////////////////////////////////////////////////////////////////
                //INSERT NEW Customer Phone if Necessary
                ////////////////////////////////////////////////////////////////////////////////
                extra_command[1] = new SqlCommand("InsertPhone", sqlComputerDepot);

                foreach (Phone ph in cust.Phone)
                {
                    if (ph.PhoneID != -1)
                        continue;

                    extra_command[1].Parameters.Clear();
                    extra_command[1].CommandType = CommandType.StoredProcedure;

                    SqlParameter param = new SqlParameter();

                    //@CIN int
                    extra_command[1].Parameters.Add("@ID", SqlDbType.Int).Value = cust.CIN;

                    //OUTPUT Paramter => PhoneID
                    param = new SqlParameter("@PhoneID", SqlDbType.Int);
                    param.Direction = ParameterDirection.Output; // This is important!
                    extra_command[1].Parameters.Add(param);

                    //@PhoneNumber nvarchar(50)  
                    extra_command[1].Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = ph.PhoneNumber;

                    //,@PhoneType
                    extra_command[1].Parameters.Add("@PhoneType", SqlDbType.Int).Value = ph.PhoneType;

                    extra_command[1].ExecuteNonQuery();

                    //Retrieve NEW PhoneID
                    cust.PhoneID = (int)param.Value;
          
                }
                
                //////////////////////////////////////////////////////////////////////////
                // Update Customer EMail
                //////////////////////////////////////////////////////////////////////////
                extra_command[1] = new SqlCommand("UpdateCustomerEMail", sqlComputerDepot);

                foreach (Email email in cust.EMail)
                {
                    extra_command[1].Parameters.Clear();
                    extra_command[1].CommandType = CommandType.StoredProcedure;

                    //@EmailID int
                    extra_command[1].Parameters.Add("@EmailID", SqlDbType.Int).Value = email.EmailID;

                    //@@EmailAddress nvarchar(50)  
                    extra_command[1].Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = email.EmailAddress;

                    //@ID  
                    extra_command[1].Parameters.Add("@ID", SqlDbType.Int).Value = cust.CIN;
                    
                    extra_command[1].ExecuteNonQuery();
                }

                //////////////////////////////////////////////////////////////////////////
                // INSERT NEW EMail Addresses (IF user added additional)
                //////////////////////////////////////////////////////////////////////////
                extra_command[1] = new SqlCommand("InsertEmailAddress", sqlComputerDepot);

                try
                {
                    foreach (Email email in cust.EMail)
                    {
                        extra_command[1].Parameters.Clear();
                        extra_command[1].CommandType = CommandType.StoredProcedure;

                        //@EmailID int
                        SqlParameter param = new SqlParameter("@EmailID", SqlDbType.Int);
                        param.Direction = ParameterDirection.Output; // This is important!
                        extra_command[1].Parameters.Add(param);

                        //@@EmailAddress nvarchar(50)  
                        extra_command[1].Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = email.EmailAddress;

                        //@ID  
                        extra_command[1].Parameters.Add("@ID", SqlDbType.Int).Value = cust.CIN;

                        extra_command[1].ExecuteNonQuery();

                        //Retrieve NEW EMailID
                        cust.EmailID = (int)param.Value;
                    }

                }
                catch (Exception except)
                {
                    error_reporter.ReportException(except);
                }


                //////////////////////////////////////////////////////////////////////////
                //Update ADDRESS Table
                //////////////////////////////////////////////////////////////////////////
                //,@Zip [nvarchar](10) 

                extra_command[3] = new SqlCommand("UpdateAddress", sqlComputerDepot);

                extra_command[3].Parameters.Clear();
                extra_command[3].CommandType = CommandType.StoredProcedure;

                //@AddressID int
                extra_command[3].Parameters.Add("@AddressID", SqlDbType.Int).Value = cust.AddressID ;

                //@Street
                extra_command[3].Parameters.Add("@Street", SqlDbType.NVarChar).Value = cust.Street;

                //@StreetNumber
                extra_command[3].Parameters.Add("@StreetNumber", SqlDbType.NVarChar).Value = cust.StreetNumber;

                //@Apt
                extra_command[3].Parameters.Add("@Apt", SqlDbType.NVarChar).Value = cust.Apt;

                //@City 
                extra_command[3].Parameters.Add("@City", SqlDbType.NVarChar).Value = cust.City;

                //@State 
                extra_command[3].Parameters.Add("@State", SqlDbType.NVarChar).Value = cust.State;

                //@Zip  
                extra_command[3].Parameters.Add("@Zip", SqlDbType.NVarChar).Value = cust.Zip;

                extra_command[3].ExecuteNonQuery();
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

        
        public override int Update()
        {
            // 'ALTER PROCEDURE [dbo].[UpdateCusomter]

            SqlCommand[] extra_command = new SqlCommand[2];
            
            extra_command[0] = null;
            extra_command[1] = null;
            
            try
            {
                command = new SqlCommand("UpdateCustomer", sqlComputerDepot);

                foreach ( Customer cust in  m_customers)
                {
                    command.Parameters.Clear();
                    command.CommandType = CommandType.StoredProcedure;

                    //@FirstName nvarchar(50)  
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = cust.FirstName;

                    //,@LastName nvarchar(50)
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = cust.LastName;

                    //,@CIN int      
                    command.Parameters.Add("@CIN", SqlDbType.Int).Value = cust.CIN;

                    //,@PaymentID int    
                    command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = cust.PaymentID;

                    //,@AddressID int 
                    command.Parameters.Add("@AddressID", SqlDbType.Int).Value = cust.AddressID;

                    command.ExecuteNonQuery();

            
                    extra_command[0] = new SqlCommand("UpdateCustomerPhone", sqlComputerDepot);
                    
                    foreach (Phone ph in cust.Phone)
                    {
                        extra_command[0].Parameters.Clear();
                        extra_command[0].CommandType = CommandType.StoredProcedure;


                        //@PhoneID int
                        extra_command[0].Parameters.Add("@PhoneID", SqlDbType.Int).Value = ph.PhoneID;

                        //@PhoneNumber nvarchar(50)  
                        extra_command[0].Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = ph.PhoneNumber;

                        //,@PhoneType
                        extra_command[0].Parameters.Add("@PhoneType", SqlDbType.Int).Value = ph.PhoneType;

                        extra_command[0].ExecuteNonQuery();
                    
                    }

                    extra_command[1] = new SqlCommand("UpdateCustomerEMail", sqlComputerDepot);

                    foreach (Email email in cust.EMail)
                    {
                        extra_command[1].Parameters.Clear();
                        extra_command[1].CommandType = CommandType.StoredProcedure;

                        //@EmailID int
                        extra_command[1].Parameters.Add("@EmailID", SqlDbType.Int).Value = email.EmailID;

                        //@@EmailAddress nvarchar(50)  
                        extra_command[1].Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = email.EmailAddress;

                        extra_command[1].ExecuteNonQuery();
                    }
   
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

        public override int Delete(int cust_id)
        {
            int error_code = 0;

            try
            {
                command = new SqlCommand("DeleteCustomer", sqlComputerDepot);
                command.Parameters.Clear();

                //@CIN
                command.Parameters.Add("@CIN", SqlDbType.Int).Value = cust_id;

                command.CommandType = CommandType.StoredProcedure;

                command.ExecuteNonQuery();
            }
            catch (Exception except)
            {
                error_reporter.ReportException(except);
                error_code = -1;
            }
            Console.WriteLine("Called from Customers::Delete())");
            return error_code;
        }

        public Customers(SqlConnection sql_connection)  : base(sql_connection)
        {
           m_customers = new List<Customer>();
        }
  
    }
}











