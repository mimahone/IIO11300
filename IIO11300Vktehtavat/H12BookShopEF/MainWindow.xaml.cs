using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // for ObservableCollection
using System.ComponentModel;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace H12BookShopEF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    BookShopEntities ctx;
    ObservableCollection<Book> localBooks;
    ICollectionView view; // Filtteröintiä varten
    bool IsBooks;

    public MainWindow()
    {
      InitializeComponent();
      IniMyStaff();
    }

    private void IniMyStaff()
    {
      ctx = new BookShopEntities();
      ctx.Books.Load();
      localBooks = ctx.Books.Local;

      // ComboBoxin täyttäminen kirjojen eri mailla
      cbCountries.DataContext = localBooks.Select(n => n.country).Distinct().OrderBy(b => b);
      cbCountries.Visibility = Visibility.Visible;

      // View kirjojen filtterointia varten
      view = CollectionViewSource.GetDefaultView(localBooks);
    }

    private void btnGetCustomers_Click(object sender, RoutedEventArgs e)
    {
      var customers = ctx.Customers.ToList();
      dgBooks.DataContext = customers;
      IsBooks = false;
    }

    private void btnGetBooks_Click(object sender, RoutedEventArgs e)
    {
      dgBooks.DataContext = localBooks;
      IsBooks = true;
      cbCountries.SelectedIndex = -1;
    }

    private void dgBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (IsBooks)
      {
        spBook.DataContext = dgBooks.SelectedItem;
      }
      else
      {
        // Valittu item (tässä tapauksessa Customer-olio) asetetaan StackPanelin DataContextiksi
        spCustomer.DataContext = dgBooks.SelectedItem;
      }      
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      // Tallennetaan kirjaan tehdyt muutokset kantaan
      try
      {
        ctx.SaveChanges();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
      // Luodaan uusi kirja-olio ensin kontextiin ja sitten tietokantaan
      Book newBook;
      if(btnNew.Content.ToString() == "Uusi")
      {
        newBook = new Book();
        newBook.name = "Anna kirjan nimi";
        spBook.DataContext = newBook;
        btnNew.Content = "Tallenna kantaan";
      }
      else
      {
        try
        {
          newBook = (Book)spBook.DataContext;
          ctx.Books.Add(newBook);
          ctx.SaveChanges();
          btnNew.Content = "Uusi";
          MessageBox.Show("Kirja " + newBook.DisplayName + " lisätty kantaan");
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
      }      
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
      // Poistetaan kirja-olio ensin kontextista ja sitten tietokannasta
      try
      {
        Book current = (Book)spBook.DataContext;
        var result = MessageBox.Show("Haluatko varmasti poistaa kirjan " + current.DisplayName, "Poiston varmistus", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
          ctx.Books.Remove(current);
          ctx.SaveChanges();
          MessageBox.Show("Kirja " + current.DisplayName + " on poistettu");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void cbCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // Suodatetaan kirjat maan valinnan mukaan
      // Suodatus tehdään predikaatti-funktiolla
      view.Filter = MyCountryFilter;
    }

    private bool MyCountryFilter(object item)
    {
      if (cbCountries.SelectedIndex == -1)
      {
        return true;
      }
      else
      {
        return (item as Book).country.Contains(cbCountries.SelectedItem.ToString());
      }
    }


    private void btnGetCustomerOrders_Click(object sender, RoutedEventArgs e)
    {
      // Haetaan valitun asiakkaan tilaukset navigation properties avulla
      string msg = "";
      Customer current = (Customer)spCustomer.DataContext;

      msg += string.Format("Asiakkaalla {0} on {1} tilausta:\n", current.DisplayName, current.OrderCount);
      
      foreach (var order in current.Orders)
      {
        msg += string.Format("Tilaus {0} sisältää {1} tilausriviä:\n", order.odate, order.Orderitems.Count);
        // Kunkin tilauksen rivit ja sitä vastaava kirja
        decimal sum = 0;
        foreach (var item in order.Orderitems)
        {
          msg += string.Format(" - kirja {0} kappaletta {1}\n", item.Book.name, item.count);
          sum += item.count * item.Book.price.Value;          
        }
        msg += string.Format(" -- tilaus yhteensä {0}\n", sum);
      }

      MessageBox.Show(msg);
    }
  }
}
