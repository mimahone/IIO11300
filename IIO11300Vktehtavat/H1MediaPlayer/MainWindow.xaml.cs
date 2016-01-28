using System;
using System.Windows;
using Microsoft.Win32;

namespace H1MediaPlayer
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    bool IsPlaying = false;

    public MainWindow()
    {
      InitializeComponent();
      IniMyStaff();
    }

    private void IniMyStaff()
    {
      // Kootaan tänne kaikki alustukset mitä tarvitaan ohjelman suorittamiseksi
      txtFileName.Text = @"D:\k6058\ILMOV2\REPOS\IIO11300\IIO11300Vktehtavat\H1MediaPlayer\CoffeeMaker.mp4";
    }

    private void btnPlay_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (txtFileName.Text.Length > 0 && System.IO.File.Exists(txtFileName.Text))
        {
          mediaElement.Source = new Uri(txtFileName.Text);
          mediaElement.Play();
          IsPlaying = true;
          // Nappulat käyttöön
          SetMyButtons();
        }
        else
        {
          MessageBox.Show("Tiedostoa " + txtFileName.Text + " ei löydy.");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void btnPause_Click(object sender, RoutedEventArgs e)
    {
      if (IsPlaying)
      {
        mediaElement.Pause();
        IsPlaying = false;
        // Nappulat käyttöön
        btnPause.Content = "Continue";
      }
      else
      {
        mediaElement.Play();
        IsPlaying = true;
        btnPause.Content = "Pause";
      }
    }

    private void btnStop_Click(object sender, RoutedEventArgs e)
    {
      mediaElement.Stop();
      IsPlaying = false;
      // Nappulat käyttöön
      SetMyButtons();
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private void btnBrowse_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.InitialDirectory = @"D:\k6058\ILMOV2\REPOS\IIO11300\IIO11300Vktehtavat\H1MediaPlayer\";
      ofd.Filter = "Video-tiedostot mp4|*.mp4|All files|*.*";
      Nullable<bool> result = ofd.ShowDialog();
      if (result == true)
      {
        txtFileName.Text = ofd.FileName;
      }
    }

    private void SetMyButtons()
    {
      btnPause.IsEnabled = IsPlaying;
      btnStop.IsEnabled = IsPlaying;
      btnPlay.IsEnabled = !IsPlaying;
    }
  }
}
