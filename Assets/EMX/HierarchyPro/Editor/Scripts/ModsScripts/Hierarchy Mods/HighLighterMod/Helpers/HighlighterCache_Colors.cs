using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;



namespace EMX.HierarchyPlugin.Editor.Mods
{
	enum GetIconRectIfNextToLabelType
	{
		CustomIcon,
		DefaultIcon
	}

	internal struct HighlighterResult
	{
		internal int BroadCast;
		internal int id;
		internal int[] data;

		internal bool IsTrue(bool CheckBroadCast)
		{
			if (CheckBroadCast) return BroadCast != -1 && id != -1;
			else return id != -1;
		}

		static HighlighterResult _CreateResult;
		internal static HighlighterResult CreateResult(int brct, int id, int[] data)
		{
			_CreateResult.id = id;
			_CreateResult.data = data;
			_CreateResult.BroadCast = brct;
			return _CreateResult;
		}
	}

	class HighlighterCache_Colors
	{

		static PluginInstance adapter { get { return Root.p[0]; } }
		static HighlighterMod HL_MOD { get { return Root.p[0].modsController.highLighterMod; } }




		//static internal Dictionary<int, IntList> IconColorCacher = new Dictionary<int, IntList>();
		//static internal Dictionary<int, Int32List> IconImageCacher = new Dictionary<int, Int32List>();


		//internal static Action additionalClear = null;

		internal static void ClearCacheAdditional()
		{
			//addIconChecherCache.Clear();
			//cached_colors.Clear();
			HighlighterDrawWorks.ClearGroupingCache();

			cacheDichighlighter.Clear();
			HighlighterCache_Icons.new_perfomance_onlycaher_icons.Clear();
			HighlighterCache_Icons.new_perfomance_includecaher_icons.Clear();

			//if (IconColorCacher != null) IconColorCacher.cacheDic.Clear();
			//if (additionalClear != null) additionalClear();

			//Hierarchy.ClearM_CustomIconsCache(); // nulll and missings
			//HighlighterCache_Icons.icon_cacher.Clear(); //IconExternalData

			HL_MOD.ResetStack();
		}
		internal static void ClearByObject(HierarchyObject o)
		{
			//HighlighterDrawWorks.ClearGroupingCache();
			o.ah.filterAssigned = false; // replaced clear group

			cacheDichighlighter.Remove(o.id);
			HighlighterCache_Icons.new_perfomance_onlycaher_icons.Remove(o.id);
			HighlighterCache_Icons.new_perfomance_includecaher_icons.Remove(o.id);

			HL_MOD.ResetStack();
		}











		static HighlighterResult HighlighterResultOut = new HighlighterResult() { id = -1 };
		static HighlighterResult HighlighterResultFalse = new HighlighterResult() { id = -1 };



		//static Dictionary<int, Dictionary<int, HighlighterResult>> cacheDichighlighter = new Dictionary<int, Dictionary<int, HighlighterResult>>();
		static Dictionary<int, HighlighterResult> cacheDichighlighter = new Dictionary<int, HighlighterResult>();
		//static Dictionary<int, HighlighterResult> tr_g_k;
		//  internal  bool anyNeedBroadcast = false;
		static internal HighlighterResult HighlighterHasKey(HierarchyObject _o)
		{
			if (!_o.Validate() || !adapter.par_e.USE_MANUAL_HIGHLIGHTER_MOD) return HighlighterResultFalse;

			/*	if (!cacheDichighlighter.TryGetValue(_o.scene, out tr_g_k))
				{
					cacheDichighlighter.Add(_o.scene, tr_g_k = new Dictionary<int, HighlighterResult>());
				}*/
			/*	var data = HierarchyTempSceneDataGetter.GetAllObjectData(SaverType.ModManualHighligher, _o.go.scene);
					for (int i = 0; i < data.Count; i++)
					{
						tr_g_k.Add(r, new HighlighterResult()
						{ PTR = i, BroadCast = h2[i].list.Count > 4 && h2[i].list[4] == 1 ? _o.id : -1 });
					}*/

			if (!cacheDichighlighter.TryGetValue(_o.id, out HighlighterResultOut))
			{
				var data = HierarchyTempSceneDataGetter.GetObjectData(SaverType.ModManualHighligher, _o);
				if (data == null || data.highLighterData.Length == 0 || data.highLighterData[0].TempColorData.Length == 0)
				{
					cacheDichighlighter.Add(_o.id, HighlighterResultFalse);
				}
				else
				{
					var list = data.highLighterData[0].TempColorData;
					cacheDichighlighter.Add(_o.id, HighlighterResult.CreateResult(_o.id, list.Length > 4 && list[4] == 1 ? _o.id : -1, data.highLighterData[0].TempColorData));
				}
			}
			return HighlighterResultOut;
		}

