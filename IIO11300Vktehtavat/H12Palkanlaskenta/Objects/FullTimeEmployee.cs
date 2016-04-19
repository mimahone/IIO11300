namespace JAMK.IT
{
  public class FullTimeEmployee : Employee
  {
    #region PROPERTIES

    public float MontlySalary
    {
      get { return salary; }
      set { salary = value; }
    }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    public FullTimeEmployee()
    {
      EmployeeType = EmployeeType.FullTime;
    }

    #endregion CONSTRUCTORS

    #region METHODS

    public override float CalculateSalary()
    {
      return MontlySalary;
    }

    #endregion METHODS
  }
}
