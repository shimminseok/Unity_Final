using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class HPBarUI : MonoBehaviour, IPoolObject
{
    [SerializeField] private string poolId;

    [SerializeField] private int poolSize;

    [SerializeField] RectTransform barRect;

    [SerializeField] Image fillImage;

    [SerializeField] Vector3 offset;

    public GameObject GameObject => gameObject;
    public string     PoolID     => poolId;
    public int        PoolSize   => poolSize;

    private IDamageable target;
    private Transform targetTransform;
    private Camera mainCamera;
    private float heightOffset;

    StatManager statManager;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void Initialize(IDamageable owner)
    {
        target = owner;
        OnSpawnFromPool();
        statManager = target.Collider.GetComponent<StatManager>();
    }

    public void UpdatePosion()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetTransform.position + offset);
        barRect.position = screenPos;
    }

    public void UpdateHealthBarWrapper(float cur)
    {
        UpdateFill(cur, statManager.GetValue(StatType.MaxHp));
    }

    /// <summary>
    /// FillAmount를 업데이트 시켜주는 메서드
    /// </summary>
    /// <param name="cur">현재 값</param>
    /// <param name="max">맥스 값</param>
    private void UpdateFill(float cur, float max)
    {
        fillImage.fillAmount = Mathf.Clamp01(cur / max);
    }

    public void UnLink()
    {
        HealthBarManager.Instance.DespawnHealthBar(this);
    }

    public void OnSpawnFromPool()
    {
        targetTransform = target.Collider.transform;
        heightOffset = target.Collider.bounds.size.y;
        offset.y += heightOffset;
        transform.SetParent(HealthBarManager.Instance.hpBarCanvas.transform);
    }

    public void OnReturnToPool()
    {
        target = null;
        fillImage.fillAmount = 1f;
        barRect.position = Vector3.zero;
    }
}