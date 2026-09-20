namespace PocketFactory.Core.Simulation;

public static class FactoryProgression
{
    public static bool IsDrillUnlocked(GameState state) =>
        state.PickaxeLevel >= EconomyRules.MaxPickaxeLevel;

    public static bool IsProductionLine1Unlocked(GameState state) =>
        state.DrillLevel >= 8;

    public static bool CanBuildProductionLine(GameState state, int lineNumber)
    {
        if (lineNumber < 1 || lineNumber > EconomyRules.MaxStandardFactoryProductionLines)
        {
            throw new ArgumentOutOfRangeException(nameof(lineNumber));
        }

        if (lineNumber == 1)
        {
            return IsProductionLine1Unlocked(state);
        }

        if (state.Supervisors < EconomyRules.GetRequiredSupervisorsForLine(lineNumber))
        {
            return false;
        }

        for (var previousLine = 1; previousLine < lineNumber; previousLine++)
        {
            if (!state.ProductionLineLevels.ContainsKey(previousLine))
            {
                return false;
            }
        }

        return true;
    }

    public static double GetProductionLineCurrencyDropChance(GameState state, int lineNumber)
    {
        if (!state.ProductionLineLevels.TryGetValue(lineNumber, out var level))
        {
            throw new InvalidOperationException($"Production Line {lineNumber} has not been built.");
        }

        var levelRule = EconomyRules.GetProductionLineLevelRule(level);

        if (state.HasActivatedResetRates)
        {
            return levelRule.PostResetCurrencyDropChance;
        }

        return lineNumber == 2
            ? EconomyRules.GetProductionLine2FirstRunCurrencyDropChance(level)
            : levelRule.FirstRunCurrencyDropChance;
    }
}
