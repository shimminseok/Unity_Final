using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSlot : MonoBehaviour
{
    private StageSO stageSo;

    UIStageSelect stageSelectUI;

    public void Initialize(StageSO data)
    {
        stageSo = data;
        stageSelectUI = UIManager.Instance.GetUIComponent<UIStageSelect>();
    }

    public void OnClickStageSlot()
    {
        stageSelectUI.SetStageInfo(stageSo);
    }
}