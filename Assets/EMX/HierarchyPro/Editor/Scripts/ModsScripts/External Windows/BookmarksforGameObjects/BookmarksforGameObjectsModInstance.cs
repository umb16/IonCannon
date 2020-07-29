using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace EMX.HierarchyPlugin.Editor.Mods.BookObject
{

	class BookmarksforGameObjectsModInstance
	{






		static PluginInstance adapter { get { return Root.p[0]; } }

		static internal GameObject[] ignoreLock = new GameObject[0];

		static MemType cType = MemType.Custom;

		DrawButtonsOld dob = new DrawButtonsOld();



		internal void DoCustom(Rect line, ExternalDrawContainer controller, Scene scene) // refStyle = EditorStyles.helpBox;
		{


			var list = DrawButtonsOld.GET_BOOKMARKS(scene);


			/*if (!controller.CheckCategoryIndex(scene))
			{
				var c = controller.GetCurerentCategoryName();

				var tooltip = GETTOOLTIPPEDCONTENT(MemType.Custom, null, controller);
				tooltip.text = "\"" + Adapter.GET_SCENE_BY_ID(scene).name + "\" doesn't contain a \"" + c + "\" category";
				tooltip.tooltip = "";
				Label(line, tooltip);

				return;
			}*/

			//var rp = DrawButtonsOld.GET_DISPLAY_PARAMS(MemType.Custom);
			//if (controller.GetCategoryIndex(scene) == 0) BG_COLOR = adapter.bottomInterface.RowsParams[0].BgColorValue;

			if (Event.current.type == EventType.Repaint && adapter.par_e.BOOKMARKS_OB_DRAW_BG_COLOR)
			{
				var BG_COLOR = list[controller.GetCategoryIndex(scene)].BgColor;
				if (BG_COLOR.HasValue && BG_COLOR.Value.a != 0)
					EditorGUI.DrawRect(line, BG_COLOR.Value * GUI.color);
			}

			var l = line;
			l.y -= 1;

			var rowParams = DrawButtonsOld.GET_DISPLAY_PARAMS(cType);

			if (!dob.DrawButtons(line, cType, rowParams.BgOverrideColor.a != 0 ? Color.Lerp(Color.white, rowParams.BgOverrideColor, rowParams.BgOverrideColor.a) : Color.white, controller, scene))
			{
				var tooltip = DrawButtonsOld.GETTOOLTIPPEDCONTENT(cType, null, controller);
				tooltip.text = "Drag an object here";
				GUI.Label(line, tooltip, adapter.STYLE_LABEL_8_middle);
				//GUI.Label(l, "- - -", adapter.STYLE_LABEL_10_middle);
			}
			UpdateDragArea(line, controller);
		}












		internal static void DRAW_CATEGORY(Rect buttonRect, ExternalDrawContainer controller, Scene scene, BookmarksforGameObjectsModInstance instance, bool empty)
		{
			var idOffset = DrawButtonsOld.idOffset;// IDOFFSET(MemType.Custom);
			var HHH = Math.Min(24, buttonRect.height - 2);

			var colorR = buttonRect;


			colorR.width = HHH;
			colorR.y += (buttonRect.height - HHH) / 2;
			colorR.y = Mathf.RoundToInt(colorR.y);
			colorR.height = colorR.width;
			colorR.x += (buttonRect.width - HHH) / 2;

			//var clamp = colorR.x;
			var list = DrawButtonsOld.GET_BOOKMARKS(scene);



			DO_BUTTON(controller, scene, instance, buttonRect, idOffset + 200);

			if (adapter.par_e.BOOKMARKS_OB_DRAW_CATEGORY_NAME) DrawButtonsOld.categoryColorContent2.text = DrawButtonsOld.categoryColorContent.text + " " + controller.GetCurerentCategoryName(scene);
			else DrawButtonsOld.categoryColorContent2.text = DrawButtonsOld.categoryColorContent.text;

			if (empty) DrawButtonsOld.categoryColorContent2.text += " - Drag an object here";

			var st = adapter.STYLE_LASTSEL_BUTTON;
			st.fontSize = DrawButtonsOld.GET_DISPLAY_PARAMS(MemType.Custom).fontSize;
			var width = st.CalcSize(DrawButtonsOld.categoryColorContent2);
			colorR.x += colorR.width / 2;
			colorR.x -= width.x / 2;
			colorR.width = width.x;

			adapter.INTERNAL_BOX(Shrink(colorR, 4), "");
			// if (!controller.IS_MAIN) Adapter. INTERNAL_BOX( inforR , "" );


			//var BG_COLOR = list[controller.GetCategoryIndex(scene)].GET_COLOR() ?? adapter.bottomInterface.RowsParams[0].BgColorValue;
			//var BG_COLOR = list[controller.GetCategoryIndex(scene)].BgColor;
			//if (BG_COLOR.HasValue && BG_COLOR.Value.a != 0) EditorGUI.DrawRect(colorR, BG_COLOR.Value);
			if (Event.current.type == EventType.Repaint && adapter.par_e.BOOKMARKS_OB_DRAW_BG_COLOR)
			{
				var BG_COLOR = list[controller.GetCategoryIndex(scene)].BgColor;
				if (BG_COLOR.HasValue && BG_COLOR.Value.a != 0)
					EditorGUI.DrawRect(colorR, BG_COLOR.Value * GUI.color);

			}
			GUI.Label(colorR, DrawButtonsOld.categoryColorContent2, st);

			EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Link);
		}

		static Rect Shrink(Rect r, float s)
		{
			r.x += s;
			r.y += s;
			r.width -= s * 2;
			r.height -= s * 2;
			return r;
		}

		static void DO_BUTTON(ExternalDrawContainer controller, Scene scene, BookmarksforGameObjectsModInstance instance, Rect buttonRect, int idOffset)
		{
			if (Event.current.type == EventType.MouseDown && buttonRect.Contains(Event.current.mousePosition) /*&& Event.current.button == 0*/)
			{
				controller.selection_button = idOffset;
				controller.selection_window = controller.tempRoot;
				controller.lastRect = buttonRect;
				adapter.RepaintWindowInUpdate();
				var captureCell = buttonRect;
				controller.selection_action = (mouseUp, deltaTIme) =>
				{
					if (mouseUp && captureCell.Contains(Event.current.mousePosition))
					{
						DrawButtonsOld.SET_BOOK_WIHTOUT_OBJECTS(controller, scene, instance);
					}

					return Event.current.delta.x == 0 && Event.current.delta.x == 0;
				}; // ACTION
			}

			if (Event.current.type == EventType.Repaint && buttonRect.Contains(Event.current.mousePosition) && controller.selection_action != null && controller.selection_button == idOffset)
			{
				adapter.STYLE_DEFBUTTON_middle.Draw(buttonRect, REALEMPTY_CONTENT, false, false, false, true);
			}
		}

		static GUIContent REALEMPTY_CONTENT = new GUIContent();




		bool dragReady = false;

		bool IsValidDrag()
		{
			var type = (MemType?)DragAndDrop.GetGenericData(adapter.pluginname);
			if (type.HasValue && type.Value == MemType.Custom) return false;
			//   if ( GetDragData().Length != 0 ) Debug.Log( GetDragData()[0] );

			return GetDragData().Length != 0;
		}

		UnityEngine.Object[] GetDragData()
		{
			return DragAndDrop.objectReferences.Select(o => o as GameObject).Where(o => o && o.transform && string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o))).ToArray();

			// return DragAndDrop.objectReferences.Where(o => !string.IsNullOrEmpty(Adapter.isProjectObject(o))).ToArray();
		}

		void SetDragData(UnityEngine.Object[] data, MemType? type)
		{
			if (data != null)
			{
				adapter.ha.InternalClearDrag();

				DragAndDrop.objectReferences = data;
				DragAndDrop.SetGenericData(adapter.pluginname, type);
			}
		}

		void UpdateDragArea(Rect dropArea, ExternalDrawContainer controller)
		{ // Cache References:
			Event currentEvent = Event.current;
			EventType currentEventType = currentEvent.rawType;

			if (currentEventType == EventType.DragExited)
			{
				adapter.ha.InternalClearDrag();
				// EventUse();
			}


			switch (currentEventType)
			{

				case EventType.DragUpdated:
					if (IsValidDrag())
					{
						dragReady = true;
						DragAndDrop.visualMode = DragAndDropVisualMode.Link;
					}
					else DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;

					Tools.EventUse();
					break;
				case EventType.Repaint:
					if (
						DragAndDrop.visualMode == DragAndDropVisualMode.None ||
						DragAndDrop.visualMode == DragAndDropVisualMode.Rejected || !dragReady) break;
					// MonoBehaviour.print(DragAndDrop.visualMode);
					var c = Color.grey;
					c.a = 0.2f;
					EditorGUI.DrawRect(dropArea, c);
					break;
				case EventType.DragPerform:
					DragAndDrop.AcceptDrag();
					//if (data == null) {
					if (IsValidDrag())
					{
						//	if (adapter.IS_HIERARCHY())
						{
							var res = GetDragData();
							var ss = res.Select(r => r as GameObject).ToArray();
							var sc = ss.FirstOrDefault();
							if (sc != null)
							{
								var scene = sc.scene.GetHashCode();
								var result = ss.Where(s => s.scene.GetHashCode() == scene).ToArray();
								//DrawButtonsOld. AddAndRefreshCustom(result, result[0], controller.GetCategoryIndex(sc.scene), scene, controller);
								DrawButtonsOld.AddAndRefreshCustom(result, controller, sc.scene);
							}
						}
						/*	else
							{
								var result = GetDragData().Where(o => o).ToArray();
								if (result.Length != 0)
									AddAndRefreshCustom(result, result[0], controller.GetCategoryIndex(adapter.GET_ACTIVE_SCENE), adapter.GET_ACTIVE_SCENE);
							}*/

					}

					adapter.ha.InternalClearDrag();

					Tools.EventUse();
					break;
				case EventType.MouseUp:
					adapter.ha.InternalClearDrag();
					break;


				case EventType.MouseDown:

					break;
			}
		}



	}
}
