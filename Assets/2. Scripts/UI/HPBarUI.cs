using TMPro;
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

    [SerializeField] private TextMeshProUGUI speedText;

    public GameObject GameObject => gameObject;
    public string     PoolID     => poolId;
    public int        PoolSize   => poolSize;

    private IDamageable target;
    private Transform targetTransform;
    private Camera mainCamera;
    private float heightOffset;

    private StatManager statManager;
    private CalculatedStat speedStat;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void Initialize(IDamageable owner)
    {
        target = owner;
        OnSpawnFromPool();
        statManager = target.Collider.GetComponent<StatManager>();
        statManager.GetStat<ResourceStat>(StatType.CurHp).OnValueChanged += UpdateHealthBarWrapper;
        //speedStat = statManager.GetStat<CalculatedStat>(StatType.Speed);

        //speedStat.OnValueChanged += UpdateSpeedText;
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

    // 속도 스탯 받기
    private void UpdateSpeedText(float curSpeed)
    {
        speedText.text = $"{curSpeed:N0}";
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
        //speedStat.OnValueChanged -= UpdateSpeedText;
    }

    public void OnSpawnFromPool()
    {
        targetTransform = target.Collider.transform;
        heightOffset = target.Collider.bounds.size.y - 1.2f; // 위치 보정을 위해 임시로 -1.4f
        offset.x += 0.1f; // x도 위치보정 임시
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