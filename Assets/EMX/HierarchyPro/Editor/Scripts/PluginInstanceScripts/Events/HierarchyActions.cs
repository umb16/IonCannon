using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;



namespace EMX.HierarchyPlugin.Editor.Events
{
	partial class HierarchyActions
	{
		int pluginID;
		EditorWindow window { get { return Root.p[pluginID].window.Instance; } }
		internal HierarchyActions(int pId)
		{
			pluginID = pId;

			if (pluginID == 1)
			{
				m_SearchFilterString = Root.p[1].SceneHierarchyWindow.GetField("m_SearchFilter", ~(BindingFlags.Static | BindingFlags.InvokeMethod));
				m_SearchFilterClass_Has = m_SearchFilterString.FieldType.GetMethod("IsSearching", ~(BindingFlags.Static | BindingFlags.SetField));
			}
			if (pluginID == 0)
			{
				SearchableWindowType = Assembly.GetAssembly(typeof(EditorWindow)).GetType("UnityEditor.SearchableEditorWindow");
				m_SearchFilterString = SearchableWindowType.GetField("m_SearchFilter", ~(BindingFlags.Static | BindingFlags.InvokeMethod));
				showingPrefabHeader = Root.p[0].SceneHierarchyWindowRoot.GetProperty("showingPrefabHeader", ~(BindingFlags.Static | BindingFlags.InvokeMethod));
				hasShowingPrefabHeader = showingPrefabHeader != null;
#if UNITY_2020_1_OR_NEWER
				showingPrefabHeader = null;
				hasShowingPrefabHeader = true;
#endif


			}

		}







		//SearchFilter
		bool? seach_baked;
		FieldInfo m_SearchFilterString;
		MethodInfo m_SearchFilterClass_Has;
		PropertyInfo showingPrefabHeader;
		Type SearchableWindowType;
		internal bool hasShowingPrefabHeader;

		internal void BAKE_SEARCH()
		{
			if (m_SearchFilterString == null)
			{
				seach_baked = false;
				return;
			}

			if (pluginID == 0)
			{
				seach_baked = !string.IsNullOrEmpty((string)m_SearchFilterString.GetValue(window));
				return;
			}

			seach_baked = (bool)m_SearchFilterClass_Has.Invoke(m_SearchFilterString.GetValue(window), null);
		}


		internal bool IS_SEARCH_MOD_OPENED()
		{
			// if ( ChechButton( _S_HideBttomIfNoFunction ) ) return true;

			if (!seach_baked.HasValue) BAKE_SEARCH();

			return seach_baked.Value;
		}

		internal bool IS_PREFAB_MOD_OPENED()
		{

#if UNITY_2020_1_OR_NEWER
			return UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null;
#else
            if ( hasShowingPrefabHeader && showingPrefabHeader.GetValue( window, null ).Equals( true ) ) return true;
            return false;
#endif
		}


		// bool? oldIS_SEARCH_MODE_OR_PREFAB_OPENED;

		internal bool IS_SEARCH_MODE_OR_PREFAB_OPENED()
		{
			var res = __IS_SEARCH_MODE_OR_PREFAB_OPENED();
			// if ( !oldIS_SEARCH_MODE_OR_PREFAB_OPENED.HasValue ) oldIS_SEARCH_MODE_OR_PREFAB_OPENED = res;
			// if ( oldIS_SEARCH_MODE_OR_PREFAB_OPENED.Value != res ) RedrawInit = true;
			return res;
		}

		bool __IS_SEARCH_MODE_OR_PREFAB_OPENED()
		{ // return false;

			if (!seach_baked.HasValue) BAKE_SEARCH();
			if (hasShowingPrefabHeader && showingPrefabHeader.GetValue(window, null).Equals(true)) return true;
			return seach_baked.Value;
		}

		object inst, em;
		MethodInfo meth;
		List<Type> AllTypesOfIRepository;

		internal void CLOSE_PREFAB_MODE()
		{
			if (!hasShowingPrefabHeader) return;
			if (showingPrefabHeader == null) return;
			if (!showingPrefabHeader.GetValue(window, null).Equals(true)) return;

			if (AllTypesOfIRepository == null)
				AllTypesOfIRepository = (from x in Assembly.GetAssembly(typeof(EditorWindow)).GetTypes()
										 let y = x.BaseType
										 where !x.IsAbstract && !x.IsInterface &&
											   y != null && y.IsGenericType &&
											   y.GetGenericTypeDefinition() == typeof(ScriptableSingleton<>)
										 select x
					).ToList();

			/*GUI_ONESHOTPUSH( () =>
            {   UnityEditor.SceneManagement.StageUtility.GoBackToPreviousStage();
            } );*/
			Root.p[pluginID].PUSH_GUI_ONESHOT(() =>
			 {
				 foreach (var asd in AllTypesOfIRepository)
				 {
					 if (asd.Name == "StageNavigationManager")
					 {
						 if (meth == null)
						 {
							 inst = asd.BaseType.GetProperty("instance", (BindingFlags)(-1)).GetValue(null, null);
							 meth = inst.GetType().GetMethod("NavigateBack", (BindingFlags)(-1));
							 em = Enum.Parse(meth.GetParameters()[0].ParameterType, "NavigateBackViaHierarchyHeaderLeftArrow"); //NavigateBackViaUnknown  NavigateBackViaHierarchyHeaderLeftArrow NavigateViaBreadcrumb
							}

						 meth.Invoke(inst, new[] { em });
					 }
				 }
			 });
			Tools.EventUseFast();

			// SendEventAll( new Event() { type = EventType.Layout, mousePosition = Vector2.zero, button = 0 } );
		}
	}
}