		static internal HighlighterResult HighlighterHasKey_IncludeFilters(HierarchyObject _o)
		{
			if (!_o.Validate()) return HighlighterResultFalse;
			var manual = HighlighterHasKey(_o);
			if (manual.id != -1) return manual;


			var filtered = HL_MOD.autoMod.GetFilter(_o);
			if (filtered != null && (filtered.HAS_BG_COLOR || filtered.HAS_LABEL_COLOR))
			{
				HighlighterResultOut.id = -2;
				HighlighterResultOut.BroadCast = filtered.child ? _o.id : -1;
				HighlighterResultOut.data = filtered.ToList();
				return HighlighterResultOut;
			}
			return HighlighterResultFalse;
		}




		//	[Obsolete("Use HighlighterHasKey")]
		static internal int[] GetHighlighterValue(HierarchyObject o)
		{
			var res = HighlighterCache_Colors.HighlighterHasKey(o);
			if (!res.IsTrue(false)) return null;
			return res.data;

			/*var res = HighlighterCache_Colors.HighlighterHasKey(o);
			if (!res.IsTrue(false)) return null;
			var d = adapter.MOI.des(scene);
			if (d == null) return null;
			var ptr = cacheDichighlighter[scene][o].PTR;
			if (ptr == -1) return null;
			if (ptr >= d.GetHash6().Count)
			{
				if (loop)
				{
					adapter.logProxy.LogWarning("GetHighlighterValue Out Of Range please contact support");
				}

				loop = true;
				cacheDichighlighter.Clear();
				return GetHighlighterValue(scene, o);
			}

			loop = false;
			return d.GetHash6()[ptr];*/
			//return null;
		}
		static internal void SetHighlighterValue(TempColorClass c, HierarchyObject _o) // var o = _o.go;
		{
			if (!_o.Validate()) return;
			/*	var d = adapter.MOI.des(s);
				if (d == null) return;
				var list = getDoubleList(s);*/

			if (c == null || (!c.HAS_BG_COLOR && !c.HAS_LABEL_COLOR))
			{
				//list.RemoveAll(SetPair(_o));
				HierarchyTempSceneDataGetter.RemoveObjectData(SaverType.ModManualHighligher, _o);
			}
			else
			{

				TempSceneObjectPTR data = HierarchyTempSceneDataGetter.GetObjectData(SaverType.ModManualHighligher, _o);
				if (data == null) data = new TempSceneObjectPTR(_o.go, -1);
				if (data.highLighterData.Length < 1)
				{
					Array.Resize(ref data.highLighterData, 1);
					data.highLighterData[0] = new HighlighterExternalData();
				}
				data.highLighterData[0].TempColorData = c.ToList();
				HierarchyTempSceneDataGetter.SetObjectData(SaverType.ModManualHighligher, _o, data);

				/*	var value = new SingleList() { list = c.ToList() };
					var p = SetPair(_o);
					if (list.ContainsKey(p)) list[p] = value;
					else list.Add(p, value);*/
			}
			//if (SaveRegistrator) adapter.DescriptionModule.TrySaveHiglighterRegistrator(_o, c);

			/*if (!Application.isPlaying)
			{  
				adapter.SetDirtyDescription(d, d.gameObject ? d.gameObject.scene.GetHashCode() : adapter.GET_ACTIVE_SCENE);
				adapter.MarkSceneDirty(d.gameObject ? d.gameObject.scene.GetHashCode() : adapter.GET_ACTIVE_SCENE);
			}*/

			ClearCacheAdditional();
			adapter.RepaintWindowInUpdate();
		}






















	}













}
