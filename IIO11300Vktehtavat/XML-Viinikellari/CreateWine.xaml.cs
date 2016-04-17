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
using System.Windows.Shapes;
using System.Xml;

namespace XML_Viinikellari
{
  /// <summary>
  /// Interaction logic for CreateWine.xaml
  /// </summary>
  public partial class CreateWine : Window
  {
    public CreateWine()
    {
      InitializeComponent();
    }

    private void btnCreate_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        // Lisätään uusi noodi XMLDocumenttiin
        // Haetaan XmlDataProviderin XML-tiedoston nimi
        string filu = @"..\..\Viinit1.xml";

        // Viittaus XmlDocumenttiin ja sen juurielementtiin
        XmlDocument doc = xdpWines.Document;
        XmlNode root = doc.SelectSingleNode("/viinikellari");

        // Luodaan uusi node
        XmlNode newMovie = doc.CreateElement("wine");

        // Lisätään nodelle tarvittavat attribuutit
        XmlNode xa1 = doc.CreateElement("nimi");
        xa1.Value = txtName.Text;
        newMovie.AppendChild(xa1);

        XmlNode xa2 = doc.CreateElement("maa");
        xa2.Value = txtCountry.Text;
        newMovie.AppendChild(xa2);

        XmlNode xa3 = doc.CreateElement("arvio");
        xa3.Value = int.Parse(txtPoints.Text).ToString();
        newMovie.AppendChild(xa3);

        // Lisätään noodi juureen
        root.AppendChild(newMovie);

        // Tallennetaan XmlDocument uusine nodeineen XML-tiedostoon olemassa olevan XML-tiedoston päälle!
        xdpWines.Document.Save(filu);

        MessageBox.Show("Uusi viini lisätty onnistuneesti");
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }
  }
}
