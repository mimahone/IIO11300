/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 13.2.2016 Modified: 17.2.2016
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

    /// <summary>
    /// Constructor
    /// </summary>
    public Pelaajat()
    {
      connectDatabase();
    }

    /// <summary>
    /// Database connection creating
    /// </summary>
    private void connectDatabase()
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
ORDER BY seura, sukunimi, etunimi
";

      try
      {
        playerList.RemoveAll(p => p.Status == Status.Unchanged);

        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        using (OleDbCommand command = new OleDbCommand(sql, connection))
        {
          using (OleDbDataReader reader = command.ExecuteReader())
          {
            Pelaaja player;
            int id = 0;

            while (reader.Read())
            {
              id = int.Parse(reader.GetValue(0).ToString());

              if ((playerList.Find(p => p.Id == id)) == null)
              {
                player = new Pelaaja(
                    reader.GetString(1),
                    reader.GetString(2),
                    int.Parse(reader.GetValue(3).ToString()),
                    reader.GetString(4)
                );
                player.Id = id;

                playerList.Add(player);
              }
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

    /// <summary>
    /// Get player by full name
    /// </summary>
    /// <param name="fullName">Full name search criteria</param>
    /// <returns>Matching player</returns>
    public Pelaaja GetPlayerByFullName(string fullName)
    {
      return playerList.Find(p => p.KokoNimi == fullName);
    }

    /// <summary>
    /// Get player by display name
    /// </summary>
    /// <param name="displayName">Display name search criteria</param>
    /// <returns>Matching player</returns>
    public Pelaaja GetPlayerByDisplayName(string displayName)
    {
      return playerList.Find(p => p.EsitysNimi == displayName);
    }

    /// <summary>
    /// Created new player to the players list
    /// </summary>
    /// <param name="player">Player to create</param>
    /// <returns>Created player</returns>
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

    /// <summary>
    /// Updates existing player's data of players list
    /// </summary>
    /// <param name="player">Player to update</param>
    /// <returns>Updated player</returns>
    public Pelaaja UpdatePlayer(Pelaaja player)
    {
      try
      {
        if (player.Status != Status.Created)
        {
          player.Status = Status.Modified; 
        }

        return playerList.Find(p => p.KokoNimi == player.KokoNimi);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Deletes existing player of players list
    /// </summary>
    /// <param name="player">Player to delete</param>
    /// <returns>True if deleted else false</returns>
    public bool DeletePlayer(Pelaaja player)
    {
      try
      {
        player = GetPlayerByDisplayName(player.EsitysNimi);

        if (player != null)
        {
          player.Status = Status.Deleted;
          return true;
        }

        return false;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Get data of players (for saving to file)
    /// </summary>
    /// <returns>Data of players as string</returns>
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

    /// <summary>
    /// Saves changes to database
    /// </summary>
    public void SaveChangesToDatabase()
    {
      try
      {
        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        // Remove deleted players from database
        List<Pelaaja> deletedPlayers = playerList.FindAll(p => p.Status == Status.Deleted);

        if (deletedPlayers.Count > 0)
        {
          OleDbCommand command = new OleDbCommand("DELETE FROM Pelaajat WHERE id = @id", connection);

          foreach (Pelaaja player in deletedPlayers)
          {
            command.Parameters.Clear();
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
            command.Parameters.Clear();
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
            command.Parameters.Clear();
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

    /// <summary>
    /// Get next id for new record helper method
    /// </summary>
    /// <returns>Id as int</returns>
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
