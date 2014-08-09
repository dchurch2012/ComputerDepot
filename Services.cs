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
    public class Services : Database
    {
        protected List<Service> m_services = null;
        protected SqlConnection m_sqlComputerDepot = null;

        public override Object GetList()
        {
            return (Object)m_services;
        }

        public List<Service> ServicesList
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

        public Services(SqlConnection sqlConnection)
        {
            m_services = new List<Service>();
            m_sqlComputerDepot = sqlConnection;
        }

        public override int Create()
        {
            return 0;
        }

        public override int Read()
        {
            try
            {
                command = new SqlCommand("GetServices", m_sqlComputerDepot);
                command.CommandType = CommandType.StoredProcedure;
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Service service = new Service();

                    //[ServiceID] 
                    if (reader.IsDBNull(reader.GetOrdinal("ServiceID")))
                    {
                        service.ServiceID = -1;
                    }
                    else
                    {
                        service.ServiceID = reader.GetInt32(reader.GetOrdinal("ServiceID"));
                    }

                    //[ServiceCost]
                    if (reader.IsDBNull(reader.GetOrdinal("ServiceCost")))
                    {
                        service.ServiceCost = -1;
                    }
                    else
                    {
                        service.ServiceCost = (double)reader.GetDecimal(reader.GetOrdinal("ServiceCost"));
                    }


                    //[ServiceDescrip]
                    if (reader.IsDBNull(reader.GetOrdinal("ServiceDescrip")))
                    {
                        service.ServiceDescrip = string.Empty;
                    }
                    else 
                    {
                        service.ServiceDescrip = reader.GetString(reader.GetOrdinal("ServiceDescrip"));
                    }

                    //ServiceOrderID 
                    if (reader.IsDBNull(reader.GetOrdinal("ServiceOrderID")))
                    {
                        service.ServiceOrderID = -1;
                    }
                    else
                    {
                        service.ServiceOrderID = reader.GetInt32(reader.GetOrdinal("ServiceOrderID"));
                    }

                    //DomainID 
                    if (reader.IsDBNull(reader.GetOrdinal("DomainID")))
                    {
                        service.DomainID = -1;
                    }
                    else
                    {
                        service.DomainID = reader.GetInt32(reader.GetOrdinal("DomainID"));
                    }

                    //DomainName 
                    if (reader.IsDBNull(reader.GetOrdinal("DomainName")))
                    {
                        service.DomainName = string.Empty;
                    }
                    else
                    {
                        service.DomainName = reader.GetString(reader.GetOrdinal("DomainName"));
                    }

                    m_services.Add(service);
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


    }
}
