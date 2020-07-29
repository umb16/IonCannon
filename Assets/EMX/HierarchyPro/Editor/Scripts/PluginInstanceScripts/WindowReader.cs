using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEditor.SceneManagement;

namespace EMX.HierarchyPlugin.Editor
{

	struct TempModData
	{
		internal int sibling, width;
		internal bool enabled;
		internal Rect rect;
		internal bool fade_narrow;
		internal RightModBaseClass targetMod;
		internal int i_targetMod;
	}


	internal class Window
	{

		internal EditorWindow Instance;
		internal float? oldPositionWidth;
		internal Rect position;
		internal Rect windowPosition;
		PluginInstance p;
		internal int modsCount;
		internal TempModData[] tempModsData = new TempModData[50];
		internal float[] modulesPosesX = Enumerable.Repeat(-1f, 50).ToArray();
		// static Dictionary<int, Window> _update = new Dictionary<int, Window>();
		internal static void AssignInstance(PluginInstance plugin, ref Window targetWindow, Type overrideType = null)
		{

			var guiView = Root.GUIView_current.GetValue(null, null) as UnityEngine.Object;
			EditorWindow Instance;
			if (guiView.GetType().Name == "MaximizedHostView")
			{
				Instance = Root.HostView_actualView.GetValue(guiView, null) as EditorWindow;
			}
			else
			{
				var wins = (List<EditorWindow>)Root.DockArea_m_Panes.GetValue(guiView);
				var selected = (int)Root.DockArea_selected.GetValue(guiView, null);
				// var ht = Scene
				/*  Debug.Log(selected);
				  Debug.Log(wins.Count);
				  Debug.Log(wins[1] + " " + plugin.SceneHierarchyWindowRoot);*/
				var T = overrideType ?? plugin.SceneHierarchyWindowRoot;
				if (selected >= 0 && selected < wins.Count && wins[selected].GetType() == T) Instance = wins[selected] as EditorWindow;
				else if (wins.Count == 1) Instance = wins[0] as EditorWindow;
				else Instance = wins.FirstOrDefault(w => w.GetType() == T) ?? throw new Exception("Cannot find hierarchy window");

			}

			// if (!_update.TryGetValue(Instance.GetInstanceID(), out targetWindow)) _update.Add(Instance.GetInstanceID(), targetWindow = new Window());


			// Instance = Root.View_window.GetValue( guiView, null ) as UnityEngine.Object;
			targetWindow.Instance = Instance;
			targetWindow.p = plugin;
			targetWindow.position = (Rect)Root.View_position.GetValue(guiView, null);
			targetWindow.windowPosition = (Rect)Root.View_windowPosition.GetValue(guiView, null);

		}

		public class ReflType
		{
			public bool isprop;
			public FieldInfo field;
			public PropertyInfo prop;

			public ReflType(FieldInfo f, PropertyInfo p)
			{
				this.field = f;
				this.prop = p;
				this.isprop = p != null;
			}

			public ReflType(Type type, string key)
			{
				this.field = type.GetField(key, ~(BindingFlags.InvokeMethod | BindingFlags.Static));
				if (this.field == null) this.prop = type.GetProperty(key, ~(BindingFlags.InvokeMethod | BindingFlags.Static));
				this.isprop = this.prop != null;
				if (this.prop == null && this.field == null) throw new Exception(key);
			}

			public object GetValue(object target)
			{
				return isprop ? prop.GetValue(target, null) : field.GetValue(target);
			}

