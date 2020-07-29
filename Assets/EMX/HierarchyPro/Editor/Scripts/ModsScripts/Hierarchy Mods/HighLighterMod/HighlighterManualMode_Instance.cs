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






	internal struct DrawBgDynamicColorHelperStruct
	{
		internal Color BGCOLOR;
		internal float alpha;

	}


	internal partial class HighlighterManualMode_Instance
	{


		HighlighterMod root;
		PluginInstance adapter;
		GlDrawer backgroundDrawer;
		internal HighlighterManualMode_Instance(PluginInstance a, HighlighterMod root)
		{
			this.root = root;
			adapter = a;
			backgroundDrawer = new GlDrawer(a);

		}





		GUIContent content = new GUIContent() { tooltip = "HighLighter" };
		GUIContent contentNull = new GUIContent() { tooltip = "Empty Transform" };
		GUIContent contentMis = new GUIContent() { tooltip = "This Object Has Missing MonoScript" };
		Color prefabColorPro = new Color32(76, 128, 217, 255);
		Color prefabColorPersonal = new Color32(0, 39, 131, 255);
		Color prefabMissinColorPro = new Color32(164, 94, 94, 255);
		Color prefabMissinColorPersonal = new Color32(63, 13, 13, 255);
		GUIContent switchedConten = new GUIContent();

		//internal ObjectCacheHelper<GoGuidPair, SingleList> IconColorCacher;
		//internal ObjectCacheHelper<GoGuidPair, Int32List> IconImageCacher;




		Color? bgCol;
		Rect tRr;






		/*	if (START_DRAW(selectionRect, adapter.o))
				{
					END_DRAW(adapter.o);
	}
	*/


		internal void DrawBackgroundInFirstFrame(Rect rect)
		{

			for (int i = 0; i < adapter.drew_mods_count; i++)
			{
				var o = adapter.drew_mods_objects[i];
				_prepare(rect, o);
				rect.y += rect.height;
			}
			//backgroundDrawer.ClearStacks();
			for (int i = 0; i < adapter.drew_mods_count; i++)
			{
				var o = adapter.drew_mods_objects[i];
				_drawBackgroundItem(rect, o);
				rect.y += rect.height;
			}
			backgroundDrawer.ReleaseStack();
		}

		internal void DrawLabelsInLastFrame(Rect rect)
		{
			for (int i = 0; i < adapter.drew_mods_count; i++)
			{
				var o = adapter.drew_mods_objects[i];
				_drawBackgroundItem(rect, o);
				rect.y += rect.height;
			}
		}


		DynamicRect __tempDynamicRect;
		DynamicRect tempDynamicRect { get { return __tempDynamicRect ?? (__tempDynamicRect = new DynamicRect()); } }

		TempColorClass asD(Rect selectionRect, HierarchyObject _o, bool IS_LAYOUT)
		{

			_o.ah.BG_RECT = null;
			_o.ah.BACKGROUNDED = 0;
			_o.ah.FLAGS = 0;

			var targetRect = selectionRect;
			if (UnityVersion.UNITY_CURRENT_VERSION < UnityVersion.UNITY_2019_2_0_VERSION) targetRect.width -= adapter.hierarchyModification.LEFT_PADDING;

			var f = targetRect;
			if (fadeRect.HasValue) f = fadeRect.Value;
			else f.x += f.width;

			tempDynamicRect.Set(targetRect, f, true, _o, true, adapter.hierarchyModification.LEFT_PADDING);
			var cm = selectionRect;
			cm.width += cm.x;
			cm.x = 0;
			return HighlighterDrawWorks.DrawBackground(cm, IS_LAYOUT, tempDynamicRect, _o);
		}
		void _prepare(Rect selectionRect, HierarchyObject _o)
		{
			_o.ah.MIXINCOLOR = null;
			asD(selectionRect, _o, true);
		}

		Rect? fadeRect { get { return adapter.modsController.rightModsManager.headerEventsBlockRect; } }
		void _drawBackgroundItem(Rect selectionRect, HierarchyObject _o)
		{

			root.emptyStack.gl = backgroundDrawer;
			var res = root.START_DRAW_PARTLY_TRYDRAW(selectionRect, _o);
			if (!res) return;

			root.START_DRAW_PARTLY_CREATEINSTANCE(selectionRect, _o, true, backgroundDrawer); //, Event.current.type == EventType.Repaint
																							  //ColorModule.TryToFadeBG(selectionRect, _o);

			//var color = HighlighterChildWorks.DrawColoredBG()
			asD(selectionRect, _o, false);



			root.END_DRAW(_o);






			//var label_icon_rect = HighlighterGetRect.GetIconRectIfNextToLabel(selectionRect, GetIconRectIfNextToLabelType.DefaultIcon);



			/*var treeItem = !adapter..IS_SEARCH_MOD_OPENED() && !callFromExternal() ? _o.GetTreeItem(adapter) : null;
			var active = _o.Active();

			if ((!_o.drawIcon.add_icon || adapter._S_bgIconsPlace != 0) && adapter.HAS_LABEL_ICON())
			{


				var targetIcon = treeItem != null ? treeItem.icon : null;
				var skipoverlay = !targetIcon;
				if (!targetIcon && adapter.HAS_LABEL_ICON())
				{


					if (adapter.IS_PROJECT())
					{
						targetIcon = EditorGUIUtility.ObjectContent(_o.GetHardLoadObject(), _o.GET_TYPE(adapter)).image as Texture2D;
					}

					else
					{
						var loadObject = _o.go;

						if (adapter.FindPrefabRoot(loadObject) != loadObject) loadObject = null;

						targetIcon = EditorGUIUtility.ObjectContent(loadObject, _o.GET_TYPE(adapter)).image as Texture2D;

						if (_o.drawIcon.add_icon && _o.drawIcon.add_icon == targetIcon)
						{
							____SetIconOnlyInternal(_o, null);
							targetIcon = EditorGUIUtility.ObjectContent(loadObject, _o.GET_TYPE(adapter)).image as Texture2D;
							____SetIconOnlyInternal(_o, _o.drawIcon.add_icon as Texture2D);
						}
					}

				}

				if (targetIcon && active)
				{
					var S = 4;
					var R = label_icon_rect;
					R.x -= S;
					R.y -= S;
					R.width += S * 2;
					R.height += S * 2;
					ICON_STACK.Draw_AdapterTexture(R, adapter.GetIcon("HIPERUI_BUTTONGLOW"), Color.black, true);

					if (!_o.Active())
					{
						ICON_STACK.Draw_AdapterTexture(label_icon_rect, targetIcon, active ? Color.white : inactiveColor, false);
					}
					else
					{
						ICON_STACK.Draw_AdapterTexture(label_icon_rect, targetIcon, active ? Color.white : inactiveColor, false);
					}


					if (!skipoverlay)
					{
						overlayButStr.treeItem = treeItem;
						overlayButStr.active = active;
						ICON_STACK.Draw_Action(label_icon_rect, ICON_OVERLAY_ACTION_HASH, overlayButStr);
					}
				}
			}

			if (treeItem != null)
			{

				overlayButStr.treeItem = treeItem;
				ICON_STACK.Draw_Action(selectionRect, LABEL_OVERLAY_ACTION_HASH, overlayButStr);

				if (active && (_o.BACKGROUNDED != 2 || (_o.FLAGS & 1) != 1) && _o.ChildCount(adapter) != 0 && !adapter.IS_SEARCH_MOD_OPENED())
				{

					label_icon_rect.x -= adapter.foldoutStyleWidth + 1;
					label_icon_rect.width = adapter.foldoutStyle.fixedWidth;
					var d = label_icon_rect.height - adapter.foldoutStyleHeight;
					label_icon_rect.y += d / 2;
					label_icon_rect.y = Mathf.FloorToInt(label_icon_rect.y);
					label_icon_rect.height = adapter.foldoutStyleHeight;

					overlayButStr.treeItem = treeItem;
					ICON_STACK.Draw_Action(label_icon_rect, FOLD_ACTION_HASH, overlayButStr);


				}
			}*/

		}



		/*

	static DrawBgDynamicColorHelperStruct DrawBgDynamicColorHelperStructStr;


		internal void DRAW_BGTEXTURE(Color BGCOLOR, Rect rect, DynamicColor color, float alpha = 1, Rect? clipRect = null)
		{
			if (Event.current.type == EventType.Repaint)
			{
				DrawBgDynamicColorHelperStructStr.alpha = alpha;
				DrawBgDynamicColorHelperStructStr.BGCOLOR = BGCOLOR;

				var border = adapter.par_e.HIGHLIGHTER_TEXTURE_BORDER;
				var t = Icons.GetIconDataFromTexture(adapter.par_e.BG_TEXTURE ?? Texture2D.whiteTexture);
				//if (border > Mathf.Min(BG_TEXTURE.width, BG_TEXTURE.height) / 2) border = Mathf.Min(BG_TEXTURE.width, BG_TEXTURE.height) / 2;
				//if (border > Mathf.Min(rect.width, rect.height) / 2) border = Mathf.RoundToInt(Mathf.Min(rect.width, rect.height) / 2);

				if (!adapter.par_e.HIGHLIGHTER_USE_SPECUAL_SHADER || !adapter.par_e.HIghlighterExternalMaterial || adapter.par_e.HIGHLIGHTER_TEXTURE_STYLE == 0 || !adapter.par_e.BG_TEXTURE)
					root.Draw_GUITextureWithBorders(rect, t, border, color, DrawBgDynamicColorHelperStructStr, clipRect);
				else
					root.Draw_GUITextureWithBordersAndMaterial(rect, t, border, adapter.par_e.HIghlighterExternalMaterial, color, DrawBgDynamicColorHelperStructStr, clipRect);
			}
		}

		 internal void DRAW_BGTEXTURE(Color BGCOLOR, Rect rect, Texture2D BG_TEXTURE, DynamicColor color)
		{
			if (Event.current.type == EventType.Repaint)
			{
				DrawBgDynamicColorHelperStructStr.alpha = 1;
				DrawBgDynamicColorHelperStructStr.BGCOLOR = BGCOLOR;
				var t = Icons.GetIconDataFromTexture(adapter.par_e.BG_TEXTURE ?? Texture2D.whiteTexture);

				if (!adapter.par_e.HIGHLIGHTER_USE_SPECUAL_SHADER || !adapter.par_e.HIghlighterExternalMaterial || adapter.par_e.HIGHLIGHTER_TEXTURE_STYLE == 0 || !BG_TEXTURE)
					root.Draw_GUITextureWithBorders(rect, t, 0, color, DrawBgDynamicColorHelperStructStr);
				else
					root.Draw_GUITextureWithBordersAndMaterial(rect, t, 0, adapter.par_e.HIghlighterExternalMaterial, color, DrawBgDynamicColorHelperStructStr);
			}
		}


	*/










	}








}

