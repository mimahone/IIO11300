using System;
using System.Collections.ObjectModel;

namespace JAMK.IT
{
  public class Employees
  {
    //EmployeeEntities ctx;

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

    /// <summary>
    /// Create Employee
    /// </summary>
    /// <param name="employee">Employee to Create</param>
    /// <returns>Count of affected rows</returns>
    public static int CreateEmployee(Employee employee)
    {
      try
      {
        //ctx.Employees.Add(employee);
        //int i = ctx.SaveChanges();
        int i = 1; // temporarily

        if (i > 0)
        {
          employees.Add(employee);
        }

        return i;
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Save Employee Changes
    /// </summary>
    /// <returns>Count of affected rows</returns>
    public static int SaveChanges()
    {
      try
      {
        //return ctx.SaveChanges();
        return 1; // temporarily
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Delete Employee
    /// </summary>
    /// <param name="employee">Employee to Delete</param>
    /// <returns>Count of affected rows</returns>
    public static int DeleteEmployee(Employee employee)
    {
      try
      {
        //ctx.Employees.Remove(employee);
        //int i = ctx.SaveChanges();
        int i = 1; // temporarily

        if (i > 0)
        {
          employees.Remove(employee);
        }

        return i;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static float GetSalarySum()
    {
      float sum = 0;

      foreach (var employee in EmployeeList)
      {
        if (employee is FullTimeEmployee)
        {
          sum += (employee as FullTimeEmployee).CalculateSalary();
        }
        else if (employee is PartTimeEmployee)
        {
          sum += (employee as PartTimeEmployee).CalculateSalary();
        }
      }

      return sum;
    }

    #endregion METHODS

  }
}
