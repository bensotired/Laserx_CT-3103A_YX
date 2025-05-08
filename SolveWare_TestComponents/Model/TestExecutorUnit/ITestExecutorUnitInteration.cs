namespace SolveWare_TestComponents.Model
{
    public interface ITestExecutorUnitInteration
    {
        string Name { get; set; }
        TestExecutorUnitStatus Status { get; }
    }
}