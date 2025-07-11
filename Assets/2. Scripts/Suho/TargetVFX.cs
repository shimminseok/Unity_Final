using System.Collections;
using UnityEngine;

public class TargetVFX : PoolableVFX
{
    public IDamageable Target;
    public TargetVFX(VFXData vfxData, IDamageable target) 
    {
        this.VFXData = vfxData;
        this.Target = target;
    }

    public override IEnumerator PlayVFX()
    {
        particle.Play();
        yield return null;
    }

    public override void AdjustTransform()
    {
        transform.position = Target.Collider.bounds.center;
        transform.rotation = Target.Collider.transform.rotation;
        if (VFXData.isParent == true)
        {
            transform.parent = Target.Collider.transform;
        }
        transform.localPosition = VFXData.LocalPosition;
        transform.localRotation = Quaternion.Euler(VFXData.LocalRotation);
        transform.localScale = VFXData.LocalScale;
       
    }
}