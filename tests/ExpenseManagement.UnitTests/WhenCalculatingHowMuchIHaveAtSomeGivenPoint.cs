using ExpenseManagement.Core;

namespace ExpenseManagement.UnitTests
{
    public class WhenCalculatingHowMuchIHaveAtSomeGivenPoint
    {
        private readonly DateOnly StartOfJulyMonthDate = new(2024, 07, 01);
        private readonly DateOnly EndOfJulyMonthDate = new(2024, 07, 31);
        private readonly DateOnly StartOfFebruaryMonthDate = new(2024, 02, 01);
        private readonly DateOnly EndOfFebruaryMonthDate = new(2024, 02, 29);
        private readonly DateOnly StartOfJuneMonthDate = new(2024, 06, 01);
        private readonly DateOnly EndOfJuneMonthDate = new(2024, 06, 30);
        private readonly DateOnly EndOfAugustMonthDate = new(2024, 8, 30);
        private readonly string AnyReason = "I was feeling bored";

        [Fact]
        public void CanForecastSingleMonthOnlyStartingBalance()
        {
            var startingBalance = 100;
            
            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJulyMonthDate,EndOfJulyMonthDate);

            amount.Should().Be(startingBalance);
        }


        [Fact]
        public void CanForecastSingleMonthSingleExpense()
        {
            var startingBalance = 100;
            var expenseAmount = 50;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .WithOneTimeExpense(expenseAmount, StartOfJulyMonthDate.AddDays(1), AnyReason)
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJulyMonthDate,EndOfJulyMonthDate);

