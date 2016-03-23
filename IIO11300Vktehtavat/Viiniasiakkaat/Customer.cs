/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 23.3.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/

namespace JAMK.IT.IIO11300
{
  public enum Status
  {
    Unchanged = 0,
    Created = 1,
    Modified = 2,
    Deleted = 3
  }

  public class Customer
  {
    public Customer() { }

    public Customer(string firstName, string lastName, string address, string zip, string city)
    {
      FirstName = firstName;
      LastName = lastName;
      Address = address;
      ZIP = zip;
      City = city;
    }

    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Address { get; set; }

    public string ZIP { get; set; }

    public string City { get; set; }

    public Status Status { get; set; }
  }
}
