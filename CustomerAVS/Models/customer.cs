using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerAVS.Models
{
	public class customer
	{
        public int ID { get; set; }
        public int TotalVisits { get; set; }
        public string AccountNumber { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime AccountOpened { get; set; }
        public DateTime LastVisit { get; set; }

        

    }
}