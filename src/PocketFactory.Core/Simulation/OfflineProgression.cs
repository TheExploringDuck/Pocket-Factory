namespace PocketFactory.Core.Simulation;

/// <summary>
/// Applies deterministic, bounded offline drill production without trusting invalid clock values.
/// </summary>
public static class OfflineProgression
{
    public static OfflineProgressionResult ApplyDrillProduction(
        GameState state,
        TimeSpan elapsed,
        TimeSpan maximumDuration)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (maximumDuration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumDuration));
        }

        if (elapsed <= TimeSpan.Zero || !FactoryProgression.IsDrillUnlocked(state) || state.DrillLevel < 1)
        {
            return OfflineProgressionResult.Empty;
        }

        var appliedDuration = elapsed > maximumDuration ? maximumDuration : elapsed;
        var rule = EconomyRules.GetDrillRule(state.DrillLevel);
        var completedCycles = (long)Math.Floor(appliedDuration.TotalSeconds / rule.CycleTimeSeconds);
        if (completedCycles <= 0)
        {
            return new OfflineProgressionResult(appliedDuration, 0, 0m, 0m, elapsed > maximumDuration);
        }

        var metalGained = completedCycles * (decimal)rule.MetalPerAction;
        var currencyGained = rule.CurrencyPerDrop is null
            ? 0m
            : completedCycles * (decimal)rule.NormalCurrencyDropChance * rule.CurrencyPerDrop.Value;

        state.AddMetal(metalGained);
        state.AddCurrency(currencyGained);
        return new OfflineProgressionResult(appliedDuration, completedCycles, metalGained, currencyGained, elapsed > maximumDuration);
    }
}

public sealed record OfflineProgressionResult(
    TimeSpan AppliedDuration,
    long CompletedCycles,
    decimal MetalGained,
    decimal CurrencyGained,
    bool WasCapped)
{
    public static OfflineProgressionResult Empty { get; } = new(TimeSpan.Zero, 0, 0m, 0m, false);
}
