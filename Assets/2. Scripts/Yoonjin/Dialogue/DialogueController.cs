using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueController : Singleton<DialogueController>
{
    private DialogueGroupSO currentGroup;
    private int currentLineIndex = 0;

    // 이전 씬 이름 기억
    private string previousSceneName;

    protected override void Awake()
    {
        base.Awake();
    }

    // DialogueController가 활성화될 때 씬 로드 이벤트에 등록
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // DialogueController가 비활성화될 때 씬 로드 이벤트에서 제거 (메모리 누수 방지)
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 완전히 로드된 직후 호출되는 콜백
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 현재 대사 그룹이 존재하고, Fullscreen 모드이며, DialogueScene으로 진입한 경우
        if (currentGroup != null && currentGroup.mode == DialogueMode.Fullscreen && scene.name == "DialogueScene")
        {
            // 대사 출력 시작
            ShowCurrentLine();
        }
    }

    // 특정 그룹 키에 해당하는 대사 시퀀스 재생
    // 대사 출력 방식에 따라 다른 UI로 분기
    public void Play(string groupKey)
    {
        // 그룹 키를 기준으로 DialogueGroupSO를 찾아서 반환
        var table = TableManager.Instance.GetTable<DialogueGroupTable>();
        var group = table.GetDataByID(groupKey);

        previousSceneName = SceneManager.GetActiveScene().name;


        if (group == null)
        {
            Debug.LogError($"GroupKey '{groupKey}'를 찾을 수 없습니다.");
            return;
        }

        currentGroup = group;
        currentLineIndex = 0;

        // 출력 방식에 따라 분기
        if (group.mode == DialogueMode.Fullscreen)
        {
            // 전용 대사 연출을 위한 DialogueScene으로 이동
            LoadSceneManager.Instance.LoadScene("DialogueScene");
            // 민석님이 씬 두개 띄우고 LoadScene.Mode(additive)
        }
        else
        {
            // 현재 씬에서 Overlay UI로 바로 출력
            ShowCurrentLine();
        }
    }

    // 유저 입력으로 다음 대사 줄로 이동
    // 대사 끝에 도달하면 자동 종료 처리
    public void Next()
    {
        if (currentGroup == null) return;

        currentLineIndex++;
        ShowCurrentLine();
    }

    // 현재 인덱스의 대사 줄을 화면에 출력
    // 출력 방식에 따라 서로 다른 UI를 사용
    private void ShowCurrentLine()
    {
        if (currentGroup == null || currentLineIndex >= currentGroup.lines.Count)
        {
            EndDialogue();
            return;
        }

        var line = currentGroup.lines[currentLineIndex];

        // 출력 방식 분기
        if (currentGroup.mode == DialogueMode.Overlay)
        {
            // Overlay UI를 열고 대사 출력
            var ui = UIManager.Instance.GetUIComponent<OverlayDialogueUI>();
            ui.Show(line);
        }
        else
        {
            // Fullscreen UI는 씬 내부에 존재하는 오브젝트에서 찾음
            var fullscreenUI = FindObjectOfType<FullscreenDialogueUI>();
            fullscreenUI?.Show(line);
        }
    }

    // 대사 시퀀스 종료 처리
    // UI 닫기 및 상태 초기화 수행
    private void EndDialogue()
    {
        if (currentGroup.mode == DialogueMode.Fullscreen)
        {
            LoadSceneManager.Instance.LoadScene(previousSceneName);
        }
        else
        {
            UIManager.Instance.GetUIComponent<OverlayDialogueUI>()?.Close();
        }

        // 상태 초기화
        currentGroup = null;
        currentLineIndex = 0;
    }
}

// 리소스 (초상화, 배경) 로더
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
