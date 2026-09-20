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

    public GameStateSnapshot ToSnapshot() => new(
        Metal,
        Currency,
        PickaxeLevel,
        DrillLevel,
        Supervisors,
        HasActivatedResetRates,
        productionLineLevels
            .Select(pair => new ProductionLineSnapshot(pair.Key, pair.Value))
            .ToArray());

    public static GameState FromSnapshot(GameStateSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        if (snapshot.Metal < 0m || snapshot.Currency < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(snapshot), "Saved resources cannot be negative.");
        }

        var state = NewRun();
        state.SetPickaxeLevel(snapshot.PickaxeLevel);
        state.SetDrillLevel(snapshot.DrillLevel);
        state.SetSupervisors(snapshot.Supervisors);
        state.AddMetal(snapshot.Metal);
        state.AddCurrency(snapshot.Currency);

        if (snapshot.HasActivatedResetRates)
        {
            state.ActivateResetRates();
        }

        foreach (var line in snapshot.ProductionLines ?? [])
        {
            state.SetProductionLineLevel(line.LineNumber, line.Level);
        }

        return state;
    }

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

public sealed record GameStateSnapshot(
    decimal Metal,
    decimal Currency,
    int PickaxeLevel,
    int DrillLevel,
    int Supervisors,
    bool HasActivatedResetRates,
    IReadOnlyList<ProductionLineSnapshot>? ProductionLines);

public sealed record ProductionLineSnapshot(int LineNumber, int Level);
