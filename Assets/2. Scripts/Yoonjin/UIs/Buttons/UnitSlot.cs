using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText; // 캐릭터 이름
    [SerializeField] private TextMeshProUGUI unitLevel;
    [SerializeField] private List<GameObject> unitTierStar;
    [SerializeField] private GameObject competedMarker;
    [SerializeField] private Image iconImage; // 캐릭터 이미지
    [SerializeField] private GameObject selectedNotiImg;

    private bool isSelected;
    private EntryDeckData selectedUnit;

    // 버튼에 데이터를 집어넣는 초기화 작업

    public event Action<EntryDeckData> OnClickSlot;

    private UIDeckBuilding uiDeckBuilding => UIManager.Instance.GetUIComponent<UIDeckBuilding>();

    public void Initialize(EntryDeckData data)
    {
        selectedUnit = data;
        PlayerUnitSO characterSo = data.CharacterSo;

        // UI 이미지와 텍스트 교체
        iconImage.sprite = characterSo.UnitIcon;
        nameText.text = characterSo.UnitName;
        unitLevel.text = $"Lv.{data.Level}";

        SetCompetedMarker(data.IsCompeted);
        for (int i = 0; i < unitTierStar.Count; i++)
        {
            unitTierStar[i].SetActive(i <= (int)characterSo.Tier);
        }
    }

    public void SetCompetedMarker(bool isCompeted)
    {
        competedMarker.SetActive(isCompeted);
    }

    public void SetSelectedMarker(bool isSelected)
    {
        selectedNotiImg.SetActive(isSelected);
    }

    public void Deselect()
    {
        isSelected = false;
        SetSelectedMarker(false);
    }

    public void OnClicked()
    {
        if (!isSelected)
        {
            isSelected = true;
            uiDeckBuilding.SetSelectedUnitSlot(this);
        }
        else
        {
            OnClickSlot?.Invoke(selectedUnit);
        }

        SetCompetedMarker(selectedUnit.IsCompeted);
        SetSelectedMarker(isSelected);
    }
}