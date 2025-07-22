using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHUD : MonoBehaviour
{
    private UIManager          UIManager        => UIManager.Instance;
    private UIEquipmentCombine EquipmentCombine => UIManager.GetUIComponent<UIEquipmentCombine>();
    private UIDeckBuilding     DeckBuilding     => UIManager.GetUIComponent<UIDeckBuilding>();

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClickOpenEquipmentCombineBtn()
    {
        UIManager.Open(EquipmentCombine);
    }

    public void OnClickOpenEditPartyBtn()
    {
        UIManager.Open(DeckBuilding);
    }
}