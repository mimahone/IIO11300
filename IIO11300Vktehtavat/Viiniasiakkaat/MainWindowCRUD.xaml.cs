using JAMK.IT.IIO11300;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Viiniasiakkaat
{
  /// <summary>
  /// Interaction logic for MainWindowCRUD.xaml
  /// </summary>
  public partial class MainWindowCRUD : Window
  {
    DBCustomer dao;
    

    public MainWindowCRUD()
    {
      InitializeComponent();
      IniMyStaff();
    }

    private void IniMyStaff()
    {
      dao = new DBCustomer();
    }

    private void btnGetCustomers_Click(object sender, RoutedEventArgs e)
    {
      string message = "";

      try
      {
        grdCustomers.DataContext = dao.GetCustomers(out message);
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

    private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
    {
      spNewCustomer.Height = 20;
      txtFirstName.Focus();
    }

    private void btnCreateCustomer_Click(object sender, RoutedEventArgs e)
    {
      if (HasDetailsErrors()) return;

      string message = "";

      try
      {
        Customer customer = new Customer();
        customer.FirstName = txtFirstName.Text;
        customer.LastName = txtLastName.Text;
        customer.Address = txtAddress.Text;
        customer.ZIP = txtZIP.Text;
        customer.City = txtCity.Text;
        dao.CreateCustomer(customer, out message);
        MessageBox.Show(message);

        btnGetCustomers_Click(this, null);
        spNewCustomer.Height = 0;
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
        if (grdCustomers.SelectedItem == null)
        {
          MessageBox.Show("Valitse ensin asiakas!");
          return;
        }

        object[] items = ((DataRowView)grdCustomers.SelectedItem).Row.ItemArray;
        int id = (int)items[0];
        string firstName = (string)items[1];
        string lastName = (string)items[2];

        MessageBoxResult result = MessageBox.Show(
            string.Format("Haluatko varmasti poistaa asiakkaan {0} {1}?", firstName, lastName),
            "Viinikellarin asiakkaat",
            MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

        if (result == MessageBoxResult.Yes)
        {
          dao.DeleteCustomer(id, out message);
          message = string.Format("Asiakas {0} {1} on poistettu", firstName, lastName);
          btnGetCustomers_Click(this, null);
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
