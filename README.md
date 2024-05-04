# Expense Tracker

A Library to Track and forecast your expenses.

``` C#
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
```
## Output

- Month:2024-3:

|Date| Amount|AfterBalance|Reason|
|----|-------|------------|----|
|3/29/2024 | 500|500 ||

- Month:2024-4:

|Date| Amount|AfterBalance|Reason|
|----|-------|------------|----|
| 4/1/2024  |   -400 |          100 | Rent           |
| 4/1/2024  |   -200 |         -100 | Fun            |
| 4/10/2024 |   -650 |         -750 | Basic Expenses |
| 4/10/2024 |  -1300 |        -2050 | Urgent Expense |
| 4/27/2024 |   1300 |         -750 | Paycheck       |
| 4/30/2024 |   1200 |          450 | Gift           |


- Month:2024-5:
  
|Date| Amount|AfterBalance|Reason|
|----|-------|------------|----|
| 5/1/2024  |   -400 |           50 | Rent           |
| 5/1/2024  |   -200 |         -150 | Fun            |
| 5/10/2024 |   -650 |         -800 | Basic Expenses |
| 5/27/2024 |   1300 |          500 | Paycheck       |

- Month:2024-6:
 
|Date| Amount|AfterBalance|Reason|
|----|-------|------------|----|
| 6/1/2024  |   -400 |          100 | Rent           |
| 6/1/2024  |   -200 |         -100 | Fun            |
| 6/10/2024 |   -650 |         -750 | Basic Expenses |
| 6/27/2024 |   1300 |          550 | Paycheck       |

...
