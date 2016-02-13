using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace OudotOliot
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
      // Käynnistyksen yhteydessä Seura -combobox täytetään nykyisillä 15 SM-Liigan seuralla
      string[] teams = { "Blues", "HIFK", "HPK", "Ilves", "JYP", "Kalpa", "KooKoo", "Kärpät", "Lukko", "Pelicans", "Saipa", "Sport", "Tappara", "TPS", "Ässät" };

      cboTeam.Items.Clear();

      foreach (string team in teams)
      {
        cboTeam.Items.Add(team);
      }

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

    private void refreshPlayerList()
    {
      lstPlayers.Items.Clear();

      foreach (Pelaaja player in players.GetPlayers())
      {
        lstPlayers.Items.Add(player.EsitysNimi);
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

    private void btnWritePlayers_Click(object sender, RoutedEventArgs e)
    {
      // Kirjoitetaan listassa olevien kaikkien olioitten tiedot käyttäjän Save-dialogissa valitsemaan tiedostoon
      string playersData = players.GetPlayerData();

      if (string.IsNullOrEmpty(playersData))
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
          File.WriteAllText(sfd.FileName, playersData);
          lblInfo.Text = "Tiedostoon tallennus on suoritettu";
        }
      }
      catch (Exception ex)
      {
        lblInfo.Text = "Tiedostoon tallennus ei onnistunut: " + ex.Message;
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
        txtFirstName.Text = player.Etunimi;
        txtLastName.Text = player.Sukunimi;
        txtPrice.Text = player.Siirtohinta.ToString();
        cboTeam.SelectedValue = player.Seura;
        lblInfo.Text = "Valittu pelaaja " + player.EsitysNimi;
      }
    }
 
  }
}
