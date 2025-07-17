using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitInfoSlotUI : MonoBehaviour
{
    [SerializeField] private PlayerUnitInfoSlotHpBarUI hpBarUI;
    [SerializeField] private Image unitIcon;

    public void UpdateUnitInfo(Unit playerUnit)
    {
        unitIcon.sprite = playerUnit.UnitSo.UnitIcon;
    }

    public void UpdateHpBar(IDamageable owner)
    {
        hpBarUI.Initialize(owner);
    }
}
