using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Viiniasiakkaat
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void btnGetCustomers_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.ConnectionString))
        {
          //yhteys
          //dataadapter
          const string sql = "SELECT Lastname, Firstname, Address, City FROM vCustomers ORDER BY Lastname";
          SqlDataAdapter da = new SqlDataAdapter(sql, conn);
          DataTable dt = new DataTable("Customers");
          da.Fill(dt);
          //sidotaan datatable UI-kontrolliin
          //Aseta ListBoxin dataconteksi DataTable ja aseta sopivasti sen DisplayMemberPath
          lbxCustomers.DataContext = dt;
          myGrid.DataContext = dt;
          lbxCustomers.DisplayMemberPath = "Firstname";
          conn.Close();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }
  }
}
