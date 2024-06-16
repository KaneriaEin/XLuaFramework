using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPool : PoolBase
{
    public override Object Spawn(string name)
    {
        return base.Spawn(name);
    }

    public override void UnSpawn(string name, Object obj)
    {
        base.UnSpawn(name, obj);
    }

    public override void Release()
    {
        base.Release();
        foreach (PoolObject item in m_Objects)
        {
            //Debug.Log("AssetPool PoolObject name : " + item.Name);
            if (System.DateTime.Now.Ticks - item.LastUseTime.Ticks >= m_ReleaseTime * 10000000)
            {
                Debug.Log("AssetPool release time: " + System.DateTime.Now + "unload ab : " + item.Name);
                Manager.Resource.UnloadBundle(item.Object);
                m_Objects.Remove(item);
                Release();
                return;
            }
        }
    }
}
