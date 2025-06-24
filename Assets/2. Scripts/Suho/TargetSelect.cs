using System.Collections.Generic;
using UnityEngine;

public class TargetSelect
{
    public List<IDamageable> FindTargets(IDamageable mainTarget, SelectTargetType type)
    {
        switch (type)
        {
            case SelectTargetType.Single:
                List<IDamageable> targets = new List<IDamageable>();
                return targets;
            default:
                return null;
                break;
        }

    }
    
}

