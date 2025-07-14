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

    
    public static List<PoolableVFX> VFXListPlay(List<VFXData> vfxList, VFXType vfxType,VFXSpawnReference unit, IEffectProvider effectProvider, bool isAwakePlay)
    {
        List<PoolableVFX> returnVFX = new List<PoolableVFX>();
        foreach (var vfxData in vfxList)
        {
            if (vfxData.reference != unit) continue; 
            if (vfxData.type == vfxType)
            {
                PoolableVFX vfx = InstantiateVFX(vfxData.VFXPoolID, vfxData.VFXPrefab);
                vfx.SetData(vfxData,effectProvider);
                returnVFX.Add(vfx);
                if (isAwakePlay)
                { 
                    vfx.OnSpawnFromPool();
                }
            }
        }
        return returnVFX;
    }
    
}