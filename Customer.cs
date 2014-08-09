using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computerdepot
{
    public class Customer
    {
        // Columns:

        //FirstName, 
        //LastName, 
        //CIN,
        //[Customer].AddressID, 
        //[Address].Street,
        //[Address].StreetNumber,
        //[Address].Apt,
        //[Address].City, 
        //[Address].[State],
        //[Address].[Zip],

        public int EmailID { get; set; }
        public int PhoneID { get; set; }

        public Customer()
        {
            m_EMail = new List<Email>();
            m_Phone = new List<Phone>();
        }
       
        private List<Email> m_EMail;
        
        public List<Email> EMail
        {
            get
            {
                return this.m_EMail;
            }
            set
            {
                this.m_EMail = value;
            }
        }

        private List<Phone> m_Phone;
     
        public List<Phone> Phone
        {
            get
            {
                return this.m_Phone;
            }
            set
            {
                this.m_Phone = value;
            }
        }


        private string m_FirstName; 

		//FirstName property
		public string FirstName
		{
			get
			{
				return this.m_FirstName;
			}
			set
			{
				this.m_FirstName = value;
			}
		}

		private string m_LastName;
		
		//LastName property
		public string LastName
		{
			get
			{
				return this.m_LastName;
			}
			set
			{
				this.m_LastName = value;
			}
		}
		
		private int m_CIN;

		//CIN property
		public int CIN
		{
			get
			{
				return this.m_CIN;
			}
			set
			{
				this.m_CIN = value;
			}
		}
		

		private int m_PaymentID;
		
		//PaymentID property
		public int PaymentID
		{
			get
			{
				return this.m_PaymentID;
			}
			set
			{
				this.m_PaymentID = value;
			}
		}


		private int m_AddressID;

		//AddressID property
		public int AddressID
		{
			get
			{
				return this.m_AddressID;
			}
			set
			{
				this.m_AddressID = value;
			}
		}

        
        private string m_Street;

        public string Street
        {
            get
            {
                return this.m_Street;
            }
            set
            {
                this.m_Street = value;
            }
        }

        private string m_StreetNumber;

        public string StreetNumber
        {
            get
            {
                return this.m_StreetNumber;
            }
            set
            {
                this.m_StreetNumber = value;
            }
        }

        private string m_Apt;

        public string Apt
        {
            get
            {
                return this.m_Apt;
            }
            set
            {
                this.m_Apt = value;
            }
        }

        private string m_City;

        public string City
        {
            get
            {
                return this.m_City;
            }
            set
            {
                this.m_City = value;
            }
        }


        private string m_State;

        public string State
        {
            get
            {
                return this.m_State;
            }
            set
            {
                this.m_State = value;
            }
        }

        private string m_Zip;

        public string Zip
        {
            get
            {
                return this.m_Zip;
            }
            set
            {
                this.m_Zip = value;
            }
        }


        




    }
}
