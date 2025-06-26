using System;
using UnityEngine;

public class SkillProjectile : MonoBehaviour, IPoolObject
{
    [SerializeField] private string poolId;

    [SerializeField] private int poolSize;
    
    private float projectileSpeed;
    
    private Skill skill;
    
    private float smoothTime = 0.3f;
    private Vector3 startPosition = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    public GameObject GameObject => gameObject;
    public string     PoolID     => poolId;
    public int        PoolSize   => poolSize;
    
    
    public Skill Skill     => skill;
    public float ProjectileSpeed => projectileSpeed;
    public Vector3 Direction => direction;
    public Vector3 StartPosition => startPosition;

    public bool isShooting = false;
    public ProjectileInterpolationMode mode;


    private void Update()
    {
        if (isShooting)
        {
            ShootProjectile(mode);
        }
    }


    public void ShootProjectile(ProjectileInterpolationMode interpolationMode)
    {
        {
            float delta = projectileSpeed * Time.deltaTime;

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
                    Vector3 offset = Vector3.Slerp(Vector3.zero, direction.normalized * 100f, delta);
                    transform.position = direction + offset;
                    break;
            }
        }
    }

    public void Initialize(Skill skillInfo,float speed, Vector3 startPos,Vector3 dir, ProjectileInterpolationMode interpolationMode)
    {
        skill = skillInfo;
        projectileSpeed = speed;
        startPosition = startPos;
        direction = dir;
        mode = interpolationMode;
        OnSpawnFromPool();
    }

    public void OnSpawnFromPool()
    {
        isShooting = true;
    }

    public void OnReturnToPool()
    {
        isShooting = false;
    }
}
