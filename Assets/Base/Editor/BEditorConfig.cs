using UnityEditor;
using UnityEngine;
public static class BEditorConfig {
    // Build APP ---------------------------------------------------------------------------
    public static string AndroidPath = Application.dataPath + "/../BuildTarget/Android/";
    public static string IOSPath = Application.dataPath + "/../BuildTarget/IOS/";
    public static string WindowsPath = Application.dataPath + "/../BuildTarget/Windows/";
    public static string BuildNameTxtPath = Application.dataPath + "/../buildname.txt";
    
    public static string BuildBundleTargetPath {
        get {
            return Application.dataPath.Replace("Assets", "AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString());
        }
    }

    public static string BundleTargetPath {
        get {
            return Application.streamingAssetsPath;
        }
    }

    // 配置表 -----------------------------------------------------------------------------
    //打包时生成AB包配置表的二进制路径
    public static string ABBytePath = "Assets/App/Config/ABData/AssetBundleConfig.bytes";
    //xml文件夹路径
    public static string XmlPath = "Assets/App/Config/Xml/";
    //二进制文件夹路径
    public static string BinaryPath = "Assets/App/Config/Binary/";
    // Excel 文件路径
    public static string ExcelPath = Application.dataPath + "/../Data/Excel/";
    // Reg 文件路径
    public static string m_RegPath = Application.dataPath + "/../Data/Reg/";
    // 测试数据
    public static string TestReadXmlPath = Application.dataPath + "/../Data/Reg/MonsterData.xml";
    public static string TestWriteExcel = Application.dataPath + "/../Data/Excel/G怪物.xlsx";

    // ------------------------------------
    public static string AssetBundleConfigXmlPath = Application.dataPath + "/AssetbundleConfig.xml";
    // ABConfig.asset的路径
    public static string ABConfigPath = "Assets/Base/BaseConfig/ABConfig.asset";
    // 特殊目录 ----------------------------------------------------------------
    public static string PrefabPath = "Assets/App/Res/Prefabs";
    public static string PrefabPathFilePath = "Assets/App/ConstString/PrefabPath.cs";

    public static string AudioPath = "Assets/App/Res/Audios";
    public static string AudioPathFilePath = "Assets/App/ConstString/AudioPath.cs";
}