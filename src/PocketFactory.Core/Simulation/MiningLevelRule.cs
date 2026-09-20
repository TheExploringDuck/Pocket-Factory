namespace PocketFactory.Core.Simulation;

public sealed record MiningLevelRule(
    int Level,
    double CycleTimeSeconds,
    double MetalPerAction,
    int? CurrencyPerDrop,
    double NormalCurrencyDropChance,
    double? PostResetCurrencyDropChance)
{
    public double ActionsPerSecond => 1d / CycleTimeSeconds;

    public double MetalPerSecond => MetalPerAction / CycleTimeSeconds;
}
