using System;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public static PoolableVFX InstantiateVFX(string poolID, GameObject prefab)
    {
        GameObject go = ObjectPoolManager.Instance.GetObject(poolID);
        if (go == null) go = Instantiate(prefab);
        PoolableVFX vfx = go.GetComponent<PoolableVFX>();
        vfx.particle.Stop();
        return vfx;
    }

    public static void VFXListPlay(List<VFXData> vfxList, VFXType vfxType,VFXSpawnReference unit, IEffectProvider effectProvider)
    {
        foreach (var vfxData in vfxList)
        {
            if (vfxData.reference != unit) continue; 
            if (vfxData.type == vfxType)
            {
                PoolableVFX vfx = VFXController.InstantiateVFX(vfxData.VFXPoolID, vfxData.VFXPrefab);
                vfx.SetData(vfxData,effectProvider);
                vfx.OnSpawnFromPool();
            }
        }
    }
    
    public static void VFXListPlay(List<VFXData> vfxList, VFXType vfxType,VFXSpawnReference unit, IEffectProvider effectProvider,Action trigger)
    {
        foreach (var vfxData in vfxList)
        {
            if (vfxData.reference != unit) continue; 
            if (vfxData.type == vfxType)
            {
                PoolableVFX vfx = VFXController.InstantiateVFX(vfxData.VFXPoolID, vfxData.VFXPrefab);
                vfx.SetData(vfxData,effectProvider,trigger);
            }
        }
    }
    
}