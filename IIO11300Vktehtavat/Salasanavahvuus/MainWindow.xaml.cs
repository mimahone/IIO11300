using System.Windows;
using System.Windows.Input;

namespace Salasanavahvuus
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    PasswordStrength ps = new PasswordStrength();

    public MainWindow()
    {
      InitializeComponent();
      spPasswordStrength.DataContext = PasswordChecker;
      dpPasswordStrength.DataContext = PasswordChecker;
      pwdPassword.Focus();
    }

    public PasswordStrength PasswordChecker { get { return ps; } }

    private void pwdPassword_KeyUp(object sender, KeyEventArgs e)
    {
      ps.Password = pwdPassword.Password;
    }
  }
}
