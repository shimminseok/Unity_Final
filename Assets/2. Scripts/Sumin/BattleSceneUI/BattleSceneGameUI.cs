using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneGameUI : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private GameObject playingImage;
    [SerializeField] private TextMeshProUGUI turnText;

    private BattleManager battleManager;

    private void OnEnable()
    {
        battleManager = BattleManager.Instance;
        battleManager.OnBattleEnd += UpdateTurnCount;
    }

    public void ToggleActiveStartBtn(bool toggle)
    {
        startBtn.gameObject.SetActive(toggle);
        playingImage.SetActive(!toggle);
    }

    public void ToggleInteractableStartButton(bool toggle)
    {
        startBtn.interactable = toggle;
    }

    public void OnStartButton()
    {
        InputManager.Instance.OnClickTurnStartButton();
    }

    private void UpdateTurnCount()
    {
        turnText.text = $"Turn {battleManager.TurnCount}";
    }

    private void OnDisable()
    {
        if (battleManager != null)
            battleManager.OnBattleEnd -= UpdateTurnCount;
    }
}
