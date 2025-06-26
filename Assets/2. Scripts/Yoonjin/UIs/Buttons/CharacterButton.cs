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

    // 보유 중 영역에 위치한 버튼인지
    private bool isSelected;

    // 버튼 클릭 후 UI 갱신을 위한 콜백
    private Action<PlayerUnitSO, bool> onAfterClick;

    // 버튼에 데이터를 집어넣는 초기화 작업
    public void Initialize(PlayerUnitSO data, bool isSelectedCharacter, Action<PlayerUnitSO, bool> onClick)
    {
        characterSO = data;
        isSelected = isSelectedCharacter;
        onAfterClick = onClick;

        // UI 이미지와 텍스트 교체
        iconImage.sprite = data.UnitIcon;
        nameText.text = data.UnitName;

        // 버튼 클릭 이벤트
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
    }

    // 버튼 클릭
    private void OnClicked()
    {
        // 외부에서 갱신
        onAfterClick?.Invoke(characterSO, isSelected);
    }

    public PlayerUnitSO GetCharacterSO() => characterSO;
}
