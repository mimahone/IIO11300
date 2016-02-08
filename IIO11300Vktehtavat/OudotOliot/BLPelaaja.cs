using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OudotOliot
{
  public class Pelaaja
  {
    public Pelaaja() { }

    public Pelaaja(string firstName, string lastName, int price, string team)
    {
      Etunimi = firstName;
      Sukunimi = lastName;
      Siirtohinta = price;
      Seura = team;
    }

    public string Etunimi { get; set; }

    public string Sukunimi { get; set; }

    public string KokoNimi { get { return string.Format("{0} {1}", Sukunimi, Etunimi); } }

    public string EsitysNimi { get { return string.Format("{0} {1}, {2}", Etunimi, Sukunimi, Seura); } }

    public string Seura { get; set; }

    public int Siirtohinta { get; set; }
  }
}
