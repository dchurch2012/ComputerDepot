using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using System.Configuration;

namespace computerdepot
{
    public class Database
    {
        protected SqlConnection sqlComputerDepot = null;
        public SqlConnection sqlComputerDepotConnection { get; set; }

        protected SqlDataReader reader = null;
        protected SqlCommand command = null;
        protected ErrorReporter error_reporter = null;

        public virtual Object GetList() //runtime polymorphism passing the database as an object and recast on other side
        {
            return null;
        }

        public Database()
        {
        }

        public Database(SqlConnection sql_connection)
        {
            sqlComputerDepot = sql_connection;
            error_reporter = new ErrorReporter();
        }
   
        public virtual int Create(Object obj)
        {
            return 0;
        }


        public virtual int Create()
        {
            Console.WriteLine("Called from Database::Create())");
            return 0;
        }

        public virtual int Read()
        {
            Console.WriteLine("Called from Database::Read())");
            return 0;
        }

        public virtual int Read(int type)
        {
            Console.WriteLine("Called from Database::Read())");
            return 0;
        }
        
        public virtual int Update()
        {
            Console.WriteLine("Called from Database::Update())");
            return 0;
        }

        public virtual int Update(Object obj)
        {
            return 0;
        }


        public virtual int Delete(int nvalue)
        {
            return 0;
        }

        public virtual int Delete()
        {
            Console.WriteLine("Called from Database::Delete())");
            return 0;
        }
    }
}
