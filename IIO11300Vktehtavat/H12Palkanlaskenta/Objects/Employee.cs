using System;

namespace JAMK.IT
{
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
      set { employeeNumber = value; }
    }

    private DateTime startDate;

    public DateTime StartDate
    {
      get { return startDate; }
      set { startDate = value; }
    }

    private float salary = 42;

    public float Salary
    {
      get { return salary; }
      set { salary = value; }
    }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    public Employee()
    {
      salary = 42;
    }

    #endregion CONSTRUCTORS

    #region METHODS

    public virtual float CalculateSalary()
    {
      throw new NotImplementedException();
    }

    #endregion METHODS
  }
}