			public void SetValue(object target, object args)
			{
				if (isprop) prop.SetValue(target, args, null);
				else field.SetValue(target, args);
			}
		}
		internal static ReflType k_LineHeight, k_IndentWidth, k_BaseIndent, k_IconWidth;
		static FieldInfo foldoutYOffset, m_UseHorizontalScroll, m_Ping, m_PingStyle;
		internal static void InitFields()
		{
			var GameObjectTreeViewGUI = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameObjectTreeViewGUI") ?? throw new Exception("GameObjectTreeViewGUI");
			var treeViewBaseType = GameObjectTreeViewGUI.BaseType;
			k_LineHeight = new ReflType(treeViewBaseType, "k_LineHeight");
			k_IndentWidth = new ReflType(treeViewBaseType, "k_IndentWidth");
			k_BaseIndent = new ReflType(treeViewBaseType, "k_BaseIndent");
			k_IconWidth = new ReflType(treeViewBaseType, "k_IconWidth");
			foldoutYOffset = treeViewBaseType.GetField("foldoutYOffset", ~(BindingFlags.InvokeMethod | BindingFlags.Static));
			if (foldoutYOffset == null) foldoutYOffset = treeViewBaseType.GetField("customFoldoutYOffset", ~(BindingFlags.InvokeMethod | BindingFlags.Static)) ?? throw new Exception("foldoutYOffset");
			m_UseHorizontalScroll = treeViewBaseType.GetField("m_UseHorizontalScroll", (BindingFlags)(-1)) ?? throw new Exception("m_UseHorizontalScroll");
			m_Ping = treeViewBaseType.GetField("m_Ping", (BindingFlags)(-1)) ?? throw new Exception("m_Ping");
			m_PingStyle = m_Ping.FieldType.GetField("m_PingStyle", (BindingFlags)(-1)) ?? throw new Exception("m_PingStyle");

		}

		static bool HAS_SES<T>(string key)
		{
			if (typeof(T) == typeof(int)) return SessionState.GetInt(key, -999) != -999;
			else if (typeof(T) == typeof(float)) return SessionState.GetFloat(key, -999) != -999;
			throw new Exception("HAS_SES");
		}

		static MethodInfo dataInitMinimal = null;

