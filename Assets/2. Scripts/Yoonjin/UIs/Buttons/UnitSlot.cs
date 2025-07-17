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
    [SerializeField] private Button button;   // 클릭 버튼


    //선택된 유닛인지
    private bool isSelected;
    private EntryDeckData selectedUnit;

    // 버튼에 데이터를 집어넣는 초기화 작업

    public event Action<EntryDeckData> OnClickSlot;

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

    // 버튼 클릭
    public void OnClicked()
    {
        OnClickSlot?.Invoke(selectedUnit);
    }
}