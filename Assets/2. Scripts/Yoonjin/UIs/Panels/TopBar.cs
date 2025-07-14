using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI opalText;

    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void Start()
    {
        AccountManager.Instance.OnGoldChanged += UpdateGoldText;
        AccountManager.Instance.OnOpalChanged += UpdateOpalText;
    }

    private void OnBackButtonClicked()
    {
        UIManager.Instance.CloseLastOpenedUI();
    }


    public void UpdateGoldText(int gold)
    {
        goldText.text = $"{gold:N0}";
    }

    public void UpdateOpalText(int opal)
    {
        opalText.text = $"{opal:N0}";
    }


    private void OnDisable()
    {
        AccountManager.Instance.OnGoldChanged -= UpdateGoldText;
        AccountManager.Instance.OnOpalChanged -= UpdateOpalText;
    }
}