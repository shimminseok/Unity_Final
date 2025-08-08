using UnityEngine;


public class ItemSO : ScriptableObject
{
    public int ID;
    public string ItemName;
    public string ItemDescription;
    public ItemType ItemType;
    public Sprite ItemSprite;
    public Tier Tier;
}