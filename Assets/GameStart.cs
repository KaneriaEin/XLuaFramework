using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameMode GameMode;
    public bool OpenLog;
    // Start is called before the first frame update
    void Start()
    {
        Manager.Event.Subscribe(10000, OnLuaInit);

        AppConst.GameMode = this.GameMode;
        AppConst.OpenLog = this.OpenLog;
        DontDestroyOnLoad(this);

        Manager.Resource.ParseVersionFile();
        Manager.Lua.Init();
        //Manager.Lua.StartLua("Main");
    }

    private void OnLuaInit(object args)
    {
        Manager.Lua.StartLua("Main");

        XLua.LuaFunction func = Manager.Lua.LuaEnv.Global.Get<XLua.LuaFunction>("Main");
        func.Call();

        Manager.Pool.CreateGameObjectPool("UI", 10);
        Manager.Pool.CreateGameObjectPool("Monster", 120);
        Manager.Pool.CreateGameObjectPool("Effect", 120);
        Manager.Pool.CreateAssetPool("AssetBundle", 10);
    }

    private void OnApplicationQuit()
    {
        Manager.Event.UnSubscribe(10000, OnLuaInit);
    }
}
