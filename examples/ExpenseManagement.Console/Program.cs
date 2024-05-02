using ConsoleTables;
using ExpenseManagement.Core;

var expenseDescriptions = ExpenseTrackerForcasterBuilder
                  .New()
                  .WithStartingBalanceAt(500, new DateOnly(2024, 03, 29))

                  .WithRecurrentMonthly(builder => builder
                      .StartingAtMonth(new DateOnly(2024, 04, 01))
                      .WithRegularRevenue(1300, 27, "Paycheck")
                      .WithRegularExpense(400, 01, "Rent")
                      .WithRegularExpense(650, 10, "Basic Expenses")
                      .WithRegularExpense(200, 01, "Fun")
                   )
                  .WithOneTimeExpense(1300, new DateOnly(2024, 04, 10), "Urgent Expense")
                  .WithOneTimeRevenue(1200, new DateOnly(2024, 04, 30), "Gift")
                  .Build();
var from = new DateOnly(2024, 01, 01);
var to = new DateOnly(2025, 01, 31);

var movements = expenseDescriptions.DetailedForecastUntillInclusive(from, to);

PrintMovementsByMonth("This is a test", movements);

void PrintMovementsByMonth(string title, DetailedForecastResult movements)
{
    Console.WriteLine($"================= Plan: {title} =================");

    var movementsByMonth = movements.Results
        .GroupBy(x => new DateOnly(x.Date.Year, x.Date.Month, 01));

    foreach (var movementInMonth in movementsByMonth)
    {
        var startOfMonth = movementInMonth.Key;
        var movementsOfThisMonth = movementInMonth
            .ToList();
        Console.WriteLine("");
        Console.WriteLine($"=====  Month:{startOfMonth.Year}-{startOfMonth.Month}  =======");

        PrintMovementsTable(movementsOfThisMonth);
        PrintEndOfTheMonthBalance(movementInMonth);
    }
    Console.WriteLine($"================= End of Plan: {title} =================");
    Console.WriteLine("");
}

void PrintMovementsTable(List<AccountMovementResult> movementsOfThisMonth)
{
    ConsoleTable
        .From(movementsOfThisMonth)
        .Configure(o => o.NumberAlignment = Alignment.Right)
        .Write(Format.Alternative);
}

static void PrintEndOfTheMonthBalance(IGrouping<DateOnly, AccountMovementResult> movementInMonth)
{
    var endOfMonthAmount = movementInMonth.Last().AfterBalance;
    var lastTransactionDate = movementInMonth.Last().Date;

    var startOfMonthAmount = movementInMonth.First().AfterBalance - movementInMonth.First().Amount;
    var balance = endOfMonthAmount - startOfMonthAmount;


    Console.WriteLine($"==> Balance at End of the month: {endOfMonthAmount:+#;-#;0} ({balance:+#;-#;0})");
    Console.WriteLine($"==> Last Movement: {lastTransactionDate}");
    Console.WriteLine("");
}