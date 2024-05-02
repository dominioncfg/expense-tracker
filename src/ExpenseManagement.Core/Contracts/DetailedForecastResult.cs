namespace ExpenseManagement.Core;

public record DetailedForecastResult
{
    private readonly List<AccountMovementResult> _results;

    public DetailedForecastResult(List<AccountMovementResult> results)
    {
        _results = results.ToList();
    }

    public IReadOnlyCollection<AccountMovementResult> Results => _results.AsReadOnly();
}