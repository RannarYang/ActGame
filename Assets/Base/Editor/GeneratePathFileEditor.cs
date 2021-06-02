using System.Text;
using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
public class GeneratePathFileEditor : MonoBehaviour
{
    public static string PrefabPath = BEditorConfig.PrefabPath;
    public static string PrefabNameFilePath = BEditorConfig.PrefabPathFilePath;

    public static string AudioPath = BEditorConfig.AudioPath;
    public static string AudioNameFilePath = BEditorConfig.AudioPathFilePath;

    [MenuItem("Tools/路径表生成/Prefabs")]
    public static void GeneratePrefabPathFile() {
        string[] allStr = AssetDatabase.FindAssets("t:Prefab", new string[] {PrefabPath});
        List<string> paths = new List<string>();

        for (int i = 0; i < allStr.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(allStr[i]);
            paths.Add(path);
        }
        GeneratePathFile(PrefabNameFilePath, "PrefabPath", paths);
    }

    [MenuItem("Tools/路径表生成/Audios")]
    public static void GenerateAudioPathFile() {
        string[] allStr = AssetDatabase.FindAssets("", new string[] {AudioPath});
        List<string> paths = new List<string>();

        for (int i = 0; i < allStr.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(allStr[i]);
            if(path.EndsWith(".mp3") || path.EndsWith(".wav")) {
                paths.Add(path);
            }
        }
        GeneratePathFile(AudioNameFilePath, "AudioPath", paths);
    }

    private static void GeneratePathFile(string filePath, string fileName, List<string> paths)
    {
        Dictionary<string, string> name2path = new Dictionary<string, string>();
        foreach(string path in paths) {
            string nameWithExt = path.Substring(path.LastIndexOf("/") + 1);
            string name = nameWithExt.Remove(nameWithExt.LastIndexOf("."));
            if (name2path.ContainsKey(name))
            {
                Debug.LogError($"文件名重复了：{name}");
                return;
            }
            name2path.Add(name, path);
        }
        // 生成文件
        try {
            if(File.Exists(filePath)) {
                File.Delete(filePath);
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite)) {
                using (StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8)) {
                    StringBuilder content = new StringBuilder(300);
                    content.Append("public static class " + fileName + "  {");
                    content.AppendLine();
                    foreach (KeyValuePair<string, string> kv in name2path) {
                        content.Append($"\tpublic const string {kv.Key} = \"{kv.Value}\";");
                        content.AppendLine();
                    }
                    content.Append("}");
                    sw.Write(content.ToString());
                }
            }
            Debug.Log($"生成文件成功, 路径为：{filePath}");
        } catch(Exception e) {
            Debug.LogError(e);
        }

    }
}
