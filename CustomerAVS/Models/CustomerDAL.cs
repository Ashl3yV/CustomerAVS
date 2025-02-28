using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CustomerAVS.Models;

public class CustomerDAL
{
    private string connectionString = ConfigurationManager.ConnectionStrings["DBCustomer"].ConnectionString;

    public List<customer> GetAllCustomers()
    {
        List<customer> customers = new List<customer>();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {

            string query = @"SELECT * FROM Customer";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new customer
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            TotalVisits = Convert.ToInt32(reader["TotalVisits"]),
                            AccountNumber = reader["AccountNumber"].ToString(),
                            Company = reader["Company"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            LastUpdated = Convert.ToDateTime(reader["LastUpdated"]),
                            AccountOpened = Convert.ToDateTime(reader["AccountOpened"]),
                            LastVisit = Convert.ToDateTime(reader["LastVisit"])
                        });
                    }
                }
            }
        }

        return customers;
    }

    public void AddCustomer(customer newCustomer)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_InsertCustomer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TotalVisits", newCustomer.TotalVisits);
                cmd.Parameters.AddWithValue("@AccountNumber", newCustomer.AccountNumber);
                cmd.Parameters.AddWithValue("@Company", newCustomer.Company);
                cmd.Parameters.AddWithValue("@FirstName", newCustomer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", newCustomer.LastName);
                cmd.Parameters.AddWithValue("@PhoneNumber", newCustomer.PhoneNumber);
                cmd.Parameters.AddWithValue("@LastUpdated", newCustomer.LastUpdated);
                cmd.Parameters.AddWithValue("@AccountOpened", newCustomer.AccountOpened);
                cmd.Parameters.AddWithValue("@LastVisit", newCustomer.LastVisit);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }


}
