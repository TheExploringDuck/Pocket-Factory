namespace PocketFactory.Core.Simulation;

public sealed class GameState
{
    private readonly SortedDictionary<int, int> productionLineLevels = [];

    public decimal Metal { get; private set; }

    public decimal Currency { get; private set; }

    public int PickaxeLevel { get; private set; } = 1;

    public int DrillLevel { get; private set; }

    public int Supervisors { get; private set; }

    public bool HasActivatedResetRates { get; private set; }

    public IReadOnlyDictionary<int, int> ProductionLineLevels => productionLineLevels;

    public static GameState NewRun() => new();

    public void AddMetal(decimal amount)
    {
        if (amount < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Metal cannot be added as a negative value.");
        }

        Metal += amount;
    }

    public void AddCurrency(decimal amount)
    {
        if (amount < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Currency cannot be added as a negative value.");
        }

        Currency += amount;
    }

    public void SetPickaxeLevel(int level)
    {
        if (level < 1 || level > EconomyRules.MaxPickaxeLevel)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }

        PickaxeLevel = level;
    }

    public void SetDrillLevel(int level)
    {
        if (level < 0 || level > EconomyRules.MaxDrillLevel)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }

        DrillLevel = level;
    }

    public void SetSupervisors(int count)
    {
        if (count < 0 || count > EconomyRules.MaxStandardFactorySupervisors)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        Supervisors = count;
    }

    public void ActivateResetRates() => HasActivatedResetRates = true;

    public void SetProductionLineLevel(int lineNumber, int level)
    {
        if (lineNumber < 1 || lineNumber > EconomyRules.MaxStandardFactoryProductionLines)
        {
            throw new ArgumentOutOfRangeException(nameof(lineNumber));
        }

        if (level < 1 || level > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }

        productionLineLevels[lineNumber] = level;
    }
}
