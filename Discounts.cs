using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace computerdepot
{
    public class Discounts : Database 
    {
        private List<Discount> m_discounts = null;

        public Discounts(SqlConnection sql_connection)  : base(sql_connection)
        {
            m_discounts = new List<Discount>();
        }
        
        public override Object GetList()
        {
            return (Object)m_discounts;
        }

        public List<Discount> Discount
        {
            get
            {
                return this.m_discounts;
            }
            set
            {
                this.m_discounts = value;
            }
        }
      
        public override int Read()
        {
            // 'ALTER PROCEDURE [dbo].[GetDiscount]

            try
            {
                command = new SqlCommand("GetDiscounts", sqlComputerDepot);
                command.CommandType = CommandType.StoredProcedure;
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Discount discount = new Discount();

                    //DiscountID 
                    if (reader.IsDBNull(reader.GetOrdinal("DiscountID")))
                    {
                        discount.DiscountID = -1;
                    }
                    else
                    {
                        discount.DiscountID = reader.GetInt32(reader.GetOrdinal("DiscountID"));
                    }

                    //DiscountType
                    if (reader.IsDBNull(reader.GetOrdinal("DiscountType")))
                    {
                        discount.DiscountType = -1;
                    }
                    else
                    {
                        discount.DiscountType = reader.GetInt32(reader.GetOrdinal("DiscountType"));
                    }

                    //DiscountAmount 
                    if (reader.IsDBNull(reader.GetOrdinal("DiscountAmount")))
                    {
                        discount.DiscountAmount = -1;
                    }
                    else
                    {
                        discount.DiscountAmount = (double)reader.GetDecimal(reader.GetOrdinal("DiscountAmount"));
                    }

                    // DiscountDescription 
                    if (reader.IsDBNull(reader.GetOrdinal("DiscountDescription")))
                    {
                        discount.DiscountDescription = string.Empty;
                    }
                    else
                    {
                        discount.DiscountDescription = reader.GetString(reader.GetOrdinal("DiscountDescription"));
                    }
                    
                    m_discounts.Add(discount);
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
