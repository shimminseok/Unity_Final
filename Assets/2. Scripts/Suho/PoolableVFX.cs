using System;
using System.Collections;
using UnityEngine;

public class PoolableVFX : MonoBehaviour, IPoolObject
{
    [SerializeField]private string poolId;
    [SerializeField]private int poolSize;
    public ParticleSystem particle;
    public GameObject GameObject => gameObject;
    public string PoolID => poolId;
    public int PoolSize => poolSize;
    protected VFXData VFXData;
    public IEffectProvider VFXTarget { get; set; }
    
    public Action OnTrigger { get; set; }


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
    public void SetData(VFXData data, IEffectProvider effectProvider)
    {
        VFXData = data;
        VFXTarget = effectProvider;
    }
    
    public void SetData(VFXData data,IEffectProvider effectProvider, Action trigger)
    {
        VFXData = data;
        VFXTarget = effectProvider;
        
        if (trigger != null)
        {
            trigger += OnSpawnFromPool;
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
