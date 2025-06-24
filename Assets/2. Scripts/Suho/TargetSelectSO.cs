using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSelectSO : ScriptableObject
{
    public abstract List<IDamageable> FindTargets(IDamageable mainTarget);
    
}

[CreateAssetMenu(fileName = "TargetSelectData", menuName = "ScriptableObject/TargetSelectSO/Single")]
public class SingleTargetSelectSO : TargetSelectSO
{
    public override List<IDamageable> FindTargets(IDamageable mainTarget)
    {
        List<IDamageable> targets = new List<IDamageable>();
        targets.Add(mainTarget);
        return targets;
    }
}

