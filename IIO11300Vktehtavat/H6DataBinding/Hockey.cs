using System.Collections.Generic;
using System.ComponentModel;

namespace H6DataBinding
{
  public class HockeyPlayer : INotifyPropertyChanged
  {
    #region PROPERTIES

    private string name;

    public string Name {
      get { return name; }
      set
      {
        name = value;
        Notify("Name");
        Notify("NameAndNumber");
      }
    }

    private string number;    

    public string Number
    {
      get { return number; }
      set
      {
        number = value;
        Notify("Number");
        Notify("NameAndNumber");
      }
    }

    public string NameAndNumber { get { return name + "#" + number; } }

    #endregion PROPERTIES

    #region CONSTRUCTORS
    public HockeyPlayer()
    {

    }

    public HockeyPlayer(string name, string number)
    {
      this.name = name;
      this.number = number;
    }

    #endregion CONSTRUCTORS

    public override string ToString()
    {
      return name + "#" + number;
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    void Notify(string propName)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propName));
      }
    }

    #endregion INotifyPropertyChanged Members
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
