using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class StageInfoPanel : MonoBehaviour
{
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private TextMeshProUGUI stageName;
    [SerializeField] private List<StagePanelMonsterSlot> spawnMonsters;
    [SerializeField] private List<StagePanelHeroSlot> competedHeroes;
    [SerializeField] private List<InventorySlot> rewardSlots;


    private Vector3 onScreenScale;
    private StageSO stageSo;

    private List<EntryDeckData> myDeck => DeckSelectManager.Instance.GetSelectedDeck();

    private void Awake()
    {
        onScreenScale = panelRect.localScale;
        panelRect.localScale = Vector3.zero;
        gameObject.SetActive(false);


        DeckSelectManager.Instance.OnChangedDeck += SetCompetedUnitSlot;
    }


    public void OpenPanel()
    {
        DOTween.KillAll();
        gameObject.SetActive(true);
        panelRect.DOScale(onScreenScale, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            panelRect.localScale = onScreenScale;
        });
    }

    public void ClosePanel()
    {
        DOTween.KillAll();
        panelRect.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void SetCompetedUnitSlot(int index)
    {
        competedHeroes[index].SetHeroSlot(myDeck[index]?.CharacterSo);
    }

    public void SetStageInfo(StageSO stage)
    {
        stageSo = stage;

        for (int i = 0; i < spawnMonsters.Count; i++)
        {
            if (stageSo.Monsters.Count > i)
            {
                spawnMonsters[i].SetMonsterSlot(stageSo.Monsters[i]);
            }
            else
            {
                spawnMonsters[i].EmptySlot();
            }
        }

        for (int i = 0; i < competedHeroes.Count; i++)
        {
            SetCompetedUnitSlot(i);
        }

        RewardSo firstClearReward = stageSo.FirstClearReward;

        string   rewardId    = $"{stageSo.ID}_Clear_Reward";
        RewardSo clearReward = TableManager.Instance.GetTable<RewardTable>().GetDataByID(rewardId);

        // int index = 0;
        // foreach (RewardData rewardData in firstClearReward.RewardList)
        // {
        //     rewardSlots[index].Initialize(rewardData);
        // }
    }

    private void OnDestroy()
    {
        DeckSelectManager.Instance.OnChangedDeck -= SetCompetedUnitSlot;
    }
}