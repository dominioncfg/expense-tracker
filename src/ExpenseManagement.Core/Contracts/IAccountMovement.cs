namespace ExpenseManagement.Core;

public interface IAccountMovement
{
    public bool AppliesAt(DateOnly dateOnly);
    public AccountMovementResult Apply(DateOnly dateOnly, decimal beforeBalance);
}
