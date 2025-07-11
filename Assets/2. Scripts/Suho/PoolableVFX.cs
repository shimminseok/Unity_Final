using System;
using System.Collections;
using UnityEngine;

public abstract class PoolableVFX : MonoBehaviour, IPoolObject
{
    private string poolId;
    private int poolSize;
    protected ParticleSystem particle;
    public GameObject GameObject => gameObject;
    public string PoolID => poolId;
    public int PoolSize => poolSize;
    protected VFXData VFXData;
    

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public abstract IEnumerator PlayVFX();

    public abstract void AdjustTransform();
    public virtual void SetData(VFXData data)
    {
        VFXData = data;
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
