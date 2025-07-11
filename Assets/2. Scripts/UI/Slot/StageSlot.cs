using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSlot : MonoBehaviour
{
    [SerializeField] private GameObject lockImg;
    private StageSO stageSo;

    UIStageSelect stageSelectUI;

    public void Initialize(StageSO data)
    {
        stageSo = data;
        stageSelectUI = UIManager.Instance.GetUIComponent<UIStageSelect>();


        int nextStageID = AccountManager.Instance.GetNextStageId(AccountManager.Instance.BestStage);
        lockImg.SetActive(nextStageID < stageSo.ID);
    }

    public void OnClickStageSlot()
    {
        if (stageSo.HasBeforeDialogue)
        {
            DialogueController.Instance.Play(stageSo.beforeDialogueKey);
        }

        stageSelectUI.SetStageInfo(stageSo);
    }
}