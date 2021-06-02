/*
 * @Author       : RannarYang
 * @Date         : 2021-04-25 21:52:25
 * @LastEditors  : RannarYang
 * @LastEditTime : 2021-04-26 14:55:30
 * @FilePath     : \Client\Assets\Base\Editor\Resource\ABConfig.cs
 */
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ABConfig", menuName = "CreatABConfig", order = 0)]
public class ABConfig : ScriptableObject
{
    //单个文件所在文件夹路径，会遍历这个文件夹下面所有Prefab,所有的Prefab的名字不能重复，必须保证名字的唯一性
    public List<string> m_AllPrefabPath = new List<string>();
    public List<FileDirABName> m_AllFileDirAB = new List<FileDirABName>();

    [System.Serializable]
    public struct FileDirABName
    {
        public string ABName;
        public string Path;
    }
}
