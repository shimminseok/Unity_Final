using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText; // 캐릭터 이름
    [SerializeField] private Image iconImage;   // 캐릭터 이미지
    [SerializeField] private Button button;     // 클릭 버튼

    private PlayerUnitSO characterSO;
    private Action<PlayerUnitSO> onClickAction;

    // 버튼에 데이터를 집어넣는 초기화 작업
    public void Initialize(PlayerUnitSO data, Action<PlayerUnitSO> onClick)
    {
        characterSO = data;
        onClickAction = onClick;

        // UI 이미지와 텍스트 교체
        iconImage.sprite = data.UnitIcon;
        nameText.text = data.UnitName;

        // 버튼 클릭 이벤트
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
    }

    // 버튼 클릭 시 등록된 콜백 실행
    private void OnClicked()
    {
        onClickAction?.Invoke(characterSO);
    }
}
