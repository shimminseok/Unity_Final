using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSeleteStageUI : MonoBehaviour
{
    public void OpenStageSeletUI()
    {
        UIManager.Instance.Open<UIStageSelect>();
    }
}