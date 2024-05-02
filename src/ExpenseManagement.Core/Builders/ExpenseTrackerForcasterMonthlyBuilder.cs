namespace ExpenseManagement.Core;

public class ExpenseTrackerForcasterMonthlyBuilder
{
    private DateOnly? _fromMonth;
    private DateOnly? _toMonth;
    private readonly List<(decimal Amount, int DayOfTheMonth, string? Reason)> _expenses = new();
    private readonly List<(decimal Amount, int DayOfTheMonth, string? Reason)> _revenues = new();

    public ExpenseTrackerForcasterMonthlyBuilder WithRegularRevenue(decimal amount, int dayOfTheMonth, string? reason)
    {
        _revenues.Add((amount, dayOfTheMonth, reason));
        return this;
    }

    public ExpenseTrackerForcasterMonthlyBuilder WithRegularExpense(decimal amount, int dayOfTheMonth, string? reason)
    {
        _expenses.Add((amount, dayOfTheMonth, reason));
        return this;
    }

    public ExpenseTrackerForcasterMonthlyBuilder StartingAtMonth(DateOnly from)
    {
        _fromMonth = from;
        return this;
    }


    public ExpenseTrackerForcasterMonthlyBuilder EndingAtMonth(DateOnly to)
    {
        _toMonth = to;
        return this;
    }


    public List<IAccountMovement> Build()
    {
        var movements = new List<IAccountMovement>();

        foreach (var (amount, dayOfTheMonth, reason) in _revenues)
        {
            IAccountMovement movement = new OnceAMonthTimeRevenueMovement(amount, dayOfTheMonth, reason);
            if (_fromMonth.HasValue || _toMonth.HasValue)
            {
                var fromMonth = _fromMonth ?? DateOnly.MinValue;
                var toMonth = _toMonth ?? new DateOnly(2100,08,01);
                movement = new OnceAMonthTimePeriodMovementDecorator(fromMonth, toMonth, movement);
            }
            movements.Add(movement);
        }

        foreach (var (amount, dayOfTheMonth, reason) in _expenses)
        {
            IAccountMovement movement = new OnceAMonthTimeExpenseMovement(amount, dayOfTheMonth, reason);
            if (_fromMonth.HasValue || _toMonth.HasValue)
            {
                var fromMonth = _fromMonth ?? DateOnly.MinValue;
                var toMonth = _toMonth ?? new DateOnly(2100, 08, 01);
                movement = new OnceAMonthTimePeriodMovementDecorator(fromMonth, toMonth, movement);
            }

            movements.Add(movement);
        }

        return movements;
    }
}
