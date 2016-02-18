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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace H5Movies
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void btnOpenMovies1_Click(object sender, RoutedEventArgs e)
    {
      Movies1 win = new Movies1();
      win.ShowDialog();
    }

    private void btnOpenMovies2_Click(object sender, RoutedEventArgs e)
    {
      Movies2 win = new Movies2();
      win.ShowDialog();
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

  }
}
