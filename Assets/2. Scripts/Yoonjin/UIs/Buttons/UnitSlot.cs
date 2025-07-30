using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TextMeshProUGUI nameText; // 캐릭터 이름
    [SerializeField] private TextMeshProUGUI unitLevel;
    [SerializeField] private Image unitSlotFrame;
    [SerializeField] private List<GameObject> unitTierStar;
    [SerializeField] private GameObject competedMarker;
    [SerializeField] private Image iconImage; // 캐릭터 이미지
    [SerializeField] private GameObject selectedNotiImg;
    [SerializeField] private Image holdCheckImage;
    [SerializeField] private List<Sprite> unitGradeSprites;
    [SerializeField] private Image jobTypeImage;
    [SerializeField] private List<Sprite> jobTypeSprites;

    private bool isSelected;
    private EntryDeckData selectedUnit;

    // 버튼에 데이터를 집어넣는 초기화 작업

    public event Action<EntryDeckData> OnClicked;
    public event Action<EntryDeckData> OnHeld;
    private UIDeckBuilding             uiDeckBuilding => UIManager.Instance.GetUIComponent<UIDeckBuilding>();


    private Coroutine holdCoroutine;
    private bool holdTriggered;
    private bool isHolding = false;
    private float holdTime = 0.5f;

    private bool isDoubleClickSlot = true;

    public void Initialize(EntryDeckData data)
    {
        selectedUnit = data;
        PlayerUnitSO characterSo = data.CharacterSo;

        // UI 이미지와 텍스트 교체
        iconImage.sprite = characterSo.UnitIcon;
        nameText.text = characterSo.UnitName;
        unitLevel.text = $"Lv.{data.Level}";
        unitSlotFrame.sprite = unitGradeSprites[(int)characterSo.Tier];
        jobTypeImage.sprite = jobTypeSprites[(int)characterSo.JobType];

        gameObject.name = $"UnitSlot_{characterSo.ID}";

        SetCompetedMarker(data.CompeteSlotInfo.IsInDeck);
        for (int i = 0; i < unitTierStar.Count; i++)
        {
            unitTierStar[i].SetActive(i <= (int)characterSo.Tier);
        }
    }

    public void SetCompetedMarker(bool isCompeted)
    {
        if (isCompeted != competedMarker.activeSelf)
        {
            competedMarker.SetActive(isCompeted);
        }
    }

    public void SetSelectedMarker(bool isSelect)
    {
        if (isSelect != selectedNotiImg.activeSelf)
        {
            selectedNotiImg.SetActive(isSelect);
        }
    }

    public void Deselect()
    {
        isSelected = false;
        SetSelectedMarker(false);
    }

    public void SetDoubleClicked(bool isDoubleClicked = true)
    {
        isDoubleClickSlot = isDoubleClicked;
    }

    public void HandleClick()
    {
        if (holdTriggered)
        {
            holdTriggered = false;
            return;
        }

        if (isDoubleClickSlot)
        {
            if (!isSelected)
            {
                isSelected = true;
                uiDeckBuilding.SetSelectedUnitSlot(this);
            }
            else
            {
                OnClicked?.Invoke(selectedUnit);
            }
        }
        else
        {
            isSelected = true;
            uiDeckBuilding.SetSelectedUnitSlot(this);
            OnClicked?.Invoke(selectedUnit);
        }

        SetCompetedMarker(selectedUnit.CompeteSlotInfo.IsInDeck);
        SetSelectedMarker(isSelected);
    }

    private void HandleHold()
    {
        //팝업 창이 나타남
        holdTriggered = true;
        OnHeld?.Invoke(selectedUnit);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnHeld == null)
        {
            return;
        }

        if (holdCoroutine == null)
        {
            holdCoroutine = StartCoroutine(HoldCheckCoroutine());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }

        isHolding = false;
        holdCheckImage?.gameObject.SetActive(false);
    }

    private IEnumerator HoldCheckCoroutine()
    {
        if (holdCheckImage == null)
        {
            yield break;
        }

        isHolding = true;

        holdCheckImage.fillAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < holdTime)
        {
            if (elapsedTime > 0.1f && !holdCheckImage.gameObject.activeSelf)
            {
                holdCheckImage.gameObject.SetActive(true);
            }

            elapsedTime += Time.deltaTime;
            holdCheckImage.fillAmount = Mathf.Clamp01(elapsedTime / holdTime);
            yield return null;
        }

        if (isHolding)
        {
            holdCheckImage.gameObject.SetActive(false);
            HandleHold();
        }
    }
}