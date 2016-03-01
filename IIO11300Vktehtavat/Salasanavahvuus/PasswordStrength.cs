using System.ComponentModel;
using System.Linq;

namespace Salasanavahvuus
{
  public class PasswordStrength : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(name));
      }
    }

    public PasswordStrength(string password = "")
    {
      Password = password;
    }

    private string _password = "";

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

    public int Chars { get { return Password.Length; } }

    public int UpperCaseLetters { get { return Password.Count(c => char.IsUpper(c)); } }

    public int LowerCaseLetters { get { return Password.Count(c => char.IsLower(c)); } }

    public int Numbers { get { return Password.Count(c => char.IsNumber(c)); } }

    public int SpecialMarks { get { return Password.Count(c => !char.IsLetterOrDigit(c)); } }

    private int _markTypes = 0;

    private string _strengthness = "anna salasana";

    public string Strengthness
    {
      get { return _strengthness; }
    }

    private string _strengthnessColor = "";

    public string StrengthnessColor
    {
      get { return _strengthnessColor; }
    }

    private void countMarkTypes()
    {
      int i = 0;
      if (UpperCaseLetters > 0) i++;
      if (LowerCaseLetters > 0) i++;
      if (Numbers > 0) i++;
      if (SpecialMarks > 0) i++;
      _markTypes = i;
    }

    private void getStrengthness()
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

    private void getStrengthnessColor()
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
  }
}

//https://msdn.microsoft.com/fi-fi/library/ms752039%28v=vs.100%29.aspx
