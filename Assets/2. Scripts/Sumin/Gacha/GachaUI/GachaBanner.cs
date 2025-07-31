using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GachaBanner : MonoBehaviour
{
    [SerializeField] private Image bannerImage;
    [SerializeField] private Sprite[] bannerSprites;
    [SerializeField] private RectTransform bannerTransform;
    [SerializeField] private CanvasGroup bannerCanvasGroup;
    [SerializeField] private RectTransform[] gachaImageTransforms;
    [SerializeField] private CanvasGroup[] gachaImageCanvasGroups;

    [SerializeField] private float fadeInDuration = 0.3f;

    private Sequence bannerSequence;
    private Vector2 originalPos;
    private Vector2 skillsOriginalPos;
    private bool initialized = false;


    public void ShowBanner(int index)
    {
        // 초기 위치 저장
        if (!initialized)
        {
            originalPos = bannerTransform.anchoredPosition;
            skillsOriginalPos = gachaImageTransforms[index].anchoredPosition;
            initialized = true;
        }

        foreach (CanvasGroup item in gachaImageCanvasGroups)
        {
            item.alpha = 0f;
            item.gameObject.SetActive(false);
        }

        bannerTransform.DOKill();
        bannerCanvasGroup.DOKill();
        gachaImageTransforms[index].DOKill();
        gachaImageCanvasGroups[index].DOKill();

        gachaImageCanvasGroups[index].gameObject.SetActive(true);
        bannerImage.sprite = bannerSprites[index];
        bannerCanvasGroup.alpha = 0f;
        gachaImageCanvasGroups[index].alpha = 0f;

        bannerSequence = DOTween.Sequence();

        bannerSequence.Append(bannerTransform.DOAnchorPos(originalPos, 0.3f).From(originalPos + (Vector2.right * 200f)).SetEase(Ease.OutBack));
        bannerSequence.Join(bannerCanvasGroup.DOFade(1f, fadeInDuration));

        bannerSequence.Append(gachaImageTransforms[index].DOAnchorPos(skillsOriginalPos, 0.3f).From(skillsOriginalPos + (Vector2.left * 200f)).SetEase(Ease.OutBack));
        bannerSequence.Join(gachaImageCanvasGroups[index].DOFade(1f, fadeInDuration));
    }

    public void HideBanner(int index)
    {
        gachaImageCanvasGroups[index].gameObject.SetActive(false);
    }

    public void HideAllBanner()
    {
        foreach (CanvasGroup item in gachaImageCanvasGroups)
        {
            item.gameObject.SetActive(false);
        }
    }
}