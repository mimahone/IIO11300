/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 8.2.2016 Modified: 13.2.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/

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
