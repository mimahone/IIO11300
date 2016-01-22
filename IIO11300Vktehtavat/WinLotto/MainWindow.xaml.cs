/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 20.1.2016 Modified: 22.1.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/
using System;
using System.Windows;

namespace WinLotto
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

    private void btnDraw_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (int.Parse(txtDrawns.Text) < 1)
        {
          txtDrawns.Text = "1";
        }

        Lotto ltt = new Lotto();
        ltt.Game = (Lotto.GameVersion)cboGame.SelectedIndex;
        ltt.Drawns = int.Parse(txtDrawns.Text);
        txtNumbers.Text = ltt.Numbers;
      }
      catch (Exception ex)
      {
        MessageBox.Show("Tapahtui virhe: " + ex.Message);
      }
    }

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
      cboGame.SelectedIndex = 0;
      txtDrawns.Text = "1";
      txtNumbers.Text = "";
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }
  }
}
