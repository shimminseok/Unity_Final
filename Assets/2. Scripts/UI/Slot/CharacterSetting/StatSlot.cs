using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatSlot : MonoBehaviour
{
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;


    public StatType StatType => statType;

    private void Start()
    {
        statName.text = Define.GetStatName(statType);
    }

    public void Initialize(float statValue, float equipValue)
    {
        UpdateStatValue(statValue, equipValue);
    }

    public void UpdateStatValue(float value, float equipValue = 0)
    {
        statValue.text = $"{value:N1} +({equipValue:N1})";
    }
}