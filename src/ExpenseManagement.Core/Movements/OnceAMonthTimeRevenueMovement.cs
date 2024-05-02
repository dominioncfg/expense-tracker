
namespace ExpenseManagement.Core;
public class OnceAMonthTimeRevenueMovement : IAccountMovement
{
    private readonly decimal _amount;
    private readonly int _day;
    private readonly string? _reason;
    private readonly bool applyOnLastDayIfDayIsGreaterThanLastDayOfMonth = true;

    public OnceAMonthTimeRevenueMovement(decimal amount, int day, string? reason)
    {
        if (amount < 0)
            throw new ArgumentException("Amount has to be possitive");

        if (day < 1 || day > 31)
            throw new ArgumentException("Invalid day");

        _amount = amount;
        _day = day;
        _reason = reason;
    }

    public bool AppliesAt(DateOnly dateOnly)
    {
        if (dateOnly.Day == _day)
            return true;

        if (!applyOnLastDayIfDayIsGreaterThanLastDayOfMonth)
            return false;

        return IsEndOfMonthAndHasNotBeingApplied(dateOnly);
    }

    private bool IsEndOfMonthAndHasNotBeingApplied(DateOnly dateOnly)
    {
        var daysInThisMonth = DateTime.DaysInMonth(dateOnly.Year, dateOnly.Month);
        var currentDateIsLastDayOfMonth = dateOnly.Day == daysInThisMonth;
        var hasBeenApplied = _day < daysInThisMonth;

        if (currentDateIsLastDayOfMonth && !hasBeenApplied)
            return true;

        return false;
    }

    public AccountMovementResult Apply(DateOnly dateOnly, decimal beforeBalance) =>
       AccountMovementResult.Postive(dateOnly, beforeBalance, _amount, _reason);
}
