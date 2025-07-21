using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillForDetailButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject skillDetailPopup;
    [SerializeField] private float hideDelay;

    private bool interactable = true;
    private Coroutine hideCoroutine;

    // 스킬일 때만 Interact하도록 설정
    public void SetInteractable(bool value)
    {
        interactable = value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable) return;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        skillDetailPopup.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!interactable) return;
        hideCoroutine = StartCoroutine(HidePopupAfterDelay());
    }

    private IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        skillDetailPopup.gameObject.SetActive(false);
    }

    // 버튼 꺼졌을때 자동으로 꺼지게
    private void OnDisable()
    {
        if (skillDetailPopup != null && skillDetailPopup.activeSelf)
        {
            skillDetailPopup.SetActive(false);
        }

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }
    }
}
