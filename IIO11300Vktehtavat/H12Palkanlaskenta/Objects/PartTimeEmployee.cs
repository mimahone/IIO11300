namespace JAMK.IT
{
  public class PartTimeEmployee : Employee
  {
    #region PROPERTIES

    public float HourlySalary
    {
      get { return salary; }
      set { salary = value; }
    }

    private int workHours;

    public int WorkHours
    {
      get { return workHours; }
      set { workHours = value; }
    }


    #endregion PROPERTIES

    #region CONSTRUCTORS

    public PartTimeEmployee()
    {
      EmployeeType = EmployeeType.PartTime;
    }

    #endregion CONSTRUCTORS

    #region METHODS

    public override float CalculateSalary()
    {
      return HourlySalary * WorkHours;
    }

    #endregion METHODS
  }
}
