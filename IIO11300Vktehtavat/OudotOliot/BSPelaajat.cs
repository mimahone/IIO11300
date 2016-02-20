/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 13.2.2016 Modified: 20.2.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace JAMK.IT.IIO11300
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
    /// Get player from txt-file
    /// </summary>
    /// <param name="filePath">File path to read</param>
    public List<Pelaaja> GetPlayersFromTxtFile(string filePath)
    {
      playerList.Clear();

      try
      {
        var lines = File.ReadAllLines(filePath);
        string[] items;
        Pelaaja player;

        foreach (var line in lines)
        {
          items = line.Split(',');

          if (items.Length >= 4)
          {
            player = new Pelaaja(items[1], items[2], int.Parse(items[3]), items[4]);
            player.Id = int.Parse(items[0]);
            playerList.Add(player);
          }
        }

        return playerList;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Serialize players into bin-file
    /// </summary>
    /// <param name="filePath">Saving path</param>
    /// <param name="ic">Collection to serialize</param>
    public static void SerializePlayersToBinFile(string filePath, ICollection ic)
    {
      FileStream fs = new FileStream(filePath, FileMode.Create);
      BinaryFormatter bf = new BinaryFormatter();

      try
      {
        bf.Serialize(fs, ic);
      }
      catch (SerializationException se)
      {
        throw se;
      }
      finally
      {
        fs.Close();
      }
    }

    /// <summary>
    /// Deserialize players from bin-file
    /// </summary>
    /// <param name="filePath">File path to deserialize</param>
    /// <param name="obj">Deserialized object containing data</param>
    public static void DeserializePlayersFromBinFile(string filePath, ref object obj)
    {
      FileStream fs = new FileStream(filePath, FileMode.Open);

      try
      {
        BinaryFormatter bf = new BinaryFormatter();
        obj = bf.Deserialize(fs);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        fs.Close();
      }
    }

    /// <summary>
    /// Serialize players into xml-file
    /// </summary>
    /// <param name="filePath">Saving path</param>
    /// <param name="ic">Collection to serialize</param>
    public static void SerializePlayersToXmlFile(string filePath, ICollection ic)
    {
      XmlSerializer xs = new XmlSerializer(ic.GetType());
      TextWriter tw = new StreamWriter(filePath);

      try
      {
        xs.Serialize(tw, ic);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        tw.Close();
      }
    }

    /// <summary>
    /// Deserialize players from xml-file
    /// </summary>
    /// <param name="filePath">File path to deserialize</param>
    public static List<Pelaaja> DeserializePlayersFromXmlFile(string filePath)
    {
      XmlSerializer deserializer = new XmlSerializer(typeof(List<Pelaaja>));
      TextReader tr = new StreamReader(filePath);

      try
      {
        return (List<Pelaaja>)deserializer.Deserialize(tr);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        tr.Close();
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
          sb.AppendLine(string.Format("{0},{1},{2},{3},{4}", player.Id, player.Etunimi, player.Sukunimi, player.Siirtohinta, player.Seura));
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
