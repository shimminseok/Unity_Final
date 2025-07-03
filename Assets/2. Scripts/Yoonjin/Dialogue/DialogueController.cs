using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueController : Singleton<DialogueController>
{
    private DialogueGroupSO currentGroup;
    private int currentLineIndex = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    // 특정 그룹 키에 해당하는 대사 시퀀스 재생
    // 대사 출력 방식에 따라 다른 UI로 분기
    public void Play(string groupKey)
    {
        // 그룹 키를 기준으로 DialogueGroupSO를 찾아서 반환
        var table = TableManager.Instance.GetTable<DialogueGroupTable>();
        var group = table.GetDataByID(groupKey);
        
        if(group == null)
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
        }
        else
        {
            // 현재 씬에서 Overlay UI로 바로 출력
            ShowCurrentLine();
        }
    }


    // 씬이 다시 로드될 경우(Fullscreen), 대사를 이어서 출력
    private void Start()
    {
        if (currentGroup != null && currentGroup.mode == DialogueMode.Fullscreen)
        {
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
        // Overlay UI 닫기 (Fullscreen은 씬 언로드 자체가 종료)
        UIManager.Instance.GetUIComponent<OverlayDialogueUI>()?.Close();

        // 상태 초기화
        currentGroup = null;
        currentLineIndex = 0;
    }
}
