using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using EMX.HierarchyPlugin.Editor.Mods;



namespace EMX.HierarchyPlugin.Editor.Mods
{
	internal partial class HighlighterMod : DrawStackAdapter, IModSaver
	{

		PluginInstance adapter;
		public HighlighterAutoMode_Instance autoMod;
		public HighlighterManualMode_Instance manualMod;
		//public PresetManager_Instance presetManagerMod;

		internal HighlighterMod(PluginInstance p) : base(p.pluginID)
		{
			autoMod = new HighlighterAutoMode_Instance(p, this);
			manualMod = new HighlighterManualMode_Instance(p, this);
			//presetManagerMod = new PresetManager_Instance(p, this);
			adapter = p;
		}

		internal void Subscribe(EditorSubscriber sbs)
		{
			// sbs.OnPlayModeStateChanged += PlaymodeStateChanged;
			if (adapter.par_e.USE_MANUAL_HIGHLIGHTER_MOD || adapter.par_e.USE_AUTO_HIGHLIGHTER_MOD)
			{
				sbs.BuildedOnGUI_first += DrawBackgroundInFirstFrame;
				sbs.BuildedOnGUI_last_butBeforeGL += DrawLabelsInLastFrame;
				sbs.BuildedOnGUI_middle += prepare_hierarchy_draw;
				sbs.saveModsInterator.Add(this);

				autoMod.SubscribeAutoColorHighLighter(sbs);
				//manualMod.SubscribeAutoColorHighLighter(sbs);

				sbs.OnUndoAction += OnUndoAction;
				sbs.OnSceneOpening += OnUndoAction;
			}

			if (adapter.par_e.USE_CUSTOM_PRESETS_MOD)
			{
				//presetManagerMod.SubscribeAutoColorHighLighter(sbs);
			}

			HighlighterCache_Colors.ClearCacheAdditional();
		}

		void OnUndoAction()
		{
			HighlighterCache_Colors.ClearCacheAdditional();
		}





		internal override void ResetStack()
		{
			foreach (var nc in HighlighterDrawWorks.new_child_cache_dic)
			{
				nc.Value.wasLastAssign = false;
			}
			base.ResetStack();
		}



#pragma warning disable
		internal class DynamicColor { internal Func<HierarchyObject, object, Color> GET; }
#pragma warning restore
		internal TempColorClass __INTERNAL_TempColor_Empty = new TempColorClass().AddIcon(null);

		/*
		internal float DEFAULT_ICON_SIZE
		{
			get
			{
				float res;
				if (par.USEdefaultIconSize) res = par.defaultIconSize;
				else res = EditorGUIUtility.singleLineHeight;
				return res;
			}
		}*/

		GUIStyle __foldoutStyle;
		internal GUIStyle foldoutStyle
		{
			get { return __foldoutStyle ?? (__foldoutStyle = (GUIStyle)"IN Foldout"); }
		}
		internal float foldoutStyleWidth
		{
			get { return foldoutStyle.fixedWidth + 2; }
		}
		internal float foldoutStyleHeight
		{
			get { return foldoutStyle.fixedHeight != 0 ? foldoutStyle.fixedHeight : EditorGUIUtility.singleLineHeight; }
		}
		internal bool LocalIsSelected(int id)
		{
			if (!adapter.par_e.USE_MANUAL_HIGHLIGHTER_MOD) return false;
			return adapter.ha.IsSelected(id);
		}



		/*

		static TempColorClass temp = new TempColorClass().empty;
		static public TempColorClass String4ToColor(string[] res)
		{
			var list = String4ToList(res);
			var el = new SingleList();
			el.list = list;
			temp.AssignFromList(el);
			return temp;
		}

		static public List<int> String4ToList(string[] res)
		{   //byte[] result = new byte[res.Length];
			bool error = false;
			List<int> result = new List<int>();
			for (int i = 0; i < res.Length; i++)
			{
				int parse;
				if (!int.TryParse(res[i], out parse))
				{
					error = true;
					break;
				}
				result.Add(parse);
			}
			if (!error) return result;
			return null;
		}

		static public bool StringToBool(int index, string[] res)
		{
			if (res.Length <= index) return false;
			return res[index] == "1";
		}
		*/








		private void prepare_hierarchy_draw()
		{
			DrawButton(ref adapter.selectionRect, adapter.o);
			//  throw new NotImplementedException();
		}

		private void DrawBackgroundInFirstFrame()
		{
			if (adapter.EVENT.type == EventType.Layout) return;

			if (adapter.EVENT.type == EventType.Repaint)
			{

				var selectionRect = adapter.selectionRect;
				manualMod.DrawBackgroundInFirstFrame(selectionRect);
			}
			else //Events
			{

			}

			// throw new NotImplementedException();
		}

