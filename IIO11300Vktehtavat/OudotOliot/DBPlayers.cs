/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 13.2.2016 Modified: 20.2.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace JAMK.IT.IIO11300
{
  public class DBPlayers
  {
    /// <summary>
    /// ConnectionString
    /// </summary>
    private static string ConnectionString
    {
      get { return ConfigurationManager.ConnectionStrings["SMLiiga"].ConnectionString; }
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
    public static ObservableCollection<Player> DeserializePlayersFromXmlFile(string filePath)
    {
      XmlSerializer deserializer = new XmlSerializer(typeof(List<Player>));
      TextReader tr = new StreamReader(filePath);

      try
      {
        return (ObservableCollection<Player>)deserializer.Deserialize(tr);
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
    /// Get Players as DataTable from Database
    /// </summary>
    /// <param name="message">return message</param>
    /// <returns>DataTable containing Players</returns>
    public static DataTable GetPlayers(out string message)
    {
      const string sql = @"SELECT id, etunimi, sukunimi, arvo, seura FROM Pelaajat ORDER BY seura, sukunimi, etunimi";

      try
      {
        using (OleDbConnection connection = new OleDbConnection(ConnectionString))
        {
          OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
          DataTable table = new DataTable("Players");
          adapter.Fill(table);

          message = string.Format("Haettu {0} pelaajan tiedot", table.Rows.Count);
          return table;
        }
      }
      catch (Exception ex)
      {
        message = ex.Message;
        throw ex;
      }
    }

    /// <summary>
    /// Add Player to Database
    /// </summary>
    /// <param name="player">Player to Create</param>
    /// <param name="message">return message</param>
    /// <returns>Count of affected rows in database</returns>
    public static int InsertPlayer(Player player, out string message)
    {
      try
      {
        const string cmdText = @"INSERT INTO Pelaajat (id, etunimi, sukunimi, arvo, seura) VALUES (?, ?, ?, ?, ?)";

        using (OleDbConnection connection = new OleDbConnection(ConnectionString))
        {
          OleDbCommand command = new OleDbCommand(cmdText, connection);
          command.Parameters.AddWithValue("@id", getNextId());
          command.Parameters.AddWithValue("@firstName", player.FirstName);
          command.Parameters.AddWithValue("@lastName", player.LastName);
          command.Parameters.AddWithValue("@price", player.Price);
          command.Parameters.AddWithValue("@team", player.Team);

          connection.Open();
          int c = command.ExecuteNonQuery();
          connection.Close();

          if (c > 0)
          {
            message = "Uuden pelaajan tiedot on tallennettu";
          }
          else
          {
            throw new Exception("InsertCustomer() failed to insert new player!");
          }

          return c;
        }
      }
      catch (Exception ex)
      {
        message = ex.Message;
        throw ex;
      }
    }

    /// <summary>
    /// Update Player to Database
    /// </summary>
    /// <param name="player">Player to Update</param>
    /// <param name="message">return message</param>
    /// <returns>Count of affected rows in database</returns>
    public static int UpdatePlayer(Player player, out string message)
    {
      try
      {
        const string cmdText = @"UPDATE Pelaajat SET etunimi=?, sukunimi=?, arvo=?, seura=? WHERE id=?";

        using (OleDbConnection connection = new OleDbConnection(ConnectionString))
        {
          OleDbCommand command = new OleDbCommand(cmdText, connection);
          command.Parameters.AddWithValue("@firstName", player.FirstName);
          command.Parameters.AddWithValue("@lastName", player.LastName);
          command.Parameters.AddWithValue("@price", player.Price);
          command.Parameters.AddWithValue("@team", player.Team);
          command.Parameters.AddWithValue("@id", player.Id);

          connection.Open();
          int c = command.ExecuteNonQuery();
          connection.Close();

          if (c > 0)
          {
            message = "Pelaajan tiedot on päivitetty";
          }
          else
          {
            throw new Exception("UpdatePlayer() failed to update player's details!");
          }

          return c;
        }
      }
      catch (Exception ex)
      {
        message = ex.Message;
        throw ex;
      }
    }

    /// <summary>
    /// Delete Player from Database
    /// </summary>
    /// <param name="id">Id of Player to delete</param>
    /// <param name="message">return message</param>
    public static int DeletePlayer(int id, out string message)
    {
      try
      {
        const string cmdText = @"DELETE FROM Pelaajat WHERE id = @ID";

        using (OleDbConnection connection = new OleDbConnection(ConnectionString))
        {
          OleDbCommand command = new OleDbCommand(cmdText, connection);
          command.Parameters.AddWithValue("@ID", id);

          connection.Open();
          int c = command.ExecuteNonQuery();
          connection.Close();

          if (c > 0)
          {
            message = "Pelaajan tiedot on poistettu";
          }
          else
          {
            throw new Exception("DeletePlayer() failed to delete player!");
          }

          return c;
        }
      }
      catch (Exception ex)
      {
        message = ex.Message;
        throw ex;
      }
    }

    /// <summary>
    /// Get next id for new record helper method
    /// </summary>
    /// <returns>Id as int</returns>
    private static int getNextId()
    {
      try
      {
        using (OleDbConnection connection = new OleDbConnection(ConnectionString))
        {
          OleDbCommand command = new OleDbCommand("SELECT MAX(id) + 1 FROM Pelaajat", connection);
          connection.Open();
          int id = (int)command.ExecuteScalar();
          connection.Close();
          return id;
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
