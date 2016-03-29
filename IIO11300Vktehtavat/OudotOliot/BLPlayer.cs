/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 8.2.2016 Modified: 29.3.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace JAMK.IT.IIO11300
{
  /// <summary>
  /// Enum to contain object's modification status
  /// </summary>
  public enum Status
  {
    Unchanged = 0,
    Created = 1,
    Modified = 2,
    Deleted = 3
  }

  /// <summary>
  /// Class for Player object
  /// </summary>
  [Serializable]
  public class Player : INotifyPropertyChanged
  {
    #region PROPERTIES

    public int Id { get; set; }

    private string firstName;

    public string FirstName
    {
      get { return firstName; }
      set {
        firstName = value;
        Notify("FirstName");
        Notify("FullName");
        Notify("DisplayName");
      }
    }

    private string lastName;

    public string LastName
    {
      get { return lastName; }
      set
      {
        lastName = value;
        Notify("LastName");
        Notify("FullName");
        Notify("DisplayName");
      }
    }

    public string FullName { get { return string.Format("{0} {1}", LastName, FirstName); } }

    public string DisplayName { get { return string.Format("{0} {1}, {2}", FirstName, LastName, Team); } }

    private string team;

    public string Team
    {
      get { return team; }
      set
      {
        team = value;
        Notify("DisplayName");
      }
    }

    private int price;

    public int Price
    {
      get { return price; }
      set
      {
        price = value;
        Notify("Price");
      }
    }

    [XmlIgnore]
    public Status Status { get; set; }

    #endregion

    #region CONSTRUCTORS

    public Player()
    {
      // XML-serializing demands this
    }

    public Player(int id)
    {
      Id = id;
    }

    public Player(string firstName, string lastName, int price, string team)
    {
      FirstName = firstName;
      LastName = lastName;
      Price = price;
      Team = team;
    }

    #endregion

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    void Notify(string propName)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propName));
        Status = Status.Modified;
      }
    }

    #endregion INotifyPropertyChanged Members
  }

  /// <summary>
  /// Class for Players Business Logic
  /// </summary>
  public class Players
  {
    #region PROPERTIES

    private static ObservableCollection<Player> players;

    public static ObservableCollection<Player> PlayerList { get { return players; } }

    /// <summary>
    /// Shows if collection has changes
    /// </summary>
    public static bool IsDirty
    {
      get { return players.ToList().Exists(p => p.Status == Status.Deleted || p.Status == Status.Created || p.Status == Status.Modified); }
    }

    #endregion

    #region METHODS

    /// <summary>
    /// Refresh Players List
    /// </summary>
    public static void RefreshPlayers()
    {
      try
      {
        players = new ObservableCollection<Player>();

        string message = "";
        DataTable dt = DBPlayers.GetPlayers(out message);

        // ORM =  Muutetaan datatablen rivit olioiksi
        Player player;

        foreach (DataRow row in dt.Rows)
        {
          player = new Player(int.Parse(row[0].ToString()));
          player.FirstName = row[1].ToString();
          player.LastName = row[2].ToString();
          player.Price = int.Parse(row[3].ToString());
          player.Team = row[4].ToString();
          players.Add(player);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Update Player
    /// </summary>
    /// <param name="player">Player to Update</param>
    /// <param name="message">return message</param>
    /// <returns>Count of affected rows</returns>
    public static int UpdatePlayer(Player player, out string message)
    {
      try
      {
        return DBPlayers.UpdatePlayer(player, out message);
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Create Player
    /// </summary>
    /// <param name="player">Player to Create</param>
    /// <param name="message">return message</param>
    /// <returns>Count of affected rows</returns>
    public static int CreatePlayer(Player player, out string message)
    {
      try
      {
        return DBPlayers.InsertPlayer(player, out message);
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Delete Player
    /// </summary>
    /// <param name="player">Player to Insert</param>
    /// <param name="message">return message</param>
    /// <returns>Count of affected rows</returns>
    public static int DeletePlayer(Player player, out string message)
    {
      try
      {
        return DBPlayers.DeletePlayer(player.Id, out message);
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Saves changes to database
    /// </summary>
    /// <param name="message">return message</param>
    /// <returns>Count of affected rows</returns>
    public static int SaveChangesToDatabase(out string message)
    {
      int i = 0;

      try
      {
        // Remove deleted players
        List<Player> deletedPlayers = players.ToList().FindAll(p => p.Status == Status.Deleted);

        foreach (Player player in deletedPlayers)
        {
          if (DeletePlayer(player, out message) > 0)
          {
            player.Status = Status.Unchanged;
            i++;
          }
        }

        // Save created players
        List<Player> createdPlayers = players.ToList().FindAll(p => p.Status == Status.Created || p.Id == 0);

        foreach (Player player in createdPlayers)
        {
          if (CreatePlayer(player, out message) > 0)
          {
            player.Status = Status.Unchanged;
            i++;
          }
        }

        // Save modified players
        List<Player> modifiedPlayers = players.ToList().FindAll(p => p.Status == Status.Modified && p.Id > 0);

        foreach (Player player in modifiedPlayers)
        {
          if (UpdatePlayer(player, out message) > 0)
          {
            player.Status = Status.Unchanged;
            i++;
          }
        }

        message = "Muuttuneet tiedot tallennettu";
        return i;
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
    public static string GetPlayerData()
    {
      try
      {
        if (players == null || players.Count == 0)
        {
          return null;
        }

        StringBuilder sb = new StringBuilder();

        foreach (Player player in players)
        {
          sb.AppendLine(string.Format("{0},{1},{2},{3},{4}", player.Id, player.FirstName, player.LastName, player.Price, player.Team));
        }

        return sb.ToString();
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
    public static ObservableCollection<Player> GetPlayersFromTxtFile(string filePath)
    {
      players.Clear();

      try
      {
        var lines = File.ReadAllLines(filePath);
        string[] items;
        Player player;

        foreach (var line in lines)
        {
          items = line.Split(',');

          if (items.Length >= 4)
          {
            player = new Player(items[1], items[2], int.Parse(items[3]), items[4]);
            player.Id = int.Parse(items[0]);
            players.Add(player);
          }
        }

        return players;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    #endregion
  }
}
