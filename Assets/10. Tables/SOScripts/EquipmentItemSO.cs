using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewEquipmentData", menuName = "ScriptableObjects/Item/Equipment", order = 0)]
public class EquipmentItemSO : ItemSO
{
    public bool IsEquipableByAllJobs;

    [ShowIfFalse("IsEquipableByAllJobs")]
    public JobType JobType;

    public EquipmentType EquipmentType;
    public List<StatData> Stats;
}