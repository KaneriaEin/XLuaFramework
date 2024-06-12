using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UObject = UnityEngine.Object;

public class ResourceManager : MonoBehaviour
{
    internal class BundleInfo
    {
        public string AssetsName;
        public string BundleName;
        public List<string> Dependences;
    }

    private Dictionary<string, BundleInfo> m_BundleInfos = new Dictionary<string, BundleInfo>();

    /// <summary>
    /// �����汾�ļ�
    /// </summary>
    public void ParseVersionFile()
    {
        //�汾�ļ���·��
        string url = Path.Combine(PathUtil.BundleResourcePath, AppConst.FileListName);
        string[] data = File.ReadAllLines(url);

        //�����ļ���Ϣ
        for(int i = 0; i < data.Length; i++)
        {
            BundleInfo bundleInfo = new BundleInfo();
            string[] info = data[i].Split('|');
            bundleInfo.AssetsName = info[0];
            bundleInfo.BundleName = info[1];
            //list���ԣ����������飬���ɶ�̬����
            bundleInfo.Dependences = new List<string>(info.Length - 2);
            for(int j = 2; j < info.Length; j++)
            {
                bundleInfo.Dependences.Add(info[j]);
            }
            m_BundleInfos.Add(bundleInfo.AssetsName, bundleInfo);
        }
    }

    /// <summary>
    /// �첽������Դ
    /// </summary>
    /// <param name="assetName">��Դ��</param>
    /// <param name="action">��ɻص�</param>
    /// <returns></returns>
    IEnumerator LoadBundleAsync(string assetName, Action<UObject> action = null)
    {
        string bundleName = m_BundleInfos[assetName].BundleName;
        string bundlePath = Path.Combine(PathUtil.BundleResourcePath, bundleName);
        List<string> dependences = m_BundleInfos[assetName].Dependences;
        if(dependences != null && dependences.Count > 0)
        {
            for(int i = 0; i < dependences.Count; i++)
            {
                yield return LoadBundleAsync(dependences[i]);
            }
        }
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return request;

        AssetBundleRequest bundleRequest = request.assetBundle.LoadAssetAsync(assetName);
        yield return bundleRequest;

        Debug.Log("this is LoadBundleAsync");
        action?.Invoke(bundleRequest?.asset);
    }

    /// <summary>
    /// ����������������Դ
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="action"></param>
#if UNITY_EDITOR
    void EditorLoadAsset(string assetName, Action<UObject> action = null)
    {
        Debug.Log("this is EditorMode");
        UObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath(assetName, typeof(UObject));
        if(obj == null)
        {
            Debug.LogError("assets name is not exitst:" + assetName);
        }
        action?.Invoke(obj);
    }
#endif

    private void LoadAsset(string assetName, Action<UObject> action)
    {
#if UNITY_EDITOR
        if(AppConst.GameMode == GameMode.EditorMode)
        {
            EditorLoadAsset(assetName, action);
        }
        else
#endif
        {
            StartCoroutine(LoadBundleAsync(assetName, action));
        }
    }

    //����UI
    public void LoadUI(string assetName, Action<UObject> action = null)
    {
        LoadAsset(PathUtil.GetUIPath(assetName), action);
    }

    public void LoadMusic(string assetName, Action<UObject> action = null)
    {
        LoadAsset(PathUtil.GetMusicPath(assetName), action);
    }

    public void LoadSound(string assetName, Action<UObject> action = null)
    {
        LoadAsset(PathUtil.GetSoundPath(assetName), action);
    }

    public void LoadEffect(string assetName, Action<UObject> action = null)
    {
        LoadAsset(PathUtil.GetEffectPath(assetName), action);
    }

    public void LoadScene(string assetName, Action<UObject> action = null)
    {
        LoadAsset(PathUtil.GetScenePath(assetName), action);
    }

    public void LoadSprite(string assetName, Action<UObject> action = null)
    {
        LoadAsset(PathUtil.GetSpritePath(assetName), action);
    }

    //Tag: ж����ʱ����
}