		internal void SET_LINE_HEIGHT(PluginInstance p)
		{

			if (p.par_e.USE_LINE_HEIGHT)
			{

				bool init = false;
				if (!HAS_SES<int>(Folders.PREFS_PATH + "k_LineHeight"))
				{
					SessionState.SetInt(Folders.PREFS_PATH + "k_LineHeight", (int)(float)k_LineHeight.GetValue(p.gui_currentTree));
					SessionState.SetInt(Folders.PREFS_PATH + "foldoutYOffset", (int)(float)foldoutYOffset.GetValue(p.gui_currentTree));
					init = true;
				}





				var H = p.par_e.LINE_HEIGHT;
				if ((int)(float)k_LineHeight.GetValue(p.gui_currentTree) != H)
				{
					k_LineHeight.SetValue(p.gui_currentTree, H);
					if (dataInitMinimal == null) dataInitMinimal = p.data_currentTree.GetType().GetMethod("InitializeMinimal", (System.Reflection.BindingFlags)(-1));
					try
					{
						dataInitMinimal.Invoke(p.data_currentTree, null);
					}
					catch { }

				}


				//foldoutYOffset.SetValue( p.gui_currentTree,(float) Mathf.RoundToInt( (H - EditorGUIUtility.singleLineHeight) / 2 ) );

				foreach (var item in p.hierarchyModification.INTERNAL_LABEL_STYLES)
				{
					// item.fixedHeight = H;
					item.SetHeight(H);
				}

				var ping = m_Ping.GetValue(p.gui_currentTree);
				var style = m_PingStyle.GetValue(ping) as GUIStyle;
				if (style != null)
				{
					if (init)
					{
						SessionState.SetFloat(Folders.PREFS_PATH + "style.fixedHeight", style.fixedHeight);
						SessionState.SetBool(Folders.PREFS_PATH + "style.stretchHeight", style.stretchHeight);
					}
					style.fixedHeight = H;
					style.stretchHeight = true;
					//fixedHeight.SetValue(style, H, null);
				}
			}

		}
		internal static void RESET_LINE_HEIGHT(PluginInstance p, EditorWindow w)
		{
			if (HAS_SES<int>(Folders.PREFS_PATH + "k_LineHeight"))
			{
				var TreeController_current = p.GetTreeViewontroller(w);
				var gui_currentTree = p._gui.GetValue(TreeController_current);
				k_LineHeight.SetValue(gui_currentTree, (float)SessionState.GetInt(Folders.PREFS_PATH + "k_LineHeight", 16));
				foldoutYOffset.SetValue(p.gui_currentTree, (float)SessionState.GetInt(Folders.PREFS_PATH + "foldoutYOffset", 0));
				var ping = m_Ping.GetValue(p.gui_currentTree);
				var style = m_PingStyle.GetValue(ping) as GUIStyle;
				if (style != null)
				{
					style.fixedHeight = SessionState.GetFloat(Folders.PREFS_PATH + "style.fixedHeight", 16);
					style.stretchHeight = SessionState.GetBool(Folders.PREFS_PATH + "style.stretchHeight", false);

					//fixedHeight.SetValue(style, H, null);
				}
				SessionState.EraseInt(Folders.PREFS_PATH + "k_LineHeight");

				foreach (var item in p.hierarchyModification.INTERNAL_LABEL_STYLES)
				{
					item.SetHeight(16);
				}
			}
		}
		static void SET_CHILD_INDENT(PluginInstance p)
		{

			if (p.par_e.USE_CHILD_INDENT)
			{
				//if (p.gui_currentTree == null) throw new Exception("p.gui_currentTree = null");
				if (!HAS_SES<int>(Folders.PREFS_PATH + "k_IndentWidth"))
				{
					SessionState.SetInt(Folders.PREFS_PATH + "k_IndentWidth", (int)(float)k_IndentWidth.GetValue(p.gui_currentTree));
					SessionState.SetInt(Folders.PREFS_PATH + "k_BaseIndent", (int)(float)k_BaseIndent.GetValue(p.gui_currentTree));
				}
				var addIndent = Mathf.RoundToInt(14 - p.par_e.CHILD_INDENT);
				k_IndentWidth.SetValue(p.gui_currentTree, (float)p.par_e.CHILD_INDENT);
				if (p.ha.IS_SEARCH_MODE_OR_PREFAB_OPENED()) k_BaseIndent.SetValue(p.gui_currentTree,/* defaultextraInsertionMarkerIndent +*/ (float)p.hierarchyModification.LEFT_PADDING);
				else k_BaseIndent.SetValue(p.gui_currentTree,/* defaultextraInsertionMarkerIndent +*/ (float)addIndent + p.hierarchyModification.LEFT_PADDING);

			}

		}
		internal static void RESET_CHILD_INDENT(PluginInstance p, EditorWindow w)
		{
			if (HAS_SES<int>(Folders.PREFS_PATH + "k_IndentWidth"))
			{
				var TreeController_current = p.GetTreeViewontroller(w);
				var gui_currentTree = p._gui.GetValue(TreeController_current);
				k_IndentWidth.SetValue(gui_currentTree, (float)SessionState.GetInt(Folders.PREFS_PATH + "k_IndentWidth", 14));
				k_BaseIndent.SetValue(p.gui_currentTree, (float)SessionState.GetInt(Folders.PREFS_PATH + "k_BaseIndent", 0));
				SessionState.EraseInt(Folders.PREFS_PATH + "k_IndentWidth");
			}
		}
		static void SET_DEFAULT_ICON_SIZE(PluginInstance p)
		{
			if (p.par_e.USE_OVERRIDE_DEFAULT_ICONS_SIZE)
			{
				if (!HAS_SES<int>(Folders.PREFS_PATH + "k_IconWidth"))
				{
					SessionState.SetInt(Folders.PREFS_PATH + "k_IconWidth", (int)(float)k_IconWidth.GetValue(p.gui_currentTree));
				}
				k_IconWidth.SetValue(p.gui_currentTree, (float)p.par_e.OVERRIDE_DEFAULT_ICONS_SIZE);
			}

		}
		internal static int DefaultIconSize(PluginInstance p) { return (int)(float)k_IconWidth.GetValue(p.gui_currentTree); }
		internal static void RESET_DEFAULT_ICON_SIZE(PluginInstance p, EditorWindow w)
		{
			if (HAS_SES<int>(Folders.PREFS_PATH + "k_IconWidth"))
			{
				var TreeController_current = p.GetTreeViewontroller(w);
				var gui_currentTree = p._gui.GetValue(TreeController_current);
				k_IconWidth.SetValue(gui_currentTree, (float)SessionState.GetInt(Folders.PREFS_PATH + "k_IconWidth", (int)16));
				SessionState.EraseInt(Folders.PREFS_PATH + "k_IconWidth");
			}
		}

