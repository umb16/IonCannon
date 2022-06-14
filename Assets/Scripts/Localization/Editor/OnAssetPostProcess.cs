using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;
using Object = UnityEngine.Object;

public class OnAssetPostProcess : AssetPostprocessor
{
    private const string PATH = "/Scripts/Generated/Localization";
    private const string TEMPLATE_PATH = "Assets/Locales/CodeGeneration/Editor/Template.cs.txt";

    private static string GetFullPath(string fileName) => $"{Application.dataPath}{PATH}{fileName}.cs";

    private static void GenerateTableAccessorFile(SharedTableData table, string fileName)
    {

        var genPath = GetFullPath(fileName);
        using var writer = new StreamWriter(genPath, false);
        writer.WriteLine(GenerateFileContent(table));
        AssetDatabase.ImportAsset($"Assets{PATH}{fileName}.cs");
    }

    private static string KeyToCSharp(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentOutOfRangeException(nameof(key), "Translation key cannot be empty or null.");
        if (char.IsNumber(key[0])) key = $"_{key}";
        key = key.Replace(" ", "_");
        return $"@{key}";
    }

    private static bool IsSmart(StringTableCollection tableCollection, long id)
    {
        if (tableCollection == null) return false;
        return tableCollection.StringTables
            .Select(stable => stable.GetEntry(id))
            .Where(x=>x!=null)
            .Any(tableEntry => tableEntry.IsSmart);
    }

    private static string GenerateFileContent(SharedTableData table)
    {
        var tableCollection = LocalizationEditorSettings
                .GetStringTableCollection(table.TableCollectionNameGuid);
        var template = AssetDatabase.LoadAssetAtPath<TextAsset>(TEMPLATE_PATH).text;
        var sb = new StringBuilder();

        foreach (var entry in table.Entries)
        {
            var key = KeyToCSharp(entry.Key);
            sb.Append($"\t\t\tpublic static LocalizedString {key}");
            //if (IsSmart(tableCollection, entry.Id)) sb.Append("(List<object> o)");
            //sb.Append($" => LocalizationSettings.StringDatabase.GetLocalizedString(NAME, {entry.Id});");
            sb.Append(" => new LocalizedString() { TableReference = NAME, TableEntryReference = \""+ entry.Key + "\" };");
            sb.Append(Environment.NewLine);
        }
        return string.Format(
            template, DateTime.Now, KeyToCSharp(table.TableCollectionName), table.TableCollectionName, sb);
    }

    private static string GetFileName(string fileName) =>
            Path.GetFileNameWithoutExtension(fileName).Replace(" ", "_");

    private static void OnPostprocessAllAssets(
        string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (var path in importedAssets)
        {
            var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (obj is not SharedTableData tableData) continue;
            GenerateTableAccessorFile(tableData, GetFileName(path));
        }

        foreach (var path in deletedAssets)
        {
            var fileName = GetFileName(path);
            if (File.Exists(GetFullPath(fileName))) File.Delete(GetFullPath(fileName));
        }
    }
}
