using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewEquipmentData", menuName = "ScriptableObject/Item/Equipment", order = 0)]
public class EquipmentItemSO : ItemSO
{
    public JobType JobType;
    public EquipmentType EquipmentType;
    public List<StatData> Stats;
}