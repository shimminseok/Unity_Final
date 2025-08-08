using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewEquipmentData", menuName = "ScriptableObjects/Item/Equipment", order = 0)]
public class EquipmentItemSO : ItemSO
{
    public bool IsEquipableByAllJobs;
#if UNITY_EDITOR
    [ShowIfFalse("IsEquipableByAllJobs")]
#endif
    public JobType JobType;

    public EquipmentType EquipmentType;
    public List<StatData> Stats;
}