using JAMK.ICT.Data;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JAMK.ICT.ADOBlanco
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private string connStr;
    private string tableName;
    private DataTable dt;

    public MainWindow()
    {
      InitializeComponent();
      IniMyStuff();
    }

    private void IniMyStuff()
    {
      try
      {
        string msg = "";
        connStr = Properties.Settings.Default.Tietokanta;
        lbMessages.Content = connStr;
        tableName = Properties.Settings.Default.Taulu;
        dt = DBPlacebo.GetAllCustomersFromSQLServer(connStr, tableName, "", out msg);
        FillMyCombo();
        lbMessages.Content = msg;
      }
      catch (Exception ex)
      {
        lbMessages.Content = ex.Message;
      }
    }

    private void FillMyCombo()
    {
      var result = (from c in dt.AsEnumerable()
                    select c.Field<string>("city")).Distinct().ToList();
      cbCountries.ItemsSource = result;
    }

    private void btnGet3_Click(object sender, RoutedEventArgs e)
    {
      dgCustomers.ItemsSource = DBPlacebo.GetTestCustomers().DefaultView;
    }

    private void btnGetAll_Click(object sender, RoutedEventArgs e)
    {
      // Haetaan asiakastietopalvelimelta
      string msg = "";

      try
      {
        // Kaikki asiakkaat DataGridiin
        dgCustomers.ItemsSource = dt.DefaultView;
      }
      catch (Exception ex)
      {
        msg = ex.Message;
      }
      finally
      {
        lbMessages.Content = msg;
      }
    }

    private void btnGetFrom_Click(object sender, RoutedEventArgs e)
    {
      //TODO
    }

    private void btnYield_Click(object sender, RoutedEventArgs e)
    {
        JSON.JSONPlacebo2015 roskaa = new JSON.JSONPlacebo2015();
        MessageBox.Show(roskaa.Yield());
    }

    private void cbCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {      
      string msg = "";

      try
      {
        // Haetaan asiakkaat valitusta kaupungista
        dt.DefaultView.RowFilter = string.Format("city='{0}'", cbCountries.SelectedValue);
        dgCustomers.ItemsSource = dt.DefaultView;
      }
      catch (Exception ex)
      {
        msg = ex.Message;
      }
      finally
      {
        lbMessages.Content = msg;
      }
    }

  }
}
