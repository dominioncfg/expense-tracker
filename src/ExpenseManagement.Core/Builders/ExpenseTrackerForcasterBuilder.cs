namespace ExpenseManagement.Core;

public class ExpenseTrackerForcasterBuilder
{
    private readonly List<IAccountMovement> _movements = new();


    public static ExpenseTrackerForcasterBuilder New() => new();

    public ExpenseTrackerForcasterBuilder WithStartingBalanceAt(decimal startingBalance, DateOnly start)
    {
        _movements.Add(new OneTimeRevenueMovement(startingBalance, start, null));
        return this;
    }


    public ExpenseTrackerForcasterBuilder WithRecurrentMonthly(Action<ExpenseTrackerForcasterMonthlyBuilder> builder)
    {
        var config = new ExpenseTrackerForcasterMonthlyBuilder();
        builder(config);
        _movements.AddRange(config.Build());
        return this;
    }

    public ExpenseTrackerForcasterBuilder WithOneTimeExpense(decimal amount, DateOnly date, string? reason)
    {
        _movements.Add(new OneTimeExpenseMovement(amount, date, reason));
        return this;
    }

    public ExpenseTrackerForcasterBuilder WithOneTimeRevenue(decimal amount, DateOnly date, string? reason)
    {
        _movements.Add(new OneTimeRevenueMovement(amount, date, reason));
        return this;
    }

    public ExpenseTrackerForcaster Build()
    {
        var forecaster = new ExpenseTrackerForcaster();

        foreach (var movement in _movements)
        {
            forecaster.RegisterMovement(movement);
        }

        return forecaster;
    }
}
