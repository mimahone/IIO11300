/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 1.3.2016 Modified: 2.3.2016
* Authors: Mika Mähönen (K6058), Esa Salmikangas
*/

using System.ComponentModel;
using System.Linq;

namespace JAMK.IT.IIO11300
{
  /// <summary>
  /// Class for PasswordStrength
  /// </summary>
  public class PasswordStrength : INotifyPropertyChanged
  {
    #region CONSTRUCTOR

    /// <summary>
    /// Constructor with password
    /// </summary>
    /// <param name="password">Password to evaluate</param>
    public PasswordStrength(string password = "")
    {
      Password = password;
    }

    #endregion //CONSTRUCTOR

    #region PROPERTIES and FIELDS

    /// <summary>
    /// Field for Password
    /// </summary>
    protected string _password = "";

    /// <summary>
    /// Property for Password
    /// </summary>
    public string Password
    {
      get
      {
        return _password;
      }
      set
      {
        if (value == _password) return;
        _password = value;
        countMarkTypes();
        getStrengthness();
        getStrengthnessColor();
        OnPropertyChanged("Password");
        OnPropertyChanged("Chars");
        OnPropertyChanged("UpperCaseLetters");
        OnPropertyChanged("LowerCaseLetters");
        OnPropertyChanged("Numbers");
        OnPropertyChanged("SpecialMarks");
        OnPropertyChanged("Strengthness");
        OnPropertyChanged("StrengthnessColor");
      }
    }

    /// <summary>
    /// Property to get Password characters count
    /// </summary>
    public int Chars { get { return Password.Length; } }

    /// <summary>
    /// Property to get Password uppercase letters count
    /// </summary>
    public int UpperCaseLetters { get { return Password.Count(c => char.IsUpper(c)); } }

    /// <summary>
    /// Property to get Password lowercase letters count
    /// </summary>
    public int LowerCaseLetters { get { return Password.Count(c => char.IsLower(c)); } }

    /// <summary>
    /// Property to get Password numbers count
    /// </summary>
    public int Numbers { get { return Password.Count(c => char.IsNumber(c)); } }

    /// <summary>
    /// Property to get Password special marks count
    /// </summary>
    public int SpecialMarks { get { return Password.Count(c => !char.IsLetterOrDigit(c)); } }

    /// <summary>
    /// Field for Strengthness
    /// </summary>
    protected string _strengthness = "anna salasana";

    /// <summary>
    /// Property to get Password strengthness description text
    /// </summary>
    public string Strengthness
    {
      get { return _strengthness; }
    }

    /// <summary>
    /// Field for StrengthnessColor
    /// </summary>
    protected string _strengthnessColor = "";

    /// <summary>
    /// Property to get Password strengthness color as string
    /// </summary>
    public string StrengthnessColor
    {
      get { return _strengthnessColor; }
    }

    #endregion //PROPERTIES and FIELDS

    #region HELPER METHODS and FIELDS

    /// <summary>
    /// Event for PropertyChangedEventHandler
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Helper method for OnPropertyChanged
    /// </summary>
    /// <param name="name">Name of changed property</param>
    protected void OnPropertyChanged(string name)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(name));
      }
    }

    /// <summary>
    /// Field for count of MarkTypes
    /// </summary>
    protected int _markTypes = 0;

    /// <summary>
    /// Helper method to count marks types of password
    /// </summary>
    protected void countMarkTypes()
    {
      int i = 0;
      if (UpperCaseLetters > 0) i++;
      if (LowerCaseLetters > 0) i++;
      if (Numbers > 0) i++;
      if (SpecialMarks > 0) i++;
      _markTypes = i;
    }

    /// <summary>
    /// Helper method to get strengthness description of password
    /// </summary>
    protected void getStrengthness()
    {
      if (Chars == 0)
      {
        _strengthness = "anna salasana";
        return;
      }

      //good = merkkejä vähintään kuusitoista ja sisältää kaikkien neljän merkkiluokan merkkejä
      if (Chars >= 16 && _markTypes == 4)
      {
        _strengthness = "good";
        return;
      }
      //moderate = merkkejä vähemmän kuin kuusitoista ja kolmen merkkiluokan merkkejä
      else if (Chars < 16 && _markTypes == 3)
      {
        _strengthness = "moderate";
        return;
      }
      //fair = merkkejä vähemmän kuin kaksitoista ja vain kahden merkkiluokan merkkejä
      else if (Chars < 12 && _markTypes == 2)
      {
        _strengthness = "fair";
        return;
      }
      //bad = merkkejä vähemmän kuin kahdeksan tai pelkästään yhden merkkiluokan merkkejä
      else if (Chars < 8 || _markTypes == 1)
      {
        _strengthness = "bad";
        return;
      }     
    }

    /// <summary>
    /// Helper method to get strengthness color as string according strengthness description
    /// </summary>
    protected void getStrengthnessColor()
    {
      switch (_strengthness)
      {
        case "good":
          _strengthnessColor = "Green";
          break;
        case "moderate":
          _strengthnessColor = "LightGreen";
          break;
        case "fair":
          _strengthnessColor = "Yellow";
          break;
        case "bad":
          _strengthnessColor = "Orange";
          break;
        default:
          _strengthnessColor = "DarkGray";
          break;
      }
    }

    #endregion //HELPER METHODS and FIELDS
  }
}

