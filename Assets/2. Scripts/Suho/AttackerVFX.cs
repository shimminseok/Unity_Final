using System;
using System.Collections;
using UnityEngine;

public class AttackerVFX : PoolableVFX
{
    public IAttackable Attacker;

    public override IEnumerator PlayVFX()
    {
        particle.Play();
        yield return new WaitWhile(() => particle.IsAlive(true));
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }

    public override void SetData(VFXData data)
    {
        base.SetData(data);
        Attacker = VFXData.Attacker;
    }

    public override void AdjustTransform()
    {
       transform.position = Attacker.Collider.bounds.center;
       transform.rotation = Attacker.Collider.transform.rotation;
       if (VFXData.isParent == true)
       {
           transform.parent = Attacker.Collider.transform;
       }
       transform.localPosition = VFXData.LocalPosition;
       transform.localRotation = Quaternion.Euler(VFXData.LocalRotation);
       transform.localScale = VFXData.LocalScale;
       
    }
}
