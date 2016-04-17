using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAMK.IT.IIO11300
{
  public class Ikkuna
  {
    #region Properties

    public double Korkeus { get; set; }

    public double Leveys { get; set; }

    public int Muoto { get; set; }

    public string Materiaali { get; set; }

    public float Hinta
    {
      get { return LaskeHinta(); }
    }

    #endregion Properties

    #region Methods

    public double LaskePiiri()
    {
      return Korkeus * 2 + Leveys * 2;
    }

    public double LaskePintaAla()
    {
      return Korkeus * Leveys;
    }

    private float LaskeHinta()
    {
      double tyonHinta = 200;
      double kate = 100;

      //1) karmiin tulevat puu- tai alumiiniosat olkoot 50,00€ per juoksumetri, eli piirin pituus (metreinä) * 50€.
      //2) ikkuna-lasin hinta olkoot 100€ per neliömetri, eli pinta-ala(neliömetreinä) * 100€.
      double metriHinta = 50; // puu- tai alumiiniosat 50,00€/jm
      double lasinHinta = 100; // ikkuna-lasin hinta 100€/m2
      double materiaalinHinta = ((LaskePiiri() / 100) * metriHinta) + (LaskePintaAla() * lasinHinta);

      double hinta = tyonHinta + materiaalinHinta + kate;

      return float.Parse(hinta.ToString());
    }

    #endregion Methods
  }
}
