using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
public class UIStageSelect : UIBase
{
    [SerializeField] private RectTransform mapContent;
    [SerializeField] private RectTransform viewPort;
    [SerializeField] private StageInfoPanel stageInfoPanel;

    [SerializeField] private List<StageSlot> stageSlots;
    private Vector2 dragStartPos;
    private Vector2 contentStartPos;

    [SerializeField] private List<RectTransform> cloudList;
    public void SetStageInfo(StageSO stage)
    {
        PlayerDeckContainer.Instance.SetStage(stage);
        stageInfoPanel.SetStageInfo(stage, DeckSelectManager.Instance.GetSelectedDeck());
        stageInfoPanel.OpenPanel();
    }
    public void OnClickEnterStage()
    {
        bool isDeckEmpty = DeckSelectManager.Instance
            .GetSelectedDeck()
            .All(u => u == null);

        if (isDeckEmpty)
        {
            OnClickEditDeckButton(); // 덱이 비어 있으면 편집 창 오픈
            return;
        }

        LoadSceneManager.Instance.LoadScene("BattleScene_Main");
    }

    public void OnClickEditDeckButton()
    {
        UIDeckBuilding uiDeckBuilding = UIManager.Instance.GetUIComponent<UIDeckBuilding>();
        UIManager.Instance.Open(uiDeckBuilding);
    }

    public override void Open()
    {
        base.Open();
        int index = 0;
        foreach (StageSO dataDicValue in TableManager.Instance.GetTable<StageTable>().DataDic.Values)
        {
            if (stageSlots.Count <= index)
                break;

            stageSlots[index++].Initialize(dataDicValue);
        }

        foreach (RectTransform rectTransform in cloudList)
        {
            StartCloudLoop(rectTransform);
        }
    }
    public override void Close()
    {
        base.Close();
        stageInfoPanel.ClosePanel();
    }

    private void StartCloudLoop(RectTransform rect)
    {
        rect.anchoredPosition = new Vector2(0, rect.anchoredPosition.y);
        MoveCloud(rect);
    }

    private void MoveCloud(RectTransform rect)
    {
        float moveSpeed = Random.Range(10f, 30f);
        float delayTime = Random.Range(0f, 4f);
        rect.DOKill();
        rect.DOAnchorPos(new Vector2(-(Screen.width + rect.sizeDelta.x), rect.anchoredPosition.y), moveSpeed)
            .SetEase(Ease.Linear)
            .SetDelay(delayTime).OnComplete(() =>
            {
                rect.anchoredPosition = new Vector2(0, rect.anchoredPosition.y);
                MoveCloud(rect); // 재귀 호출로 반복
            });
    }
}