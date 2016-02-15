/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 13.2.2016 Modified: 15.2.2016
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
    private OleDbConnection connection;
    private List<Pelaaja> playerList = new List<Pelaaja>();
    private List<Pelaaja> deletedPlayers = new List<Pelaaja>();

    /// <summary>
    /// Constructor
    /// </summary>
    public Pelaajat()
    {
      connectDataBase();
    }

    private void connectDataBase()
    {
      try
      {
        connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["SMLiiga"].ConnectionString);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Get list of players
    /// </summary>
    /// <returns>List of players</returns>
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
        if (connection.State == ConnectionState.Open)
        {
          connection.Close();
        }
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

        player.Status = Status.Created;
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
      try
      {
        player.Status = Status.Modified;

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
          player.Status = Status.Deleted;
          deletedPlayers.Add(player);
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

    public void SaveChangesToDatabase()
    {
      try
      {
        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        // Remove deleted players from database
        if (deletedPlayers.Count > 0)
        {
          OleDbCommand command = new OleDbCommand("DELETE FROM Pelaajat WHERE id = @id", connection);

          foreach (Pelaaja player in deletedPlayers)
          {
            command.Parameters.AddWithValue("@id", player.Id);

            if (command.ExecuteNonQuery() > 0)
            {
              player.Status = Status.Unchanged;
            }
          }
        }

        // Save created players
        List<Pelaaja> createdPlayers = playerList.FindAll(p => p.Status == Status.Created);

        if (createdPlayers.Count > 0)
        {
          OleDbCommand command = new OleDbCommand(
              "INSERT INTO Pelaajat (id, etunimi, sukunimi, arvo, seura) VALUES (?, ?, ?, ?, ?)",
              connection
            );

          foreach (Pelaaja player in createdPlayers)
          {
            // add named parameters
            command.Parameters.AddWithValue("@id", getNextId());
            command.Parameters.AddWithValue("@firstName", player.Etunimi);
            command.Parameters.AddWithValue("@lastName", player.Sukunimi);
            command.Parameters.AddWithValue("@price", player.Siirtohinta);
            command.Parameters.AddWithValue("@team", player.Seura);

            if (command.ExecuteNonQuery() > 0)
            { 
              player.Status = Status.Unchanged;
            }
          }
        }
        
        // Save modified players
        List<Pelaaja> modifiedPlayers = playerList.FindAll(p => p.Status == Status.Modified);

        if (modifiedPlayers.Count > 0)
        {
          OleDbCommand command = new OleDbCommand(
              "UPDATE Pelaajat SET etunimi=?, sukunimi=?, arvo=?, seura=? WHERE id=?",
              connection
            );

          foreach (Pelaaja player in modifiedPlayers)
          {
            command.Parameters.AddWithValue("@firstName", player.Etunimi);
            command.Parameters.AddWithValue("@lastName", player.Sukunimi);
            command.Parameters.AddWithValue("@price", player.Siirtohinta);
            command.Parameters.AddWithValue("@team", player.Seura);
            command.Parameters.AddWithValue("@id", player.Id);

            if (command.ExecuteNonQuery() > 0)
            {
              player.Status = Status.Unchanged;
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
        if (connection.State == ConnectionState.Open)
        {
          connection.Close();
        }
      }
    }

    private int getNextId()
    {
      try
      {
        OleDbCommand command = new OleDbCommand("SELECT MAX(id) + 1 FROM Pelaajat", connection);
        int id = (int)command.ExecuteScalar();
        return id;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
