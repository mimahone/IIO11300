using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace H4TyontekijatConsole
{
  class Program
  {
    static void CalculateSalarySumFromXML(string filu)
    {
      try
      {
        // Tutkitaan onko tiedosto olemassa
        if (System.IO.File.Exists(filu))
        {
          // Luetaan XML-tiedosto XmlDocument-olioon
          XmlDocument xmlDoc = new XmlDocument();
          xmlDoc.Load(filu);

          // Haetaan kaikkien vakituisten työntekijöiden palkka-elementit XPath:lla
          XmlNodeList xnl = xmlDoc.SelectNodes("/tyontekijat/tyontekija[tyosuhde='vakituinen']/palkka");

          // Loopitetaan nodelista läpi
          int sum = 0;

          for (int i = 0; i < xnl.Count; i++)
          {
            sum += Convert.ToInt32(xnl.Item(i).InnerText);
          }

          Console.WriteLine(string.Format("Vakituisia on {0} ja heidän palkat yhteensä {1}", xnl.Count, sum));
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    static void ReadWorkersFromXML(string filu)
    {
      try
      {
        // Tutkitaan onko tiedosto olemassa
        if (System.IO.File.Exists(filu))
        {
          // Luetaan XML-tiedosto XmlDocument-olioon
          XmlDocument xmlDoc = new XmlDocument();
          xmlDoc.Load(filu);

          // Haetaan kaikki työntekija-elementit XPath:lla
          XmlNodeList xnl = xmlDoc.SelectNodes("/tyontekijat/tyontekija");
          XmlNode xn; // Edustaa yksittäistä noodia
          XmlNodeList xnl2;

          // Loopitetaan nodelista läpi
          for (int i = 0; i < xnl.Count; i++)
          {
            // Näytetään käyttäjälle noodien sisältö
            //Console.WriteLine(xnl.Item(i).InnerText);
            xn = xnl.Item(i);
            xnl2 = xn.ChildNodes;

            for (int j = 0; j < xnl2.Count; j++)
            {
              Console.WriteLine(xnl2.Item(j).InnerText);
            }

          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    static void Main(string[] args)
    {
      try
      {
        //ReadWorkersFromXML(@"D:\Työntekijät2016.xml");
        CalculateSalarySumFromXML(@"D:\Työntekijät2016.xml");
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
  }
}

// Console-sovelluksen testaus Ctrl+F5