/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 12.1.2016 Modified: 13.1.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/
using System;
using System.Windows;

namespace Tehtava1
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

    private void btnCalculate_Click(object sender, RoutedEventArgs e)
    {
      //TODO
      try
      {
        double l = Math.Abs(double.Parse(txtWidth.Text));
        double h = Math.Abs(double.Parse(txtHeight.Text));
        double w = Math.Abs(double.Parse(txtFrameWidth.Text));
        txtResult1.Text = BusinessLogicWindow.CalculateWindowArea(l, h, w).ToString();
        txtResult2.Text = BusinessLogicWindow.CalculatePerimeter(l, h).ToString();
        txtResult3.Text = BusinessLogicWindow.CalculateFrameArea(l, h, w).ToString();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
      finally
      {
        //yield to an user that everything okay
      }
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }
  }

}
