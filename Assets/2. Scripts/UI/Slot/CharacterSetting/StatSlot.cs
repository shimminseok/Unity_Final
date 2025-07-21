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

    public void Initialize(StatType type, float statValue)
    {
        statType = type;
        statName.text = Define.GetStatName(type);
        UpdateStatValue(statValue);
    }

    public void UpdateStatValue(float value, float value2 = 0)
    {
        string statvalue = value2 == 0 ? $"{value:N1}" : $"{value:N1} (+{value2:N1})";
        statValue.text = statvalue;
    }
}