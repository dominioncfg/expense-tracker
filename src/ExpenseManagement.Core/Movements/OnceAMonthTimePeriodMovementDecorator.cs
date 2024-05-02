namespace ExpenseManagement.Core;
public class OnceAMonthTimePeriodMovementDecorator : IAccountMovement
{
    private readonly DateOnly _fromMonth;
    private readonly DateOnly _startOfNextMonthOfEndMonth;
    private readonly IAccountMovement _decorated;

    public OnceAMonthTimePeriodMovementDecorator(DateOnly fromMonth, DateOnly toMonth, IAccountMovement decorated)
    {
        _fromMonth = new DateOnly(fromMonth.Year, fromMonth.Month, 01);
        var nextMonth = toMonth.AddMonths(1);
        _startOfNextMonthOfEndMonth = new DateOnly(nextMonth.Year, nextMonth.Month, 01); ;
        _decorated = decorated;
    }

    public bool AppliesAt(DateOnly dateOnly)
    {
        if (dateOnly >= _fromMonth && dateOnly < _startOfNextMonthOfEndMonth)
            return _decorated.AppliesAt(dateOnly);
        return false;
    }

    public AccountMovementResult Apply(DateOnly dateOnly, decimal beforeBalance)
    {
        return _decorated.Apply(dateOnly, beforeBalance);
    }
}
