using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    [SerializeField] private Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        // 열려 있는 UI들을 확인
        var uiManager = UIManager.Instance;

        var equipUI = uiManager.GetUIComponent<SelectEquipUI>();
        var skillUI = uiManager.GetUIComponent<SelectSkillUI>();
        var mainUI  = uiManager.GetUIComponent<SelectMainUI>();

        EntryDeckData currentEntry = DeckSelectManager.Instance.GetCurrentSelectedCharacter();

        // 활성화 상태 확인 후 조건 분기
        if (equipUI != null && equipUI.gameObject.activeSelf)
        {
            uiManager.Close<SelectEquipUI>();
            uiManager.Open<SelectMainUI>();

            // 메인 UI 갱신
            if (mainUI != null && currentEntry != null)
            {
                // mainUI.UpdateCharacterInfoPanel(currentEntry.characterSO);
            }
        }
        else if (skillUI != null && skillUI.gameObject.activeSelf)
        {
            uiManager.Close<SelectSkillUI>();
            uiManager.Open<SelectMainUI>();

            // 메인 UI 갱신
            if (mainUI != null && currentEntry != null)
            {
                // mainUI.UpdateCharacterInfoPanel(currentEntry.characterSO);
            }
        }
        else if (mainUI != null && mainUI.gameObject.activeSelf)
        {
            uiManager.Close<SelectMainUI>();
        }
    }
}