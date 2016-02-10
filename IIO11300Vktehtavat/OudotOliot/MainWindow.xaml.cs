using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

    private List<Pelaaja> playerList = new List<Pelaaja>();

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

    private string getSavingData()
    {
      var sb = new StringBuilder();

      foreach (var player in playerList)
      {
        sb.AppendLine(string.Format("{0},{1},{2},{3}", player.Etunimi, player.Sukunimi, player.Siirtohinta, player.Seura));
      }

      return sb.ToString();
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

        var player = new Pelaaja(txtFirstName.Text, txtLastName.Text, price, cboTeam.Text);

        // Tarkistus ettei ole toista saman nimistä pelaajaa
        int i = playerList.Count(p => p.KokoNimi == player.KokoNimi);

        if (i > 0)
        {
          throw new Exception(string.Format("saman niminen pelaaja ({0}) on jo listalla!", player.KokoNimi));
        }

        playerList.Add(player);
        updatePlayerList();
        lstPlayers.SelectedIndex = lstPlayers.Items.IndexOf(player.EsitysNimi);
        lblInfo.Text = "Uusi pelaaja lisätty";
      }
      catch (Exception ex)
      {
        lblInfo.Text = "Tallennusta ei voitu suorittaa koska " + ex.Message;
      }
    }

    private void updatePlayerList()
    {
      lstPlayers.Items.Clear();

      foreach (var player in playerList)
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
        player = playerList.FirstOrDefault<Pelaaja>(p => p.EsitysNimi == lstPlayers.SelectedValue.ToString());
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

          // Tarkistus ettei ole toista saman nimistä pelaajaa
          int i = playerList.Count(p => p.KokoNimi == player.KokoNimi);

          if (i > 1)
          {
            throw new Exception(string.Format("saman niminen pelaaja ({0}) on jo listalla!", player.KokoNimi));
          }

          updatePlayerList();

          lstPlayers.SelectedIndex = lstPlayers.Items.IndexOf(player.EsitysNimi);
          lblInfo.Text = "Pelaajan tiedot on päivitetty";
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
      var player = playerList.FirstOrDefault<Pelaaja>(p => p.EsitysNimi == lstPlayers.SelectedValue.ToString());

      if (player != null)
      {
        playerList.Remove(player);

        lstPlayers.Items.Remove(player.EsitysNimi);

        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtPrice.Text = "0";
        cboTeam.SelectedIndex = 0;
        lblInfo.Text = "Pelaajan tiedot on poistettu";
      }
      else
      {
        lblInfo.Text = "Pelaajan tietoja ei voi poistaa koska poistettavaa pelaajaa ei löydy";
      }
    }

    private void btnWritePlayers_Click(object sender, RoutedEventArgs e)
    {
      // Kirjoitetaan listassa olevien kaikkien olioitten tiedot käyttäjän Save-dialogissa valitsemaan tiedostoon
      if (playerList == null || playerList.Count == 0)
      {
        lblInfo.Text = "Tallennusta ei voi tehdä koska listalla ei ole pelaajia";
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
          File.WriteAllText(sfd.FileName, getSavingData());
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

      var player = playerList.FirstOrDefault<Pelaaja>(p => p.EsitysNimi == lstPlayers.SelectedValue.ToString());

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
