using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UnitLevelUpPanel : MonoBehaviour
{
    [SerializeField] private PlayerUnitIncreaseSo increaseSo;
    [SerializeField] private GameObject contents;
    [SerializeField] private CanvasGroup panelRect;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    [SerializeField] private TextMeshProUGUI maxLevelTxt;

    [SerializeField] private Image requiredDupeCountFill;
    [SerializeField] private TextMeshProUGUI requiredDupeCountTxt;
    [SerializeField] private List<IncreaseStatSlot> increaseStatSlots;

    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private TextMeshProUGUI requireLevelUpGoldTxt;
    [SerializeField] private TextMeshProUGUI requireTranscendGoldTxt;

    private Vector2 onScreenScale;
    private Vector2 offScreenPos;


    private EntryDeckData currentPlayerUnitData;

    private readonly Dictionary<StatType, IncreaseStatSlot> increaseStatSlotDic = new();


    private readonly int requireLevelUpGold = Define.RequierUnitLevelUpGold;
    private readonly int requireTranscendGoldGold = Define.RequierUnitTranscendGold;

    private void Awake()
    {
        contents.SetActive(false);

        InitializeIncreaseStatSlotDic();
    }

    private void InitializeIncreaseStatSlotDic()
    {
        increaseStatSlotDic.Clear();

        foreach (IncreaseStatSlot slot in increaseStatSlots)
        {
            if (!increaseStatSlotDic.TryAdd(slot.StatType, slot))
            {
                Debug.LogWarning($"Duplicate increase slot for StatType: {slot.StatType}");
            }
        }
    }

    private void UpdateDupeCount()
    {
        int requiredDupeCount = Define.DupeCountByTranscend[currentPlayerUnitData.TranscendLevel];
        int currentDupeCount  = currentPlayerUnitData.Amount;
        requiredDupeCountFill.fillAmount = (float)currentDupeCount / requiredDupeCount;
        requiredDupeCountTxt.text = $"{currentDupeCount} / {requiredDupeCount}";

        maxLevelTxt.text = $"{currentPlayerUnitData.MaxLevel}";

        requireTranscendGoldTxt.text = AccountManager.Instance.Gold >= requireTranscendGoldGold ? $"<color=#ffffffff>{requireTranscendGoldGold:N0}G</color>" : $"<color=#ff0000ff>{requireTranscendGoldGold:N0}G</color>";
    }

    private void UpdateLevelText()
    {
        currentLevelTxt.text = $"{currentPlayerUnitData.Level}";
        int    level  = currentPlayerUnitData.Level;
        UnitSO unitSo = currentPlayerUnitData.CharacterSo;

        foreach (StatData increaseStat in increaseSo.Stats)
        {
            StatType statType = increaseStat.StatType;

            if (!increaseStatSlotDic.TryGetValue(statType, out IncreaseStatSlot slot))
            {
                continue;
            }

            StatData baseStat  = unitSo.GetStat(statType);
            float    baseValue = baseStat != null ? baseStat.Value : 0;

            float curValue  = baseValue + (increaseStat.Value * (level - 1));
            float nextValue = baseValue + (increaseStat.Value * level);

            slot.SetStatSlot(curValue, nextValue);
        }

        requireLevelUpGoldTxt.text = AccountManager.Instance.Gold >= requireLevelUpGold ? $"<color=#ffffffff>{requireLevelUpGold:N0}G</color>" : $"<color=#ff0000ff>{requireLevelUpGold:N0}G</color>";
    }

    public void OpenPanel(EntryDeckData unitData)
    {
        if (currentPlayerUnitData != null)
        {
            currentPlayerUnitData.OnLevelUp -= UpdateLevelText;
            currentPlayerUnitData.OnTranscendChanged -= UpdateDupeCount;
        }

        currentPlayerUnitData = unitData;
        contents.SetActive(true);

        panelRect.alpha = 0;
        panelRect.DOFade(1f, fadeInDuration).SetEase(Ease.InOutSine);

        currentPlayerUnitData.OnLevelUp += UpdateLevelText;
        currentPlayerUnitData.OnTranscendChanged += UpdateDupeCount;
        UpdateDupeCount();
        UpdateLevelText();
    }

    public void ClosePanel()
    {
        DOTween.KillAll();
        panelRect.DOFade(0f, fadeOutDuration).SetEase(Ease.OutSine).OnComplete(() =>
        {
            if (currentPlayerUnitData != null)
            {
                currentPlayerUnitData.OnLevelUp -= UpdateLevelText;
                currentPlayerUnitData.OnTranscendChanged -= UpdateDupeCount;
            }

            contents.SetActive(false);
            currentPlayerUnitData = null;
        });
    }

    public void OnClickTranscend()
    {
        currentPlayerUnitData.Transcend(out bool result);
    }

    public void OnClickLevelUp()
    {
        currentPlayerUnitData.LevelUp(out bool result);
    }
}