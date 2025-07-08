using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UnitLevelUpPanel : MonoBehaviour
{
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    [SerializeField] private TextMeshProUGUI maxLevelTxt;

    [SerializeField] private Image requiredDupeCountFill;
    [SerializeField] private TextMeshProUGUI requiredDupeCountTxt;
    private Vector2 onScreenScale;
    private Vector2 offScreenPos;


    private EntryDeckData currentPlayerUnitData;

    private void Awake()
    {
        onScreenScale = panelRect.localScale;
        panelRect.localScale = Vector3.zero;

        gameObject.SetActive(false);
    }

    private void UpdateDupeCount()
    {
        int requiredDupeCount = Define.DupeCountByTranscend[currentPlayerUnitData.TranscendLevel];
        int currentDupeCount  = currentPlayerUnitData.Amount;
        requiredDupeCountFill.fillAmount = (float)currentDupeCount / requiredDupeCount;
        requiredDupeCountTxt.text = $"{currentDupeCount} / {requiredDupeCount}";

        maxLevelTxt.text = $"{currentPlayerUnitData.MaxLevel}";
    }

    private void UpdateLevelText()
    {
        currentLevelTxt.text = $"{currentPlayerUnitData.Level}";
    }

    public void OpenPanel(EntryDeckData unitData)
    {
        if (currentPlayerUnitData != null)
        {
            currentPlayerUnitData.OnLevelUp -= UpdateLevelText;
            currentPlayerUnitData.OnTranscendChanged -= UpdateDupeCount;
        }

        currentPlayerUnitData = unitData;
        gameObject.SetActive(true);
        panelRect.DOScale(onScreenScale, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            panelRect.localScale = onScreenScale;
        });

        currentPlayerUnitData.OnLevelUp += UpdateLevelText;
        currentPlayerUnitData.OnTranscendChanged += UpdateDupeCount;
        UpdateDupeCount();
        UpdateLevelText();
    }

    public void ClosePanel()
    {
        DOTween.KillAll();
        panelRect.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            if (currentPlayerUnitData != null)
            {
                currentPlayerUnitData.OnLevelUp -= UpdateLevelText;
                currentPlayerUnitData.OnTranscendChanged -= UpdateDupeCount;
            }

            gameObject.SetActive(false);
            currentPlayerUnitData = null;
        });
    }

    public void OnClickTranscend()
    {
        currentPlayerUnitData.Transcend(out bool result);
        if (!result)
        {
            //초월 실패
        }
    }

    public void OnClickLevelUp()
    {
        currentPlayerUnitData.LevelUp(out bool result);
        if (!result)
        {
            //레벨업 실패
        }
    }
}