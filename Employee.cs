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
    class Employee : Database
    {
        public override int Create()
        {
            Console.WriteLine("Called from Employee::Create())");
            return 0;
        }

        public override int Read()
        {
            Console.WriteLine("Called from Employee::Read())");
            return 0;
        }

        public override int Update()
        {
            Console.WriteLine("Called from Employee::Update())");
            return 0;
        }

        public override int Delete()
        {
            Console.WriteLine("Called from Employee::Delete())");
            return 0;
        }

        public Employee(SqlConnection sql_connection)
        {
        }
    }
}
