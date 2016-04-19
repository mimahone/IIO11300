using System;
using System.Collections.ObjectModel;

namespace JAMK.IT
{
  public class Employees
  {
    #region PROPERTIES

    private static ObservableCollection<Employee> employees;

    public static ObservableCollection<Employee> EmployeeList { get { return employees; } }

    #endregion PROPERTIES

    #region METHODS

    /// <summary>
    /// Refresh Employees List
    /// </summary>
    public static void GetEmployees()
    {
      try
      {
        employees = new ObservableCollection<Employee>();

        employees.Add(new FullTimeEmployee() { EmployeeNumber = 1, FirstName = "Alfred", LastName = "Nodel", Position = "toimitusjohtaja", Salary = 5000 });
        employees.Add(new FullTimeEmployee() { EmployeeNumber = 2, FirstName = "Beatrix", LastName = "Bamboo", Position = "sihteeri", Salary = 2500 });
        employees.Add(new FullTimeEmployee() { EmployeeNumber = 3, FirstName = "Cecilia", LastName = "Tapper", Position = "arkkitehti", Salary = 3500 });
        employees.Add(new PartTimeEmployee() { EmployeeNumber = 4, FirstName = "Daavid", LastName = "Cooper", Position = "koodaaja", Salary = 20, WorkHours = 120 });
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    #endregion METHODS

  }
}
