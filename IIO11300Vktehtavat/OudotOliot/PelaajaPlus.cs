namespace OudotOliot
{
  public partial class Pelaaja
  {
    public string Kokonimi
    {
      get { return this.etunimi + " " + this.sukunimi + "@" + this.seura; }
    }
  }
}
