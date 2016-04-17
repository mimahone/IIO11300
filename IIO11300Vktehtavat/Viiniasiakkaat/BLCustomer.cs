/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 23.3.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/

using System;
using System.Collections.Generic;
using System.Data;

namespace JAMK.IT.IIO11300
{
  //public enum Status
  //{
  //  Unchanged = 0,
  //  Created = 1,
  //  Modified = 2,
  //  Deleted = 3
  //}

  /// <summary>
  /// Class for Customer object
  /// </summary>
  public class Customer
  {
    #region PROPERTIES

    public int Id { get; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Address { get; set; }

    public string ZIP { get; set; }

    public string City { get; set; }

    //public Status Status { get; set; }

    #endregion

    #region CONSTRUCTORS

    public Customer(int id)
    {
      Id = id;
    }

    public Customer(string firstName, string lastName, string address, string zip, string city)
    {
      FirstName = firstName;
      LastName = lastName;
      Address = address;
      ZIP = zip;
      City = city;
    }

    #endregion
  }

  /// <summary>
  /// Class for Customers Business Logic
  /// </summary>
  public static class Customers
  {
    /// <summary>
    /// Get Customers
    /// </summary>
    /// <param name="message">return message</param>
    /// <returns>List of Customers</returns>
    public static List<Customer> GetCustomers(out string message)
    {
      try
      {
        DataTable dt;
        List<Customer> customers = new List<Customer>();

        dt = DBCustomers.GetCustomers(out message);

        // ORM =  Muutetaan datatablen rivit olioiksi
        Customer customer;

        foreach (DataRow row in dt.Rows)
        {
          customer = new Customer((int)row[0]);
          customer.FirstName = row[1].ToString();
          customer.LastName = row[2].ToString();
          customer.Address = row[3].ToString();
          customer.ZIP = row[4].ToString();
          customer.City = row[5].ToString();
          customers.Add(customer);
        }

        // Palautus
        return customers;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Update Customer
    /// </summary>
    /// <param name="customer">Customer to Update</param>
    /// <param name="message">return message</param>
    /// <returns>Count of affected rows in database</returns>
    public static int UpdateCustomer(Customer customer, out string message)
    {
      try
      {
        int c = DBCustomers.UpdateCustomer(customer, out message);
        return c;
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Update Customer
    /// </summary>
    /// <param name="customer">Customer to Insert</param>
    /// <param name="message">return message</param>
    /// <returns>True id succeeded else false</returns>
    public static bool InsertCustomer(Customer customer, out string message)
    {
      try
      {
        int c = DBCustomers.InsertCustomer(customer, out message);
        return (c > 0);
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Delete Customer
    /// </summary>
    /// <param name="customer">Customer to Insert</param>
    /// <param name="message">return message</param>
    /// <returns>True id succeeded else false</returns>
    public static bool DeleteCustomer(Customer customer, out string message)
    {
      try
      {
        int c = DBCustomers.DeleteCustomer(customer.Id, out message);
        return (c > 0);
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
