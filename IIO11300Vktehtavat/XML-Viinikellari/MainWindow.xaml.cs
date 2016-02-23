using System;
using System.Collections.Generic;
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
using System.Xml;

namespace XML_Viinikellari
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

    private void btnGetWines_Click(object sender, RoutedEventArgs e)
    {
      // Poistetaan käyttäjän valitsema elokuva --> poistettava Node haetaan Name-attribuutin avulla
      try
      {
        string filu = xdpWines.Source.LocalPath;
        XmlDocument doc = xdpWines.Document;
        XmlNode root = doc.SelectSingleNode("/viinikellari");

        // Haetaan XPathilla poistettava node
        var country = doc.SelectSingleNode(string.Format("/viinikellari/wine[@maa='{0}']", cboCountry.Text));

        if (country != null)
        {
          
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

  }
}
