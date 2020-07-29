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
	class HighlighterOldDraws
	{

		static PluginInstance adapter { get { return Root.p[0]; } }

		/*

		// bool internalIcon = false;
		static internal void TryToFadeBG(Rect selectionRect, HierarchyObject _o)
		{
			_o.internalIcon = false;
			if (!_o.Validate() || adapter.MOI.des(_o.scene) == null) return;
			if (!adapter.HAS_LABEL_ICON()) return;
			_o.drawIcon = GET_CONTENT(_o);
			if (adapter._S_bgIconsPlace != 0 && _o.switchType != 1) return;
			//  if ( _o.switchType == 1 ) Debug.Log( "ASD" );
			_o.internalIcon = true;
			if ((!_o.drawIcon.add_icon
				//|| !HighlighterHasKey_IncludeFilters(__o.scene, __o).IsTrue(false)
				) && (_o.switchType == 0)) return;

			tt = GetIconRectIfNextToLabel(selectionRect, GetIconRectIfNextToLabelType.CustomIcon);
			Draw_AdapterTextureWithDynamicColor(tt, SourceBGColor);

		}

		static Color inactiveColor = new Color(1, 1, 1, 0.2f);


		internal static bool DoFoldout(Rect rect, UnityEditor.IMGUI.Controls.TreeViewItem item, int id) // if (!active) return;
		{
			adapter.obj_array[0] = id;
			var expandedState =
				(bool)adapter.m_IsExpanded.Invoke(adapter.m_data.GetValue(adapter.m_TreeView(adapter.window()), null),
												   adapter.obj_array);
			return expandedState;
			////  Rect foldoutRect = new Rect(rect.x + foldoutIndent, this.GetFoldoutYPosition(rect.y), foldoutStyleWidth, EditorGUIUtility.singleLineHeight);
			// var on = GUI.color;
			// if (!active)GUI.color *= inactiveColor;
			//  adapter.foldoutStyle.Draw(rect,  GUIContent.none, adapter.foldoutStyle, );
			// GUI.color = on;
		}



		static Dictionary<object, Action<UnityEditor.IMGUI.Controls.TreeViewItem, Rect>> __ti = new
	static Dictionary<object, Action<UnityEditor.IMGUI.Controls.TreeViewItem, Rect>>();

	internal static void IconOverlayGUI(Rect rect,
							UnityEditor.IMGUI.Controls.TreeViewItem item) // rect1.width = this.k_IconWidth + this.iconTotalPadding;
		{
			var tree = adapter.m_TreeView(adapter.window());

			if (!__ti.ContainsKey(tree))
			{
				__ti.Add(tree, null);
				var gui = adapter.guiProp.GetValue(tree, null);
				__ti[tree] =
					adapter.iconOverlayGUI.GetValue(gui, null) as Action<UnityEditor.IMGUI.Controls.TreeViewItem, Rect>;
			}

			if (__ti[tree] == null) return;

			__ti[tree].Invoke(item, rect);
		}

		static Dictionary<object, Action<UnityEditor.IMGUI.Controls.TreeViewItem, Rect>> __lo = new
	static Dictionary<object, Action<UnityEditor.IMGUI.Controls.TreeViewItem, Rect>>();

		internal static void LabelOverlayGUI(Rect rect,
							 UnityEditor.IMGUI.Controls.TreeViewItem item) // rect1.width = this.k_IconWidth + this.iconTotalPadding;
		{
			if (!adapter.haslabelOverlayGUI) return;

			var tree = adapter.m_TreeView(adapter.window());

			if (!__lo.ContainsKey(tree))
			{
				__lo.Add(tree, null);
				var gui = adapter.guiProp.GetValue(tree, null);
				__lo[tree] =
					adapter.labelOverlayGUI.GetValue(gui, null) as
					Action<UnityEditor.IMGUI.Controls.TreeViewItem, Rect>;
			}

			if (__lo[tree] == null) return;

			__lo[tree].Invoke(item, rect);
		}

		internal static void OverlayIconGUI(Rect rect, UnityEditor.IMGUI.Controls.TreeViewItem item, bool active) 
		{
			if (!adapter.HasoverlayIcon) return;

			var icon = adapter.overlayIcon.GetValue(item, null) as Texture2D;

			if (!icon) return;

			Adapter.DrawTexture(rect, icon, active ? Color.white : inactiveColor);
		}
	*/





















			/*


		private Color oldGuiColor;

		private Color gcn = new Color(1, 1, 1, 0.35f);
		//static int ICON_HEIGHT = 16;



		Adapter.SwithcerMethodsWrapper __sourceBgColor_fadeBg_swithcer_HASH = null;

		Adapter.SwithcerMethodsWrapper sourceBgColor_fadeBg_swithcer_HASH
		{
			get
			{
				return __sourceBgColor_fadeBg_swithcer_HASH ?? (__sourceBgColor_fadeBg_swithcer_HASH
						= new Adapter.SwithcerMethodsWrapper(sourceBgColor_fadeBg_swithcer));
			}
		}

		int sourceBgColor_fadeBg_swithcer(Adapter.HierarchyObject _o)
		{
			return adapter.hashoveredItem && adapter.hoverID == _o.id && !adapter.HIDE_HOVER_BG ? 0 : 1;
		}

	*/


	}
}
