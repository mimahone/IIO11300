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
  public class DBCustomer
  {
    SqlConnection connection;
    DataTable customers;

    /// <summary>
    /// Constructor
    /// </summary>
    public DBCustomer()
    {
      connection = new SqlConnection(Viiniasiakkaat.Properties.Settings.Default.ConnectionString);
    }

    /// <summary>
    /// Get Customers as DataTable from Database
    /// </summary>
    /// <param name="message">return message</param>
    /// <returns>DataTable containing customers</returns>
    public DataTable GetCustomers(out string message)
    {
      try
      {
        const string sql = "SELECT ID, Firstname, Lastname, Address, ZIP, City FROM Customer ORDER BY Lastname, Firstname";
        SqlDataAdapter da = new SqlDataAdapter(sql, connection);
        customers = new DataTable("Customers");
        da.Fill(customers);

        message = string.Format("Haettu {0} asiakkaan tiedot", customers.Rows.Count);
        return customers;
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
    public void CreateCustomer(Customer customer, out string message)
    {
      try
      {
        const string cmdText = @"
INSERT INTO customer (Firstname, Lastname, Address, ZIP, City) 
VALUES (@FirstName, @LastName, @Address, @ZIP, @City)
";

        SqlCommand command = new SqlCommand(cmdText, connection);
        command.Parameters.AddWithValue("@FirstName", customer.FirstName);
        command.Parameters.AddWithValue("@LastName", customer.LastName);
        command.Parameters.AddWithValue("@Address", customer.Address);
        command.Parameters.AddWithValue("@ZIP", customer.ZIP);
        command.Parameters.AddWithValue("@City", customer.City);

        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        if (!(command.ExecuteNonQuery() > 0))
        {
          throw new Exception("CreateCustomer() failed to save new customer!");
        }

        message = "Uuden asiakkaan tiedot on tallennettu";
      }
      catch (Exception ex)
      {
        message = ex.Message;
        throw ex;
      }
      finally
      {
        if (connection.State == ConnectionState.Open)
        {
          connection.Close();
        }
      }
    }

    /// <summary>
    /// Update Customer to Database
    /// </summary>
    /// <param name="customer">Customer to Update</param>
    /// <param name="message">return message</param>
    public void UpdateCustomer(Customer customer, out string message)
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

        SqlCommand command = new SqlCommand(cmdText, connection);
        command.Parameters.AddWithValue("@ID", customer.Id);
        command.Parameters.AddWithValue("@FirstName", customer.FirstName);
        command.Parameters.AddWithValue("@LastName", customer.LastName);
        command.Parameters.AddWithValue("@Address", customer.Address);
        command.Parameters.AddWithValue("@ZIP", customer.ZIP);
        command.Parameters.AddWithValue("@City", customer.City);

        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        if (!(command.ExecuteNonQuery() > 0))
        {
          throw new Exception("UpdateCustomer() failed to update customer details!");
        }

        message = "Asiakkaan tiedot on päivitetty";
      }
      catch (Exception ex)
      {
        message = ex.Message;
        throw ex;
      }
      finally
      {
        if (connection.State == ConnectionState.Open)
        {
          connection.Close();
        }
      }
    }

    /// <summary>
    /// Delete Customer fromDatabase
    /// </summary>
    /// <param name="id">Id of Customer to delete</param>
    /// <param name="message">return message</param>
    public void DeleteCustomer(int id, out string message)
    {
      try
      {
        const string cmdText = @"DELETE FROM customer WHERE ID = @ID";

        SqlCommand command = new SqlCommand(cmdText, connection);
        command.Parameters.AddWithValue("@ID", id);

        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        if (!(command.ExecuteNonQuery() > 0))
        {
          throw new Exception("DeleteCustomer() failed to delete customer!");
        }

        message = "Asiakkaan tiedot on poistettu";
      }
      catch (Exception ex)
      {
        message = ex.Message;
        throw ex;
      }
      finally
      {
        if (connection.State == ConnectionState.Open)
        {
          connection.Close();
        }
      }
    }

  }
}
