using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
namespace H6DataBinding
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    HockeyLeague smliiga;
    List<HockeyTeam> liigajoukkueet;
    int pos;

    public MainWindow()
    {
      InitializeComponent();
      AlustaKontrollit();
    }

    private void AlustaKontrollit()
    {
      // Täytetään Comboboxi listan alkoilla
      List<string> joukkueet = new List<string>();
      joukkueet.Add("Ilves");
      joukkueet.Add("JYP");
      joukkueet.Add("Kärpät");
      cbxTeams.ItemsSource = joukkueet;

      // Perustaa SM-liiga
      smliiga = new HockeyLeague();
      liigajoukkueet = smliiga.GetTeams();
    }

    private void btnGetSetting_Click(object sender, RoutedEventArgs e)
    {
      // Tämä koodi lukee app.configin asetuksen UserName
      btnGetSetting.Content = Properties.Settings.Default.UserName;
    }

    private void btnBind_Click(object sender, RoutedEventArgs e)
    {
      // Sidotaan kokoelma stackpanelin datacontextiksi
      spLiiga.DataContext = liigajoukkueet;
    }

    private void btnForward_Click(object sender, RoutedEventArgs e)
    {
      // Siirretään osoitinta kokoelmassa yhdellä eteenpäin
      if (pos < liigajoukkueet.Count - 1)
      {
        pos++;
        spLiiga.DataContext = liigajoukkueet[pos]; 
      }
    }

    private void btnBackward_Click(object sender, RoutedEventArgs e)
    {
      // Siirretään osoitinta kokoelmassa yhdellä taaksepäin
      if (pos > 0)
      {
        pos--;
        spLiiga.DataContext = liigajoukkueet[pos]; 
      }
    }

    private void Create_Click(object sender, RoutedEventArgs e)
    {
      // Lisätään kokoelmaan uusi joukkue
      HockeyTeam newTeam = new HockeyTeam();
      liigajoukkueet.Add(newTeam);
      pos = liigajoukkueet.Count - 1;
      spLiiga.DataContext = liigajoukkueet[pos];
    }
  }
}
