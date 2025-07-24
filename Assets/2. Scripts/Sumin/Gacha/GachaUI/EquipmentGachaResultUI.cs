using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentGachaResultUI : UIBase
{
    [SerializeField] private Button resultExitBtn;
    [SerializeField] private EquipmentGachaSlotUI[] slots;
    private CanvasGroup canvasGroup;
    private Sequence resultSeq;

    void Start()
    {
        resultExitBtn.onClick.RemoveAllListeners();
        resultExitBtn.onClick.AddListener(() => OnResultExitBtn());
        ResetSlots();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Open()
    {
        base.Open();

        canvasGroup.DOKill();
        canvasGroup.alpha = 0f;

        resultSeq = DOTween.Sequence();
        resultSeq.Append(canvasGroup.DOFade(1f, 0.5f));
    }

    public void ShowEquipments(EquipmentItemSO[] equipments)
    {
        resultSeq.OnComplete(() =>
        {
            for (int i = 0; i < equipments.Length; i++)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].Initialize(equipments[i]);

                slots[i].transform.localScale = Vector3.zero;
                slots[i].transform.DOScale(Vector3.one, 0.3f)
                    .SetEase(Ease.OutBack)
                    .SetDelay(i * 0.1f);
            }
        });
        
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
