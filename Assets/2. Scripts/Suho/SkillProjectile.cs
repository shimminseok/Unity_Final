using System;
using UnityEngine;

public class SkillProjectile : MonoBehaviour, IPoolObject
{
    [SerializeField] private string poolId;

    [SerializeField] private int poolSize;
    
    [SerializeField] private float projectileSpeed;
    
    private StatBaseSkillEffect effectData;
    
    private float smoothTime = 0.3f;
    private Vector3 startPosition = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private float _delta = 0;
    public GameObject GameObject => gameObject;
    public string     PoolID     => poolId;
    public int        PoolSize   => poolSize;

    public Unit Target;
    
    
    public StatBaseSkillEffect EffectData     => effectData;
    public float ProjectileSpeed => projectileSpeed;
    public Vector3 Direction => direction;
    public Vector3 StartPosition => startPosition;

    public bool isShooting = false;
    public ProjectileInterpolationMode mode;


    private void Update()
    {
        if (isShooting)
        {
            _delta += Time.deltaTime * ProjectileSpeed;
            ShootProjectile(mode,_delta);
        }
    }


    public void ShootProjectile(ProjectileInterpolationMode interpolationMode, float delta)
    {
        {
            switch (interpolationMode)
            {
                case ProjectileInterpolationMode.Linear:
                    transform.position += direction.normalized * delta;
                    break;

                case ProjectileInterpolationMode.Lerp:
                    transform.position = Vector3.Lerp(transform.position, direction, delta);
                    break;

                case ProjectileInterpolationMode.MoveTowards:
                    transform.position = Vector3.MoveTowards(transform.position, direction, delta);
                    break;

                case ProjectileInterpolationMode.SmoothDamp:
                    transform.position = Vector3.SmoothDamp(transform.position, direction, ref velocity, smoothTime);
                    break;

                case ProjectileInterpolationMode.Slerp:
                    Vector3 offset = Vector3.Slerp(startPosition, direction, delta);
                    transform.position = direction + offset;
                    break;
            }
        }
    }

    public void Initialize(StatBaseSkillEffect effect, Vector3 startPos,Vector3 dir, Unit target)
    {
        effectData = effect;
        startPosition = startPos;
        direction = dir;
        this.gameObject.transform.position = startPos;
        Target = target;
        OnSpawnFromPool();
    }

    public void OnSpawnFromPool()
    {
        _delta = 0;
        isShooting = true;
    }

    public void OnReturnToPool()
    {
        isShooting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Unit>() == Target)
        {
            effectData.AffectTargetWithSkill(Target);
            ObjectPoolManager.Instance.ReturnObject(gameObject);

        }
    }
}
