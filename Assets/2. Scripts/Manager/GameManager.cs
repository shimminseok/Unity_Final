using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    private AccountManager   AccountManager   => AccountManager.Instance;
    private InventoryManager InventoryManager => InventoryManager.Instance;
    private SaveLoadManager  SaveLoadManager  => SaveLoadManager.Instance;

    private void Start()
    {
        SaveLoadManager.Instance.LoadAll();
        ApplySaveDataToManagers();
    }


    private void ApplySaveDataToManagers()
    {
        // Gold
        if (SaveLoadManager.SaveDataMap[SaveModule.Gold] is SaveGoldData goldData)
        {
            AccountManager.SetGold(goldData.Gold);
        }

        if (SaveLoadManager.SaveDataMap[SaveModule.Opal] is SaveOpalData opalData)
        {
            AccountManager.SetOpal(opalData.Opal);
        }

        // Inventory Items
        if (SaveLoadManager.SaveDataMap[SaveModule.InventoryItem] is SaveInventoryItemData itemData)
        {
            InventoryManager.ApplyLoadedInventory(itemData.InventoryItems);
        }

        // Skill Inventory
        if (SaveLoadManager.SaveDataMap[SaveModule.InventorySkill] is SaveInventorySkill skillData)
        {
            AccountManager.ApplyLoadedSkills(skillData.SkillInventory);
        }

        // Unit Inventory
        if (SaveLoadManager.SaveDataMap[SaveModule.InventoryUnit] is SaveUnitInventoryData unitData)
        {
            AccountManager.ApplyLoadedUnits(unitData.UnitInventory);
        }

        if (SaveLoadManager.SaveDataMap[SaveModule.BestStage] is SaveBestStageData bestStageData)
        {
            AccountManager.SetBestStage(bestStageData.BestStage);
        }

        if (SaveLoadManager.SaveDataMap[SaveModule.CurrentStage] is SaveCurrentStageData currentStageData)
        {
            AccountManager.UpdateLastChallengedStageId(currentStageData.LastClearedStage);
        }

        // 등등...
    }
}