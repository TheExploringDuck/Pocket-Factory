namespace PocketFactory.Core.Simulation;

public static class EconomyRules
{
    public const int MaxPickaxeLevel = 10;
    public const int MaxDrillLevel = 10;
    public const int MaxStandardFactoryProductionLines = 10;
    public const int MaxStandardFactorySupervisors = 5;
    public const double RewardedAdProductionMultiplier = 2d;
    public static readonly TimeSpan RewardedAdProductionMultiplierDuration = TimeSpan.FromMinutes(5);

    private static readonly MiningLevelRule[] PickaxeRules =
    [
        new(1, 0.70d, 1.00d, null, 0.00d, 0.02d),
        new(2, 0.65d, 1.90d, null, 0.00d, 0.04d),
        new(3, 0.60d, 2.70d, null, 0.00d, 0.06d),
        new(4, 0.55d, 3.40d, null, 0.00d, 0.08d),
        new(5, 0.50d, 4.00d, null, 0.00d, 0.10d),
        new(6, 0.45d, 4.50d, null, 0.00d, 0.12d),
        new(7, 0.40d, 4.90d, null, 0.00d, 0.14d),
        new(8, 0.35d, 5.20d, null, 0.00d, 0.16d),
        new(9, 0.35d, 5.85d, null, 0.00d, 0.18d),
        new(10, 0.35d, 6.50d, null, 0.00d, 0.20d),
    ];

    private static readonly MiningLevelRule[] DrillRules =
    [
        new(1, 0.53d, 10.275d, 3, 0.02d, 0.20d),
        new(2, 0.50d, 20.50d, 6, 0.04d, 0.22d),
        new(3, 0.45d, 30.60d, 9, 0.06d, 0.25d),
        new(4, 0.43d, 40.72d, 12, 0.08d, 0.29d),
        new(5, 0.40d, 50.75d, 15, 0.10d, 0.34d),
        new(6, 0.35d, 60.60d, 18, 0.12d, 0.40d),
        new(7, 0.30d, 70.35d, 21, 0.14d, 0.47d),
        new(8, 0.25d, 80.00d, 24, 0.16d, null),
        new(9, 0.25d, 90.00d, 27, 0.18d, null),
        new(10, 0.25d, 100.00d, 30, 0.20d, null),
    ];

    private static readonly ProductionLineLevelRule[] ProductionLineRules =
    [
        new(1, 8, 80d, 0.005d, 112d, 0.165d, 0.50d),
        new(2, 8, 80d, 0.010d, 144d, 0.170d, 0.52d),
        new(3, 9, 90d, 0.030d, 333d, 0.190d, 0.54d),
        new(4, 9, 90d, 0.040d, 414d, 0.220d, 0.56d),
        new(5, 10, 100d, 0.050d, 600d, 0.250d, 0.58d),
    ];

    private static readonly double[] ProductionLine2FirstRunCurrencyDropChances =
    [
        0.22d,
        0.24d,
        0.26d,
        0.28d,
        0.30d,
    ];

    public static MiningLevelRule GetPickaxeRule(int level) =>
        PickaxeRules.Single(rule => rule.Level == level);

    public static MiningLevelRule GetDrillRule(int level) =>
        DrillRules.Single(rule => rule.Level == level);

    public static ProductionLineLevelRule GetProductionLineLevelRule(int level) =>
        ProductionLineRules.Single(rule => rule.Level == level);

    public static double GetProductionLine2FirstRunCurrencyDropChance(int level) =>
        ProductionLine2FirstRunCurrencyDropChances[level - 1];

    public static int GetRequiredSupervisorsForLine(int lineNumber)
    {
        if (lineNumber < 1 || lineNumber > MaxStandardFactoryProductionLines)
        {
            throw new ArgumentOutOfRangeException(nameof(lineNumber));
        }

        return lineNumber / 2;
    }
}
