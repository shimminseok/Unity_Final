using UnityEngine;
using UnityEngine.UI;

public class CharacterGachaResultUI : UIBase
{
    [SerializeField] private Button resultExitBtn;
    [SerializeField] private CharacterGachaSlotUI[] slots;

    void Start()
    {
        resultExitBtn.onClick.RemoveAllListeners();
        resultExitBtn.onClick.AddListener(() => OnResultPanelExitBtn());
    }

    public void ShowCharacters(PlayerUnitSO[] characters)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Initialize(characters[i]);
        }
    }

    public void OnResultPanelExitBtn()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
        UIManager.Instance.Close(this);
    }
}
