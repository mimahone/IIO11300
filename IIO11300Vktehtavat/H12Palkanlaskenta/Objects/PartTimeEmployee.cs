namespace JAMK.IT
{
  public class PartTimeEmployee : Employee
  {
    #region PROPERTIES

    private float hourlySalary;

    public float HourlySalary
    {
      get { return hourlySalary; }
      set { hourlySalary = value; }
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
