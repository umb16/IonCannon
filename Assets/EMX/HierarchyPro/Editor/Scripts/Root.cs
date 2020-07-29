using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace EMX.HierarchyPlugin.Editor
{

	[InitializeOnLoad]
	public class Root
	{
		
		public const string PN = "Hierarchy PRO";
		public const string PN_NS = "HierarchyPlugin";
		public const string CUST_NS = "CustomizationHierarchy";
		public const string PN_FOLDER = Folders.PN_FOLDER;
		static PluginInstance[] __p = null;
		internal static PluginInstance[] p
		{
			get
			{
				if (!created || __p == null) CREATE();
				return __p;
			}
		}
		static bool created = false;

		static Icons _icons = null;
		static internal Icons icons { get { return _icons ?? (_icons = AssetDatabase.LoadAssetAtPath<Icons>(Folders.PluginInternalFolder + "/Editor/Icons/IconsArray.asset")); } }
		static bool wasInit = false;
		static Root()
		{
			CREATE();
		}
		static void CREATE()
		{
			if (created) return;
			created = true;
			Folders.CheckFolders();
			Settings.MainSettingsEnabler_Window.CheckSettings();
			__p = new PluginInstance[2];
			__p[0] = PluginInstance.CreateInstance("Hierarchy");
			__p[0].par_e = new EditorSettingsAdapter(0);
			if (!__p[0].par_e.ENABLE_ALL)
			{
				return;
			}
			Init();
			//p[0].par_e
		}
		static void Init()
		{
			if (wasInit) return;
			wasInit = true;
			InitializeRoot();

			var hierarchyPlugin = PluginInstance.CreateInstance("Hierarchy");
			p[0] = hierarchyPlugin;
			// p[ 1 ] = hierarchyPlugin;
			for (int i = 0; i < p.Length; i++) if (p[i] != null) p[i].Init(i);
			ENABLE(true);

			if (EditorPrefs.GetInt(Folders.PREFS_PATH + "|showWelcome", 0) != 1)
			{
				EditorPrefs.SetInt(Folders.PREFS_PATH + "|showWelcome", 1);
				WelcomeScreen.Init(null);
			}
		}


		internal static void SET_EXTERNAl_MOD(Mods.ExternalModRoot mod)
		{
			if (!p[0].par_e.ENABLE_ALL) return;
			p[0].modsController.externalMods.RemoveAll(m => !m || m.currentWindow == mod.currentWindow);
			p[0].modsController.externalMods.Add(mod);
			p[0].modsController.REBUILD_PLUGINS(true);
		}
		internal static void REMOVE_EXTERNAl_MOD(Mods.ExternalModRoot mod)
		{
			if (!p[0].par_e.ENABLE_ALL) return;
			p[0].modsController.externalMods.RemoveAll(m => !m || m.currentWindow == mod.currentWindow);
			p[0].modsController.externalMods.Remove(mod);
			p[0].modsController.REBUILD_PLUGINS(true);
		}

		static bool enabled = false;
		internal static void DISABLE()
		{
			if (!enabled) return;
			enabled = false;
			foreach (var item in Root.p[0].modsController.externalMods.ToList())
			{
				if (!item) continue;
				item.Close();
			}
			var sbs = new EditorSubscriber();
			Root.p[0].REBUILD_EDITOR_EVENTS(sbs);

			foreach (var item in p)
			{
				if (item == null) continue;
				EditorApplication.hierarchyWindowItemOnGUI -= item.gui;
				EditorApplication.update -= item.Update;
				EditorSceneManager.sceneOpened -= item.invoke_EditorSceneManagerOnSceneOpening;
				Selection.selectionChanged -= item.invoke_onSelectionChanged;
				EditorApplication.hierarchyChanged -= item.invoke_onHierarchyChanged;
				EditorApplication.playModeStateChanged -= item.invoke_onPlayModeStateChanged;
				UnityEditor.Undo.undoRedoPerformed -= item.invoke_unUndo;
				EditorApplication.projectWindowItemOnGUI -= item.invoke_OnProjectWindow;
				EditorApplication.wantsToQuit -= item.invoke_OnEditorWantsToQuit;
#if !UNITY_2017_1_OR_NEWER
                SceneView.onSceneGUIDelegate -= item.invoke_DuringSceneGUI;
#else
				SceneView.duringSceneGui -= item.invoke_DuringSceneGUI;
#endif
				EditorApplication.modifierKeysChanged -= item.invoke_ModifiyKeyChanged;
				var info = typeof(EditorApplication).GetField("globalEventHandler", (BindingFlags)(-1));
				var value = (EditorApplication.CallbackFunction)info.GetValue(null);
				value -= item.EditorGlobalKeyPress;
				info.SetValue(null, value);
			}
			if (Mods.SnapMod.SET_ENABLE(Root.p[0].par_e.USE_SNAP_MOD && Root.p[0].par_e.ENABLE_ALL))
				EditorUtility.RequestScriptReload();
		}


		internal static void ENABLE(bool skipReload = false)
		{
			if (enabled) return;
			Init();
			enabled = true;
			foreach (var item in p)
			{
				if (item == null) continue;
				EditorApplication.hierarchyWindowItemOnGUI += item.gui;
				EditorApplication.update += item.Update;
				EditorSceneManager.sceneOpened += item.invoke_EditorSceneManagerOnSceneOpening;
				//  EditorSceneManager.activeSceneChangedInEditMode += item.invoke_EditorSceneManagerOnSceneOpening2;
				//  EditorSceneManager.sce += item.invoke_EditorSceneManagerOnSceneOpening3;
				Selection.selectionChanged += item.invoke_onSelectionChanged;
				EditorApplication.hierarchyChanged += item.invoke_onHierarchyChanged;
				EditorApplication.playModeStateChanged += item.invoke_onPlayModeStateChanged;
				UnityEditor.Undo.undoRedoPerformed += item.invoke_unUndo;
				EditorApplication.projectWindowItemOnGUI += item.invoke_OnProjectWindow;
				EditorApplication.wantsToQuit += item.invoke_OnEditorWantsToQuit;
#if !UNITY_2017_1_OR_NEWER
                SceneView.onSceneGUIDelegate += item.invoke_DuringSceneGUI;
#else
				SceneView.duringSceneGui += item.invoke_DuringSceneGUI;
#endif
				EditorApplication.modifierKeysChanged += item.invoke_ModifiyKeyChanged;
				var info = typeof(EditorApplication).GetField("globalEventHandler", (BindingFlags)(-1));
				var value = (EditorApplication.CallbackFunction)info.GetValue(null);
				value += item.EditorGlobalKeyPress;
				info.SetValue(null, value);
			}
			Root.p[0].modsController.REBUILD_PLUGINS();
			if (Mods.SnapMod.SET_ENABLE(Root.p[0].par_e.USE_SNAP_MOD && Root.p[0].par_e.ENABLE_ALL))
				if (!skipReload) EditorUtility.RequestScriptReload();
		}


		internal static PropertyInfo GUIView_current, View_window, View_position, View_windowPosition, DockArea_selected, HostView_actualView;
		internal static Type DockArea;
		internal static FieldInfo DockArea_m_Panes;
		internal static Type UnityEventArgsType;
		internal static MethodInfo SceneFrameMethod;
		
		static MethodInfo _SetMouseTooltip;

		static void InitializeRoot()
		{
			var GUIView = Assembly.GetAssembly(typeof(EditorGUIUtility)).GetType("UnityEditor.GUIView") ?? throw new Exception("GUIView");
			GUIView_current = GUIView.GetProperty("current", ~(BindingFlags.Instance | BindingFlags.InvokeMethod)) ?? throw new Exception("current");
			View_window = GUIView.BaseType.GetProperty("window", ~(BindingFlags.Static | BindingFlags.InvokeMethod)) ?? throw new Exception("window");
			View_position = GUIView.BaseType.GetProperty("position", ~(BindingFlags.Static | BindingFlags.InvokeMethod)) ?? throw new Exception("position");
			View_windowPosition = GUIView.BaseType.GetProperty("windowPosition", ~(BindingFlags.Static | BindingFlags.InvokeMethod)) ?? throw new Exception("windowPosition");
			DockArea = Assembly.GetAssembly(typeof(EditorGUIUtility)).GetType("UnityEditor.DockArea") ?? throw new Exception("DockArea");
			DockArea_m_Panes = DockArea.GetField("m_Panes", ~(BindingFlags.Static | BindingFlags.InvokeMethod)) ?? throw new Exception("m_Panes");
			DockArea_selected = DockArea.GetProperty("selected", ~(BindingFlags.Static | BindingFlags.InvokeMethod)) ?? throw new Exception("selected");
			UnityEventArgsType = Assembly.GetAssembly(typeof(UnityEngine.Events.UnityEvent)).GetType("UnityEngine.Events.ArgumentCache", true) ?? throw new Exception("ArgumentCache");
			SceneFrameMethod = (typeof(SceneView).GetMethod("Frame", ~(BindingFlags.Static | BindingFlags.GetField))) ?? throw new Exception("Frame");
			_SetMouseTooltip = (typeof(GUIStyle).GetMethod("SetMouseTooltip", ~(BindingFlags.Instance | BindingFlags.GetField))) ?? throw new Exception("SetMouseTooltip");
			var HostView = Assembly.GetAssembly(typeof(EditorGUIUtility)).GetType("UnityEditor.HostView") ?? throw new Exception("HostView");
			HostView_actualView = HostView.GetProperty("actualView", ~(BindingFlags.Static | BindingFlags.InvokeMethod)) ?? throw new Exception("actualView");
			Window.InitFields();

			
		}

		static object[] _setTooltipArgs = new object[2];
		internal static void SetMouseTooltip(string content, Rect rect)
		{
			_c.tooltip = content;
			SetMouseTooltip(_c, rect);
		}
		static GUIContent _c = new GUIContent();
		internal static void SetMouseTooltip(GUIContent content, Rect rect)
		{

			if (content.tooltip == null || content.tooltip == "") return;

			if (!rect.Contains(Root.p[0].EVENT.mousePosition) || !Root.p[0].GUIClip_visibleRect.Contains(Root.p[0].EVENT.mousePosition)) return;

			_setTooltipArgs[0] = content.tooltip;
			_setTooltipArgs[1] = rect;
			_SetMouseTooltip.Invoke(null, _setTooltipArgs);
		}

	}



	internal class StyleInitHelper
	{
		public static implicit operator bool(StyleInitHelper h)
		{
			return h.value == true && h.proSkin == EditorGUIUtility.isProSkin;
		}
		public static implicit operator StyleInitHelper(bool h)
		{
			return new StyleInitHelper() { proSkin = PluginInstance.WAS_GUI_FLAG ? EditorGUIUtility.isProSkin : (bool?)null, value = h };
		}
		internal bool? value = null;
		internal bool? proSkin = null;
	}


	internal partial class PluginInstance
	{

		internal int pluginID;
		internal HierarchyObject o;
		internal Window window;
		internal bool UseRootWindow;
		internal string pluginname;

		internal Events.HierarchyActions ha;
		internal EditorSettingsAdapter par_e;
		DuplicateHelper duplicate;
		WindowsManager windowsManager;
		internal HierarchyModification hierarchyModification;
		internal ModsController modsController;
		internal GlDrawer gl;




		internal MethodInfo gui_getFirstAndLastRowVisible, data_FindItem_Slow,
						gui_GetRowRect, ExpansionAnimator_CullRow, data_m_dataSetExpanded, data_m_dataIsExpanded, data_m_dataSetExpandWithChildrens, hierwin_DuplicateGO;
		FieldInfo _AssetTreeState, _FolderTreeState, _TreeViewController, _FoldView, _AssetsView, _ViewMode, /*gui_m_LineHeight, gui_k_IndentWidth, gui_k_IconWidth,  gui_customFoldoutYOffset,*/   tree_m_ContentRect, m_UseExpansionAnimation
				// ,tree_m_KeyboardControlIDField
				;
		internal FieldInfo state_scrollPos, tree_m_ExpansionAnimator, m_SearchFieldText, tree_m_TotalRect, m_SearchFilter;
		internal MethodInfo data_GetRows, PrefabModeButton, data_GetItemRowFast, IsSearching,
#pragma warning disable
		GetInstanceIDFromGUID, GetMainAssetOrInProgressProxyInstanceID;
#pragma warning restore
		internal int hoverID;
		bool _hashoveredItem;
		internal bool hashoveredItem
		{
			get { return _hashoveredItem && !par_e.HIDE_HOVER_BG; }
			set { _hashoveredItem = value; }
		}
		internal PropertyInfo _data, _gui, _state, hoveredItem, showPrefabModeButton, tree_animatingExpansion, ExpansionAnimator_endRow, data_rowCount;
		PropertyInfo data_m_RootItem, guiclip_visibleRect;
		object[] args = new object[2];
		int firstRowVisible, lastRowVisible;

		// ???
		// ???
		// ???
		internal Rect _currentClipRect;
		// ???
		// ???
		// ???

		internal IconData GetNewIcon(NewIconTexture t, string key) { return Root.icons.GetNewIcon(t, ref key); }
		internal IconData GetOldIcon(string s) { return Root.icons.GetOldIcon(ref s); }
		internal Texture2D GetExternalModOld(string s) { return Root.icons.GetOldExternalMod(ref s); }
		//  delegate void GetFirstAndLastRowVisible( out int firstRowVisible, out int lastRowVisible );
		//  GetFirstAndLastRowVisible gui_getFirstAndLastRowVisible;
		//(GetFirstAndLastRowVisible)Delegate.CreateDelegate( typeof( GetFirstAndLastRowVisible ), this,


		internal bool NEW_PERFOMANCE { get { return pluginID == 0; } }
		// return UNITY_CURRENT_VERSION >= UNITY_2019_VERSION;



		internal static PluginInstance CreateInstance(string name)
		{
			var res = new PluginInstance();
			res.pluginname = name;
			return res;
		}
		internal void Init(int pId)
		{
			pluginID = pId;
			Init();
			par_e = new EditorSettingsAdapter(pId);
			window = new Window();
			ha = new Events.HierarchyActions(pId);
			duplicate = new DuplicateHelper(pId);
			windowsManager = new WindowsManager(pId);
			_mouse_uo_helper = new Events.MouseRawUp(this);
			hierarchyModification = new HierarchyModification(this);
			modsController = new ModsController(this);
			gl = new GlDrawer(this);


			modsController.REBUILD_PLUGINS(true);
		}




		//FIELDS-
		object[] argsa = new object[1];
		internal EventType? lastEvent, fixDrawLastEvent;
		internal Event EVENT;
		internal int firstFrame;
		int index = 0;
		int num = 0;
		int numVisibleRows = 0;
		int rowCount;
		internal Vector2 scrollPos;
		bool animatingExpansion;
		object m_ExpansionAnimator;
		internal Rect m_TotalRect, m_ContentRect, GUIClip_visibleRect;
		int endRow;
		float rowWidth;
		int CalcRow(ref int index, ref int num)
		{
			int row = -1;
			for (; index < numVisibleRows; ++index)
			{
				row = firstRowVisible + index;
				if (this.animatingExpansion)
				{
					// int endRow = this.m_ExpansionAnimator.endRow;
					//  if ( this.m_ExpansionAnimator.CullRow( row, this.gui ) )
					args[0] = row;
					args[1] = gui_currentTree;
					var res = (bool)ExpansionAnimator_CullRow.Invoke(m_ExpansionAnimator, args);
					if (res)
					{
						++num;
						row = endRow + num;
					}
					else
						row += num;
					// if ( row >= this.data.rowCount )
					if (row >= rowCount)
						continue;
				}
				else
				{
					args[0] = row;
					args[1] = 0f;
					var res = (Rect)gui_GetRowRect.Invoke(gui_currentTree, args);
					if ((double)(res.y - scrollPos.y) > m_TotalRect.height)
						continue;
				}
				break;
			}
			return row;
		}
		// -

		//EVENTS-
		internal void PUSH_ONMOUSEUP(Action ac, EditorWindow win = null) { _mouse_uo_helper.PUSH_ONMOUSEUP(ac, win); }
		Events.MouseRawUp _mouse_uo_helper;
		internal void PUSH_GUI_ONESHOT(Action ac)
		{
			bool allow = false;
			foreach (var w in hierarchyWindows) if (w.Value.w) allow = true;
			if (!allow)
			{
				ac();
				return;
			}
			_oneShotGui.Add(ac);
			RepaintWindowInUpdate();
		}
		List<Action> _oneShotGui = new List<Action>();
		internal void PUSH_UPDATE_ONESHOT(Action ac) { _oneShotUpdate.Add(ac); }
		List<Action> _oneShotUpdate = new List<Action>();
		// -

		// OTHER
		bool? oldProSkin;
		// --

		//MAIN GUI-
		internal object TreeController_current;
		internal object gui_currentTree, data_currentTree, state_currentTree;
		internal IList<int> current_DragSelection_List = new List<int>();
		internal IList<int> current_selectedIDs = new List<int>();

		bool GuiReady = true;
		internal Rect selectionRect;
		internal Rect fullLineRect, _first_FullLineRect, _last_FullLineRect;
		internal float rightOffset = 0;

		internal int drew_mods_count = 0;
		internal HierarchyObject[] drew_mods_objects = new HierarchyObject[200];
		internal class EditorWindowData
		{
			internal EditorWindow w;
			internal object lastTree;
			internal bool nowSearch;
		}
		internal static Dictionary<int, EditorWindowData> hierarchyWindows = new Dictionary<int, EditorWindowData>();
		void CHECK_ONESHOT_GUI()
		{
			if (_oneShotGui.Count > 0 && Event.current.type == EventType.Repaint)
			{
				foreach (var item in _oneShotGui) item();
				_oneShotGui.Clear();
			}
		}
		static bool firstFixDraw = false;
		static bool firstFixDrawStart = false;
		internal bool HoverDisabled = false;
		bool shouldBuildedBeginGUI = false;

		internal int MOUSE_BUTTON_0 { get { return par_e.USE_SWAP_FOR_BUTTONS_ACTION ? 1 : 0; } }
		internal int MOUSE_BUTTON_1 { get { return par_e.USE_SWAP_FOR_BUTTONS_ACTION ? 0 : 1; } }

		internal static bool WAS_GUI_FLAG = false;
		// bool sended = false;
		internal void gui(int instanceID, Rect selectionRect)
		{

			// if ( Event.current.type == EventType.Layout ) return;
			if (!WAS_GUI_FLAG) WAS_GUI_FLAG = true;
			this.selectionRect = selectionRect;



			EVENT = Event.current;
			var _go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;



			if (GuiReady/* || !_go*/  || lastEvent != EVENT.type)
			{

				_mouse_uo_helper.Invoke();
				modsController.rightModsManager.headerEventsBlockRect = null;
				HoverDisabled = false;
				CHECK_ONESHOT_GUI();

				if (!oldProSkin.HasValue) oldProSkin = EditorGUIUtility.isProSkin;
				if (oldProSkin != EditorGUIUtility.isProSkin) Cache.ClearHierarchyObjects();

				if (firstFrame != 5) firstFrame++;
				GuiReady = true;
				lastEvent = EVENT.type;

				Window.AssignInstance(this, ref window); // WINDOW INIT
				TreeController_current = GetTreeViewontroller();
				gui_currentTree = _gui.GetValue(TreeController_current);
				data_currentTree = _data.GetValue(TreeController_current);
				state_currentTree = _state.GetValue(TreeController_current);

				hierarchyModification.TryToInitializeDefaultStyles();
				animatingExpansion = (bool)tree_animatingExpansion.GetValue(TreeController_current, null);
				if (animatingExpansion)
				{
					m_ExpansionAnimator = tree_m_ExpansionAnimator.GetValue(TreeController_current);
					endRow = (int)ExpansionAnimator_endRow.GetValue(m_ExpansionAnimator, null);
					/*if ( !sended && lastEvent.Value == EventType.Layout)
					{
							EditorGUIUtility.ExitGUI();
							window.Instance.SendEvent( new Event() { type = lastEvent.Value } );
					}
					sended = true;*/
				}
				m_TotalRect = (Rect)tree_m_TotalRect.GetValue(TreeController_current);
				m_ContentRect = (Rect)tree_m_ContentRect.GetValue(TreeController_current);
				GUIClip_visibleRect = (Rect)guiclip_visibleRect.GetValue(null, null);
				if (!hierarchyWindows.ContainsKey(window.Instance.GetInstanceID()))
				{
					hierarchyWindows.Add(window.Instance.GetInstanceID(), new EditorWindowData() { w = window.Instance, lastTree = TreeController_current, nowSearch = false });
					window.SetHeightAndIndents(this); // WINDOW SET
				}
				ha.BAKE_SEARCH();
				if (!ReferenceEquals(hierarchyWindows[window.Instance.GetInstanceID()].lastTree, TreeController_current) || hierarchyWindows[window.Instance.GetInstanceID()].nowSearch != ha.IS_SEARCH_MOD_OPENED())
				{
					hierarchyWindows[window.Instance.GetInstanceID()].lastTree = TreeController_current;
					hierarchyWindows[window.Instance.GetInstanceID()].nowSearch = ha.IS_SEARCH_MOD_OPENED();
					window.SetHeightAndIndents(this); // WINDOW SET
				}

				args[0] = args[1] = 0;
				gui_getFirstAndLastRowVisible.Invoke(gui_currentTree, args);
				firstRowVisible = (int)args[0];
				lastRowVisible = (int)args[1];
				numVisibleRows = lastRowVisible - firstRowVisible + 1;

				scrollPos = (Vector2)state_scrollPos.GetValue(state_currentTree);
				rowCount = (int)data_rowCount.GetValue(data_currentTree);
				rowWidth = Mathf.Max(GUIClip_visibleRect.width, m_ContentRect.width);
				// currentRow = firstRowVisible;
				index = 0; num = 0;

				args[0] = lastRowVisible;
				args[1] = 0f;
				_last_FullLineRect = (Rect)gui_GetRowRect.Invoke(gui_currentTree, args);

				hierarchyModification.UpdateBGHover();

				window.CheckMouseEvents();

			}
			else
			{
				index++;
				//currentRow++;
			}

			if (!firstFixDraw)
			{
				firstFixDraw = true;
				firstFixDrawStart = true;
				fixDrawLastEvent = EVENT.type;
			}
			if (firstFixDrawStart)
			{
				if (fixDrawLastEvent != EVENT.type)
				{
					firstFixDrawStart = false;
				}
				else
				{
					return;
				}
			}

			int row = CalcRow(ref index, ref num);
			var fakeIndex = index + 1;
			var fakeNum = num;
			CalcRow(ref fakeIndex, ref fakeNum);
			bool thisIsLast = fakeIndex == numVisibleRows || row == lastRowVisible;
			argsa[0] = row;
			// Debug.Log( row + " " + " " + numVisibleRows + " " + thisIsLast );
			var currentTreeItemFast = data_GetItemRowFast.Invoke(data_currentTree, argsa);
			args[0] = row;
			args[1] = rowWidth;
			fullLineRect = (Rect)gui_GetRowRect.Invoke(gui_currentTree, args);
			// lineRect.width = selectionRect.x + selectionRect.width;
			///fullLineRect.width = rowWidth;
			/// 
			//  selectionRect.y = fullLineRect.y;
			fullLineRect.y = selectionRect.y;


			if (GuiReady)
			{

				//modsController.toolBarModification. hotButtons.DrawButtonsOnTopBar();
				//modsController.toolBarModification.layoutsMod.DrawLayers();
				// _first_SelectionRect = selectionRect;
				//Debug.Log( EVENT.type + " " + fullLineRect + " " +selectionRect );
				_first_FullLineRect = fullLineRect;
				if (!firstFixDraw)
				{

					firstFixDraw = true;
					//EditorGUIUtility.ExitGUI();
					//window.Instance.SendEvent( new Event() { type =  EventType.Layout } );
					//window.Instance.SendEvent( new Event() { type =  EventType.Repaint } );
					////return;
					/*var s = GUI.skin.verticalScrollbarUpButton.fixedWidth;
					fullLineRect.width -=s;
					rowWidth -=s;
					_last_FullLineRect.width -=s;
					_last_FullLineRect.width -=s; */
					//RepaintWindow( true );
				}
				GuiReady = false;
				rightOffset = 0;
				if (ha.hasShowingPrefabHeader) hierarchyModification.prebapButtonStyle.margin.right = 0;
				///if ( par_e.RIGHT_RIGHT_PADDING_AFFECT_TO_SETACTIVE_AND_KEEPER && par_e.USE_RIGHT_ALL_MODS ) 
				rightOffset += hierarchyModification.PREFAB_BUTTON_SIZE;
				ButtonsActionsDetect();
				ha.OnSelectionChanged_SaveCache();

				InternalOnGUI_first();
				o = null;
				shouldBuildedBeginGUI = true;
				if (BuildedOnGUI_first != null) BuildedOnGUI_first();
				drew_mods_count = 0;
			}


			//  Debug.Log(row + " " + lineRect);
			//selectionRect.height = lineRect.height = 16;
			// Debug.Log(lineRect.y);
			if (!_go || selectionRect.height <= 0 /*|| animatingExpansion && par_e.DISABLE_DRAWING_ANIMATING_ITEMS*/) // THIS IS SCENE
			{
				//EditorUtility.InstanceIDToObject( instanceID ) as SceneAsset;
				//return;
				goto end;
			}

			if (shouldBuildedBeginGUI)
			{

				shouldBuildedBeginGUI = false;
			}

			o = Cache.GetHierarchyObjectByInstanceID(instanceID, _go);
			o._visibleTreeItem = currentTreeItemFast as UnityEditor.IMGUI.Controls.TreeViewItem;
			o.lastFullLineRect = fullLineRect;
			o.lastSelectionRect = selectionRect;

			if (drew_mods_count > drew_mods_objects.Length) Array.Resize(ref drew_mods_objects, drew_mods_objects.Length + 100);
			drew_mods_objects[drew_mods_count] = o;
			drew_mods_count++;

			// Debug.Log( row );
			// if ( row == 1 || lineRect.y != 0 ) 
			// GUI.BeginClip( selectionRect );
			// selectionRect.x -= lineRect.x;
			// selectionRect.y = 0;
			//  lineRect.x = lineRect.y = 0;
			//OnGUI();
			if (BuildedOnGUI_middle != null && EVENT.type != EventType.Layout) BuildedOnGUI_middle();
			// GUI.EndClip();


			end:;
			EditorActions_EveryFrame_AfterOnGUI();
			if (thisIsLast)
			{
				GuiReady = true;
				CHECK_ONESHOT_GUI();
				if (BuildedOnGUI_last_butBeforeGL != null && EVENT.type != EventType.Layout) BuildedOnGUI_last_butBeforeGL();
				gl.ReleaseStack();
				if (BuildedOnGUI_last != null && EVENT.type != EventType.Layout) BuildedOnGUI_last();
				// sended = false;
			}

			if (HoverDisabled) ha.internal_DisableHover();

			// Debug.Log( GetTreeItem( instanceID ) );
			// current._visibleTreeItem =
		}


		internal Action BuildedOnGUI_first;
		internal Action BuildedOnGUI_middle;
		internal Action BuildedOnGUI_last;
		internal Action BuildedOnGUI_last_butBeforeGL;


		void InternalOnGUI_first()
		{

			//Init
			Colors.UpdateColorsBefore_OnGUI(this);

			//RESET_DRAW_STACKS-
			if (window.position.width != _drawStack_oldWPos.width || window.position.height != _drawStack_oldWPos.height)
			{
				_drawStack_oldWPos = window.position;
				_drawStack_reset_stacks();
			}
			if (!_drawStack_lastCacheClean.HasValue) _drawStack_lastCacheClean = EditorApplication.timeSinceStartup;
			if (Math.Abs(_drawStack_lastCacheClean.Value - EditorApplication.timeSinceStartup) > DRAWSTACK_RESET_TIME)
			{
				_drawStack_RESET_TIMER_DRAWSTACKS();
				// Debug.Log( Math.Abs( lastCacheClean.Value - EditorApplication.timeSinceStartup ) );
				_drawStack_lastCacheClean = EditorApplication.timeSinceStartup;
			}
			if (EVENT.type == EventType.Layout && (_drawStack_resetStacks || _drawStack_resetTimerStack))
			{
				_drawStack_reset_stacks();
				_drawStack_resetStacks = false;
				_drawStack_resetTimerStack = false;
			}
			//-

			//ANIMATION EXPAND
			if (par_e.USE_EXPANSION_ANIMATION != (bool)m_UseExpansionAnimation.GetValue(TreeController_current))
				m_UseExpansionAnimation.SetValue(TreeController_current, par_e.USE_EXPANSION_ANIMATION);


			//DUPLICATE
			duplicate.Duplicate_FirstFrame_OnGUI();


			//HOVER LINES
			if (hashoveredItem && !HoverDisabled)
			{

				if (windowsManager.InputWindowsCount() > 0) ha.internal_DisableHover();
				/*if ( GetNavigatorRect( 10000 ).y - HEIGHT < EVENT.mousePosition.y )
		DISABLE_HOVER();*/
				var h = hoveredItem.GetValue(TreeController_current, null) as UnityEditor.IMGUI.Controls.TreeViewItem;
				//hoveredItem.SetValue(tree, null, null);
				if (h != null) hoverID = h.id;
				else hoverID = -1;
			}
			else
			{
				hoverID = -1;
			}
		}




		// DRAW_STACK
		const double DRAWSTACK_RESET_TIME = 4;
		double? _drawStack_lastCacheClean;
		Rect _drawStack_oldWPos;
		bool _drawStack_resetStacks = false, _drawStack_resetTimerStack = false;
		internal void RESET_DRAWSTACK()
		{
			if (_OnResetDrawStack != null) _OnResetDrawStack();
			_drawStack_resetStacks = true;

			//#EMX_TODO check for a little optimization
			RepaintWindowInUpdate();

#if EMX_HIERARCHY_DEBUG_STACKS
		Debug.Log( "RESET_DRAW_STACKS" );
#endif
		}
		void _drawStack_RESET_TIMER_DRAWSTACKS()
		{ //  __reset_stacks();
			_drawStack_resetTimerStack = true;
		}
		void _drawStack_reset_stacks()
		{

			modsController.RESET_DRAW_STACKS();
			// foreach ( var m in internalMods ) m.Value.ResetDrawStack();
			// foreach ( var m in externalMods ) m.Value.ResetDrawStack();
			/*for ( int i = 0 ; i < modules.Length ; i++ )
			{
					foreach ( var stack in modules[ i ].DRAW_STACK )
					{
							stack.Value.ResetStack();
					}
			}*/
		}
		// -





		/// <summary>
		/// #############################################################################################################################################################
		/// </summary>

		static object[] ob_arr2 = new object[1];
		static UnityEditor.IMGUI.Controls.TreeViewItem tiv;
		Dictionary<object, UnityEditor.IMGUI.Controls.TreeViewItem> __ti = new Dictionary<object, UnityEditor.IMGUI.Controls.TreeViewItem>();
		UnityEditor.IMGUI.Controls.TreeViewItem emptyreeitem = new UnityEditor.IMGUI.Controls.TreeViewItem();

		internal UnityEditor.IMGUI.Controls.TreeViewItem GetTreeItem(int id)
		{

			// var tree = m_TreeViewontroller();
			var tree = TreeController_current;
			if (tree == null) return null;

			if (!__ti.TryGetValue(tree, out tiv) || tiv == null || tiv.id != id)
			{

				var data = _data.GetValue(tree);
				ob_arr2[0] = id;
				// ob_arr2[ 1 ] = data_m_RootItem.GetValue( data, null );
				var res = data_FindItem_Slow.Invoke(data, ob_arr2) as UnityEditor.IMGUI.Controls.TreeViewItem;

				if (__ti.ContainsKey(tree))
					__ti[tree] = res;
				else
					__ti.Add(tree, res);
			}
			return tiv ?? emptyreeitem;
		}




		internal object GetTreeViewontroller(EditorWindow w = null)
		{
			if (UseRootWindow) return m_TreeViewFieldInfo.GetValue(w ?? window.Instance);
			var sub = _SceneHierarchy.GetValue(w ?? window.Instance);
			return m_TreeViewFieldInfo.GetValue(sub);
		}

		internal FieldInfo m_TreeViewFieldInfo
		{
			get
			{
				return _TreeViewController;
				// if ( !window( true ) ) return _FoldView;

			}
		}
		internal FieldInfo m_TreeViewFieldInfoForProject(EditorWindow w)
		{
			//return _AssetsView;

			if ((int)_ViewMode.GetValue(w) == 1) return _FoldView;
			return _AssetsView;
		}
		internal int ProjectViewMode(EditorWindow w)
		{
			return (int)_ViewMode.GetValue(w);
		}

		internal Type SceneHierarchyWindow
		{
			get
			{
				if (_SceneHierarchyWindow == null)
				{
					var ass = Assembly.GetAssembly(typeof(EditorWindow));
					if (pluginID == 0)
					{
						_SceneHierarchyWindow = ass.GetType("UnityEditor.SceneHierarchy");
						if (UseRootWindow = _SceneHierarchyWindow == null) { if ((_SceneHierarchyWindow = ass.GetType("UnityEditor.SceneHierarchyWindow")) == null) throw new Exception("UnityEditor.SceneHierarchyWindow"); }
						else if ((_SceneHierarchy = SceneHierarchyWindowRoot.GetField("m_SceneHierarchy", ~(BindingFlags.Static | BindingFlags.InvokeMethod))) == null) throw new Exception("m_SceneHierarchy");
					}
					else
					{
						if ((_SceneHierarchyWindow = ass.GetType("UnityEditor.ProjectBrowser")) == null) throw new Exception("UnityEditor.ProjectBrowser");
						UseRootWindow = true;
					}
				}
				return _SceneHierarchyWindow;
			}
		}
		Type _SceneHierarchyWindow;
		FieldInfo _SceneHierarchy;
		internal Type SceneHierarchyWindowRoot
		{
			get
			{
				if (_SceneHierarchyWindowRoot == null)
				{
					_SceneHierarchyWindowRoot = Assembly.GetAssembly(typeof(EditorWindow)).GetType(
							pluginID == 0 ? "UnityEditor.SceneHierarchyWindow" : "UnityEditor.ProjectBrowser"
					);
				}

				return _SceneHierarchyWindowRoot;
			}
		}
		Type _SceneHierarchyWindowRoot;
		Type _ProjectBrowserWindowType;
		internal Type ProjectBrowserWindowType
		{
			get
			{
				if (_ProjectBrowserWindowType == null)
				{
					_ProjectBrowserWindowType = Assembly.GetAssembly(typeof(EditorWindow)).GetType("UnityEditor.ProjectBrowser"
					);
				}

				return _ProjectBrowserWindowType;
			}
		}



	//	internal bool tempAdapterDisableCache = false;
		internal bool DisabledSavedData()
		{
			return false;//|| !wasAdapterInitalize;
		}

		internal void Modules_SetDirty()
		{

		}

		internal void Modules_RefreshBookmarks()
		{

		}


	}



}
