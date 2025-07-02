using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePanelMonsterSlot : MonoBehaviour
{
    [SerializeField] private Image monsterIcon;


    public void SetMonsterSlot(EnemyUnitSO enemyUnitSo)
    {
        monsterIcon.sprite = enemyUnitSo.UnitIcon;
    }
}