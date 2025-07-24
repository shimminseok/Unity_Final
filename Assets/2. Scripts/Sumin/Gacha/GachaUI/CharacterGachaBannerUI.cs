using DG.Tweening;
using UnityEngine;

public class CharacterGachaBannerUI : MonoBehaviour, IGachaBannerUI
{
    [SerializeField] private RectTransform bannerTransform;

    public void ShowBanner()
    {
        this.gameObject.SetActive(true);
        bannerTransform.localScale = Vector3.zero;
        bannerTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void HideBanner()
    {
        this.gameObject.SetActive(false);
    }
}
