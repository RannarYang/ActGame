/*
 * @Author       : RannarYang
 * @Date         : 2021-04-25 21:52:25
 * @LastEditors  : RannarYang
 * @LastEditTime : 2021-04-26 14:55:08
 * @FilePath     : \Client\Assets\Base\Res\AssetBundleConfig.cs
 */
using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
public class AssetBundleConfig
{
    [XmlElement("ABList")]
    public List<ABBase> ABList { get; set; }
}

[System.Serializable]
public class ABBase
{
    [XmlAttribute("Path")]
    public string Path{ get; set; }
    [XmlAttribute("Crc")]
    public uint Crc { get; set; }
    [XmlAttribute("ABName")]
    public string ABName { get; set; }
    [XmlAttribute("AssetName")]
    public string AssetName { get; set; }
    [XmlElement("ABDependce")]
    public List<string> ABDependce { get; set; }
}