		private void DrawLabelsInLastFrame()
		{

			if (adapter.EVENT.type == EventType.Repaint)
			{

				var selectionRect = adapter.selectionRect;
				manualMod.DrawLabelsInLastFrame(selectionRect);
			}

		}


		public bool LoadFromString(string s, HierarchyObject o)
		{
			/*	TempSceneObjectPTR data = HierarchyTempSceneDataGetter.GetObjectData(SaverType.ModManualHighligher, o);
				if (data == null) data = new TempSceneObjectPTR(o.go, -1);
				Array.Resize(ref data.highLighterData, 1);
				data.highLighterData[0].Load(s);
				HierarchyTempSceneDataGetter.SetObjectData(SaverType.ModManualHighligher, o, data);*/

			if (s == null || s == "") return false;
			var split = s.Split('½');
			if (split.Length != 2) return false;
			if (split[0] != "")
			{
				TempSceneObjectPTR data = HierarchyTempSceneDataGetter.GetObjectData(SaverType.ModManualHighligher, o);
				if (data == null) data = new TempSceneObjectPTR(o.go, -1);
				if (data.highLighterData.Length < 1)
				{
					Array.Resize(ref data.highLighterData, 1);
					data.highLighterData[0] = new HighlighterExternalData();
				}
				data.highLighterData[0].Load(split[0]);
				HierarchyTempSceneDataGetter.SetObjectData(SaverType.ModManualHighligher, o, data, true);
			}
			if (split[1] != "")
			{
				TempSceneObjectPTR data = HierarchyTempSceneDataGetter.GetObjectData(SaverType.ModManualIcons, o);
				if (data == null) data = new TempSceneObjectPTR(o.go, -1);
				if (data.iconData.Length < 1)
				{
					Array.Resize(ref data.iconData, 1);
					data.iconData[0] = new IconExternalData();
				}
				data.iconData[0].Load(split[1]);
				HierarchyTempSceneDataGetter.SetObjectData(SaverType.ModManualIcons, o, data, true);
			}
			return true;
		}

		List<SaverType> _GetSaverTypes = new List<SaverType>() { SaverType.ModManualHighligher, SaverType.ModManualIcons };
		List<SaverType> IModSaver.GetSaverTypes { get { return _GetSaverTypes; } }
		public bool SaveToString(HierarchyObject o, ref string result)
		{
			TempSceneObjectPTR data = HierarchyTempSceneDataGetter.GetObjectData(SaverType.ModManualHighligher, o);
			bool changed = false;

			if (data != null && data.highLighterData.Length != 0)
			{
				var save = data.highLighterData[0].Save();
				if (save != null)
				{
					result = save;
					changed |= true;
				}
			}
			result += '½';
			data = HierarchyTempSceneDataGetter.GetObjectData(SaverType.ModManualIcons, o);
			if (data != null && data.iconData.Length != 0)
			{
				var save = data.iconData[0].Save();
				if (save != null)
				{
					result += save;
					changed |= true;
				}
			}
			return changed;
			// throw new NotImplementedException();
		}

		internal TempColorClass GetSavedColor(HierarchyObject h)
		{
			return null;
		}

		int colorProperty = Shader.PropertyToID("_Color");
		internal void DRAW_BGTEXTURE_OLD(Rect rect, Color32 color)
		{
			if (Event.current.type == EventType.Repaint)
			{
				var border = adapter.par_e.HIGHLIGHTER_TEXTURE_BORDER;
				if (!adapter.par_e.HIGHLIGHTER_USE_SPECUAL_SHADER || !adapter.par_e.HIghlighterExternalMaterial || adapter.par_e.HIGHLIGHTER_TEXTURE_STYLE == 0 || !adapter.par_e.BG_TEXTURE)
				{
					//Adapter.DrawTexture(rect, adapter.par_e.BG_TEXTURE ?? Texture2D.whiteTexture, ScaleMode.ScaleToFit, true, 1, color, border, 0);
					GUI.DrawTexture(rect, adapter.par_e.BG_TEXTURE ?? Texture2D.whiteTexture, ScaleMode.ScaleToFit, true, 1, color, border, 0);
				}
				else
				{
					adapter.par_e.HIghlighterExternalMaterial.SetColor(colorProperty, color);
					Graphics.DrawTexture(rect, adapter.par_e.BG_TEXTURE, border, border, border, border, adapter.par_e.HIghlighterExternalMaterial, 0);
				}
			}
		}

		internal TempColorClass DrawBackground(object p1, object p2, DynamicRect tempDynamicRect, HierarchyObject oBJECT, int v, bool resetFonts)
		{
			return null;
		}












	}
}
