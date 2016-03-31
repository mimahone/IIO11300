namespace H12BookShopEF
{
  public partial class Customer
  {
    public string DisplayName { get { return firstname + " " + lastname; } }

    public int OrderCount { get { return Orders.Count; }  }
  }

  public partial class Book
  {
    public string DisplayName { get { return name + " " + year; } }
  }
}
