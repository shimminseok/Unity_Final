using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : UIBase
{
    [SerializeField] private CanvasGroup BG;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;

    private void Start()
    {
        BG.gameObject.SetActive(false);
    }

    public override void Open()
    {
        base.Open();
        BG.alpha = 0;
        BG.DOFade(1f, fadeInDuration).SetEase(Ease.InOutSine);
    }

    public override void Close()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(BG.DOFade(0f, fadeOutDuration).SetEase(Ease.OutSine));
        seq.AppendCallback(() => base.Close());
    }

    public void OnExitBtn()
    {
        Close();
    }

    public void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
