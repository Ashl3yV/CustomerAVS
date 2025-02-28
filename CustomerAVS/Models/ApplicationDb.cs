using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;

namespace CustomerAVS.Models
{
    public class ApplicationDb : DbContext
    {
        public ApplicationDb() : base("DBCustomer") { }

        public DbSet<customer> Customers { get; set; }
    }
}