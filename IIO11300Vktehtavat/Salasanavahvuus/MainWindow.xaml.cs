/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 1.3.2016 Modified: 2.3.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/

using System.Windows;
using System.Windows.Input;

namespace JAMK.IT.IIO11300
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    /// <summary>
    /// Field for PasswordStrength object for UI binding
    /// </summary>
    private PasswordStrength ps = new PasswordStrength();

    /// <summary>
    /// Constructor
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
      spPasswordStrength.DataContext = PasswordChecker;
      dpPasswordStrength.DataContext = PasswordChecker;
      pwdPassword.Focus();
    }

    /// <summary>
    /// Property to get PasswordStrength object for UI binding
    /// </summary>
    public PasswordStrength PasswordChecker { get { return ps; } }

    /// <summary>
    /// Event handler for modified password
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">KeyEventArgs</param>
    private void pwdPassword_KeyUp(object sender, KeyEventArgs e)
    {
      ps.Password = pwdPassword.Password;
    }
  }
}
