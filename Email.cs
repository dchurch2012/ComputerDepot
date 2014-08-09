using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerdepot
{
    public class Email
    {
        public int ID { get; set; }

        private int m_EmailID;

        //EmailID property
        public int EmailID
        {
            get
            {
                return this.m_EmailID;
            }
            set
            {
                this.m_EmailID = value;
            }
        }

        private string m_EmailAddress;

        public string EmailAddress
        {
            get
            {
                return this.m_EmailAddress;
            }
            set
            {
                this.m_EmailAddress = value;
            }
        }
    }
}
