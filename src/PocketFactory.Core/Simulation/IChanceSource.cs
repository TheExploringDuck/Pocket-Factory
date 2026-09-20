namespace PocketFactory.Core.Simulation;

public interface IChanceSource
{
    bool Roll(double probability);
}
