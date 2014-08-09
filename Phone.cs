using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerdepot
{
    public class Phone
    {
        public int ID { get; set; }

        private int m_PhoneID;

        //PhoneID property
        public int PhoneID
        {
            get
            {
                return this.m_PhoneID;
            }
            set
            {
                this.m_PhoneID = value;
            }
        }
        
        private string m_PhoneNumber;

        public string PhoneNumber
        {
            get
            {
                return this.m_PhoneNumber;
            }
            set
            {
                this.m_PhoneNumber = value;
            }
        }

        private int m_PhoneType;

        public int PhoneType
        {
            get
            {
                return this.m_PhoneType;
            }
            set
            {
                this.m_PhoneType = value;
            }
        }
    }
}
