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
    public class Products : Database 
    {
        private List<Product> m_products = null;

        public override Object GetList()
        {
            return (Object)m_products;
        }

        public List<Product> Product
        {
            get
            {
                return this.m_products;
            }
            set
            {
                this.m_products= value;
            }
        }

        public Products(SqlConnection sql_connection) : base(sql_connection)
        {
            m_products = new List<Product>();
        }
        

        public override int Create()
        {
            return 0;
        }

        public override int Read()
        {
            try
            {
                command = new SqlCommand("GetProducts", sqlComputerDepot);
                command.CommandType = CommandType.StoredProcedure;
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product prod = new Product();

                    if (reader.IsDBNull(reader.GetOrdinal("ProductID")))
                    {
                        prod.ProductID = -1;
                    }
                    else
                    {
                        prod.ProductID = reader.GetInt32(reader.GetOrdinal("ProductID"));
                    }

                    
                    //ProductPrice
                    prod.ProductPrice = (double)reader.GetDecimal(reader.GetOrdinal("ProductPrice"));

                    //Qty 
                    prod.Qty = reader.GetInt32(reader.GetOrdinal("Qty"));

           
                    //ProductDescrip
                    if (reader.IsDBNull(reader.GetOrdinal("ProductDescrip")))
                    {
                        prod.ProductDescription = string.Empty;
                    }
                    else
                    {
                        //GetOrdinal will return the correct array index in the reader
                        //regardless of how the order is changed (if say, the order of returned
                        //items is changed in the stored procedure
                        prod.ProductDescription = reader.GetString(reader.GetOrdinal("ProductDescrip"));
                    }

                    //StockedDate 
                    if (reader.IsDBNull(reader.GetOrdinal("StockedDate")))
                    {
                        prod.StockedDate = new DateTime();
                    }
                    else
                    {
                        prod.StockedDate = reader.GetDateTime(reader.GetOrdinal("StockedDate"));
                    }

                    //ReorderLevel 
                    if (reader.IsDBNull(reader.GetOrdinal("ReorderLevel")))
                    {
                        prod.ReorderLevel = -1;
                    }
                    else
                    {
                        prod.ReorderLevel = reader.GetInt32(reader.GetOrdinal("ReorderLevel"));
                    }
                    
                    //ProductType
                    if (reader.IsDBNull(reader.GetOrdinal("ProductType")))
                    {
                        prod.ProductType = string.Empty; 
                    }
                    else
                    {
                        prod.ProductType  = reader.GetString(reader.GetOrdinal("ProductType"));
                    }

                    //CatalogID
                    if (reader.IsDBNull(reader.GetOrdinal("CatalogID")))
                    {
                        prod.CatalogID = string.Empty;
                    }
                    else
                    {
                        prod.CatalogID = reader.GetString(reader.GetOrdinal("CatalogID"));
                    }
                    
                    //Mfr
                    if (reader.IsDBNull(reader.GetOrdinal("Mfr")))
                    {
                         prod.Mfr = string.Empty;
                    }
                    else
                    {
                        prod.Mfr = reader.GetString(reader.GetOrdinal("Mfr"));
                    }

                    //CategoryID
                    if (reader.IsDBNull(reader.GetOrdinal("CategoryID")))
                    {
                        prod.CategoryID = -1;
                    }
                    else
                    {
                        prod.CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID"));
                    }

                    //CategoryName
                    if (reader.IsDBNull(reader.GetOrdinal("CategoryName")))
                    {
                        prod.CategoryName = string.Empty;
                    }
                    else
                    {
                        prod.CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"));
                    }

                    //SubCategoryID
                    if (reader.IsDBNull(reader.GetOrdinal("SubCategoryID")))
                    {
                        prod.SubCategoryID = -1;
                    }
                    else
                    {
                        prod.SubCategoryID = reader.GetInt32(reader.GetOrdinal("SubCategoryID"));
                    }
                    
                    //SubCategoryName
                    if (reader.IsDBNull(reader.GetOrdinal("SubCategoryName")))
                    {
                        prod.SubCategoryName = string.Empty;
                    }
                    else
                    {
                        prod.SubCategoryName = reader.GetString(reader.GetOrdinal("SubCategoryName"));
                    }

                    //DomainID
                    if (reader.IsDBNull(reader.GetOrdinal("DomainID")))
                    {
                        prod.DomainID = -1;
                    }
                    else
                    {
                        prod.DomainID = reader.GetInt32(reader.GetOrdinal("DomainID"));
                    }
                    m_products.Add(prod);
                }
                reader.Close();
                //GetPhoneNumbers();
                //GetEMailAddress();
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

        protected int GetProductCategories()
        {
            try
            {
                command = null;

                command = new SqlCommand("GetCategoriesForCategoryID", sqlComputerDepot);
                command.CommandType = CommandType.StoredProcedure;
         
                for(int index = 0; index < m_products.Count(); index++)
                {
                    command.Parameters.Clear(); 
                    command.Parameters.Add("@CategoryID ", SqlDbType.Int).Value = m_products[index].CategoryID;
                    reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        continue;
                    }

                    while (reader.Read())
                    {
                        ProductCategory _cat = new ProductCategory();

                        //ProductCategory
                        if (reader.IsDBNull(reader.GetOrdinal("ProductCategory")))
                        {
                            _cat.ProductCategoryID = -1;
                        }
                        else
                        {
                            _cat.ProductCategoryID = reader.GetOrdinal("ProductCategory");
                        }

                        //CategoryID
                        if (reader.IsDBNull(reader.GetOrdinal("CategoryID")))
                        {
                           _cat.CategoryID = -1;
                        }
                        else
                        {
                            _cat.CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID"));
                        }

                        //Category
                        if (reader.IsDBNull(reader.GetOrdinal("Category")))
                        {
                            _cat.Category = string.Empty;
                        }
                        else
                        {
                            _cat.Category = reader.GetString(reader.GetOrdinal("Category"));
                        }

                        //CategoryDescription
                        if (reader.IsDBNull(reader.GetOrdinal("CategoryDescription")))
                        {
                            _cat.CategoryDescription = string.Empty;
                        }
                        else
                        {
                            _cat.CategoryDescription = reader.GetString(reader.GetOrdinal("CategoryDescription"));
                        }
                        
                        m_products[index].Categories.Add(_cat);
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

        public override int Update()
        {
            return 0;
        }

        public override int Delete()
        {
            return 0;
        }

    }
}