using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHUD : MonoBehaviour
{
    [SerializeField] private UIEquipmentCombine equipmentCombine;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClickOpenEquipmentCombineBtn()
    {
        UIManager.Instance.Open(equipmentCombine);
    }
}