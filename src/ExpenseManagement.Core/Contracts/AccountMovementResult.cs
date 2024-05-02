namespace ExpenseManagement.Core;

public record AccountMovementResult
{
    public DateOnly Date { get; init; }
    public decimal Amount { get; init; }
    public decimal AfterBalance { get; init; }
    public string? Reason { get; init; }


    private AccountMovementResult(DateOnly date, decimal amount, decimal afterBalance, string? reason)
    {
        Date = date;
        Amount = amount;
        AfterBalance = afterBalance;
        Reason = reason;
    }


    public static AccountMovementResult Postive(DateOnly date, decimal beforeBalance, decimal amount, string? reason)
    {
        return new AccountMovementResult(date, amount, beforeBalance + amount, reason);
    }

    public static AccountMovementResult Negative(DateOnly date, decimal beforeBalance, decimal amount, string? reason)
    {
        return new AccountMovementResult(date, - amount, beforeBalance - amount, reason);
    }
}