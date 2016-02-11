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
using System.Xml;
using System.Xml.Linq;

namespace H4TyontekijatWpf
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    XElement xe;

    public MainWindow()
    {
      InitializeComponent();
    }

    private void btnReadXML_Click(object sender, RoutedEventArgs e)
    {
      // Luetaan XML-tiedostosta tyontekija-elementit ja sidotaan ne DataGridiin
      try
      {
        string filu = @"D:\Työntekijät2016.xml";
        xe = XElement.Load(filu);
        dgData.DataContext = xe.Elements("tyontekija");
        tbMessage.Text = string.Format("Työntekijöitä {0} ja palkat yhteensä {1}", countWorkers("vakituinen"), calculateSalarySum("vakituinen"));
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    private int countWorkers(string tyosuhde)
    {
      try
      {
        var temp = from ele in xe.Elements()
                   where ele.Element("tyosuhde").Value == tyosuhde
                   select ele.Elements("sukunimi");
        return temp.Count();        
        //return xe.Elements("tyontekija").Count();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    private decimal calculateSalarySum(string tyosuhde)
    {
      try
      {
        var temp = from ele in xe.Elements()
                   where ele.Element("tyosuhde").Value == tyosuhde
                   select ele.Element("palkka");
        return temp.Sum(n => decimal.Parse(n.Value));
        //return xe.Elements("tyontekija").Elements("palkka").Sum(n => decimal.Parse(n.Value));
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

  }
}
