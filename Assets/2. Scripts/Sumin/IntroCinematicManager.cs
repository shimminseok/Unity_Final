using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class IntroCinematicManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private Button skipButton;

    private bool isTransitioning;
    private bool skipButtonVisible;

    private void Awake()
    {
        if (director != null)
        {
            director.playOnAwake = false;
            director.extrapolationMode = DirectorWrapMode.None;
            director.Stop();
            director.time = 0;
            director.Evaluate();
        }

        if (skipButton != null)
            skipButton.onClick.AddListener(OnClickSkip);
    }
    private void OnEnable()
    {
        LoadSceneManager.Instance.OnLoadingCompleted += StartTimeLine;

        if (director != null) // 디렉터 재생 끝나면 자동 호출
            director.stopped += OnCutsceneStopped;

        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        LoadSceneManager.Instance.OnLoadingCompleted -= StartTimeLine;

        if (director != null)
            director.stopped -= OnCutsceneStopped;

        if (skipButton != null)
            skipButton.onClick.RemoveListener(OnClickSkip);
    }

    void StartTimeLine()
    {
        StartCoroutine(PlayNextFrame());
    }

    // 로딩 후 조금 기다렸다가 타임라인 실행
    private IEnumerator PlayNextFrame()
    {
        yield return new WaitForSeconds(2f);
        if (director != null)
        {
            director.time = 0;
            director.Play();
        }
    }

    private void TransitionToNextScene()
    {
        if (isTransitioning) return; // 중복 전환 방지
        isTransitioning = true;

        LoadSceneManager.Instance.LoadScene("DeckBuildingScene");
    }

    private void OnCutsceneStopped(PlayableDirector _)
    {
        // 타임라인이 자연 종료되든, Stop()으로 끊기든 다음 씬 실행되도록
        TransitionToNextScene();
    }

    private void Update()
    {
        if (!skipButtonVisible && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            ShowSkipButton();
        }
    }

    private void ShowSkipButton()
    {
        skipButtonVisible = true;
        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(true);
            StartCoroutine(OffSkipButton());
        }
    }

    private IEnumerator OffSkipButton()
    {
        yield return new WaitForSeconds(3f);
        skipButtonVisible = false;
        skipButton.gameObject.SetActive(false);
    }


    public void OnClickSkip()
    {
        if (director == null)
        {
            TransitionToNextScene();
            return;
        }

        director.Stop();
    }

}
