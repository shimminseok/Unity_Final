using System;
using System.Collections;
using UnityEngine;

public class PoolableVFX : MonoBehaviour, IPoolObject
{
    [SerializeField]private string poolId;
    [SerializeField]private int poolSize;
    protected ParticleSystem particle;
    public GameObject GameObject => gameObject;
    public string PoolID => poolId;
    public int PoolSize => poolSize;
    protected VFXData VFXData;

    public IDamageable Target { get; set; }
    public IAttackable Attacker { get; set; }
    
    public Unit VFXTarget { get; set; }

    

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public IEnumerator PlayVFX()
    {
        particle.Play();
        yield return new WaitWhile(() => particle.IsAlive(true));
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }

    public void AdjustTransform()
    {
        transform.position = VFXTarget.Collider.bounds.center;
        transform.rotation = VFXTarget.Collider.transform.rotation;
        if (VFXData.isParent == true)
        {
            transform.parent = VFXTarget.Collider.transform;
        }
        transform.localPosition = VFXData.LocalPosition;
        transform.localRotation = Quaternion.Euler(VFXData.LocalRotation);
        transform.localScale = VFXData.LocalScale;
    }
    public void SetData(VFXData data)
    {
        VFXData = data;
        Attacker = data.Attacker;
        Target = data.Target;
        switch (VFXData.reference)
        {
            case VFXSpawnReference.Caster:
                VFXTarget = Attacker as Unit;
                break;
            case VFXSpawnReference.Target:
                VFXTarget = Target as Unit;
                break;
                default: break;
        }
       
    }
    public void OnSpawnFromPool()
    {
        AdjustTransform();
        StartCoroutine(PlayVFX());
    }

    public void OnReturnToPool()
    {
    }


}