            amount.Should().Be(startingBalance - expenseAmount);
        }


        [Fact]
        public void CanForecastSingleMonthOneRevenue()
        {
            var startingBalance = 100;
            var revenue = 50;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .WithOneTimeRevenue(revenue, StartOfJulyMonthDate.AddDays(1), AnyReason)
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJulyMonthDate,EndOfJulyMonthDate);

            amount.Should().Be(startingBalance + revenue);
        }


        [Fact]
        public void CanForecastSingleMonthOneExpenseAndOneRevenue()
        {
            var startingBalance = 100;
            var revenue = 50;
            var expense = 20;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .WithOneTimeRevenue(revenue, StartOfJulyMonthDate.AddDays(1), AnyReason)
               .WithOneTimeExpense(expense, StartOfJulyMonthDate.AddDays(1), AnyReason)
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate);

            amount.Should().Be(startingBalance + revenue - expense);
        }

        [Fact]
        public void RevenueOrExpenseAfterEndPeriodOfCalculationAreNotTakedIntoAccount()
        {
            var startingBalance = 100;
            var revenue = 50;
            var expense = 20;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .WithOneTimeRevenue(revenue, EndOfJulyMonthDate.AddDays(-1), AnyReason)
               .WithOneTimeExpense(expense, EndOfJulyMonthDate.AddDays(-1), AnyReason)
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate.AddDays(-2));

            amount.Should().Be(startingBalance);
        }


        [Fact]
        public void RevenueOrExpenseOnExactEndPeriodOfCalculationIsTakedIntoAccount()
        {
            var startingBalance = 100;
            var revenue = 50;
            var expense = 20;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .WithOneTimeRevenue(revenue, EndOfJulyMonthDate.AddDays(-1), AnyReason)
               .WithOneTimeExpense(expense, EndOfJulyMonthDate.AddDays(-1), AnyReason)
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate.AddDays(-1));

            amount.Should().Be(startingBalance + revenue - expense);
        }

        [Fact]
        public void SingleMonthWithMonthlyRevenue()
        {
            var startingBalance = 100;
            var revenue = 50;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .WithRecurrentMonthly(builder => builder
                    .WithRegularRevenue(revenue, StartOfJulyMonthDate.Day + 1 , AnyReason))
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate);

            amount.Should().Be(startingBalance + revenue);
        }

        [Fact]
        public void SingleMonthWithMonthlyExpense()
        {
            var startingBalance = 100;
            var expense = 50;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .WithRecurrentMonthly(builder => builder
                    .WithRegularExpense(expense, StartOfJulyMonthDate.Day + 1, AnyReason))
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate);

            amount.Should().Be(startingBalance - expense);
        }

        [Fact]
        public void MonthlyExpensesAndRevenuesAreAppliedEvenWhenDayIsGreaterThanEndOfTheMonth()
        {
            var startingBalance = 100;
            var revenue = 50;
            var expense = 20;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfFebruaryMonthDate)
               .WithRecurrentMonthly(builder => builder
                    .WithRegularRevenue(revenue, 31, AnyReason)
                    .WithRegularExpense(expense, 31, AnyReason))
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfFebruaryMonthDate,EndOfFebruaryMonthDate);

            amount.Should().Be(startingBalance + revenue - expense);
        }


        [Fact]
        public void MonthlyTemporallyRevenueAlreadyEndedDoesNotApplyAnymore()
        {
            var startingBalance = 100;
            var revenue = 50;
            var expense = 20;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .WithRecurrentMonthly(builder => builder
                    .StartingAtMonth(StartOfFebruaryMonthDate)
                    .EndingAtMonth(EndOfJuneMonthDate)
                    .WithRegularRevenue(revenue, 10, AnyReason)
                    .WithRegularExpense(expense, 10, AnyReason))
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate);

            amount.Should().Be(startingBalance);
        }

        [Fact]
        public void MonthlyTemporallyRevenueStartingOnSameAsPeriodApplies()
        {
            var startingBalance = 100;
            var revenue = 50;
            var expense = 20;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .WithRecurrentMonthly(builder => builder
                    .StartingAtMonth(StartOfJulyMonthDate)
                    .EndingAtMonth(EndOfAugustMonthDate)
                    .WithRegularRevenue(revenue, 1, AnyReason)
                    .WithRegularExpense(expense, 1, AnyReason))
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate);

            amount.Should().Be(startingBalance + revenue - expense);
        }


        [Fact]
        public void MonthlyTemporallyRevenueEndingOnSameDayAsPeriodApplies()
        {
            var startingBalance = 100;
            var revenue = 50;
            var expense = 20;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .WithRecurrentMonthly(builder => builder
                    .StartingAtMonth(StartOfFebruaryMonthDate)
                    .EndingAtMonth(EndOfJulyMonthDate)
                    .WithRegularRevenue(revenue, 31, AnyReason)
                    .WithRegularExpense(expense, 31, AnyReason))
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate);

            amount.Should().Be(startingBalance + revenue - expense);
        }

        [Fact]
        public void TwoMonthsWithRecurringRevenueAndExpenses()
        {
            var startingBalance = 100;
            var revenue = 50;
            var expense = 20;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJuneMonthDate)
               .WithRecurrentMonthly(builder => builder
                    .WithRegularRevenue(revenue, 10, AnyReason)
                    .WithRegularExpense(expense, 10, AnyReason))
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJuneMonthDate, EndOfJulyMonthDate);

            amount.Should().Be(startingBalance+ (2* revenue) - (2* expense));
        }


        [Fact]
        public void TwoMonthsWithRecurringRevenueAndExpensesAndOneTimeRevenueAndExpense()
        {
            var startingBalance = 100;
            var recurringRevenue = 50;
            var recurringExpense = 20;

            var oneTimeRevenue = 12;
            var oneTimeExpense = 20;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJuneMonthDate)

               .WithOneTimeRevenue(oneTimeRevenue, StartOfJuneMonthDate.AddDays(1),AnyReason)
               .WithOneTimeExpense(oneTimeExpense, StartOfJuneMonthDate.AddDays(1), AnyReason)
               
               .WithRecurrentMonthly(builder => builder
                    .WithRegularRevenue(recurringRevenue, 10, AnyReason)
                    .WithRegularExpense(recurringExpense, 10, AnyReason))
               .Build();

            var amount = financialForecaster.HowMuchIWillHaveAtUntilInclusive(StartOfJuneMonthDate, EndOfJulyMonthDate);

            amount.Should().Be(startingBalance + (2 * recurringRevenue) - (2 * recurringExpense) + oneTimeRevenue - oneTimeExpense);
            amount.Should().Be(152);
        }
    }
}