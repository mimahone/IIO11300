using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H6DataBinding
{
  public class HockeyPlayer
  {
  }

  public class HockeyTeam
  {
    #region PROPERTIES

    //HUOM! public field ei kelpaa XAMLin bindauksessa, pitää olla Property
    public string Name { get; set; }

    public string City { get; set; }

    #endregion //PROPERTIES

    #region CONSTRUCTORS

    public HockeyTeam()
    {
      Name = "";
      City = "none";
    }

    public HockeyTeam(string teamName, string city)
    {
      Name = teamName;
      City = city;
    }

    public override string ToString()
    {
      return Name + "@" + City;
    }

    #endregion //CONSTRUCTORS
  }

  public class HockeyLeague
  {
    // Perustetaan SM-liiga

    List<HockeyTeam> teams = new List<HockeyTeam>();

    public HockeyLeague()
    {
      teams.Add(new HockeyTeam("Ilves", "Tampere"));
      teams.Add(new HockeyTeam("JYP", "Jyväskylä"));
      teams.Add(new HockeyTeam("HIFK", "Helsinki"));
      teams.Add(new HockeyTeam("Kärpät", "Oulu"));
      teams.Add(new HockeyTeam("Kalpa", "Kuopio"));
    }

    // Metodi joka palauttaa liigan joukkueet
    public List<HockeyTeam> GetTeams()
    {
      return teams;
    }

  }
}
