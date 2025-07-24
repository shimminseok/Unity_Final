using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentGachaBannerUI : MonoBehaviour, IGachaBannerUI
{
    [SerializeField] private RectTransform bannerTransform;
    [SerializeField] private CanvasGroup bannerCanvasGroup;
    [SerializeField] private RectTransform equipmentsTransform;
    [SerializeField] private CanvasGroup equipmentsCanvasGroup;

    [SerializeField] private float fadeInDuration;

    private Sequence bannerSequence;
    private Vector2 originalPos;
    private Vector2 skillsOriginalPos;
    private bool initialized = false;

    public void ShowBanner()
    {
        // 초기 위치 저장
        if (!initialized)
        {
            originalPos = bannerTransform.anchoredPosition;
            skillsOriginalPos = equipmentsTransform.anchoredPosition;
            initialized = true;
        }

        bannerTransform.DOKill();
        bannerCanvasGroup.DOKill();
        equipmentsTransform.DOKill();
        equipmentsCanvasGroup.DOKill();

        this.gameObject.SetActive(true);
        bannerCanvasGroup.alpha = 0f;
        equipmentsCanvasGroup.alpha = 0f;

        bannerSequence = DOTween.Sequence();

        bannerSequence.Append(bannerTransform.DOAnchorPos(originalPos, 0.3f).From(originalPos + Vector2.right * 200f).SetEase(Ease.OutBack));
        bannerSequence.Join(bannerCanvasGroup.DOFade(1f, fadeInDuration));

        bannerSequence.Append(equipmentsTransform.DOAnchorPos(skillsOriginalPos, 0.3f).From(skillsOriginalPos + Vector2.left * 200f).SetEase(Ease.OutBack));
        bannerSequence.Join(equipmentsCanvasGroup.DOFade(1f, fadeInDuration));
    }

    public void HideBanner()
    {
        this.gameObject.SetActive(false);
    }
}
