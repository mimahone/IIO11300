using Microsoft.Win32;
using OudotOliot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JAMK.IT.IIO11300
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    SMLiigaEntities ctx;
    ObservableCollection<Pelaaja> localPlayers;

    public MainWindow()
    {
      InitializeComponent();
      IniMyStuff();
    }

    private void IniMyStuff()
    {
      ctx = new SMLiigaEntities();
      ctx.Pelaajat.Load();
      localPlayers = ctx.Pelaajat.Local;

      // ComboBoxin täyttäminen pelaajien eri seuroilla
      cboTeam.ItemsSource = localPlayers.Select(p => p.seura).Distinct().OrderBy(b => b).ToList();

      // Käynnistyksen yhteydessä Seura -combobox täytetään nykyisillä 15 SM-Liigan seuralla (+ Jokerit koska esiintyy tietokannassa)
      //cboTeam.ItemsSource = new string[] { "Blues", "HIFK", "HPK", "Ilves", "Jokerit", "JYP", "KalPa", "KooKoo", "Kärpät", "Lukko", "Pelicans", "SaiPa", "Sport", "Tappara", "TPS", "Ässät" };
      //Players.RefreshPlayers();
      //lstPlayers.DataContext = Players.PlayerList;
      //txtFirstName.Focus();
    }

    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
      // Luodaan uusi Pelaaja-olio ensin kontextiin ja sitten tietokantaan
      Pelaaja newPlayer;
      if (btnNew.Content.ToString() == "Luo uusi pelaaja")
      {
        newPlayer = new Pelaaja();
        spPlayer.DataContext = newPlayer;
        btnNew.Content = "Tallenna kantaan";
        cboTeam.SelectedIndex = 0;
        txtFirstName.Focus();
        lblInfo.Text = "Uuden pelaajan lisäys";
      }
      else
      {
        try
        {
          newPlayer = (Pelaaja)spPlayer.DataContext;
          if (HasDetailsErrors(newPlayer)) return;
          newPlayer.seura = cboTeam.SelectedValue.ToString();
          ctx.Pelaajat.Add(newPlayer);
          ctx.SaveChanges();

          ctx.Pelaajat.Load();
          lstPlayers.DataContext = ctx.Pelaajat.ToList();
          lstPlayers.SelectedIndex = lstPlayers.Items.IndexOf(newPlayer);
          lstPlayers.ScrollIntoView(lstPlayers.SelectedItem);
          btnNew.Content = "Luo uusi pelaaja";
          lblInfo.Text = string.Format("Uusi pelaaja {0} lisätty kantaan", newPlayer.Kokonimi);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
      }
    }

    private bool HasDetailsErrors(Pelaaja player)
    {
      bool errors = false;
      string message = "";
      TextBox txt = null;

      if (string.IsNullOrWhiteSpace(player.etunimi))
      {
        txt = txtFirstName;
        message = "etunimi puuttuu";
        errors = true;
      }

      if (string.IsNullOrWhiteSpace(player.sukunimi))
      {
        txt = txtLastName;
        message = "sukunimi puuttuu";
        errors = true;
      }

      if (player.arvo < 0)
      {
        throw new Exception("siirtosumma ei voi olla negatiivinen!");
      }

      if (errors)
      {
        MessageBox.Show(string.Format("Ei voi tallentaa koska {0}!", message));
        txt.Focus();
      }

      return errors;
    }

    private void refreshPlayerList(ObservableCollection<Player> playerList)
    {
      lstPlayers.Items.Clear();

      foreach (Player player in playerList)
      {
        lstPlayers.Items.Add(player.DisplayName);
      }
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Pelaaja current = (Pelaaja)spPlayer.DataContext;

        MessageBoxResult result = MessageBox.Show(
          string.Format("Haluatko varmasti poistaa pelaajan {0} tiedot?", current.Kokonimi), "Poiston varmistus",
          MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

        if (result == MessageBoxResult.Yes)
        {
          ctx.Pelaajat.Remove(current);
          ctx.SaveChanges();
          ctx.Pelaajat.Load();
          lstPlayers.DataContext = ctx.Pelaajat.ToList();

          lblInfo.Text = string.Format("Pelaaja {0} on poistettu", current.Kokonimi);
        }
      }
      catch (Exception ex)
      {
        lblInfo.Text = ex.Message;
      }
    }

    private void btnGetFromDatabase_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        lstPlayers.DataContext = ctx.Pelaajat.ToList();
      }
      catch (Exception ex)
      {
        lblInfo.Text = "Tietojen haku kannasta ei onnistunut: " + ex.Message;
      }
    }

    private void btnSaveToDatabase_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Pelaaja player = (Pelaaja)spPlayer.DataContext;

        if (HasDetailsErrors(player)) return; // Tarkistetaan että kaikissa kentissä on arvo
        ctx.SaveChanges();

        ctx.Pelaajat.Load();
        lstPlayers.DataContext = ctx.Pelaajat.ToList();
        lstPlayers.SelectedIndex = lstPlayers.Items.IndexOf(player);
        lstPlayers.ScrollIntoView(lstPlayers.SelectedItem);

        lblInfo.Text = "Muuttuneet tiedot on tallennettu";
      }
      catch (Exception ex)
      {
        lblInfo.Text = "Tietokantaan tallennus ei onnistunut: " + ex.Message;
      }
    }

    private void lstPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // Jos pelaajan nimeä napsautetaan listboxissa, niin pelaajan kaikki tiedot kirjoitetaan vasemmalla oleviin tekstikenttiin tietojen muokkausta varten
      Pelaaja player = (Pelaaja)lstPlayers.SelectedItem;
      spPlayer.DataContext = player;
      cboTeam.SelectedValue = player.seura;
      lblInfo.Text = string.Format("Valittu pelaaja {0}", player.Kokonimi);
    }

    private void btnWritePlayersTxt_Click(object sender, RoutedEventArgs e)
    {
      // Kirjoitetaan listassa olevien kaikkien olioitten tiedot käyttäjän Save-dialogissa valitsemaan tiedostoon
      if (lstPlayers.Items.Count == 0)
      {
        lblInfo.Text = "Tallennusta ei voi suorittaa koska listalla ei ole pelaajia";
        return;
      }

      var sfd = new SaveFileDialog();

      try
      {
        sfd.InitialDirectory = @"C:\temp\";
        sfd.FileName = "Pelaajalista.txt";
        sfd.Filter = "Text files|*.txt|All files|*.*";

        if (sfd.ShowDialog() == true)
        {
          File.WriteAllText(sfd.FileName, Players.GetPlayerData());
          lblInfo.Text = string.Format("{0}-tiedostoon tallennus on suoritettu", sfd.FileName);
        }
      }
      catch (Exception ex)
      {
        lblInfo.Text = "Tekstitiedostoon tallennus ei onnistunut: " + ex.Message;
      }
    }

    private void btnReadPlayersTxt_Click(object sender, RoutedEventArgs e)
    {
      var ofd = new OpenFileDialog();

      try
      {
        ofd.InitialDirectory = @"C:\temp\";
        ofd.Filter = "Text-files (*.txt)|*.txt|All files|*.*";

        if (ofd.ShowDialog() == true && File.Exists(ofd.FileName))
        {
          refreshPlayerList(Players.GetPlayersFromTxtFile(ofd.FileName));
          lblInfo.Text = string.Format("{0}-tiedoston luenta suoritettu", ofd.FileName);
        }
      }
      catch (Exception ex)
      {
        lblInfo.Text = "Tekstitiedostosta luenta ei onnistunut: " + ex.Message;
      }
    }

    private void btnWritePlayersBin_Click(object sender, RoutedEventArgs e)
    {
      // Kirjoitetaan listassa olevien kaikkien olioitten tiedot käyttäjän Save-dialogissa valitsemaan bin-tiedostoon
      if (lstPlayers.Items.Count == 0)
      {
        lblInfo.Text = "Tallennusta ei voi suorittaa koska listalla ei ole pelaajia";
        return;
      }

      var sfd = new SaveFileDialog();

      try
      {
        sfd.InitialDirectory = @"C:\temp\";
        sfd.FileName = "Pelaajalista.bin";
        sfd.Filter = "bin files|*.bin|All files|*.*";

        if (sfd.ShowDialog() == true)
        {
          DBPlayers.SerializePlayersToBinFile(sfd.FileName, Players.PlayerList);
          lblInfo.Text = string.Format("{0}-tiedostoon serialisointi on suoritettu", sfd.FileName);
        }
      }
      catch (Exception ex)
      {
        lblInfo.Text = "bin-tiedostoon serialisointi ei onnistunut: " + ex.Message;
      }
    }

    private void btnReadPlayersBin_Click(object sender, RoutedEventArgs e)
    {
      var ofd = new OpenFileDialog();

      try
      {
        ofd.InitialDirectory = @"C:\temp\";
        ofd.Filter = "bin-files (*.bin)|*.bin|All files|*.*";

        if (ofd.ShowDialog() == true && File.Exists(ofd.FileName))
        {
          object obj = new object();
          DBPlayers.DeserializePlayersFromBinFile(ofd.FileName, ref obj);
          refreshPlayerList((ObservableCollection<Player>)obj);
          lblInfo.Text = string.Format("{0}-tiedoston deserialisointi suoritettu", ofd.FileName);
        }
      }
      catch (Exception ex)
      {
        lblInfo.Text = "bin-tiedostosta deserialisointi ei onnistunut: " + ex.Message;
      }
    }

    private void btnWritePlayersXml_Click(object sender, RoutedEventArgs e)
    {
      // Kirjoitetaan listassa olevien kaikkien olioitten tiedot käyttäjän Save-dialogissa valitsemaan xml-tiedostoon
      if (lstPlayers.Items.Count == 0)
      {
        lblInfo.Text = "Tallennusta ei voi suorittaa koska listalla ei ole pelaajia";
        return;
      }

      var sfd = new SaveFileDialog();

      try
      {
        sfd.InitialDirectory = @"C:\temp\";
        sfd.FileName = "Pelaajalista.xml";
        sfd.Filter = "xml files|*.xml|All files|*.*";

        if (sfd.ShowDialog() == true)
        {
          DBPlayers.SerializePlayersToXmlFile(sfd.FileName, Players.PlayerList);
          lblInfo.Text = string.Format("{0}-tiedostoon serialisointi on suoritettu", sfd.FileName);
        }
      }
      catch (Exception ex)
      {
        lblInfo.Text = "xml-tiedostoon serialisointi ei onnistunut: " + ex.Message;
      }
    }

    private void btnReadPlayersXml_Click(object sender, RoutedEventArgs e)
    {
      var ofd = new OpenFileDialog();

      try
      {
        ofd.InitialDirectory = @"C:\temp\";
        ofd.Filter = "xml-files (*.xml)|*.xml|All files|*.*";

        if (ofd.ShowDialog() == true && File.Exists(ofd.FileName))
        {
          refreshPlayerList(DBPlayers.DeserializePlayersFromXmlFile(ofd.FileName));
          lblInfo.Text = string.Format("{0}-tiedoston deserialisointi suoritettu", ofd.FileName);
        }
      }
      catch (Exception ex)
      {
        lblInfo.Text = "xml-tiedostosta deserialisointi ei onnistunut: " + ex.Message;
      }
    }

    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
      //if (Players.IsDirty)
      //{
      //  MessageBoxResult result = MessageBox.Show(
      //    "Muutoksia ei ole tallennettu. Tallennetaanko nyt?", "Tallentamattomia muutoksia",
      //    MessageBoxButton.YesNo, MessageBoxImage.Warning);
      //  if (result == MessageBoxResult.Yes)
      //  {
      //    string msg = "";
      //    Players.SaveChangesToDatabase(out msg);
      //  }
      //}

      Application.Current.Shutdown();
    }

  }
}
