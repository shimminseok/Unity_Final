using System.Collections.Generic;
using UnityEngine;

public class GachaManager<T>
{
    private readonly IGachaStrategy<T> strategy;

    public GachaManager(IGachaStrategy<T> gachaStrategy)
    {
        strategy = gachaStrategy;
    }

    public T Draw(List<T> candidates, Dictionary<Tier, float> tierRates)
    {
        return strategy.Pull(candidates, tierRates);
    }
}
