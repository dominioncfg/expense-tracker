namespace ExpenseManagement.Core;

public class OneTimeExpenseMovement : IAccountMovement
{
    private readonly decimal _amount;
    private readonly DateOnly _date;
    private readonly string? _reason;

    public OneTimeExpenseMovement(decimal amount, DateOnly date, string? reason)
    {
        if (amount < 0)
            throw new ArgumentException("Amount has to be possitive");

        _amount = amount;
        _date = date;
        _reason = reason;
    }

    public bool AppliesAt(DateOnly dateOnly) => dateOnly == _date;


    public AccountMovementResult Apply(DateOnly dateOnly, decimal beforeBalance) =>
        AccountMovementResult.Negative(dateOnly, beforeBalance, _amount, _reason);
}
