using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneGameUI : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private GameObject playingImage;

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

}
