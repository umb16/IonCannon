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
using EMX.HierarchyPlugin.Editor.Windows;

namespace EMX.HierarchyPlugin.Editor.Mods
{
	internal partial class HighlighterMod : DrawStackAdapter, IModSaver, ISearchable
	{


		public override bool callFromExternal() { return callFromExternal_objects != null; }
		public Windows.SearchWindow.FillterData_Inputs callFromExternal_objects { get; set; }
		public Type typeFillter { get; set; }
		public string SearchHelper { get { return "Highlighter"; } set { } }
		public virtual float GetInputWidth() { return -1; }
		internal Event EVENT { get { return callFromExternal() ? Event.current : adapter.EVENT; } }

		public void DrawSearch(Rect rect, HierarchyObject o)
		{
		}





		bool ValidateWithoutNulls(HierarchyObject o)
		{
			//	if (adapter.IS_PROJECT()) return IconImageCacher.HasKey(o);
			return HighlighterCache_Icons.GetUnityContent(o, false) != null;
		}

		bool ValidateIncludeNulls(HierarchyObject o)
		{
			//if (adapter.IS_PROJECT()) return true;
			return HighlighterCache_Icons.INTERNAL_GetContent(o, true).add_icon;
		}


		/*
		void RefreshNullsAndMissings()
		{

			if (adapter.IS_HIERARCHY())
			{
				foreach (var allSceneObject in Utilities.AllSceneObjects())
				{
					var id = allSceneObject.GetInstanceID();

					// var comps = allSceneObject.GetComponents<Component>();
					var comps = HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll(allSceneObject);


					if (adapter.par.SHOW_NULLS && comps.Length == 1)
					{
						if (!Hierarchy.null_cache.ContainsKey(id)) Hierarchy.null_cache.Add(id, false);

						Hierarchy.null_cache[id] = true;
					}

					if (adapter.par.SHOW_MISSINGCOMPONENTS && comps.Any(c => !c))
					{
						if (!Hierarchy.missing_cache.ContainsKey(id)) Hierarchy.missing_cache.Add(id, false);

						Hierarchy.missing_cache[id] = true;
					}
				}
			}

		}*/




		Windows.SearchWindow.FillterData_Inputs m_CallHeader()
		{
			Func<HierarchyObject, TempColorClass> gettexture = null;
			//GetImageForObject_OnlyCacher;
			/* if ( adapter.IS_PROJECT() ) gettexture = INTERNAL_GetContent( b , false );
			 else*/
			gettexture = (b) => HighlighterCache_Icons.INTERNAL_GetContent(b, false);

			var result = new Windows.SearchWindow.FillterData_Inputs(callFromExternal_objects)
			{
				Valudator = ValidateWithoutNulls,
				SelectCompareString = (b, i) =>
				{
					var r = gettexture(b).add_icon;

					if (r == null) return "";

					return r.name;
				},
				SelectCompareCostInt = (b, i) =>
				{
					var cost = i;
					cost += b.Active() ? 0 : 100000000;
					var c = gettexture(b).add_icon;

					if (c != null && c.name.StartsWith("GUID=")) cost += 1000000; ////////////////////////////////////

					return cost;
				}
			};
			return result;
		}

		internal  Windows.SearchWindow.FillterData_Inputs CallHeader()
		{
			//if (adapter.par.SHOW_NULLS || adapter.par.SHOW_MISSINGCOMPONENTS) RefreshNullsAndMissings();

			return m_CallHeader();
		}

		internal Windows.SearchWindow.FillterData_Inputs CallHeaderFiltered(Texture contentTexture)
		{
			Func<HierarchyObject, bool> gettexture = null;
			/* if ( adapter.IS_PROJECT() ) gettexture = s => GetImageForObject_OnlyCacher( s ).add_icon == contentTexture;
			 else gettexture = s => ValidateIncludeNulls( s ) && INTERNAL_GetContent( s , true ).add_icon == contentTexture;*/

			//if (adapter.IS_PROJECT()) gettexture = s => INTERNAL_GetContent(s, true).add_icon == contentTexture;
		//else 
				gettexture = s => ValidateIncludeNulls(s) && HighlighterCache_Icons.INTERNAL_GetContent(s, true).add_icon == contentTexture;

			//if (adapter.IS_HIERARCHY())
			{
				/*if (adapter.par.SHOW_NULLS && contentTexture == adapter.GetIcon("NULL") ||
						adapter.par.SHOW_MISSINGCOMPONENTS && contentTexture == adapter.GetIcon("WARNING"))
					RefreshNullsAndMissings();*/
			}

			var result = m_CallHeader();
			result.Valudator = gettexture;
			return result;
		}



		

	

		/** CALL HEADER */
	}
}
