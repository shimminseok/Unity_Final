using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class StageInfoPanel : MonoBehaviour
{
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private Vector2 onScreenPos;
    [SerializeField] private Vector2 offScreenPos;

    [SerializeField] private TextMeshProUGUI stageName;
    [SerializeField] private TextMeshProUGUI stageDesc;
    [SerializeField] private List<StagePanelMonsterSlot> spawnMonsters;

    private StageSO stageSo;


    private void Awake()
    {
    }

    public void OpenPanel()
    {
        DOTween.KillAll();
        gameObject.SetActive(true);
        // panelRect.anchoredPosition = offScreenPos;
        panelRect.DOAnchorPos(onScreenPos, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            panelRect.anchoredPosition = onScreenPos;
        });
    }

    public void ClosePanel()
    {
        DOTween.KillAll();
        panelRect.anchoredPosition = onScreenPos;
        panelRect.DOAnchorPos(offScreenPos, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            panelRect.anchoredPosition = offScreenPos;
            gameObject.SetActive(false);
        });
    }

    public void SetStageInfo(StageSO stage)
    {
        stageSo = stage;

        for (int i = 0; i < spawnMonsters.Count; i++)
        {
            if (stageSo.Monsters.Count > i)
            {
                spawnMonsters[i].gameObject.SetActive(true);
                spawnMonsters[i].SetMonsterSlot(stageSo.Monsters[i]);
            }
            else
            {
                spawnMonsters[i].gameObject.SetActive(false);
            }
        }
    }
}