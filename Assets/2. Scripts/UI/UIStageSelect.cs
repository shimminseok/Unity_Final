using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIStageSelect : UIBase
{
    [SerializeField] private RectTransform mapContent;
    [SerializeField] private RectTransform viewPort;
    [SerializeField] private StageInfoPanel stageInfoPanel;

    [SerializeField] private List<StageSlot> stageSlots;
    private Vector2 dragStartPos;
    private Vector2 contentStartPos;


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
                return;

            stageSlots[index++].Initialize(dataDicValue);
        }
    }

    public override void Close()
    {
        base.Close();
        stageInfoPanel.ClosePanel();
    }
}