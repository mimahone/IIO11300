using JAMK.IT;
using System;
using System.Windows;

namespace H12PersonTester
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    Company company = new Company();

    public MainWindow()
    {
      InitializeComponent();
      IniMyPerson();
      spPerson.DataContext = company.Person;
    }

    private void IniMyPerson()
    {
      var person = new Person()
      {
        FirstName = "Jack",
        LastName = "Russell",
        BirthDay = new DateTime(1985, 4, 17),
        PersonID = "ddmmyyyy-123X"
      };

      company.Person = person;
    }

    private void btnCreatePerson_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnViewPerson_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
