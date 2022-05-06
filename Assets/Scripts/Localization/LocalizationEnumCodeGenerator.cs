using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Text;
#if UNITY_EDITOR
public class LocalizationEnumCodeGenerator : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (var assetPath in importedAssets)
        {
            if (assetPath.Contains("Resources/Localization") && assetPath.Contains("ru-RU.txt"))
            {
                string path = "Assets/Scripts/Localization/AutoGeneratedLocKeys.cs";

                string textEnum = "//autogenerated\npublic enum LocKeys\n{\n";
                string textClass = "public static class LocKeyConverter\n{\n" +
                                  "    public static string Convert(LocKeys key)\n" +
                                  "    {\n" +
                                  "        switch (key)\n" +
                                  "        {\n";
                string switchBody = "";
                using (StreamReader sr = File.OpenText(assetPath))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] values = s.Split("=");
                        if (values.Length != 2)
                            continue;
                        var key = values[0];
                        var value = values[1];
                        textEnum += $"    //{value}\n" +
                                    $"    {key},\n";
                        switchBody += $"            case LocKeys.{key}:\n" +
                                     $"                return \"{key}\";\n";
                    }
                }
                textClass += switchBody;
                textClass += "                default: return \"\";\n" +
                             "        }\n" +
                             "    }\n" +
                             "}";
                textEnum += "}\n" + textClass;

                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(textEnum);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
                AssetDatabase.Refresh();
                Debug.Log("imported " + assetPath);
            }
        }
    }
}
#endif