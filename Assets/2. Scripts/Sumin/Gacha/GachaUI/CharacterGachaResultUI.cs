using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGachaResultUI : UIBase
{
    [SerializeField] private Button resultExitBtn;
    [SerializeField] private CharacterGachaSlotUI[] slots;

    void Start()
    {
        resultExitBtn.onClick.RemoveAllListeners();
        resultExitBtn.onClick.AddListener(() => OnResultExitBtn());
        ResetSlots();
    }

    public void ShowCharacters(PlayerUnitSO[] characters)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].Initialize(characters[i]);

            slots[i].transform.localScale = Vector3.zero;
            slots[i].transform.DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack)
                .SetDelay(i * 0.1f);
        }
    }

    public void OnResultExitBtn()
    {
        ResetSlots();
        UIManager.Instance.Close(this);
    }

    private void ResetSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
    }
}
