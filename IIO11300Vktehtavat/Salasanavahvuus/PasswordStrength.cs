using System.Linq;

namespace Salasanavahvuus
{
  public class PasswordStrength
  {

    public PasswordStrength(string password = "")
    {
      Password = password;
    }

    public string Password { get; set; }

    public int Chars { get { return Password.Length; } }

    public int UpperCaseLetters { get { return Password.Count(c => char.IsUpper(c)); } }

    public int LowerCaseLetters { get { return Password.Count(c => char.IsLower(c)); } }

    public int Numbers { get { return Password.Count(c => char.IsNumber(c)); } }

    public int SpecialMarks { get { return Password.Count(c => !char.IsLetterOrDigit(c)); } }

    public int MarkTypes {
      get { return countMarkTypes(); }
    }

    public string Strengthness
    {
      get { return getStrengthness(); }
    }

    private int countMarkTypes()
    {
      int i = 0;
      if (UpperCaseLetters > 0) i++;
      if (LowerCaseLetters > 0) i++;
      if (Numbers > 0) i++;
      if (SpecialMarks > 0) i++;
      return i;
    }

    private string getStrengthness()
    {
      int markTypes = MarkTypes;

      if (Chars == 0) return "anna salasana";

      //bad = merkkejä vähemmän kuin kahdeksan tai pelkästään yhden merkkiluokan merkkejä
      if (Chars < 8 || markTypes == 1)
      {
        return "bad";
      }

      //fair = merkkejä vähemmän kuin kaksitoista ja vain kahden merkkiluokan merkkejä
      if (Chars < 12 || markTypes == 2)
      {
        return "fair";
      }

      //moderate = merkkejä vähemmän kuin kuusitoista ja kolmen merkkiluokan merkkejä
      if (Chars < 16 || markTypes == 3)
      {
        return "moderate";
      }

      //good = merkkejä vähintään kuusitoista ja sisältää kaikkien neljän merkkiluokan merkkejä
      if (Chars >= 16 && markTypes == 4)
      {
        return "good";
      }

      return "anna salasana";
    }
  }
}
