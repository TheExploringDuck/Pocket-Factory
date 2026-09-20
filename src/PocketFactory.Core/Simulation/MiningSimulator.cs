namespace PocketFactory.Core.Simulation;

public sealed class MiningSimulator(IChanceSource chanceSource)
{
    public MiningResult MineWithPickaxe(GameState state)
    {
        var rule = EconomyRules.GetPickaxeRule(state.PickaxeLevel);
        var currency = RollCurrency(state, rule);
        state.AddMetal((decimal)rule.MetalPerAction);

        if (currency > 0m)
        {
            state.AddCurrency(currency);
        }

        return new MiningResult((decimal)rule.MetalPerAction, currency, rule.CycleTimeSeconds);
    }

    public MiningResult MineWithDrill(GameState state)
    {
        if (!FactoryProgression.IsDrillUnlocked(state) || state.DrillLevel < 1)
        {
            throw new InvalidOperationException("The Drill is not unlocked or leveled yet.");
        }

        var rule = EconomyRules.GetDrillRule(state.DrillLevel);
        var currency = RollCurrency(state, rule);
        state.AddMetal((decimal)rule.MetalPerAction);

        if (currency > 0m)
        {
            state.AddCurrency(currency);
        }

        return new MiningResult((decimal)rule.MetalPerAction, currency, rule.CycleTimeSeconds);
    }

    private decimal RollCurrency(GameState state, MiningLevelRule rule)
    {
        var chance = state.HasActivatedResetRates
            ? rule.PostResetCurrencyDropChance
            : rule.NormalCurrencyDropChance;

        if (chance is null || chance <= 0d || rule.CurrencyPerDrop is null)
        {
            return 0m;
        }

        return chanceSource.Roll(chance.Value) ? rule.CurrencyPerDrop.Value : 0m;
    }
}
