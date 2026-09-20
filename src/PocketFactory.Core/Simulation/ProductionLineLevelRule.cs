namespace PocketFactory.Core.Simulation;

public sealed record ProductionLineLevelRule(
    int Level,
    int SupportingDrillLevel,
    double BaseMetal,
    double IncreasePercent,
    double Output,
    double FirstRunCurrencyDropChance,
    double PostResetCurrencyDropChance);
