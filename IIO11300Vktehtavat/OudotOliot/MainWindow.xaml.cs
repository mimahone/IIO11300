using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public MainWindow()
    {
      InitializeComponent();
      IniMyStuff();
    }

    private void IniMyStuff()
    {
      // Käynnistyksen yhteydessä Seura -combobox täytetään nykyisillä 15 SM-Liigan seuralla (+ Jokerit koska esiintyy tietokannassa)
      cboTeam.ItemsSource = new string[] { "Blues", "HIFK", "HPK", "Ilves", "Jokerit", "JYP", "KalPa", "KooKoo", "Kärpät", "Lukko", "Pelicans", "SaiPa", "Sport", "Tappara", "TPS", "Ässät" };
      Players.RefreshPlayers();
      lstPlayers.DataContext = Players.PlayerList;
      txtFirstName.Focus();
    }

    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
      Player newPlayer = new Player(0);
      newPlayer.Status = Status.Created;
      spPlayer.DataContext = newPlayer;
      cboTeam.SelectedIndex = 0;
      txtFirstName.Focus();
      lblInfo.Text = "Uuden pelaajan lisäys";
    }

    private bool HasDetailsErrors(Player player)
    {
      bool errors = false;
      string message = "";
      TextBox txt = null;

      if (string.IsNullOrWhiteSpace(player.FirstName))
      {
        txt = txtFirstName;
        message = "etunimi puuttuu";
        errors = true;
      }

      if (string.IsNullOrWhiteSpace(player.LastName))
      {
        txt = txtLastName;
        message = "sukunimi puuttuu";
        errors = true;
      }

      if (player.Price < 0)
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

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (spPlayer.DataContext == null)
      {
        lblInfo.Text = "Tallennusta ei voitu suorittaa koska pelaajaa ei ole valittu listalta";
        return;
      }

      Player player = (Player)spPlayer.DataContext;

      if (HasDetailsErrors(player)) return; // Tarkistetaan että kaikissa kentissä on arvo

      if (player != null)
      {
        try
        {
          // Tarkistetaan ettei saman nimistä pelaaja ole jo Pelaajat -oliokokoelmassa
          Player found = ((ObservableCollection<Player>)lstPlayers.DataContext).FirstOrDefault(p => p.FullName == player.FullName && p.Id != player.Id);

          if (found != null)
          {
            MessageBox.Show("Saman niminen pelaaja on jo listalla!");
            throw new Exception("saman niminen pelaaja on jo listalla!");
          }

          if (player.Id > 0)
          {
            player.Status = Status.Modified;
            lblInfo.Text = string.Format("Pelaajan {0} tiedot päivitetty tallennusta varten", player.FullName);
          }
          else
          {
            ((ObservableCollection<Player>)lstPlayers.DataContext).Add(player);
            lstPlayers.SelectedIndex = lstPlayers.Items.IndexOf(player);
            lstPlayers.ScrollIntoView(lstPlayers.SelectedItem);
            lblInfo.Text = string.Format("Uusi pelaaja {0} lisätty tallennusta varten", player.FullName);
          }
        }
        catch (Exception ex)
        {
          lblInfo.Text = "Tallennusta ei voitu suorittaa koska " + ex.Message;
        }
      }
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Player player = (Player)lstPlayers.SelectedItem;

        MessageBoxResult result = MessageBox.Show(
          string.Format("Haluatko varmasti poistaa pelaajan {0} tiedot?", player.DisplayName), "Poiston varmistus",
          MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

        if (result == MessageBoxResult.Yes)
        {
          string message = "";

          if (player.Id > 0)
          { 
            if (Players.DeletePlayer(player, out message) > 0)
            {
              player.Status = Status.Deleted;
              ((ObservableCollection<Player>)lstPlayers.DataContext).Remove(player);
            }
          }
          else // When not yet saved will be deleted ie. Id = 0
          {
            ((ObservableCollection<Player>)lstPlayers.DataContext).Remove(player);
          }

          spPlayer.DataContext = null;
          lblInfo.Text = message;
        }
      }
      catch (Exception ex)
      {
        lblInfo.Text = ex.Message;
      }
    }

    private void btnSaveToDatabase_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string message = "";
        Players.SaveChangesToDatabase(out message);
        Players.RefreshPlayers();
        lstPlayers.DataContext = Players.PlayerList;
        lblInfo.Text = message;
      }
      catch (Exception ex)
      {
        lblInfo.Text = "Tietokantaan tallennus ei onnistunut: " + ex.Message;
      }
    }

    private void lstPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // Jos pelaajan nimeä napsautetaan listboxissa, niin pelaajan kaikki tiedot kirjoitetaan vasemmalla oleviin tekstikenttiin tietojen muokkausta varten
      if (lstPlayers.SelectedItem != null)
      {
        Player player = (Player)lstPlayers.SelectedItem;
        spPlayer.DataContext = player;
        lblInfo.Text = string.Format("Valittu pelaaja {0}", player.DisplayName);
      }
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
      if (Players.IsDirty)
      {
        MessageBoxResult result = MessageBox.Show(
          "Muutoksia ei ole tallennettu. Tallennetaanko nyt?", "Tallentamattomia muutoksia",
          MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.Yes)
        {
          string msg = "";
          Players.SaveChangesToDatabase(out msg);
        }
      }

      Application.Current.Shutdown();
    }
  }
}
