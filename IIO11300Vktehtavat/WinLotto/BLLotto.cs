/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 20.1.2016 Modified: 22.1.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace WinLotto
{
  /// <summary>
  /// Business Logic-level Lotto-class
  /// </summary>
  class Lotto
  {
    #region Enums

    public enum GameVersion
    {
      Suomi = 0,
      Viking = 1,
      Eurojackpot = 2
    }

    #endregion Enums

    #region Fields

    private Random rnd = new Random();

    #endregion Fields

    #region Methods

    public GameVersion Game { get; set; }

    public int Drawns { get; set; }

    public string Numbers {
      get
      {
        int numbers, maxNumber;

        switch (Game)
        {
          case GameVersion.Viking:
            numbers = 6;
            maxNumber = 48;
            break;

          case GameVersion.Eurojackpot: // Eurojackpotissa 5/50 ja 2 tähtinumeroa luvuista 1-8
            numbers = 5;
            maxNumber = 50;
            break;

          default: // Suomi 7/39
            numbers = 7;
            maxNumber = 39;
            break;
        }

        var sb = new StringBuilder();

        for (int i = 0; i < Drawns; i++)
        {
          if (Game == GameVersion.Eurojackpot) // Eurojackpotissa 5/50 ja 2 tähtinumeroa luvuista 1-8
          {
            sb.Append(string.Join(", ", getRandomNumbers(numbers, maxNumber)));
            sb.Append(" + tähtinumerot ");
            sb.AppendLine(string.Join(" ja ", getRandomNumbers(2, 8)));
          }
          else
          {
            sb.AppendLine(string.Join(", ", getRandomNumbers(numbers, maxNumber)));
          }
        }

        return sb.ToString();
      }
    }

    private List<int> getRandomNumbers(int numbers, int maxNumber)
    {
      var randomizedList = new List<int>(numbers);
      var allNumbersList = new List<int>(maxNumber);

      for (var i = 1; i <= allNumbersList.Capacity; i++)
      {
        allNumbersList.Add(i);
      }

      while (allNumbersList.Count != 0 && randomizedList.Count < randomizedList.Capacity)
      {
        var index = rnd.Next(0, allNumbersList.Count);
        randomizedList.Add(allNumbersList[index]);
        allNumbersList.RemoveAt(index);
      }

      randomizedList.Sort();

      return randomizedList;
    }

    #endregion Methods
  }
}
