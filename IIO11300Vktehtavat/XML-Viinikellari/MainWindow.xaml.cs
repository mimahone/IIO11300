using System;
using System.Collections.Generic;
using System.Windows;
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
      cboCountry.ItemsSource = getCountryList();
    }

    private SortedSet<string> getCountryList()
    {
      try
      {
        SortedSet<string> countryList = new SortedSet<string>();
        XmlDocument doc = new XmlDocument();
        doc.Load(@"..\..\Viinit1.xml");

        XmlNodeList countryNodes = doc.SelectNodes("viinikellari/wine/maa");

        foreach (XmlElement countryNode in countryNodes)
        {
          countryList.Add(countryNode.InnerText);
        }

        return countryList;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    private void btnGetWines_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        XmlDocument doc = xdpWines.Document;

        // Haetaan XPathilla valitun maan nodet
        var countryWineList = doc.SelectNodes(string.Format("/viinikellari/wine[maa='{0}']", cboCountry.SelectedValue));

        if (countryWineList != null)
        {
          grdWines.ItemsSource = countryWineList;
          lblInfo.Text = string.Format("Viinit joiden valmistusmaa on {0}", cboCountry.SelectedValue);
        }
      }
      catch (Exception ex)
      {
        lblInfo.Text = string.Format("Viinien haussa tapahtui virhe: {0}",  ex.Message);
      }
    }

  }
}
