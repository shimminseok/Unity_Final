using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillForDetailButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject skillDetailPopup;
    [SerializeField] private float hideDelay;
    [SerializeField] private float holdTime;

    private bool interactable = true;
    private bool isHolding = false;
    private Coroutine hideCoroutine;
    private Coroutine holdCoroutine;

    private bool hasHeldLongEnough = false;
    public bool IsClickBolcked => hasHeldLongEnough;


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

        if (holdCoroutine == null) // 꾹 누르고 일정 시간 지났을 때 팝업 등장
        {
            holdCoroutine = StartCoroutine(HoldCheckCoroutine());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!interactable) return;

        if (isHolding && skillDetailPopup.activeSelf)
        {
            hideCoroutine = StartCoroutine(HidePopupAfterDelay());
        }
        
        if( holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
        isHolding = false;
    }

    private IEnumerator HoldCheckCoroutine()
    {
        isHolding = true;
        yield return new WaitForSeconds(holdTime);

        hasHeldLongEnough = true;

        if (isHolding)
        {
            skillDetailPopup.gameObject.SetActive(true);
        }
    }

    private IEnumerator HidePopupAfterDelay() // 뗐을 때 일정 시간 후 팝업 끄기
    {
        yield return new WaitForSeconds(hideDelay);
        skillDetailPopup.gameObject.SetActive(false);
        hasHeldLongEnough = false;
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
