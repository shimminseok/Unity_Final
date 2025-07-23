using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneGameUI : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private GameObject playingImage;

    private UIManager uiManager;
    private BattleManager battleManager;
    private LoadingScreenController loadingScreenController;

    [Header("상단에 있는 턴 UI")]
    [SerializeField] private CanvasGroup TurnTopUI;
    [SerializeField] private TextMeshProUGUI turnText;

    [Header("두트윈으로 재생되는 턴 UI")]
    [SerializeField] private CanvasGroup TurnAniUI;
    [SerializeField] private RectTransform backgroundRect;
    [SerializeField] private RectTransform titleTextRect;
    [SerializeField] private TextMeshProUGUI turnAniText;
    [SerializeField] private TextMeshProUGUI turnDescriptionText;

    [SerializeField] private float slideDuration;
    [SerializeField] private float scaleDuration;
    [SerializeField] private Vector2 slideOffset; // 아래에서 위로갈 때 위치
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private float turnAniDuration;

    private void OnEnable()
    {
        battleManager = BattleManager.Instance;
        battleManager.OnBattleEnd += UpdateTurnCount;
        loadingScreenController = LoadingScreenController.Instance;
        loadingScreenController.OnLoadingComplete += WaitForLoading;
    }

    private void WaitForLoading()
    {
        PlayTurnIntroAnimation(false);
    }

    // 턴 UI 애니메이션
    public void PlayTurnIntroAnimation(bool isBattleStart)
    {
        turnDescriptionText.text = isBattleStart ? "전투 시작" : "전략 선택";
        
        // 초기 설정
        TurnTopUI.gameObject.SetActive(false);
        TurnAniUI.gameObject.SetActive(true);
        TurnTopUI.alpha = 0f;
        TurnAniUI.alpha = 0f;

        Sequence seq = DOTween.Sequence();

        seq.Append(TurnAniUI.DOFade(1f, fadeInDuration).SetEase(Ease.InOutSine));

        seq.Join(titleTextRect.DOAnchorPosY(titleTextRect.anchoredPosition.y - slideOffset.y, slideDuration).SetEase(Ease.OutCubic));

        backgroundRect.localScale = Vector3.one * 2.3f;
        seq.Join(backgroundRect.DOScale(2f, scaleDuration).SetEase(Ease.OutBack).SetDelay(0.1f));

        seq.AppendInterval(turnAniDuration);

        seq.AppendCallback(() => TurnTopUI.gameObject.SetActive(true));

        seq.Append(TurnAniUI.DOFade(0f, fadeOutDuration).SetEase(Ease.OutSine));
        seq.Join(TurnTopUI.DOFade(1f, fadeInDuration).SetEase(Ease.InOutSine));

        seq.AppendCallback (() => TurnAniUI.gameObject.SetActive(false));

        // 애니 재생완료된 후에 전투 시작
        if (isBattleStart)
        {
            ToggleActiveStartBtn(false);
            seq.AppendCallback(() => InputManager.Instance.OnClickTurnStartButton());
        }
    }

    public void ToggleActiveStartBtn(bool toggle)
    {
        startBtn.gameObject.SetActive(toggle);
        playingImage.SetActive(!toggle);
    }

    public void ToggleInteractableStartButton(bool toggle)
    {
        startBtn.interactable = toggle;
    }

    public void OnStartButton()
    {
        PlayTurnIntroAnimation(true);
    }

    public void OnSettingButton()
    {
        PopupManager.Instance.GetUIComponent<SettingPopup>()?.Open();
    }

    public void OnExitButton()
    {
        string message = "전투를 중단하시겠습니까?";
        Action leftAction = () => LoadSceneManager.Instance.LoadScene("DeckBuildingScene");
        PopupManager.Instance.GetUIComponent<TwoChoicePopup>()?.SetAndOpenPopupUI("전투 중단", message, leftAction, null, "중단", "취소");
    }

    private void UpdateTurnCount()
    {
        string turn = $"Turn {battleManager.TurnCount}";
        turnText.text = turn;
        turnAniText.text = turn;
        PlayTurnIntroAnimation(false);
    }

    private void OnDisable()
    {
        if (battleManager != null)
            battleManager.OnBattleEnd -= UpdateTurnCount;
        if (loadingScreenController != null)
            loadingScreenController.OnLoadingComplete -= WaitForLoading;
    }
}
