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
    public class Inventories : Database
    {
        protected List<Inventory> m_inventory_list = null;

        protected Inventory m_inventory_item = null;
        
        public Inventories(SqlConnection connection) : base (connection)
        {
            m_inventory_list = new List<Inventory>();
        }

        public override Object GetList() //runtime polymorphism passing the database as an object and recast on other side
        {
            return NewProducts;
        }

        public List<Inventory> NewProducts
        {
            get
            {
                return this.m_inventory_list;
            }
            set
            {
                this.m_inventory_list  = value;
            }
        }

          
        public override int Create( Object inventory)
        {
            m_inventory_item = (Inventory) inventory;

            int error_code = 0;

            System.Diagnostics.Debug.WriteLine("Called from Inventories::Read())");
            //ALTER PROCEDURE [dbo].[GetProductCategoriesAndSubCategories]
            // 'ALTER PROCEDURE [dbo].[ReadCustomers]
            try
            {
                command = new SqlCommand("InsertProductItem", sqlComputerDepot);
                command.CommandType = CommandType.StoredProcedure;

       
                //@ProductPrice  [decimal](18, 2) 
                command.Parameters.Add("@ProductPrice", SqlDbType.Decimal).Value = m_inventory_item.ProdPrice;

                //,@Qty  [int] 
                command.Parameters.Add("@Qty", SqlDbType.Int).Value = m_inventory_item.Qty;

                //@ProductDescrip [nvarchar](100)
                command.Parameters.Add("@ProductDescrip", SqlDbType.NVarChar).Value = m_inventory_item.ProdDescrip;

                //@StockedDate 
                command.Parameters.Add("@StockedDate ", SqlDbType.DateTime).Value = m_inventory_item.StockedDate;

                //@ReorderLevel[int] 
                command.Parameters.Add("@ReorderLevel", SqlDbType.Decimal).Value = m_inventory_item.ReorderLevel;

                //@ProductType [nvarchar](50) 
                command.Parameters.Add("@ProductType ", SqlDbType.NVarChar).Value = m_inventory_item.ProductType;

                //@CatalogID [nchar](32)
                command.Parameters.Add("@CatalogID", SqlDbType.NChar).Value = m_inventory_item.CatalogID;

                //@Mfr[nchar](64) 
                command.Parameters.Add("@Mfr", SqlDbType.NChar).Value = m_inventory_item.Mfr;

                //,@ProductTypeID [int] 
                command.Parameters.Add("@ProductTypeID", SqlDbType.Int).Value = m_inventory_item.ProductTypeID;

                //,@SubCategoryID [int]
                command.Parameters.Add("@SubCategoryID ", SqlDbType.Int).Value = m_inventory_item.SubCategoryID;
            
                command.ExecuteNonQuery();

            }
            catch (Exception except)
            {
                error_reporter.ReportException(except);
                error_code = -100;
            }
            finally
            {

            }
    
            System.Diagnostics.Debug.WriteLine("Called from Inventories::Create())");
            return error_code;
        }

        public override int Read()
        {
            System.Diagnostics.Debug.WriteLine("Called from Inventories::Read())");
            //ALTER PROCEDURE [dbo].[GetProductCategoriesAndSubCategories]
            // 'ALTER PROCEDURE [dbo].[ReadCustomers]

            try
            {
                m_inventory_list.Clear();

                command = new SqlCommand("GetProductCategoriesAndSubCategories", sqlComputerDepot);
                command.CommandType = CommandType.StoredProcedure;
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Inventory invent = new Inventory();

                    //[CategoryID]
                    if (reader.IsDBNull(reader.GetOrdinal("CategoryID")))
                    {
                        invent.CategoryID = -1;
                    }
                    else
                    {
                        invent.CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID"));
                    }

                    //[SubCategoryID] 
                    if (reader.IsDBNull(reader.GetOrdinal("SubCategoryID")))
                    {
                        invent.SubCategoryID = -1;
                    }
                    else
                    {
                        invent.SubCategoryID = reader.GetInt32(reader.GetOrdinal("SubCategoryID"));
                    }

                    //[CategoryName]
                    if (reader.IsDBNull(reader.GetOrdinal("CategoryName")))
                    {
                        invent.CategoryName = string.Empty;
                    }
                    else
                    {
                        invent.CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"));
                    }

                    //[SubCategoryName]
                    if (reader.IsDBNull(reader.GetOrdinal("SubCategoryName")))
                    {
                        invent.SubCategoryName = string.Empty;
                    }
                    else
                    {
                        invent.SubCategoryName = reader.GetString(reader.GetOrdinal("SubCategoryName"));
                    }

                    m_inventory_list.Add(invent);
                }
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
            return 0;
        }

        public override int Update()
        {
            Console.WriteLine("Called from Inventories::Update())");
            return 0;
        }
    }
}
