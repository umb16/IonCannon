﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace EMX.HierarchyPlugin.Editor
{


	partial class HighLighterCommonData : ScriptableObject
	{
		[SerializeField]
		List<ColorFilter> colorFilters = new List<ColorFilter>();


		const string TypeName = "HighLighterCommonData.asset";
		internal static Func<HighLighterCommonData> Instance = () =>
		{

			Folders.CheckFolders();
			return (Instance = () =>
			{
				if (_Instance) return _Instance;
				var g = EditorPrefs.GetInt(Folders.PREFS_PATH + "|HODbjGUID" + TypeName, -1);
				if (g != -1 && (InternalEditorUtility.GetObjectFromInstanceID(g) as HighLighterCommonData))
				{
					Folders.CheckFolders(true);
					return (_Instance = InternalEditorUtility.GetObjectFromInstanceID(g) as HighLighterCommonData);
				}

				var ASSET_PATH = Folders.PluginInternalFolder + "/Editor/_SAVED_DATA/" + TypeName;

				var loaded = AssetDatabase.LoadAssetAtPath<HighLighterCommonData>(ASSET_PATH);
				if (!loaded)
				{
					if (!Directory.Exists(Folders.PluginInternalFolder + "/Editor/_SAVED_DATA/")) Directory.CreateDirectory(Folders.PluginInternalFolder + "/Editor/_SAVED_DATA/");

					var preCache = ScriptableObject.CreateInstance<HighLighterCommonData>();
					preCache.hideFlags = HideFlags.DontSaveInBuild;

					if (File.Exists(ASSET_PATH)) File.Delete(ASSET_PATH);
					AssetDatabase.CreateAsset(preCache, ASSET_PATH);
			AssetDatabase.SaveAssets();
					//HierarchyExternalSceneData.SaveAssets();
					AssetDatabase.ImportAsset(ASSET_PATH, ImportAssetOptions.ForceUpdate);

					loaded = preCache;
				}

				EditorPrefs.SetInt(Folders.PREFS_PATH + "|HODbjGUID" + TypeName, loaded.GetInstanceID());
				return (_Instance = loaded);
			})();
		};
		static HighLighterCommonData _Instance;

		internal static void Undo(string text)
		{
			UnityEditor.Undo.RecordObject(Instance(), text);
		}
		internal static new void SetDirty()
		{
			EditorUtility.SetDirty(Instance());
			HierarchyExternalSceneData.SaveAssets();
			EdtiorPrefsStringSetDirty();
			//	SaveLibrary();

		}
		/*	void SaveLibrary()
			{
				if (_GetFakeClass[adapter.pluginID] == null) return;
				var result = new System.Text.StringBuilder();
				result.AppendLine("Initialized");
				result.AppendLine(_GetFakeClass[adapter.pluginID].Initialized.ToString());
				foreach (var item in _GetFakeClass[adapter.pluginID]._colorFilters) { result.AppendLine("_colorFilters"); item.SaveToString(ref result); } //0
				LibraryFolderManager.WriteLibraryFile("HighLighterAutoFilters", ref result);
			}
			*/



		/*[HideInInspector, SerializeField] List<string> lastIconSelect = new List<string>();
		[HideInInspector, SerializeField] List<Color32> _lasHiglightBackGroundColor = new List<Color32>();
		[HideInInspector, SerializeField] List<Color32> _lasHiglightTextColor = new List<Color32>();*/

		[NonSerialized] static bool hlcih_readed;
		[NonSerialized] static List<string> hlcih_IconsHistory;
		[NonSerialized] static List<Color32> hlcih_BackGroundColorsHistory;
		[NonSerialized] static List<Color32> hlcih_TextColorsHistory;
		static void EP__CheckLists()
		{
			if (hlcih_IconsHistory == null) hlcih_IconsHistory = new List<string>();
			if (hlcih_BackGroundColorsHistory == null) hlcih_BackGroundColorsHistory = new List<Color32>();
			if (hlcih_TextColorsHistory == null) hlcih_TextColorsHistory = new List<Color32>();
		}
	static	void EdtiorPrefsStringSetDirty()
		{
			if (!hlcih_readed) EdtiorPrefsStringRead();
			EP__CheckLists();
			var result = new System.Text.StringBuilder();
			foreach (var item in hlcih_IconsHistory) result.AppendLine(item); //
			result.AppendLine("---");
			foreach (var item in hlcih_BackGroundColorsHistory) result.AppendLine(ColorFilter.ColorToString(item)); //
			result.AppendLine("---");
			foreach (var item in hlcih_TextColorsHistory) result.AppendLine(ColorFilter.ColorToString(item)); //
			LibraryFolderManager.WriteLibraryFile("HighLighterColorsIconsHistory", ref result);
		}

		static List<string> IconsHistory { get { EdtiorPrefsStringRead(); return hlcih_IconsHistory; } set { EdtiorPrefsStringRead(); hlcih_IconsHistory = value; } }
		static List<Color32> BackGroundColorsHistory { get { EdtiorPrefsStringRead(); return hlcih_BackGroundColorsHistory; } set { EdtiorPrefsStringRead(); hlcih_BackGroundColorsHistory = value; } }
		static List<Color32> TextColorsHistory { get { EdtiorPrefsStringRead(); return hlcih_TextColorsHistory; } set { EdtiorPrefsStringRead(); hlcih_TextColorsHistory = value; } }
		static void EdtiorPrefsStringRead()
		{
			if (hlcih_readed) return;
			hlcih_readed = true;
			var f = LibraryFolderManager.ReadLibraryFile("HighLighterColorsIconsHistory");
			EP__CheckLists();
			var reader = new System.IO.StringReader(f);
			{
				string line;
				while ((line = reader.ReadLine()) != "---" && line != null) hlcih_IconsHistory.Add(line);
				while ((line = reader.ReadLine()) != "---" && line != null) hlcih_BackGroundColorsHistory.Add(ColorFilter.ColorFromString(line));
				while ((line = reader.ReadLine()) != null) hlcih_TextColorsHistory.Add(ColorFilter.ColorFromString(line));
			}
			reader.Dispose();
		}

		static List<Color32> DefaultTextColors()
		{
			var res = new List<Color32>()
		{   new Color32( 49, 58, 63, 255 ),
			  new Color32( 255, 77, 67, 255 ),
			  new Color32( 38, 42, 45, 255 ),
			  new Color32( 136, 202, 96, 255 ),
			  new Color32( 255, 111, 111, 255 ),
			  new Color32( 13, 66, 98, 255 ),
			  new Color32( 0, 204, 153, 255 ),
			  new Color32( 0, 101, 153, 255 ),
			  new Color32( 130, 181, 63, 255 ),
			  new Color32( 65, 139, 202, 255 ),
			  new Color32( 232, 119, 85, 255 ),
			  new Color32( 82, 62, 125, 255 ),
			  new Color32( 246, 126, 4, 255 ),
			  new Color32( 25, 168, 40, 255 ),
			  new Color32( 245, 186, 31, 255 ),
		};
			if (EditorGUIUtility.isProSkin) return res;
			return res.Select(c => (Color)c).Select(c => (Color32)new Color(1 - (1 - c.r) / 2, 1 - (1 - c.g) / 2, 1 - (1 - c.b) / 2, c.a)).ToList();
		}
		static List<Color32> DefaultBackgroundColors()
		{
			var res = new List<Color32>()
		{   new Color32( 58, 103, 100, 255 ),
			  new Color32( 245, 41, 78, 255 ),
			  new Color32( 98, 125, 196, 255 ),
			  new Color32( 207, 27, 39, 255 ),
			  new Color32( 225, 35, 69, 255 ),
			  new Color32( 54, 180, 70, 255 ),
			  new Color32( 241, 89, 16, 255 ),
			  new Color32( 45, 80, 112, 255 ),
			  new Color32( 226, 110, 33, 255 ),
			  new Color32( 1, 114, 132, 255 ),
			  new Color32( 137, 121, 96, 255 ),
			  new Color32( 61, 148, 139, 255 ),
			  new Color32( 149, 33, 44, 255 ),
			  new Color32( 224, 224, 224, 255 ),
			  new Color32( 1, 254, 211, 255 ),
		};
			if (EditorGUIUtility.isProSkin) return res;
			return res.Select(c => (Color)c).Select(c => (Color32)new Color(1 - (1 - c.r) / 2, 1 - (1 - c.g) / 2, 1 - (1 - c.b) / 2, c.a)).ToList();
		}



		internal static List<string> GetIconsHistory()
		{
			//var o = HighLighterCommonData.Instance();
			return IconsHistory;
		}
		internal static List<Color32> GetBackGroundColorsHistory()
		{
			//var o = HighLighterCommonData.Instance();
			if (BackGroundColorsHistory.Count < 1) BackGroundColorsHistory = DefaultBackgroundColors();
			return BackGroundColorsHistory;
		}
		internal static List<Color32> GetTextColorsHistory()
		{
			//var o = HighLighterCommonData.Instance();
			if (TextColorsHistory.Count < 1) TextColorsHistory = DefaultTextColors();
			return TextColorsHistory;
		}
		internal static List<ColorFilter> GetColorFilters
		{
			get { return HighLighterCommonData.Instance().colorFilters; }
			set { HighLighterCommonData.Instance().colorFilters = value; }
		}






		//	static ArrayPrefs[] _GetLastTempColor = new ArrayPrefs[2];

		internal static TempColorClass GetLastTempColor()
		{
			//EdtiorPrefsStringRead();
			/*var sl = new SingleList() { list = _GetLastTempColor[adapter.pluginID].Value };
			if (sl.list.Count < 5)
			{
				sl.list.AddRange(Enumerable.Repeat(0, 5));
			}
			var _result = new TempColorClass();
			_result.AssignFromList(sl);

	*/
			var _result = new TempColorClass();
			var load = Root.p[0].par_e.GET("HIGHLIGHTER_LAST_TEMP_COLOR", "");
			if (load != "") _result.SetFromString(load);
			if (!_result.HAS_BG_COLOR)
			{
				var lastBG = DefaultBackgroundColors();
				if (lastBG.Count == 0) _result.BGCOLOR = Color.white;
				else _result.BGCOLOR = lastBG[0];
				_result.BG_ALIGMENT_LEFT = (int)BgAligmentLeft.Fold;
				_result.BG_ALIGMENT_RIGHT = (int)BgAligmentLeft.EndLabel;
			}
			if (!_result.HAS_LABEL_COLOR)
			{
				var lastText = DefaultTextColors();
				if (lastText.Count == 0) _result.LABELCOLOR = Color.white;
				else _result.LABELCOLOR = lastText[0];
			}
			return _result;
		}

		internal static void SetLastTempColor(TempColorClass c)
		{
			//EdtiorPrefsStringRead();

			/*var a = new IntList() { list = c.ToList() };
			
		
			TempColorClass tempColor = new TempColorClass();
			tempColor.AssignFromList(new SingleList() { list = this.ToList() });
			var last = GetLastTempColor();

			if (!tempColor.HAS_BG_COLOR && last.HAS_BG_COLOR)
			{
				TempColorClass.CopyFromTo(CopyType.BG, from: last, to: ref tempColor);
			}

			if (!tempColor.HAS_LABEL_COLOR && last.HAS_LABEL_COLOR)
			{
				CopyFromTo(CopyType.LABEL, from: last, to: ref tempColor);
			}

			_GetLastTempColor[adapter.pluginID].Value = tempColor.ToList();
			*/
			Root.p[0].par_e.SET("HIGHLIGHTER_LAST_TEMP_COLOR", c.ConvertToString());

			//SetDirty();
		}

	}
}
