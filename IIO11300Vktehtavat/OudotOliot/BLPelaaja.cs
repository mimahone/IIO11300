/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 8.2.2016 Modified: 13.2.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/

using System;

namespace OudotOliot
{
  public enum Status
  {
    Unchanged = 0,
    Created = 1,
    Modified = 2,
    Deleted = 3
  }

  [Serializable()]
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

    public int Id { get; set; }

    public string Etunimi { get; set; }

    public string Sukunimi { get; set; }

    public string KokoNimi { get { return string.Format("{0} {1}", Sukunimi, Etunimi); } }

    public string EsitysNimi { get { return string.Format("{0} {1}, {2}", Etunimi, Sukunimi, Seura); } }

    public string Seura { get; set; }

    public int Siirtohinta { get; set; }

    public Status Status { get; set; }

  }
}
