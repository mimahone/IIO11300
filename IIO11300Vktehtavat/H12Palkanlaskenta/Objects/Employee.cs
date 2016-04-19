using System;

namespace JAMK.IT
{
  public enum EmployeeType
  {
    FullTime = 0,
    PartTime = 1
  }

  public class Employee : Person
  {
    #region PROPERTIES

    private string position = "orja";

    public string Position
    {
      get { return position; }
      set { position = value; }
    }

    private int employeeNumber;

    public int EmployeeNumber
    {
      get { return employeeNumber; }
      set
      {
        employeeNumber = value;
        RaisePropertyChanged("FirstName");
        RaisePropertyChanged("LastName");
        RaisePropertyChanged("FullName");
        RaisePropertyChanged("DisplayName");
      }
    }

    private DateTime startDate;

    public DateTime StartDate
    {
      get { return startDate; }
      set { startDate = value; }
    }

    protected float salary = 42;

    public float Salary
    {
      get { return salary; }
      set { salary = value; }
    }

    private EmployeeType employeeType;

    public EmployeeType EmployeeType
    {
      get { return employeeType; }
      set
      {
        employeeType = value;
        RaisePropertyChanged("IsFullTimeEmployee");
        RaisePropertyChanged("IsPartTimeEmployee");
      }
    }

    public bool IsFullTimeEmployee
    {
      get { return EmployeeType == EmployeeType.FullTime; }
      set { EmployeeType = EmployeeType.FullTime; }
    }

    public bool IsPartTimeEmployee
    {
      get { return EmployeeType == EmployeeType.PartTime; }
      set { EmployeeType = EmployeeType.PartTime; }
    }

    /// <summary>
    /// Display name property
    /// </summary>
    public string DisplayName { get { return string.Format("{0} = {1}", EmployeeNumber, FullName); } }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    public Employee()
    {
      salary = 42;
    }

    public Employee(string firstName, string lastName, int employeeNumber, string position, float salary)
    {
      FirstName = firstName;
      LastName = lastName;
      EmployeeNumber = employeeNumber;
      Position = position;
      Salary = salary;
    }

    #endregion CONSTRUCTORS

    #region METHODS

    public virtual float CalculateSalary()
    {
      return salary;
    }

    #endregion METHODS
  }
}
