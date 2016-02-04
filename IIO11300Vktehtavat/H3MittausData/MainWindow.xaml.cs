using System;
using System.Collections.Generic;
using System.IO;
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
using JAMK.IT.IIO11300;
using Microsoft.Win32;

namespace H3MittausData
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    // Luodaan lista mittaus-olioita varten
    List<MittausData> mitatut;

    public MainWindow()
    {
      InitializeComponent();
      IniMyStuff();
    }
    private void IniMyStuff()
    {
      //omat ikkunaan liittyvät alustukset
      txtToday.Text = DateTime.Today.ToShortDateString();
      mitatut = new List<MittausData>();
    }

    private void viewSavedData(string path)
    {
      var lines = File.ReadLines(path);
      var mdList = new List<MittausData>();

      foreach (var line in lines)
      {
        var items = line.Split('=');
        mdList.Add(new MittausData(items[0], items[1]));
      }

      foreach (var md in mdList)
      {
        lbData.Items.Add(md);
      }
    }

    private string getSavingData()
    {
      var sb = new StringBuilder();

      foreach (var item in lbData.Items)
      {
        sb.AppendLine(item.ToString());
      }

      return sb.ToString();
    }

    private void btnSaveData_Click(object sender, RoutedEventArgs e)
    {
      //luodaan uusi mittausdata olio ja näytetään se käyttäjälle
      MittausData md = new MittausData(txtClock.Text, txtData.Text);
      mitatut.Add(md);
      ApplyChanges();
    }

    private void btnBrowse_Click(object sender, RoutedEventArgs e)
    {
      var ofd = new OpenFileDialog();

      try
      {
        ofd.InitialDirectory = @"C:\temp\";
        ofd.Filter = "Text files|*.txt|All files|*.*";
        if (ofd.ShowDialog() == true)
        {
          txtFileName.Text = ofd.FileName;
          viewSavedData(ofd.FileName);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Tiedoston haku ei onnistunut: " + ex.Message);
      }
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      // Kirjoitetaan mitatut-tiedostoon
      try
      {
        MittausData.SaveToFile(txtFileName.Text, mitatut);
        MessageBox.Show("Tiedot tallennettu onnistuneesti tiedostoon " + txtFileName.Text);
      }
      catch (Exception ex)
      {
        MessageBox.Show("Tiedostoon tallennus ei onnistunut: " + ex.Message);
      }
    }

    private void btnRead_Click(object sender, RoutedEventArgs e)
    {
      // Haetaan käyttäjän antamasta tiedostosta mitatut arvot
      try
      {
        mitatut = MittausData.ReadFromFile(txtFileName.Text);
        ApplyChanges();
        MessageBox.Show("Tiedot haettu onnistuneesti tiedostosta " + txtFileName.Text);
      }
      catch (Exception ex)
      {
        MessageBox.Show("Tiedostosta haku ei onnistunut: " + ex.Message);
      }
    }

    private void ApplyChanges()
    {
      lbData.ItemsSource = null;
      lbData.ItemsSource = mitatut;
    }

    private void btnSerialize_Click(object sender, RoutedEventArgs e)
    {
      // Kutsutaan serialisointia
      try
      {
        Serialisointi.SerialisoiXml(txtFileName.Text, mitatut);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void btnDeserialize_Click(object sender, RoutedEventArgs e)
    {
      // Kutsutaan deserialisointia
      try
      {
        mitatut = Serialisointi.DeSerialisoiXml(txtFileName.Text);
        ApplyChanges();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void btnSerializeBin_Click(object sender, RoutedEventArgs e)
    {
      // Kutsutaan serialisointia binääri-muotoon
      try
      {
        Serialisointi.Serialisoi(txtFileName.Text, mitatut);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void btnDeserializeBin_Click(object sender, RoutedEventArgs e)
    {
      // Kutsutaan deserialisointia binääri-muotoon, palauttaa viittauksen objektiin
      object obj = new object();

      try
      {
        Serialisointi.DeSerialisoi(txtFileName.Text, ref obj);
        // Castataan obj MittausData-oliolistaksi
        mitatut = (List<MittausData>)obj;
        ApplyChanges();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }
  }
}
