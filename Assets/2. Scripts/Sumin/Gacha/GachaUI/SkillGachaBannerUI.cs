using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGachaBannerUI : MonoBehaviour, IGachaBannerUI
{
    [SerializeField] private RectTransform bannerTransform;
    [SerializeField] private CanvasGroup bannerCanvasGroup;
    [SerializeField] private RectTransform skillsTransform;
    [SerializeField] private CanvasGroup skillsCanvasGroup;

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
            skillsOriginalPos = skillsTransform.anchoredPosition;
            initialized = true;
        }

        bannerTransform.DOKill();
        bannerCanvasGroup.DOKill();
        skillsTransform.DOKill();
        skillsCanvasGroup.DOKill();

        this.gameObject.SetActive(true);
        bannerCanvasGroup.alpha = 0f;
        skillsCanvasGroup.alpha = 0f;

        bannerSequence = DOTween.Sequence();

        bannerSequence.Append(bannerTransform.DOAnchorPos(originalPos, 0.3f).From(originalPos + Vector2.right * 200f).SetEase(Ease.OutBack));
        bannerSequence.Join(bannerCanvasGroup.DOFade(1f, fadeInDuration));

        bannerSequence.Append(skillsTransform.DOAnchorPos(skillsOriginalPos, 0.3f).From(skillsOriginalPos + Vector2.up * 200f).SetEase(Ease.OutBack));
        bannerSequence.Join(skillsCanvasGroup.DOFade(1f, fadeInDuration));
    }

    public void HideBanner()
    {
        this.gameObject.SetActive(false);
    }
}
