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
    //Employee person = null;

    public MainWindow()
    {
      InitializeComponent();
      IniMyStaff();
      //spEmployee.DataContext = company.Person;
    }

    private void IniMyStaff()
    {
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
      // Luodaan uusi työntekijä-olio ensin kontextiin ja sitten tietokantaan
      Employee newEmployee;
      if (btnAdd.Content.ToString() == "Lisää")
      {
        newEmployee = new Employee();
        spEmployee.DataContext = newEmployee;
        btnAdd.Content = "Tallenna";
        txtFirstName.Focus();
        tbMessage.Text = "Uuden työntekijän lisäys";
      }
      else
      {
        try
        {
          newEmployee = (Employee)spEmployee.DataContext;

          int i = 0;

          if (newEmployee.EmployeeType == EmployeeType.FullTime)
          {
            FullTimeEmployee fe = new FullTimeEmployee {
              FirstName = newEmployee.FirstName,
              LastName = newEmployee.LastName,
              EmployeeNumber = newEmployee.EmployeeNumber,
              Position = newEmployee.Position,
              Salary = newEmployee.Salary
            };

            i = Employees.CreateEmployee(fe);
          }
          else
          {
            PartTimeEmployee pe = new PartTimeEmployee
            {
              FirstName = newEmployee.FirstName,
              LastName = newEmployee.LastName,
              EmployeeNumber = newEmployee.EmployeeNumber,
              Position = newEmployee.Position,
              Salary = newEmployee.Salary
            };

            i = Employees.CreateEmployee(pe);
          }

          if (i < 1)
          {
            throw new Exception("Työntekijän tallennus ei onnistunut!");
          }

          btnAdd.Content = "Lisää";
          tbMessage.Text = string.Format("Työntekijä {0} on lisätty kantaan", newEmployee.FullName);
        }
        catch (Exception ex)
        {
          tbMessage.Text = ex.Message;
          MessageBox.Show(ex.Message);
        }
      }
    }

    private void btnUpdate_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        int i = Employees.SaveChanges();
        tbMessage.Text = "Muutetut tiedot on päivitetty";
      }
      catch (Exception ex)
      {
        tbMessage.Text = ex.Message;
        MessageBox.Show(ex.Message);
      }
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        var employee = (Employee)spEmployee.DataContext;

        var result = MessageBox.Show("Haluatko varmasti poistaa työntekijän " + employee.FullName, "Poiston varmistus", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
          int i = Employees.DeleteEmployee(employee);
          tbMessage.Text = "Työntekijä " + employee.FullName + " on poistettu";
        }
      }
      catch (Exception ex)
      {
        tbMessage.Text = ex.Message;
        MessageBox.Show(ex.Message);
      }
    }

    private void btnGetWorkers_Click(object sender, RoutedEventArgs e)
    {
      Employees.GetEmployees();
      lstEmployees.DataContext = Employees.EmployeeList;
    }

    private void btnCalculate_Click(object sender, RoutedEventArgs e)
    {
      tbMessage.Text = string.Format("Henkilöitä {0}, palkkoja yhteensä {1}", Employees.EmployeeList.Count, Employees.GetSalarySum());
    }

    private void lstEmployees_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      if (lstEmployees.SelectedItem == null) return;
      var employee = (Employee)lstEmployees.SelectedItem;
      spEmployee.DataContext = lstEmployees.SelectedItem;
      tbMessage.Text = "Valittu työntekijä " + employee.FullName;
    }
  }
}
