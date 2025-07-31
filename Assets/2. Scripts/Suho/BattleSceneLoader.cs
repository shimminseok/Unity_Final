using System;
using UnityEngine;

public class BattleSceneLoader : SceneOnlySingleton<BattleSceneLoader>
{
  
   public void LoadAssets()
   {
       foreach (Unit unit in BattleManager.Instance.AllUnits)
       {
           foreach (SkillData skill in unit.SkillController.skills)
           {
               if(skill==null) continue;
               foreach (var sfx in skill.skillSo.SFX)
               {
                   LoadAssetManager.Instance.LoadAudioClipAsync(sfx.ToString(), null);
               }
               
           }
       } 


   }
   
}
