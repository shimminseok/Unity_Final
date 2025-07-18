using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IReuseScrollData<T>
{
    void UpdateSlot(T data);
}

public class ReuseScrollview<T> : MonoBehaviour where T : class
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform viewportRect;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Vector2 spacing = new Vector2(15f, 15f);
    [SerializeField] private Vector2 gridStartOffset = new Vector2(0f, 0f);
    [SerializeField] private bool isVertical = true;

    private List<RectTransform> itemList = new();
    private List<T> dataList = new();

    private float itemWidth;
    private float itemHeight;
    private int visibleItemCount;
    private int currentStartIndex;
    private Vector2 lastContentPosition;

    private int columnCount;
    private int rowCount;
    private bool isInitialized;


    private void Initialize()
    {
        Canvas.ForceUpdateCanvases();

        if (itemPrefab.TryGetComponent(out RectTransform itemRect))
        {
            itemWidth = itemRect.rect.width;
            itemHeight = itemRect.rect.height;
        }

        float viewWidth  = viewportRect.rect.width;
        float viewHeight = viewportRect.rect.height;

        columnCount = Mathf.FloorToInt((viewWidth + spacing.x) / (itemWidth + spacing.x));
        rowCount = Mathf.FloorToInt((viewHeight + spacing.y) / (itemHeight + spacing.y));

        int extraBuffer = 2;
        visibleItemCount = ((isVertical ? rowCount : columnCount) + 1 + extraBuffer) * (isVertical ? columnCount : rowCount);

        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    public void SetData(List<T> items)
    {
        if (!isInitialized)
        {
            Initialize();
            isInitialized = true;
        }

        dataList = items;
        SetContentSize();

        CreateItems();
        lastContentPosition = content.anchoredPosition;
        RecycleItems();
    }

    private void SetContentSize()
    {
        int rowOrColCount = Mathf.CeilToInt((float)dataList.Count / (isVertical ? columnCount : rowCount));

        float contentWidth  = columnCount * (itemWidth + spacing.x) - spacing.x;
        float contentHeight = rowOrColCount * (itemHeight + spacing.y) + spacing.y;

        content.sizeDelta = isVertical
            ? new Vector2(contentWidth, contentHeight)
            : new Vector2(contentHeight, contentWidth);
    }

    private void CreateItems()
    {
        int createCount = Mathf.Min(visibleItemCount, dataList.Count);
        for (int i = 0; i < createCount; i++)
        {
            GameObject    item     = Instantiate(itemPrefab, content);
            RectTransform itemRect = item.GetComponent<RectTransform>();
            itemList.Add(itemRect);

            UpdateItemPosition(i, i);
        }
    }

    private void OnScrollValueChanged(Vector2 value)
    {
        Vector2 delta = content.anchoredPosition - lastContentPosition;
        if ((isVertical && Mathf.Abs(delta.y) >= itemHeight + spacing.y) ||
            (!isVertical && Mathf.Abs(delta.x) >= itemWidth + spacing.x))
        {
            RecycleItems();
            lastContentPosition = content.anchoredPosition;
        }
    }

    private void RecycleItems()
    {
        float position      = isVertical ? content.anchoredPosition.y : content.anchoredPosition.x;
        int   newStartIndex = Mathf.FloorToInt(position / (isVertical ? (itemHeight + spacing.y) : (itemWidth + spacing.x))) * (isVertical ? columnCount : rowCount);

        int maxStartIndex = Mathf.Max(0, dataList.Count - visibleItemCount);
        newStartIndex = Mathf.Clamp(newStartIndex, 0, maxStartIndex);

        if (newStartIndex != currentStartIndex)
        {
            currentStartIndex = newStartIndex;

            for (int i = 0; i < itemList.Count; i++)
            {
                UpdateItemPosition(i, currentStartIndex + i);
            }
        }
    }

    private void UpdateItemPosition(int itemIndex, int dataIndex)
    {
        if (dataIndex >= dataList.Count) return;

        RectTransform item = itemList[itemIndex];

        int row, col;
        if (isVertical)
        {
            row = dataIndex / columnCount;
            col = dataIndex % columnCount;
        }
        else
        {
            col = dataIndex / rowCount;
            row = dataIndex % rowCount;
        }

        float x = gridStartOffset.x + col * (itemWidth + spacing.x);
        float y = gridStartOffset.y - row * (itemHeight + spacing.y);

        item.anchoredPosition = new Vector2(x, y);

        if (item.TryGetComponent<IReuseScrollData<T>>(out var scrollData))
        {
            scrollData.UpdateSlot(dataList[dataIndex]);
        }
    }
}