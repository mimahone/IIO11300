/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 20.1.2016 Modified: 22.1.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
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

    private List<List<int>> numbersList;

    private string getSavingData()
    {
      var sb = new StringBuilder();

      foreach (var numbers in numbersList)
      {
        sb.AppendLine(string.Join(",", numbers));
      }

      return sb.ToString();
    }

    private void btnDraw_Click(object sender, RoutedEventArgs e)
    {
      lbxNumbers.Items.Clear();

      try
      {
        if (int.Parse(txtDrawns.Text) < 1)
        {
          txtDrawns.Text = "1";
        }

        Lotto ltt = new Lotto();
        ltt.Game = (Lotto.GameVersion)cboGame.SelectedIndex;
        ltt.Drawns = int.Parse(txtDrawns.Text);
        numbersList = ltt.NumbersList;

        foreach (var numbers in numbersList)
        {
          if (ltt.Game == Lotto.GameVersion.Eurojackpot) // Eurojackpotissa 5/50 ja 2 tähtinumeroa luvuista 1-8
          {
            var sb = new StringBuilder();

            for (int i = 0; i < 7; i++)
            {
              switch (i)
              {
                case 5:
                  sb.AppendFormat(" + tähtinumerot {0}", numbers[i]);
                  break;
                case 6:
                  sb.AppendFormat(" ja {0}", numbers[i]);
                  break;
                default:
                  if (i > 0) sb.Append(",");
                  sb.Append(numbers[i]);
                  break;
              }
            }
            
            lbxNumbers.Items.Add(sb.ToString());
          }
          else
          {
            lbxNumbers.Items.Add(string.Join(",", numbers));
          }
        }
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
      lbxNumbers.Items.Clear();
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private void btnSaveNumbers_Click(object sender, RoutedEventArgs e)
    {
      if (numbersList == null || numbersList.Count == 0)
      {
        MessageBox.Show("Tallennusta ei voi tehdä koska numeroita ei ole arvottu.");
        return;
      }

      var sfd = new SaveFileDialog();

      try
      {
        sfd.InitialDirectory = @"C:\temp\";

        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
        Calendar cal = dfi.Calendar;
        sfd.FileName = string.Format("Lottorivit_{0}_{1}{2:00}.txt", cboGame.Text, DateTime.Today.Year, cal.GetWeekOfYear(DateTime.Today, dfi.CalendarWeekRule, dfi.FirstDayOfWeek));
        sfd.Filter = "Text files|*.txt|All files|*.*";

        if (sfd.ShowDialog() == true)
        {
          File.WriteAllText(sfd.FileName, getSavingData());
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Tiedostoon tallennus ei onnistunut: " + ex.Message);
      }
    }

    private void cboGame_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
      if (lbxNumbers != null)
      {
        lbxNumbers.Items.Clear();
      }

      if (txtRaffledNumbers != null)
      {
        txtRaffledNumbers.Text = ""; 
      }
    }

    private void btnCheckNumbers_Click(object sender, RoutedEventArgs e)
    {
      if (txtRaffledNumbers == null || string.IsNullOrWhiteSpace(txtRaffledNumbers.Text))
      {
        MessageBox.Show("Tarkistusta ei voi tehdä koska arvottuja numeroita ei ole syötetty.");
        return;
      }

      int numbers, maxNumber;

      switch ((Lotto.GameVersion)cboGame.SelectedIndex)
      {
        case Lotto.GameVersion.Viking:
          numbers = 6;
          maxNumber = 48;
          break;

        case Lotto.GameVersion.Eurojackpot: // Eurojackpotissa 5/50 ja 2 tähtinumeroa luvuista 1-8
          numbers = 5;
          maxNumber = 50;
          break;

        default: // Suomi 7/39
          numbers = 7;
          maxNumber = 39;
          break;
      }

      try
      {
        HashSet<int> raffledNumbers = new HashSet<int>(Array.ConvertAll(txtRaffledNumbers.Text.Split(','), int.Parse));

        if (raffledNumbers.Count != numbers)
        {
          MessageBox.Show("Arvotut numerot eivät sisällä oikeaa määrää numeroita. Pitäisi olla " + numbers + " numeroa.");
          return;
        }

        foreach (var number in raffledNumbers)
        {
          if (number < 1 || number > maxNumber)
          {
            MessageBox.Show("Arvottu numero " + number + " on virheellinen. Pitäisi olla numero väliltä 1 - " + maxNumber + ".");
            return;
          }
        }

        // Tarkastus valitusta tiedostosta
        //todo...
      }
      catch (Exception ex)
      {
        MessageBox.Show("Arvotut numerot sisältävät virheellisiä merkkejä. Vain mahdolliset numerot ja niiden erotinpilkku on sallittuja. " + ex.Message);
      }
    }
  }
}
