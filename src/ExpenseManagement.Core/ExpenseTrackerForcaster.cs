
namespace ExpenseManagement.Core;

public class ExpenseTrackerForcaster
{
    private readonly List<IAccountMovement> _accountMovements = new();

    public decimal HowMuchIWillHaveAtUntilInclusive(DateOnly startingAt, DateOnly until)
    {
        var forecast = DetailedForecastUntillInclusive(startingAt, until);
        return forecast.Results.LastOrDefault()?.AfterBalance ?? 0;
    }

    public void RegisterMovement(IAccountMovement accountMovement)
    {
        _accountMovements.Add(accountMovement);
    }

    public DetailedForecastResult DetailedForecastUntillInclusive(DateOnly startingAt, DateOnly until)
    {
        if (startingAt > until)
            throw new InvalidOperationException("Starting date the future");

        List<AccountMovementResult> results = new();

        var currentDate = startingAt;
        while (currentDate <= until)
        {
            foreach (var accountMovement in _accountMovements)
            {
                if (accountMovement.AppliesAt(currentDate))
                {
                    var currentBalance = results.LastOrDefault()?.AfterBalance ?? 0;
                    var loggedMovement = accountMovement.Apply(currentDate, currentBalance);
                    results.Add(loggedMovement);
                }
            }
            currentDate = currentDate.AddDays(1);
        }

        return new DetailedForecastResult(results);
    }
}
