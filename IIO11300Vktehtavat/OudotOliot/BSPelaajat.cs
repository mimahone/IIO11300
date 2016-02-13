/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 13.2.2016 Modified: 13.2.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace OudotOliot
{
  public class Pelaajat
  {
    private List<Pelaaja> playerList = new List<Pelaaja>();

    public List<Pelaaja> GetPlayers()
    {
      return playerList;
    }

    public Pelaaja GetPlayerByFullName(string fullName)
    {
      return playerList.Find(p => p.KokoNimi == fullName);
    }

    public Pelaaja GetPlayerByDisplayName(string displayName)
    {
      return playerList.Find(p => p.EsitysNimi == displayName);
    }

    public Pelaaja CreatePlayer(Pelaaja player)
    {
      // Tarkistus ettei ole toista saman nimistä pelaajaa
      try
      {
        if (GetPlayerByFullName(player.KokoNimi) != null)
        {
          throw new Exception(string.Format("saman niminen pelaaja ({0}) on jo listalla!", player.KokoNimi));
        }

        playerList.Add(player);

        return playerList.Find(p => p.KokoNimi == player.KokoNimi);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public Pelaaja UpdatePlayer(Pelaaja player)
    {
      // Tarkistus ettei ole toista saman nimistä pelaajaa
      try
      {
        //if (GetPlayerByFullName(player.KokoNimi) != null)
        //{
        //  throw new Exception(string.Format("saman niminen pelaaja ({0}) on jo listalla!", player.KokoNimi));
        //}

        return playerList.Find(p => p.KokoNimi == player.KokoNimi);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public bool DeletePlayer(Pelaaja player)
    {
      try
      {
        player = GetPlayerByDisplayName(player.EsitysNimi);

        if (player != null)
        {
          playerList.Remove(player);
          return true;
        }

        return false;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public string GetPlayerData()
    {
      try
      {
        if (playerList == null || playerList.Count == 0)
        {
          return null;
        }

        StringBuilder sb = new StringBuilder();

        foreach (Pelaaja player in playerList)
        {
          sb.AppendLine(string.Format("{0},{1},{2},{3}", player.Etunimi, player.Sukunimi, player.Siirtohinta, player.Seura));
        }

        return sb.ToString();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
