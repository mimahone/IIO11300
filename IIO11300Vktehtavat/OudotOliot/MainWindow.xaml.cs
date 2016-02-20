using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

    private Pelaajat players = new Pelaajat();

    private void IniMyStuff()
    {
      // Käynnistyksen yhteydessä Seura -combobox täytetään nykyisillä 15 SM-Liigan seuralla (+ Jokerit koska esiintyy tietokannassa)
      string[] teams = { "Blues", "HIFK", "HPK", "Ilves", "Jokerit", "JYP", "KalPa", "KooKoo", "Kärpät", "Lukko", "Pelicans", "SaiPa", "Sport", "Tappara", "TPS", "Ässät" };

      cboTeam.Items.Clear();

      foreach (string team in teams)
      {
        cboTeam.Items.Add(team);
      }

      refreshPlayerList();

      cboTeam.SelectedIndex = 0;
      txtFirstName.Focus();
    }

    private void btnCreatePlayer_Click(object sender, RoutedEventArgs e)
    {
      /*
      Luo uusi pelaaja: tarkistetaan että kaikissa kentissä on arvo ja tarkistetaan ettei saman nimistä pelaaja ole jo Pelaajat -oliokokoelmassa,
      jos tarkistukset ok niin luodaan olio luokasta Pelaaja ja kirjoitetaan kuvan mukaisesti Pelaaja-olion EsitysNimi oikean puoleiseen listboxiin.
      Huom: Liigassa voi toki olla kaksi samannimistä pelaajaa, mutta tässä tehtävän yksinkertaistamiseksi sovitaan että kahta samannimistä ei voi olla.
      */
      try
      {
        if (string.IsNullOrWhiteSpace(txtFirstName.Text))
        {
          throw new Exception("etunimi puuttuu!");
        }

        if (string.IsNullOrWhiteSpace(txtLastName.Text))
        {
          throw new Exception("sukunimi puuttuu!");
        }

        int price = 0;

        int.TryParse(txtPrice.Text, out price);

        if (price < 0)
        {
          throw new Exception("siirtosumma ei voi olla negatiivinen!");
        }

        Pelaaja player = new Pelaaja(txtFirstName.Text, txtLastName.Text, price, cboTeam.Text);

        if (players.CreatePlayer(player) != null)
        {
          refreshPlayerList();

          lstPlayers.SelectedIndex = lstPlayers.Items.IndexOf(player.EsitysNimi);
          lblInfo.Text = "Uusi pelaaja lisätty";
        }
        else
        {
          lblInfo.Text = "Uuden pelaajan lisäys epäonnistui!";
        }
      }
      catch (Exception ex)
      {
        lblInfo.Text = "Tallennusta ei voitu suorittaa koska " + ex.Message;
      }
    }

    private void refreshPlayerList(List<Pelaaja> playerList)
    {
      lstPlayers.Items.Clear();

      foreach (Pelaaja player in playerList)
      {
        lstPlayers.Items.Add(player.EsitysNimi);
      }
    }

    private void refreshPlayerList()
    {
      lstPlayers.Items.Clear();

      foreach (Pelaaja player in players.GetPlayers())
      {
        if (player.Status != Status.Deleted)
        {
          lstPlayers.Items.Add(player.EsitysNimi);
        }
      }
    }

    private void btnSavePlayer_Click(object sender, RoutedEventArgs e)
    {
      // Muutetaan olemassa olevan olion tietoja ja kirjoitetaan tiedot listboxiin
      Pelaaja player = null;

      if (lstPlayers.SelectedValue == null)
      {
        lblInfo.Text = "Tallennusta ei voitu suorittaa koska pelaajaa ei ole valittu listalta";
        return;
      }
      else
      {
        player = players.GetPlayerByDisplayName(lstPlayers.SelectedValue.ToString());
      }

      if (player != null)
      {
        try
        {
          if (string.IsNullOrWhiteSpace(txtFirstName.Text))
          {
            throw new Exception("etunimi puuttuu!");
          }

          if (string.IsNullOrWhiteSpace(txtLastName.Text))
          {
            throw new Exception("sukunimi puuttuu!");
          }

          int price = 0;

          int.TryParse(txtPrice.Text, out price);

          if (price < 0)
          {
            throw new Exception("siirtosumma ei voi olla negatiivinen!");
          }

          player.Etunimi = txtFirstName.Text;
          player.Sukunimi = txtLastName.Text;
          player.Siirtohinta = price;
          player.Seura = cboTeam.Text;

          if (players.UpdatePlayer(player) != null)
          {
            refreshPlayerList();

            lstPlayers.SelectedIndex = lstPlayers.Items.IndexOf(player.EsitysNimi);
            lblInfo.Text = "Pelaajan tiedot on päivitetty";
          }
          else
          {
            lblInfo.Text = "Pelaajan tietojen päivitys epäonnistui!";
          }
        }
        catch (Exception ex)
        {
          lblInfo.Text = "Tallennusta ei voitu suorittaa koska " + ex.Message;
        }
      }
    }

    private void btnDeletePlayer_Click(object sender, RoutedEventArgs e)
    {
      // Poistetaan oliokokoelmasta sekä listboxista valittu pelaaja
      Pelaaja player = players.GetPlayerByDisplayName(lstPlayers.SelectedValue.ToString());

      if (player != null)
      {
        if (players.DeletePlayer(player))
        {
          lstPlayers.Items.Remove(player.EsitysNimi);
          txtId.Text = "";
          txtFirstName.Text = "";
          txtLastName.Text = "";
          txtPrice.Text = "0";
          cboTeam.SelectedIndex = 0;
          lblInfo.Text = "Pelaajan tiedot on poistettu"; 
        }
        else
        {
          lblInfo.Text = "Pelaajan tietojen poisto epäonnistui!";
        }
      }
      else
      {
        lblInfo.Text = "Pelaajan tietoja ei voi poistaa koska poistettavaa pelaajaa ei löydy";
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
          File.WriteAllText(sfd.FileName, players.GetPlayerData());
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
          refreshPlayerList(players.GetPlayersFromTxtFile(ofd.FileName));
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
          Pelaajat.SerializePlayersToBinFile(sfd.FileName, players.GetPlayers());
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
          Pelaajat.DeserializePlayersFromBinFile(ofd.FileName, ref obj);
          refreshPlayerList((List<Pelaaja>)obj);
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
          Pelaajat.SerializePlayersToXmlFile(sfd.FileName, players.GetPlayers());
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
          refreshPlayerList(Pelaajat.DeserializePlayersFromXmlFile(ofd.FileName));
          lblInfo.Text = string.Format("{0}-tiedoston deserialisointi suoritettu", ofd.FileName);
        }
      }
      catch (Exception ex)
      {
        lblInfo.Text = "xml-tiedostosta deserialisointi ei onnistunut: " + ex.Message;
      }
    }

    private void btnSaveToDatabase_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        players.SaveChangesToDatabase();
        refreshPlayerList();
        lblInfo.Text = "Tietokantaan tallennus suoritettu";
      }
      catch (Exception ex)
      {
        lblInfo.Text = "Tietokantaan tallennus ei onnistunut: " + ex.Message;
      }
    }

    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private void lstPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // Jos pelaajan nimeä napsautetaan listboxissa, niin pelaajan kaikki tiedot kirjoitetaan vasemmalla oleviin tekstikenttiin tietojen muokkausta varten
      if (lstPlayers.SelectedValue == null) return;

      Pelaaja player = players.GetPlayerByDisplayName(lstPlayers.SelectedValue.ToString());

      if (player != null)
      {
        txtId.Text = player.Id.ToString();
        txtFirstName.Text = player.Etunimi;
        txtLastName.Text = player.Sukunimi;
        txtPrice.Text = player.Siirtohinta.ToString();
        cboTeam.SelectedValue = player.Seura;
        lblInfo.Text = "Valittu pelaaja " + player.EsitysNimi;
      }
    }

  }
}
