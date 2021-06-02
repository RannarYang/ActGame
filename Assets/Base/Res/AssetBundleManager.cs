using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public sealed class AssetBundleManager : Singleton<AssetBundleManager>
{
    private string m_ABConfigABName = "assetbundleconfig";
    //资源关系依赖配表，可以根据crc来找到对应资源块
    private Dictionary<uint, ResourceItem> m_ResouceItemDic = new Dictionary<uint, ResourceItem>();
    //储存已加载的AB包，key为crc
    private Dictionary<uint, AssetBundleItem> m_AssetBundleItemDic = new Dictionary<uint, AssetBundleItem>();
    //AssetBundleItem类对象池
    private Pool<AssetBundleItem> m_AssetBundleItemPool = PoolManager.Instance.GetOrCreatClassPool<AssetBundleItem>(500);

#region 提供给MemoryDisplay
    public Dictionary<uint, ResourceItem> ResouceItemDic {
        get {
            return this.m_ResouceItemDic;
        }
    }
    public Dictionary<uint, AssetBundleItem> AssetBundleItemDic {
        get {
            return this.m_AssetBundleItemDic;
        }
    }
#endregion

    private string ABLoadPath
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }
    /// <summary>
    /// 加载ab配置表
    /// </summary>
    /// <returns></returns>
    public bool LoadAssetBundleConfig()
    {
#if UNITY_EDITOR
        if (!BConfigs.IsLoadFormAssetBundle)
            return false;
#endif

        m_ResouceItemDic.Clear();
        string configPath = ABLoadPath + m_ABConfigABName;
        AssetBundle configAB = AssetBundle.LoadFromFile(configPath);
        TextAsset textAsset = configAB.LoadAsset<TextAsset>(m_ABConfigABName);
        if (textAsset == null)
        {
            Debug.LogError("AssetBundleConfig is no exist!");
            return false;
        }

        MemoryStream stream = new MemoryStream(textAsset.bytes);
        BinaryFormatter bf = new BinaryFormatter();
        AssetBundleConfig config = (AssetBundleConfig)bf.Deserialize(stream);
        stream.Close();

        for (int i = 0; i < config.ABList.Count; i++)
        {
            ABBase abBase = config.ABList[i];
            ResourceItem item = new ResourceItem();
            item.m_Crc = abBase.Crc;
            item.m_AssetName = abBase.AssetName;
            item.m_ABName = abBase.ABName;
            item.m_DependAssetBundle = abBase.ABDependce;
            if (m_ResouceItemDic.ContainsKey(item.m_Crc))
            {
                Debug.LogError("重复的Crc 资源名:" + item.m_AssetName + " ab包名：" + item.m_ABName);
            }
            else
            {
                m_ResouceItemDic.Add(item.m_Crc, item);
            }
        }
        return true;
    }

    /// <summary>
    /// 根据路径的crc加载中间类ResoucItem
    /// </summary>
    /// <param name="crc"></param>
    /// <returns></returns>
    public ResourceItem LoadResouceAssetBundle(uint crc)
    {
        ResourceItem item = null;

        if (!m_ResouceItemDic.TryGetValue(crc, out item) || item == null)
        {
            Debug.LogError(string.Format("LoadResourceAssetBundle error: can not find crc {0} in AssetBundleConfig", crc.ToString()));
            return item;
        }

        item.m_AssetBundle = LoadAssetBundle(item.m_ABName);

        if (item.m_DependAssetBundle != null)
        {
            for (int i = 0; i < item.m_DependAssetBundle.Count; i++)
            {
                LoadAssetBundle(item.m_DependAssetBundle[i]);
            }
        }

        return item;
    }

    /// <summary>
    /// 加载单个assetbundle根据名字
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private AssetBundle LoadAssetBundle(string name)
    {
        AssetBundleItem item = null;
        uint crc = Crc32.GetCrc32(name);

        if (!m_AssetBundleItemDic.TryGetValue(crc, out item))
        {
            AssetBundle assetBundle = null;
            string fullPath = ABLoadPath + name;
            assetBundle = AssetBundle.LoadFromFile(fullPath);

            if (assetBundle == null)
            {
                Debug.LogError(" Load AssetBundle Error:" + fullPath);
            }

            item = m_AssetBundleItemPool.Spawn(true);
            item.assetBundle = assetBundle;
            item.RefCount++;
            m_AssetBundleItemDic.Add(crc, item);
        }
        else
        {
            item.RefCount++;
        }
        return item.assetBundle;
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="item"></param>
    public void ReleaseAsset(ResourceItem item)
    {
        if (item == null)
        {
            return;
        }

        if (item.m_DependAssetBundle != null && item.m_DependAssetBundle.Count > 0)
        {
            for (int i = 0; i < item.m_DependAssetBundle.Count; i++)
            {
                UnLoadAssetBundle(item.m_DependAssetBundle[i]);
            }
        }
        UnLoadAssetBundle(item.m_ABName);
    }

    private void UnLoadAssetBundle(string name)
    {
        AssetBundleItem item = null;
        uint crc = Crc32.GetCrc32(name);
        if (m_AssetBundleItemDic.TryGetValue(crc, out item) && item != null)
        {
            item.RefCount--;
            if (item.RefCount <= 0 && item.assetBundle != null)
            {
                item.assetBundle.Unload(true);
                item.Rest();
                m_AssetBundleItemPool.Recycle(item);
                m_AssetBundleItemDic.Remove(crc);
            }
        }
    }

    /// <summary>
    /// 根据crc朝赵ResouceItem
    /// </summary>
    /// <param name="crc"></param>
    /// <returns></returns>
    public ResourceItem FindResourceItme(uint crc)
    {
        ResourceItem item = null;
        m_ResouceItemDic.TryGetValue(crc, out item);
        return item;
    }
}


[System.Serializable]
public class AssetBundleItem
{
    public AssetBundle assetBundle = null;
    public int RefCount;

    public void Rest()
    {
        assetBundle = null;
        RefCount = 0;
    }
}

[System.Serializable]
public class ResourceItem
{
    //资源路径的CRC
    public uint m_Crc = 0;
    //该资源的文件名
    public string m_AssetName = string.Empty;
    //该资源所在的AssetBundle
    public string m_ABName = string.Empty;
    //该资源所依赖的AssetBundle
    public List<string> m_DependAssetBundle = null;
    //该资源加载完的AB包
    public AssetBundle m_AssetBundle = null;
    //-----------------------------------------------------
    //资源对象
    public Object m_Obj = null;
    //资源唯一标识
    public int m_Guid = 0;
    //资源最后所使用的时间
    public float m_LastUseTime = 0.0f;
    //引用计数
    [SerializeField]
    protected int m_RefCount = 0;
    //是否跳场景清掉
    public bool m_Clear = true;
    public int RefCount
    {
        get { return m_RefCount; }
        set
        {
            m_RefCount = value;
            if (m_RefCount < 0)
            {
                Debug.LogError("refcount < 0" + m_RefCount + " ," + (m_Obj != null ? m_Obj.name : "name is null"));
            }
        }
    }
}
