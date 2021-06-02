using UnityEngine;
public abstract class AppBase : MonoSingleton<AppBase>{
    public Transform RecyclePoolTrs;
    public Transform SceneTrs;
    protected sealed override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init() {
        GameObject.DontDestroyOnLoad(gameObject);
        // 资源管理
        AssetBundleManager.Instance.LoadAssetBundleConfig();
        ResourceManager.Instance.Init(this);
        ObjectManager.Instance.Init(RecyclePoolTrs, SceneTrs);
        // 配置管理
        LoadConfiger();

        // 调用子类的OnAwake;
        OnAwake();
    }

    //加载配置表
    protected abstract void LoadConfiger();

    protected abstract void OnAwake();


    private void Start()
    {
        this.OnStart();
    }

    protected abstract void OnStart();

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        ResourceManager.Instance.ClearCache();
        Resources.UnloadUnusedAssets();
        Debug.Log("清空编辑器缓存");
#endif
    }

    
}