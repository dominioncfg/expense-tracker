using ExpenseManagement.Core;

namespace FinancialForecaster.UnitTest
{
    public class WhenCalculatingDetailedForecast
    {
        private readonly DateOnly StartOfJulyMonthDate = new(2024, 07, 01);
        private readonly DateOnly EndOfJulyMonthDate = new(2024, 07, 31);
        private readonly string AnyReason = "I was feeling bored";

        [Fact]
        public void CanForecastSingleMonthOnlyStartingBalance()
        {
            var startingBalance = 100;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .Build();

            var result = financialForecaster.DetailedForecastUntillInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate);

            result.Should().NotBeNull();
            result.Results.Should().HaveCount(1);
            var movement = result.Results.First();
            movement.Should().NotBeNull();
            movement.Amount.Should().Be(startingBalance);
            movement.Date.Should().Be(StartOfJulyMonthDate);
            movement.AfterBalance.Should().Be(startingBalance);
            movement.Reason.Should().BeNull();

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

            var result = financialForecaster.DetailedForecastUntillInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate);

            result.Should().NotBeNull();

            result.Results.Should().HaveCount(2);
            var startingBalanceMovement = result.Results.First();
            startingBalanceMovement.Should().NotBeNull();
            startingBalanceMovement.Amount.Should().Be(startingBalance);
            startingBalanceMovement.Date.Should().Be(StartOfJulyMonthDate);
            startingBalanceMovement.AfterBalance.Should().Be(startingBalance);
            startingBalanceMovement.Reason.Should().BeNull();

            var expenseMovement = result.Results.Last();
            expenseMovement.Should().NotBeNull();
            expenseMovement.Amount.Should().Be(-expenseAmount);
            expenseMovement.Date.Should().Be(StartOfJulyMonthDate.AddDays(1));
            expenseMovement.AfterBalance.Should().Be(startingBalance - expenseAmount);
            expenseMovement.Reason.Should().Be(AnyReason);
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

            var result = financialForecaster.DetailedForecastUntillInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate);

            result.Should().NotBeNull();

            result.Results.Should().HaveCount(2);
            var startingBalanceMovement = result.Results.First();
            startingBalanceMovement.Should().NotBeNull();
            startingBalanceMovement.Amount.Should().Be(startingBalance);
            startingBalanceMovement.Date.Should().Be(StartOfJulyMonthDate);
            startingBalanceMovement.AfterBalance.Should().Be(startingBalance);
            startingBalanceMovement.Reason.Should().BeNull();

            var revenueMovement = result.Results.Last();
            revenueMovement.Should().NotBeNull();
            revenueMovement.Amount.Should().Be(revenue);
            revenueMovement.Date.Should().Be(StartOfJulyMonthDate.AddDays(1));
            revenueMovement.AfterBalance.Should().Be(startingBalance + revenue);
            revenueMovement.Reason.Should().Be(AnyReason);
        }


        [Fact]
        public void CanForecastSingleMonthOneExpenseAndOneRevenueMovementsAreSortedByDate()
        {
            var startingBalance = 100;
            var revenue = 50;
            var expense = 20;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .WithOneTimeRevenue(revenue, StartOfJulyMonthDate.AddDays(1), AnyReason)
               .WithOneTimeExpense(expense, StartOfJulyMonthDate.AddDays(2), AnyReason)
               .Build();

            var result = financialForecaster.DetailedForecastUntillInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate);

            result.Should().NotBeNull();
            result.Results.Should().HaveCount(3);

            result.Results.Should().BeInAscendingOrder(x=>x.Date);
        }

        [Fact]
        public void SingleMonthWithMonthlyRevenueAreReturned()
        {
            var startingBalance = 100;
            var revenue = 50;

            var financialForecaster = ExpenseTrackerForcasterBuilder
               .New()
               .WithStartingBalanceAt(startingBalance, StartOfJulyMonthDate)
               .WithRecurrentMonthly(builder => builder
                    .WithRegularRevenue(revenue, StartOfJulyMonthDate.Day + 1, AnyReason))
               .Build();

            var result = financialForecaster.DetailedForecastUntillInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate);

            result.Should().NotBeNull();
            result.Results.Should().HaveCount(2);
            var startingBalanceMovement = result.Results.First();
            startingBalanceMovement.Should().NotBeNull();
            startingBalanceMovement.Amount.Should().Be(startingBalance);
            startingBalanceMovement.Date.Should().Be(StartOfJulyMonthDate);
            startingBalanceMovement.AfterBalance.Should().Be(startingBalance);
            startingBalanceMovement.Reason.Should().BeNull();

            var revenueMovement = result.Results.Last();
            revenueMovement.Should().NotBeNull();
            revenueMovement.Amount.Should().Be(revenue);
            revenueMovement.Date.Should().Be(StartOfJulyMonthDate.AddDays(1));
            revenueMovement.AfterBalance.Should().Be(startingBalance + revenue);
            revenueMovement.Reason.Should().Be(AnyReason);
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

            var result = financialForecaster.DetailedForecastUntillInclusive(StartOfJulyMonthDate, EndOfJulyMonthDate);

            result.Should().NotBeNull();

            result.Results.Should().HaveCount(2);
            var startingBalanceMovement = result.Results.First();
            startingBalanceMovement.Should().NotBeNull();
            startingBalanceMovement.Amount.Should().Be(startingBalance);
            startingBalanceMovement.Date.Should().Be(StartOfJulyMonthDate);
            startingBalanceMovement.AfterBalance.Should().Be(startingBalance);
            startingBalanceMovement.Reason.Should().BeNull();

            var expenseMovement = result.Results.Last();
            expenseMovement.Should().NotBeNull();
            expenseMovement.Amount.Should().Be(-expense);
            expenseMovement.Date.Should().Be(StartOfJulyMonthDate.AddDays(1));
            expenseMovement.AfterBalance.Should().Be(startingBalance - expense);
            expenseMovement.Reason.Should().Be(AnyReason);
        }
    }
}