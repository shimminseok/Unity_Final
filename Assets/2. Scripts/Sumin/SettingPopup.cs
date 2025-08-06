using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingPopup : UIBase
{
    [SerializeField] private CanvasGroup BG;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private TextMeshProUGUI masterVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private TextMeshProUGUI bgmVolumeText;

    AudioManager audioManager;

    private void Start()
    {
        BG.gameObject.SetActive(false);
        audioManager = AudioManager.Instance;
    }

    public override void Open()
    {
        base.Open();
        BG.alpha = 0;
        BG.DOFade(1f, fadeInDuration).SetEase(Ease.InOutSine);

        InitializeVolumeSliders();
        BindSliderEvents();
    }

    private void InitializeVolumeSliders()
    {
        SetSlider(masterVolumeSlider, AudioType.Master);
        SetSlider(bgmVolumeSlider, AudioType.BGM);
        SetSlider(sfxVolumeSlider, AudioType.SFX);
    }

    private void SetSlider(Slider slider, AudioType type)
    {
        float value = audioManager.GetVolume(type);
        slider.SetValueWithoutNotify(value);
        UpdateVolumeText(type, value);
    }

    private void BindSliderEvents()
    {
        masterVolumeSlider.onValueChanged.RemoveAllListeners();
        bgmVolumeSlider.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider.onValueChanged.RemoveAllListeners();

        masterVolumeSlider.onValueChanged.AddListener(value => OnVolumeChanged(AudioType.Master, value));
        bgmVolumeSlider.onValueChanged.AddListener(value => OnVolumeChanged(AudioType.BGM, value));
        sfxVolumeSlider.onValueChanged.AddListener(value => OnVolumeChanged(AudioType.SFX, value));
    }

    public void OnVolumeChanged(AudioType type, float value)
    {
        audioManager.SetVolume(type, value);

        UpdateVolumeText(type, value);
    }

    private void UpdateVolumeText(AudioType type, float value)
    {
        int percent = Mathf.RoundToInt(value * 100);

        switch (type)
        {
            case AudioType.Master:
                masterVolumeText.text = $"{percent}%";
                break;
            case AudioType.BGM:
                bgmVolumeText.text = $"{percent}%";
                break;
            case AudioType.SFX:
                sfxVolumeText.text = $"{percent}%";
                break;
        }
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
        PopupManager.Instance.GetUIComponent<TwoChoicePopup>().SetAndOpenPopupUI("타이틀 복귀", "타이틀 화면으로 이동하시겠습니까?",
            () => {
                Close();
                LoadSceneManager.Instance.LoadScene("TitleScene"); 
            }, null, "이동하기");
    }
}
