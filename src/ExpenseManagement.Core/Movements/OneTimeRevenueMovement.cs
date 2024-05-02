namespace ExpenseManagement.Core;

public class OneTimeRevenueMovement : IAccountMovement
{
    private readonly decimal _amount;
    private readonly DateOnly _date;
    private readonly string? _reason;

    public OneTimeRevenueMovement(decimal amount, DateOnly date, string? reason)
    {
        if (amount < 0)
            throw new ArgumentException("Amount has to be possitive");

        _amount = amount;
        _date = date;
        _reason = reason;
    }

    public bool AppliesAt(DateOnly dateOnly) => dateOnly == _date;


    public AccountMovementResult Apply(DateOnly dateOnly, decimal beforeBalance) =>
      AccountMovementResult.Postive(dateOnly, beforeBalance, _amount, _reason);
}
