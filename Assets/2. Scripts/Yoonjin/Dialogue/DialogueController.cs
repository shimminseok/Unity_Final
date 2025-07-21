using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueController : Singleton<DialogueController>
{
    private DialogueGroupSO currentGroup;
    private int currentLineIndex = 0;

    // 읽은 그룹 키들을 저장
    private HashSet<string> readGroups = new();

    [Header("디버깅 중 대사 스킵")]
    [SerializeField] private bool dialogueSkip = false;


    private Action OnCallBackAction;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        // 씬 로드 완료 후 자동 호출될 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 이벤트 해제 (중복 실행, 메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 완전히 로드된 직후 호출되는 콜백
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Additive로 DialogueScene이 로드되었을 때만 실행
        if (currentGroup != null &&
            currentGroup.mode == DialogueMode.Fullscreen &&
            scene.name == "DialogueScene")
        {
            ShowCurrentLine();
        }
    }

    // 특정 그룹 키의 대사 재생 시작
    public void Play(string groupKey, Action callback = null)
    {
        if (dialogueSkip || readGroups.Contains(groupKey))
        {
            Debug.Log("스킵됨");
            callback?.Invoke();
            return;
        }


        var table = TableManager.Instance.GetTable<DialogueGroupTable>();
        var group = table.GetDataByID(groupKey);

        if (group == null)
        {
            Debug.LogError($"GroupKey '{groupKey}'를 찾을 수 없습니다.");
            return;
        }

        currentGroup = group;
        currentLineIndex = 0;

        if (group.mode == DialogueMode.Fullscreen)
        {
            // DialogueScene을 현재 씬 위에 Additive로 로드
            LoadSceneManager.Instance.LoadSceneAdditive("DialogueScene");
        }
        else
        {
            ShowCurrentLine();
        }

        OnCallBackAction = callback;
    }

    // 다음 대사 줄로 이동
    public void Next()
    {
        if (currentGroup == null) return;

        currentLineIndex++;
        ShowCurrentLine();
    }

    // 현재 대사 줄을 출력
    private void ShowCurrentLine()
    {
        if (currentGroup == null || currentLineIndex >= currentGroup.lines.Count)
        {
            EndDialogue();
            return;
        }

        var line = currentGroup.lines[currentLineIndex];

        if (currentGroup.mode == DialogueMode.Overlay)
        {
            var ui = UIManager.Instance.GetUIComponent<OverlayDialogueUI>();
            ui.Show(line);
        }
        else if (currentGroup.mode == DialogueMode.Tutorial)
        {
            var ui = UIManager.Instance.GetUIComponent<TutorialDialogueUI>();
            ui.Show(line);
        }
        else
        {
            var fullscreenUI = FindObjectOfType<FullscreenDialogueUI>();
            fullscreenUI?.Show(line);
        }
    }

    // 대사 종료 처리
    public void EndDialogue()
    {
        // 다 읽은 대사는 스킵
        if (currentGroup != null)
        {
            readGroups.Add(currentGroup.groupKey);
        }

        if (currentGroup.mode == DialogueMode.Fullscreen)
        {
            // 현재 DialogueScene을 언로드 → 원래 씬 복귀
            SceneManager.UnloadSceneAsync("DialogueScene");
        }
        else if (currentGroup.mode == DialogueMode.Overlay)
        {
            UIManager.Instance.GetUIComponent<OverlayDialogueUI>()?.Close();
        }
        else
        {
            UIManager.Instance.GetUIComponent<TutorialDialogueUI>()?.Close();
        }

            currentGroup = null;
        currentLineIndex = 0;
        OnCallBackAction?.Invoke();
        EventBus.Publish("DialogueFinished");
    }
}

// 리소스 로더
public static class DialogueResourceLoader
{
    public static Sprite LoadPortrait(string key)
    {
        return Resources.Load<Sprite>($"Portraits/{key}");
    }

    public static Sprite LoadBackground(string key)
    {
        return Resources.Load<Sprite>($"Backgrounds/{key}");
    }
}