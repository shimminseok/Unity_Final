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

    [Header ("감정 슬롯")]
    [SerializeField] private GameObject joySlot;
    [SerializeField] private GameObject angerSlot;
    [SerializeField] private GameObject depressionSlot;

    private TextMeshProUGUI joyText;
    private TextMeshProUGUI angerText;
    private TextMeshProUGUI depressionText;
    private Unit unit;

    public GameObject GameObject => gameObject;
    public string     PoolID     => poolId;
    public int        PoolSize   => poolSize;

    private IDamageable target;
    private Transform targetTransform;
    private Camera mainCamera;
    private float heightOffset;
    private int emotionCount = 0;

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
        speedStat = statManager.GetStat<CalculatedStat>(StatType.Speed);
        speedStat.OnValueChanged += UpdateSpeedText;
        UpdateSpeedText(speedStat.Value);


        unit = target.Collider.GetComponent<Unit>();

        // 감정 스택 텍스트
        joyText = joySlot.GetComponentInChildren<TextMeshProUGUI>();
        angerText = angerSlot.GetComponentInChildren<TextMeshProUGUI>();
        depressionText = depressionSlot.GetComponentInChildren<TextMeshProUGUI>();

        // 감정 관련 구독 및 초기화
        if (unit != null && unit.CurrentEmotion != null)
        {
            unit.EmotionChanged += UpdateEmotionSlot;
            unit.CurrentEmotion.StackChanged += OnEmotionStackChanged;
            UpdateEmotionSlot(unit.CurrentEmotion);
        }
    }

    public void UpdatePosion()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetTransform.position + offset);
        barRect.position = screenPos;
        barRect.localScale = Vector3.one; // Hp 바 사이즈 고정
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

    // 감정이 바뀔 때마다 호출
    private void UpdateEmotionSlot(BaseEmotion newEmotion)
    {
        joySlot.SetActive(false);
        angerSlot.SetActive(false);
        depressionSlot.SetActive(false);

        // 구독 이전 해제
        unit.CurrentEmotion.StackChanged -= OnEmotionStackChanged;

        // 새 감정 구독
        newEmotion.StackChanged += OnEmotionStackChanged;

        // 감정 타입에 따라 해당 슬롯만 활성화
        switch (newEmotion.EmotionType)
        {
            case EmotionType.Joy:
                joySlot.SetActive(true);
                joyText.text = newEmotion.Stack.ToString();
                break;
            case EmotionType.Anger:
                angerSlot.SetActive(true);
                angerText.text = newEmotion.Stack.ToString();
                break;
            case EmotionType.Depression:
                depressionSlot.SetActive(true);
                depressionText.text = newEmotion.Stack.ToString();
                break;
        }
    }

    // 감정 스택이 바뀔 때마다 호출
    private void OnEmotionStackChanged(int stack)
    {
        switch (unit.CurrentEmotion.EmotionType)
        {
            case EmotionType.Joy:
                joyText.text = stack.ToString();
                break;
            case EmotionType.Anger:
                angerText.text = stack.ToString();
                break;
            case EmotionType.Depression:
                depressionText.text = stack.ToString();
                break;
        }
    }

    public void UnLink()
    {
        if (unit != null)
        {
            unit.EmotionChanged -= UpdateEmotionSlot;
            unit.CurrentEmotion.StackChanged -= OnEmotionStackChanged;
        }

        HealthBarManager.Instance.DespawnHealthBar(this);
        speedStat.OnValueChanged -= UpdateSpeedText;
    }

    public void OnSpawnFromPool()
    {
        targetTransform = target.Collider.transform;
        heightOffset = target.Collider.bounds.size.y - 1.2f; // 위치 보정을 위해 임시로 -1.4f
        offset.x += 0.1f;                                    // x도 위치보정 임시
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