		static int?[] fonts = new int?[0];
		static void SET_GAMEOBJECTS_NAMES(PluginInstance p)
		{
			if (p.par_e.USE_OVERRIDE_FOR_GAMEOBJECTS_NAMES_LABELS_FONT_SIZE)
			{
				if (fonts.Length != p.hierarchyModification.INTERNAL_LABEL_STYLES.Count) fonts = new int?[p.hierarchyModification.INTERNAL_LABEL_STYLES.Count];
				//Array.Resize( ref fonts, p.hierarchyModification.INTERNAL_LABEL_STYLES.Count );
				for (int i = 0; i < fonts.Length; i++)
				{
					if (!fonts[i].HasValue) fonts[i] = p.hierarchyModification.INTERNAL_LABEL_STYLES[i].style.fontSize;
					p.hierarchyModification.INTERNAL_LABEL_STYLES[i].style.fontSize = p.par_e.OVERRIDE_FOR_GAMEOBJECTS_NAMES_LABELS_FONT_SIZE;
				}
			}
		}
		internal static void RESET_GAMEOBJECTS_NAMES(PluginInstance p, EditorWindow w)
		{
			if (fonts.Length == p.hierarchyModification.INTERNAL_LABEL_STYLES.Count)
			{
				for (int i = 0; i < fonts.Length; i++)
				{
					if (fonts[i].HasValue) p.hierarchyModification.INTERNAL_LABEL_STYLES[i].style.fontSize = fonts[i].Value;
				}
			}
		}
		internal void SetHeightAndIndents(PluginInstance p)
		{

			if ((bool)m_UseHorizontalScroll.GetValue(p.gui_currentTree) != p.par_e.USE_HORISONTAL_SCROLL)
				m_UseHorizontalScroll.SetValue(p.gui_currentTree, p.par_e.USE_HORISONTAL_SCROLL);

			SET_LINE_HEIGHT(p);
			SET_CHILD_INDENT(p);
			SET_GAMEOBJECTS_NAMES(p);
			SET_DEFAULT_ICON_SIZE(p);

		}







		internal Dictionary<int, Action> rightModIndexAndOnMouseUp = new Dictionary<int, Action>();

		internal void CheckMouseEvents()
		{

			/*
            foreach ( var item in externalMods )
            {
                if ( item.Value.Alive() && item.Value.currentAction != null )
                {
                    if ( !item.Value.currentAction( false, deltaTime ) )
                        item.Value.RepaintNow();
                }
            }*/
			if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
			{
				var repaint = false;
				if (mouseEvent != null)
				{
					mouseEvent(false, p.deltaTime);
					repaint = true;
				}
				if (mouseEventDrag != null)
				{
					mouseEventDrag(false, p.deltaTime);
					repaint = true;
				}
				if (rightModIndexAndOnMouseUp.Count != 0)
				{
					repaint = true;
				}
				if (Event.current.rawType == EventType.MouseUp /*|| Event.current.keyCode == KeyCode.Escape*/)
				{
					EVENT_HELPER_ONUP();
				}
				if (repaint)
				{
					p.RepaintWindowInUpdate();
				}
			}



			if (mouseEvent != null || mouseEventDrag != null || rightModIndexAndOnMouseUp.Count != 0)
			{
				p.ha.DisableHover();

			}
		}


		internal Rect DragRect;

		Action<bool, float> __mouseEvent;
		internal Action<bool, float> mouseEvent
		{
			get { return __mouseEvent; }
			set
			{
				__mouseEvent = value;
				if (value != null) p.PUSH_ONMOUSEUP(EVENT_HELPER_ONUP);
			}
		}

		Action<bool, float> __mouseEventDrag;
		internal RightModBaseClass captured_mod;
		internal bool allow;
		internal int captured_mod_arr;

		internal Action<bool, float> mouseEventDrag
		{
			get { return __mouseEventDrag; }
			set
			{
				__mouseEventDrag = value;
				if (value != null) p.PUSH_ONMOUSEUP(EVENT_HELPER_ONUP);
			}
		}

		internal void EVENT_HELPER_ONUP()
		{
			var repaint = false;

			if (mouseEvent != null)
			{
				mouseEvent(true, p.deltaTime);
				mouseEvent = null;
				// GUIUtility.hotControl = 0;
				Tools.EventUse();
				repaint = true;
			}
			if (mouseEventDrag != null)
			{
				mouseEventDrag(true, p.deltaTime);
				mouseEventDrag = null;
				//GUIUtility.hotControl = 0;
				Tools.EventUse();
				repaint = true;
			}
			if (rightModIndexAndOnMouseUp.Count != 0)
			{
				foreach (var rightModOnUp in rightModIndexAndOnMouseUp.Values.ToArray())
					rightModOnUp();
				rightModIndexAndOnMouseUp.Clear();
				repaint = true;
			}
			if (repaint)
			{
				p.RepaintWindowInUpdate();
			}
		}

	}
}
