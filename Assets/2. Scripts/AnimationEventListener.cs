using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    private Unit owner;

    public void Initialize(Unit unit)
    {
        owner = unit;
    }

    public void Attack()
    {
        owner.Attack();
    }

    private void UseSkill()
    {
        owner.UseSkill();
    }
}