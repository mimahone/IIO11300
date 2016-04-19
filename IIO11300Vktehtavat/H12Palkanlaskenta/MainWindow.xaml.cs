using System;
using System.Windows;

namespace JAMK.IT
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    Company company = new Company();
    Employee person = null;

    public MainWindow()
    {
      InitializeComponent();
      IniMyStaff();
      //spEmployee.DataContext = company.Person;
    }

    private void IniMyStaff()
    {
      Employees.GetEmployees();
      lstEmployees.DataContext = Employees.EmployeeList;
      //person = new FullTimeEmployee()
      //{
      //  FirstName = "Jack",
      //  LastName = "Russell",
      //  BirthDay = new DateTime(1985, 4, 17),
      //  PersonID = "ddmmyyyy-123X",
      //  EmployeeNumber = 141,
      //  MontlySalary = 3000
      //};

      //person = new PartTimeEmployee()
      //{
      //  FirstName = "Jack",
      //  LastName = "Russell",
      //  BirthDay = new DateTime(1985, 4, 17),
      //  PersonID = "ddmmyyyy-123X",
      //  EmployeeNumber = 141,
      //  HourlySalary = 20,
      //  WorkHours = 120
      //};

      //company.Person = person;
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnUpdate_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnGetWorkers_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnCalculate_Click(object sender, RoutedEventArgs e)
    {
      float salary = 0;

      foreach (Employee employee in Employees.EmployeeList)
      {
        if (employee is FullTimeEmployee)
        {
          salary += ((FullTimeEmployee)employee).CalculateSalary();
        }
        else if (employee is PartTimeEmployee)
        {
          salary += ((PartTimeEmployee)employee).CalculateSalary();
        }
        
      }

      tbMessage.Text = string.Format("Henkilöitä {0}, palkkoja yhteensä {1}", Employees.EmployeeList.Count, salary);
    }

    private void lstEmployees_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      spEmployee.DataContext = (Employee)lstEmployees.SelectedItem;
    }
  }
}
