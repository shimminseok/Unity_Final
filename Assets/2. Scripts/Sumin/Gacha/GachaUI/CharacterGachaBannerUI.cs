using DG.Tweening;
using UnityEngine;

public class CharacterGachaBannerUI : MonoBehaviour, IGachaBannerUI
{
    [SerializeField] private RectTransform bannerTransform;
    [SerializeField] private CanvasGroup bannerCanvasGroup;
    [SerializeField] private RectTransform charactersTransform;
    [SerializeField] private CanvasGroup charactersCanvasGroup;

    [SerializeField] private float fadeInDuration;

    private Sequence bannerSequence;
    private Vector2 originalPos;
    private bool initialized = false;

    public void ShowBanner()
    {
        // 초기 위치 저장
        if (!initialized)
        {
            originalPos = bannerTransform.anchoredPosition;
            initialized = true;
        }

        bannerTransform.DOKill();
        bannerCanvasGroup.DOKill();
        charactersTransform.DOKill();
        charactersCanvasGroup.DOKill();

        this.gameObject.SetActive(true);
        bannerCanvasGroup.alpha = 0f;
        charactersCanvasGroup.alpha = 0f;

        bannerSequence = DOTween.Sequence();

        bannerSequence.Append(bannerTransform.DOAnchorPos(originalPos, 0.3f).From(originalPos + Vector2.right * 200f).SetEase(Ease.OutBack));
        bannerSequence.Join(bannerCanvasGroup.DOFade(1f, fadeInDuration));

        charactersTransform.localScale = Vector3.one * 1.2f;
        bannerSequence.Append(charactersTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBack));
        bannerSequence.Join(charactersCanvasGroup.DOFade(1f, fadeInDuration));
    }

    public void HideBanner()
    {
        this.gameObject.SetActive(false);
    }
}
