using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageSlot : MonoBehaviour
{
    [SerializeField] private GameObject lockImg;
    [SerializeField] private TextMeshProUGUI stageNumberTxt;
    private StageSO stageSo;

    private UIStageSelect stageSelectUI;

    public void Initialize(StageSO data)
    {
        stageSo = data;
        stageSelectUI = UIManager.Instance.GetUIComponent<UIStageSelect>();
        int chapterId = data.ID / 1000000;     // 예: 1
        int stageNum  = data.ID % 10000 % 100; // 예: 01 ~ 10

        stageNumberTxt.text = $"{chapterId}-{stageNum}";
        int nextStageID = AccountManager.Instance.GetNextStageId(AccountManager.Instance.BestStage);
        lockImg.SetActive(nextStageID < stageSo.ID);
    }

    public void OnClickStageSlot()
    {
        if (stageSo.HasBeforeDialogue)
        {
            DialogueController.Instance.Play(stageSo.beforeDialogueKey, () =>
            {
                stageSelectUI.SetStageInfo(stageSo);
            });
        }
        else
        {
            stageSelectUI.SetStageInfo(stageSo);
        }
    }
}