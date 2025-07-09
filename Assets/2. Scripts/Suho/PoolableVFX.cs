using UnityEngine;

public class PoolableVFX : MonoBehaviour, IPoolObject
{
    private string poolId;
    private int poolSize;
    private ParticleSystem particle;
    public GameObject GameObject => gameObject;
    public string PoolID => poolId;
    public int PoolSize => poolSize;

    public IAttackable Attacker;
    public IDamageable Target;

    public PoolableVFX(VFXData vfxData)
    {
        
    }
    
    public void OnSpawnFromPool()
    {
    }

    public void OnReturnToPool()
    {
    }
}
