using System;

namespace JAMK.IT
{
  public class Person
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
      set { firstName = value; }
    }

    private string lastName;

    public string LastName
    {
      get { return lastName; }
      set { lastName = value; }
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
  }
}
