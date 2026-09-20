using PocketFactory.Core.Simulation;

var tests = new EconomyRuleTests();
tests.PickaxeRulesMatchApprovedTable();
tests.DrillRulesMatchApprovedTable();
tests.ProductionLine1RulesMatchApprovedTable();
tests.SupervisorCapacityMatchesApprovedTable();
tests.ProductionLineUnlocksFollowApprovedGates();
tests.TbdResetRatesRemainUnset();

Console.WriteLine("PocketFactory.Core.Tests passed.");

internal sealed class EconomyRuleTests
{
    public void PickaxeRulesMatchApprovedTable()
    {
        var level1 = EconomyRules.GetPickaxeRule(1);
        AssertEqual(0.70d, level1.CycleTimeSeconds);
        AssertEqual(1.00d, level1.MetalPerAction);
        AssertEqual(0.00d, level1.NormalCurrencyDropChance);
        AssertEqual(0.02d, level1.PostResetCurrencyDropChance);

        var level10 = EconomyRules.GetPickaxeRule(10);
        AssertEqual(0.35d, level10.CycleTimeSeconds);
        AssertEqual(6.50d, level10.MetalPerAction);
        AssertEqual(0.20d, level10.PostResetCurrencyDropChance);
    }

    public void DrillRulesMatchApprovedTable()
    {
        var level1 = EconomyRules.GetDrillRule(1);
        AssertEqual(0.53d, level1.CycleTimeSeconds);
        AssertEqual(10.275d, level1.MetalPerAction);
        AssertEqual(3, level1.CurrencyPerDrop);
        AssertEqual(0.02d, level1.NormalCurrencyDropChance);
        AssertEqual(0.20d, level1.PostResetCurrencyDropChance);

        var level10 = EconomyRules.GetDrillRule(10);
        AssertEqual(0.25d, level10.CycleTimeSeconds);
        AssertEqual(100.00d, level10.MetalPerAction);
        AssertEqual(30, level10.CurrencyPerDrop);
        AssertEqual(0.20d, level10.NormalCurrencyDropChance);
    }

    public void ProductionLine1RulesMatchApprovedTable()
    {
        var level1 = EconomyRules.GetProductionLineLevelRule(1);
        AssertEqual(8, level1.SupportingDrillLevel);
        AssertEqual(80d, level1.BaseMetal);
        AssertEqual(0.005d, level1.IncreasePercent);
        AssertEqual(112d, level1.Output);
        AssertEqual(0.165d, level1.FirstRunCurrencyDropChance);
        AssertEqual(0.50d, level1.PostResetCurrencyDropChance);

        var level5 = EconomyRules.GetProductionLineLevelRule(5);
        AssertEqual(10, level5.SupportingDrillLevel);
        AssertEqual(600d, level5.Output);
        AssertEqual(0.58d, level5.PostResetCurrencyDropChance);
    }

    public void SupervisorCapacityMatchesApprovedTable()
    {
        var expected = new Dictionary<int, int>
        {
            [1] = 0,
            [2] = 1,
            [3] = 1,
            [4] = 2,
            [5] = 2,
            [6] = 3,
            [7] = 3,
            [8] = 4,
            [9] = 4,
            [10] = 5,
        };

        foreach (var pair in expected)
        {
            AssertEqual(pair.Value, EconomyRules.GetRequiredSupervisorsForLine(pair.Key));
        }
    }

    public void ProductionLineUnlocksFollowApprovedGates()
    {
        var state = GameState.NewRun();
        state.SetPickaxeLevel(10);
        state.SetDrillLevel(8);

        AssertTrue(FactoryProgression.CanBuildProductionLine(state, 1));
        AssertFalse(FactoryProgression.CanBuildProductionLine(state, 2));

        state.SetProductionLineLevel(1, 1);
        state.SetSupervisors(1);
        AssertTrue(FactoryProgression.CanBuildProductionLine(state, 2));

        state.SetProductionLineLevel(2, 1);
        AssertTrue(FactoryProgression.CanBuildProductionLine(state, 3));
        AssertFalse(FactoryProgression.CanBuildProductionLine(state, 4));
    }

    public void TbdResetRatesRemainUnset()
    {
        AssertNull(EconomyRules.GetDrillRule(8).PostResetCurrencyDropChance);
        AssertNull(EconomyRules.GetDrillRule(9).PostResetCurrencyDropChance);
        AssertNull(EconomyRules.GetDrillRule(10).PostResetCurrencyDropChance);
    }

    private static void AssertTrue(bool value)
    {
        if (!value)
        {
            throw new InvalidOperationException("Expected true.");
        }
    }

    private static void AssertFalse(bool value)
    {
        if (value)
        {
            throw new InvalidOperationException("Expected false.");
        }
    }

    private static void AssertNull(object? value)
    {
        if (value is not null)
        {
            throw new InvalidOperationException($"Expected null but got {value}.");
        }
    }

    private static void AssertEqual<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new InvalidOperationException($"Expected {expected} but got {actual}.");
        }
    }
}
