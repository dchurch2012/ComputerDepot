using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerdepot
{
    public class Errors
    {
        public int code {get; set;}
        public string message { get; set; }

        public Errors(int err_code, string err_message)
        {
            code = err_code;
            message = err_message;
        }


    }
}




