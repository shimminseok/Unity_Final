using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIStageSelect : UIBase, IDragHandler, IBeginDragHandler
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPos = eventData.position;
        contentStartPos = mapContent.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta  = eventData.position - dragStartPos;
        Vector2 newPos = contentStartPos + delta;

        newPos = ClampToBounds(newPos);

        mapContent.anchoredPosition = newPos;
    }

    private Vector2 ClampToBounds(Vector2 pos)
    {
        Vector2 minBounds = viewPort.rect.size - mapContent.rect.size;
        minBounds.x = Mathf.Min(0, minBounds.x);
        minBounds.y = Mathf.Min(0, minBounds.y);

        float clampedX = Mathf.Clamp(pos.x, minBounds.x, 0);
        float clampedY = Mathf.Clamp(pos.y, minBounds.y, 0);
        return new Vector2(clampedX, clampedY);
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