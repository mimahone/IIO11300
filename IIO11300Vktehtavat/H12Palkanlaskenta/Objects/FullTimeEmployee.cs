namespace JAMK.IT
{
  public class FullTimeEmployee : Employee
  {
    #region PROPERTIES

    private float montlySalary;

    public float MontlySalary
    {
      get { return montlySalary; }
      set { montlySalary = value; }
    }


    #endregion PROPERTIES

    #region CONSTRUCTORS

    public FullTimeEmployee()
    {

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
