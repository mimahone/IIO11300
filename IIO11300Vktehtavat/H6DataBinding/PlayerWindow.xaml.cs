using System.Collections.ObjectModel;
using System.Windows;

namespace H6DataBinding
{
  /// <summary>
  /// Interaction logic for PlayerWindow.xaml
  /// </summary>
  public partial class PlayerWindow : Window
  {
    ObservableCollection<HockeyPlayer> players;
    int pos;

    public PlayerWindow()
    {
      InitializeComponent();
      IniMyStaff();
      dgPlayers.ItemsSource = players;
    }

    private void IniMyStaff()
    {
      players = Get3TestPlayers();
      pos = 0;
      SetData();
    }

    private void SetData()
    {
      myGrid.DataContext = players[pos];
    }

    private ObservableCollection<HockeyPlayer> Get3TestPlayers()
    {
      ObservableCollection<HockeyPlayer> temp = new ObservableCollection<HockeyPlayer>();
      temp.Add(new HockeyPlayer("Teemu Selänne", "8"));
      temp.Add(new HockeyPlayer("Jarkko Immonen", "28"));
      temp.Add(new HockeyPlayer("Ville Peltonen", "16"));
      return temp;
    }

    private void dgPlayers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      if ((dgPlayers.SelectedIndex >= 0) 
        & (dgPlayers.SelectedIndex <= players.Count))
      {
        pos = dgPlayers.SelectedIndex;
        SetData();
      }
    }
  }
}
