using JAMK.IT.IIO11300;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Viiniasiakkaat
{
  /// <summary>
  /// Interaction logic for MainWindowCRUD.xaml
  /// </summary>
  public partial class MainWindowCRUD : Window
  {
    public MainWindowCRUD()
    {
      InitializeComponent();
    }

    private void btnGetCustomers_Click(object sender, RoutedEventArgs e)
    {
      string message = "";

      try
      {
        dgCustomers.DataContext = Customers.GetCustomers(out message);
      }
      catch (Exception ex)
      {
        message = ex.Message;
        MessageBox.Show(ex.Message);
      }
      finally
      {
        lblMessage.Text = message;
      }
    }

    private void grdCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      spCustomer.DataContext = dgCustomers.SelectedItem;
    }

    private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
    {
      spCustomer.DataContext = null;
      txtFirstName.Focus();
    }

    private void btnSaveCustomer_Click(object sender, RoutedEventArgs e)
    {
      if (HasDetailsErrors()) return;

      string message = "";
      Customer customer;

      try
      {
        if (spCustomer.DataContext != null)
        {
          customer = (Customer)spCustomer.DataContext;
          Customers.UpdateCustomer(customer, out message);
        }
        else
        {
          customer = new Customer(0);
          customer.FirstName = txtFirstName.Text;
          customer.LastName = txtLastName.Text;
          customer.Address = txtAddress.Text;
          customer.ZIP = txtZIP.Text;
          customer.City = txtCity.Text;
          Customers.InsertCustomer(customer, out message);
        }

        string msg = "";
        dgCustomers.DataContext = Customers.GetCustomers(out msg);
        SelectGridRow(customer);
      }
      catch (Exception ex)
      {
        message = ex.Message;
        MessageBox.Show(ex.Message);
      }
      finally
      {
        lblMessage.Text = message;
      }
    }

    private void SelectGridRow(Customer customer)
    {
      // Vähän kömpelö rivin haku! :)
      // TODO: Do this better!
      Customer c;

      foreach (var item in dgCustomers.Items)
      {
        c = (Customer)item;

        if (c.FirstName.Equals(customer.FirstName) &&
          c.LastName.Equals(customer.LastName) &&
          c.Address.Equals(customer.Address) &&
          c.ZIP.Equals(customer.ZIP) &&
          c.City.Equals(customer.City))
        {
          dgCustomers.SelectedItem = item;
          break;
        }
      }
    }

    private bool HasDetailsErrors()
    {
      bool errors = false;
      string message = "";
      TextBox txt = null;

      if (string.IsNullOrWhiteSpace(txtFirstName.Text))
      {
        txt = txtFirstName;
        message = "etunimi puuttuu";
        errors = true;
      }
      else if (string.IsNullOrWhiteSpace(txtLastName.Text))
      {
        txt = txtLastName;
        message = "sukunimi puuttuu";
        errors = true;
      }
      else if (string.IsNullOrWhiteSpace(txtAddress.Text))
      {
        txt = txtAddress;
        message = "osoite puuttuu";
        errors = true;
      }
      else if (string.IsNullOrWhiteSpace(txtZIP.Text))
      {
        txt = txtZIP;
        message = "postinumero puuttuu";
        errors = true;
      }
      else if (string.IsNullOrWhiteSpace(txtCity.Text))
      {
        txt = txtCity;
        message = "kaupunki puuttuu";
        errors = true;
      }

      if (errors)
      {
        MessageBox.Show(string.Format("Ei voi tallentaa koska {0}!", message));
        txt.Focus();
      }

      return errors;
    }

    private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
    {
      string message = "";

      try
      {
        if (dgCustomers.SelectedItem == null)
        {
          MessageBox.Show("Valitse ensin asiakas!");
          return;
        }

        Customer current = (Customer)spCustomer.DataContext;

        MessageBoxResult result = MessageBox.Show(
            string.Format("Haluatko varmasti poistaa asiakkaan {0} {1}?", current.FirstName, current.LastName),
            "Viinikellarin asiakkaat",
            MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

        if (result == MessageBoxResult.Yes)
        {
          Customers.DeleteCustomer(current, out message);
          message = string.Format("Asiakas {0} {1} on poistettu", current.FirstName, current.LastName);
          string msg = "";
          dgCustomers.DataContext = Customers.GetCustomers(out msg);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
      finally
      {
        lblMessage.Text = message;
      }
    }

  }
}
