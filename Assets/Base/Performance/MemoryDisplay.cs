using System.Net;
/*
 * @Author       : RannarYang
 * @Date         : 2021-04-26 18:02:59
 * @LastEditors  : RannarYang
 * @LastEditTime : 2021-04-27 11:31:54
 * @FilePath     : \Client\Assets\Base\Performance\MemoryDisplay.cs
 */
using System;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;

// 显示资源相关类
public class MemoryDisplay : MonoSingleton<MemoryDisplay>
{
	// 对象池相关信息 ------------------------------------------------------------
	[SerializeField]
	StringIntDictionary m_PoolData = new StringIntDictionary();
	public IDictionary<string, int> PoolData
	{
		get { return m_PoolData; }
		set { m_PoolData.CopyFrom (value); }
	}

	// AB包相关信息 ---------------------------------------------------------------
	[SerializeField]
	StringAssetBundleItemDictionary m_Name2AssetBundleItem = new StringAssetBundleItemDictionary();
	public IDictionary<string, AssetBundleItem> Name2AssetBundleItem
	{
		get { return m_Name2AssetBundleItem; }
		set { m_Name2AssetBundleItem.CopyFrom (value); }
	}
	
	[SerializeField]
	StringResourceItemDictionary m_Name2ResourceItem = new StringResourceItemDictionary();
	public IDictionary<string, ResourceItem> Name2ResourceItem
	{
		get { return m_Name2ResourceItem; }
		set { m_Name2ResourceItem.CopyFrom (value); }
	}

	// 资源管理器相关信息 ---------------------------------------------------------------
	[SerializeField]
	StringIntDictionary m_Name2AssetRefCount = new StringIntDictionary();
	public IDictionary<string, int> Name2AssetRefCount
	{
		get { return m_Name2AssetRefCount; }
		set { m_Name2AssetRefCount.CopyFrom (value); }
	}

	public List<string> NoRefrenceAssetsName;

	// 对象管理器相关信息 ------------------------------------------------------------------
	[SerializeField]
	StringResourceObjArrayDictionary m_UnRecycleResource = new StringResourceObjArrayDictionary();
	public IDictionary<string, ResourceObj[]> UnRecycleResource
	{
		get { return m_UnRecycleResource; }
		set { m_UnRecycleResource.CopyFrom (value); }
	}

	[SerializeField]
	StringIntDictionary m_UnSpawnObject = new StringIntDictionary();
	public IDictionary<string, int> UnSpawnObject
	{
		get { return m_UnSpawnObject; }
		set { m_UnSpawnObject.CopyFrom (value); }
	}

	private void Start()
	{
		UpdateData();
	}

	public void UpdateData ()
	{
		UpdatePoolData();
		UpdateABManagerData();
		UpdateResourceManagerData();
		UpdateObjectManagerData();
	}

	/// <summary>
	/// 更新对象池数据
	/// </summary>
	private void UpdatePoolData() {
		Dictionary<Type, object> dic = PoolManager.Instance.GetPoolDic();
		Dictionary<string, int> type2Count = new Dictionary<string, int>();
		foreach (KeyValuePair<Type, object> kv in dic) {
			var pool = kv.Value;
			// 通过反射调用
			MethodInfo methodInfo = pool.GetType().GetMethod("GetCount");
			int count = (int)methodInfo.Invoke(pool, null);
			type2Count.Add(kv.Key.Name, count);
		}
		PoolData = type2Count;
	}

	private void UpdateABManagerData() {
		Dictionary<uint, ResourceItem> resouceItemDic = AssetBundleManager.Instance.ResouceItemDic;
		Dictionary<string, ResourceItem> name2ResourceItem = new Dictionary<string, ResourceItem>();
		foreach (KeyValuePair<uint, ResourceItem> kv in resouceItemDic) {
			name2ResourceItem.Add(kv.Value.m_AssetName, kv.Value);
		}
		Name2ResourceItem = name2ResourceItem;

		Dictionary<uint, AssetBundleItem> abItemDic = AssetBundleManager.Instance.AssetBundleItemDic;
		Dictionary<string, AssetBundleItem> name2ABItem = new Dictionary<string, AssetBundleItem>();
		foreach (KeyValuePair<uint, AssetBundleItem> kv in abItemDic) {
			name2ABItem.Add(kv.Value.assetBundle.name, kv.Value);
		}
		Name2AssetBundleItem = name2ABItem;
	}

	private void UpdateResourceManagerData() {
		Dictionary<uint, ResourceItem> assetDic = ResourceManager.Instance.AssetDic;
		Dictionary<string, int> name2AssetRefCount = new Dictionary<string, int>();
		foreach (KeyValuePair<uint, ResourceItem> kv in assetDic) {
			name2AssetRefCount.Add(kv.Value.m_AssetName, kv.Value.RefCount);
		}
		Name2AssetRefCount = name2AssetRefCount;
		
		NoRefrenceAssetsName = ResourceManager.Instance.GetNoRefrenceAssetsName();
	}

	private void UpdateObjectManagerData() {
		Dictionary<int, ResourceObj> resouceObjDic = ObjectManager.Instance.GetResourceObjDic();
		Dictionary<string, List<ResourceObj>> unRecycleResource = new Dictionary<string, List<ResourceObj>>();
		Dictionary<string, int> unSpawnObject = new Dictionary<string, int>();
		foreach(KeyValuePair<int, ResourceObj> kv in resouceObjDic) {
			string key = kv.Value.m_ResItem.m_AssetName;
			if(!kv.Value.m_Already) {
				if(!unRecycleResource.TryGetValue(key, out List<ResourceObj> resourceObjList) || resourceObjList == null) {
					resourceObjList = new List<ResourceObj>();
					unRecycleResource.Add(key, resourceObjList);
				}

				resourceObjList.Add(kv.Value);
			} else {
				if(!unSpawnObject.TryGetValue(key, out int count)) {
					unSpawnObject[key] = 1;
				} else {
					unSpawnObject[key] = count + 1;
				}
			}
		}
		
		Dictionary<string, ResourceObj[]> unRecycleResourceArray = new Dictionary<string, ResourceObj[]>();
		foreach(KeyValuePair<string, List<ResourceObj>> kv in unRecycleResource) {
			unRecycleResourceArray.Add(kv.Key, kv.Value.ToArray());
		}
		this.UnRecycleResource = unRecycleResourceArray;
		this.UnSpawnObject = unSpawnObject;
		
	}
		
}