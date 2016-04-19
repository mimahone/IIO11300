using System;
using System.ComponentModel;

namespace JAMK.IT
{
  public class Person : INotifyPropertyChanged
  {
    #region PROPERTIES

    private string personID;

    public string PersonID
    {
      get { return personID; }
      set { personID = value; }
    }

    private string firstName;

    public string FirstName
    {
      get { return firstName; }
      set
      {
        firstName = value;
        RaisePropertyChanged("FirstName");
        RaisePropertyChanged("FullName");
        RaisePropertyChanged("DisplayName");
      }
    }

    private string lastName;

    public string LastName
    {
      get { return lastName; }
      set
      {
        lastName = value;
        RaisePropertyChanged("LastName");
        RaisePropertyChanged("FullName");
        RaisePropertyChanged("DisplayName");
      }
    }

    public string FullName
    {
      get { return string.Format("{0} {1}", FirstName, LastName); }
    }

    private DateTime birthDay;

    public DateTime BirthDay
    {
      get { return birthDay; }
      set { birthDay = value; }
    }

    public int Age
    {
      get {
        if (BirthDay == DateTime.MinValue)
          return 0;

        int age = DateTime.Today.Year - BirthDay.Year;

        if (BirthDay > DateTime.Today.AddYears(-age))
          age--;

        return age;
      }
    }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    public Person()
    {

    }

    #endregion CONSTRUCTORS

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged(string propName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    #endregion INotifyPropertyChanged Members
  }
}
