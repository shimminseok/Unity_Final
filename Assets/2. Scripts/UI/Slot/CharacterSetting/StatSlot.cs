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


    public event Action<float> OnValueChanged;

    public StatType StatType => statType;

    private void Start()
    {
        statName.text = Define.GetStatName(statType);
    }

    public void Initialize(StatData statData)
    {
        UpdateStatValue(statData.Value);
    }

    public void UpdateStatValue(float value)
    {
        statValue.text = $"{value:N1}";
    }
}