/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 23.3.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/

using System;
using System.Data;
using System.Data.SqlClient;

namespace JAMK.IT.IIO11300
{
  public class DBCustomers
  {
    /// <summary>
    /// ConnectionString
    /// </summary>
    private static string ConnectionString {
      get { return Viiniasiakkaat.Properties.Settings.Default.ConnectionString; }
    }

    /// <summary>
    /// Get Customers as DataTable from Database
    /// </summary>
    /// <param name="message">return message</param>
    /// <returns>DataTable containing customers</returns>
    public static DataTable GetCustomers(out string message)
    {
      try
      {
        const string sql = "SELECT ID, Firstname, Lastname, Address, ZIP, City FROM Customer ORDER BY Lastname, Firstname";

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
          SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
          DataTable table = new DataTable("Customers");
          adapter.Fill(table);

          message = string.Format("Haettu {0} asiakkaan tiedot", table.Rows.Count);
          return table;
        }
      }
      catch (Exception ex)
      {
        message = ex.Message;
        throw ex;
      }
    }

    /// <summary>
    /// Add Customer to Database
    /// </summary>
    /// <param name="customer">Customer to Create</param>
    /// <param name="message">return message</param>
    /// <returns>Count of affected rows in database</returns>
    public static int InsertCustomer(Customer customer, out string message)
    {
      try
      {
        const string cmdText = @"
INSERT INTO customer (Firstname, Lastname, Address, ZIP, City) 
VALUES (@FirstName, @LastName, @Address, @ZIP, @City)
";

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
          SqlCommand command = new SqlCommand(cmdText, connection);
          command.Parameters.AddWithValue("@FirstName", customer.FirstName);
          command.Parameters.AddWithValue("@LastName", customer.LastName);
          command.Parameters.AddWithValue("@Address", customer.Address);
          command.Parameters.AddWithValue("@ZIP", customer.ZIP);
          command.Parameters.AddWithValue("@City", customer.City);

          connection.Open();        
          int c = command.ExecuteNonQuery();
          connection.Close();

          if (c > 0)
          {
            message = "Uuden asiakkaan tiedot on tallennettu";
          }
          else
          {
            throw new Exception("InsertCustomer() failed to insert new customer!");
          }

          return c;
        }
      }
      catch (Exception ex)
      {
        message = ex.Message;
        throw ex;
      }
    }

    /// <summary>
    /// Update Customer to Database
    /// </summary>
    /// <param name="customer">Customer to Update</param>
    /// <param name="message">return message</param>
    /// <returns>Count of affected rows in database</returns>
    public static int UpdateCustomer(Customer customer, out string message)
    {
      try
      {
        const string cmdText = @"
UPDATE customer SET 
  Firstname = @FirstName,
  Lastname = @LastName,
  Address = @Address,
  ZIP = @ZIP,
  City = @City
WHERE
  ID = @ID
";
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
          SqlCommand command = new SqlCommand(cmdText, connection);
          command.Parameters.AddWithValue("@ID", customer.Id);
          command.Parameters.AddWithValue("@FirstName", customer.FirstName);
          command.Parameters.AddWithValue("@LastName", customer.LastName);
          command.Parameters.AddWithValue("@Address", customer.Address);
          command.Parameters.AddWithValue("@ZIP", customer.ZIP);
          command.Parameters.AddWithValue("@City", customer.City);

          connection.Open();
          int c = command.ExecuteNonQuery();
          connection.Close();

          if (c > 0)
          {
            message = "Asiakkaan tiedot on päivitetty";
          }
          else
          {
            throw new Exception("UpdateCustomer() failed to update customer details!");
          }

          return c;
        }
      }
      catch (Exception ex)
      {
        message = ex.Message;
        throw ex;
      }
    }

    /// <summary>
    /// Delete Customer fromDatabase
    /// </summary>
    /// <param name="id">Id of Customer to delete</param>
    /// <param name="message">return message</param>
    public static int DeleteCustomer(int id, out string message)
    {
      try
      {
        const string cmdText = @"DELETE FROM customer WHERE ID = @ID";

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
          SqlCommand command = new SqlCommand(cmdText, connection);
          command.Parameters.AddWithValue("@ID", id);

          connection.Open();
          int c = command.ExecuteNonQuery();
          connection.Close();

          if (c > 0)
          {
            message = "Asiakkaan tiedot on poistettu";
          }
          else
          {
            throw new Exception("DeleteCustomer() failed to delete customer!");
          }

          return c;
        }
      }
      catch (Exception ex)
      {
        message = ex.Message;
        throw ex;
      }
    }

  }
}
