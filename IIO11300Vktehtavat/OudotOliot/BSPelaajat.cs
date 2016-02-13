/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 13.2.2016 Modified: 13.2.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace OudotOliot
{
  public class Pelaajat
  {
    OleDbConnection connection; //= new OleDbConnection(connectionString)
    private List<Pelaaja> playerList = new List<Pelaaja>();

    /// <summary>
    /// Constructor
    /// </summary>
    public Pelaajat()
    {
      string connectionString = ConfigurationManager.ConnectionStrings["SMLiiga"].ConnectionString;
      connection = new OleDbConnection(connectionString);
    }

    /// <summary>
    /// Get list of players
    /// </summary>
    /// <returns></returns>
    public List<Pelaaja> GetPlayers()
    {
      const string sql = @"
SELECT id, etunimi, sukunimi, arvo, seura
FROM Pelaajat
ORDER BY sukunimi, etunimi
";

      try
      {
        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        using (OleDbCommand command = new OleDbCommand(sql, connection))
        {
          using (OleDbDataReader reader = command.ExecuteReader())
          {
            Pelaaja player;

            while (reader.Read())
            {
              player = new Pelaaja(
                  reader.GetString(1),
                  reader.GetString(2),
                  int.Parse(reader.GetValue(3).ToString()),
                  reader.GetString(4)
              );
              player.Id = int.Parse(reader.GetValue(0).ToString());

              playerList.Add(player);
            }
          }
        }
        
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        connection.Close();
      }

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

//        const string sql = @"
//INSERT INTO Pelaajat (etunimi, sukunimi, arvo, seura)
//VALUES (@firstName, @lastName, @price, @team)
//";

//        if (connection.State == ConnectionState.Closed)
//        {
//          connection.Open();
//        }

//        using (OleDbCommand command = new OleDbCommand(sql, connection))
//        {
//          // add named parameters
//          command.Parameters.AddWithValue("@firstName", player.Etunimi);
//          command.Parameters.AddWithValue("@lastName", player.Sukunimi);
//          command.Parameters.AddWithValue("@price", player.Siirtohinta);
//          command.Parameters.AddWithValue("@team", player.Seura);

//          command.ExecuteNonQuery();
//        }

        playerList.Add(player);

        return playerList.Find(p => p.KokoNimi == player.KokoNimi);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      //finally
      //{
      //  connection.Close();
      //}
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
