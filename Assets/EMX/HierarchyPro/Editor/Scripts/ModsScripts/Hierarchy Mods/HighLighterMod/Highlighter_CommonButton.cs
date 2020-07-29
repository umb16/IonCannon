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
	internal partial class HighlighterMod : DrawStackAdapter, IModSaver
	{


		internal static int ICON_WIDTH = 20;
		//DrawStackAdapter _colorAdditionalStackAdapter;
		/*internal DrawStackAdapter ICON_STACK
		{
			get { return _colorAdditionalStackAdapter ?? (_colorAdditionalStackAdapter = new DrawStackAdapter() { adapter = adapter }); }
		}*/
		GUIContent buttonContent = new GUIContent();
		internal void DrawButton(ref Rect selectionRect, HierarchyObject _o)
		{

			if (adapter.hashoveredItem && !adapter.HoverDisabled && adapter.hoverID != _o.id || adapter.par_e.HIGHLIGHTER_HIERARCHY_BUTTON_LOCATION == 0) return;

			/*	var buttonRectLabel = selectionRect;
				buttonRectLabel.width = EditorGUIUtility.singleLineHeight;
				var buttonRectLeft = selectionRect;

				buttonRectLeft.width = selectionRect.x - foldoutStyleWidth;
				if (_o.ChildCount() == 0) buttonRectLeft.width += foldoutStyleWidth / 2;
				var left_padding = adapter.hierarchyModification.LEFT_PADDING;


				buttonRectLeft.x = left_padding;
				if (UnityVersion.UNITY_CURRENT_VERSION == UnityVersion.UNITY_2019_2_0_VERSION) buttonRectLeft.width -= left_padding;

				///(adapter.parLINE_HEIGHT - 16) / 2
			var DIF =  +2;
			buttonRectLeft.y += DIF;
			buttonRectLeft.height -= DIF * 2;

		*/
			adapter.par_e.SET("HIGHLIGHTER_HIERARCHY_BUTTON_LOCATION", 1);

			if (adapter.par_e.HIGHLIGHTER_HIERARCHY_BUTTON_LOCATION > 0)
			{

				//colButStr.localSelectionRect = ICON_STACK.ConvertToLocal(selectionRect);
				//ICON_STACK.Draw_SimpleButton(buttonRectLeft, switchedConten, BUTTON_ACTION_HASH, colButStr, true);


				var fixedRebutRectt = ButtonRect(selectionRect);
				//EditorGUI.DrawRect(fixedRebutRectt, Color.white);
				if (adapter.SimpleButton(fixedRebutRectt, buttonContent, adapter.button))
					BUTTON_ACTION(Rect.zero, Rect.zero, new DrawStackMethodsWrapperData() { args = new ColorButtonStr() { localSelectionRect = selectionRect } }, _o);


				if (!adapter.ha.IS_PREFAB_MOD_OPENED())
				{

					if (HighlighterWindow.CURRENT_WINDOW && HighlighterWindow.source == _o && adapter.par_e.HIGHLIGHTER_HIERARCHY_DRAW_BUTTON_RECTMARKER > 0)
					{
						EditorGUI.DrawRect(
							//HoverRect(_o, selectionRect, adapter.par_e.HIGHLIGHTER_HIERARCHY_BUTTON_LOCATION),
							HoverRectNew(selectionRect),
							EditorGUIUtility.isProSkin ? Color.white : new Color(1, 0, 0, 0.5f));
						/* GUI.DrawTexture( HoverRect( selectionRect,
													_S_bgButtonForIconsPlace ), GetIcon( "SETUP_SORT1" ),
										 ScaleMode.ScaleToFit );      */     //HIGHLIGHTER_COLOR_PICKER   SETUP_BLUELINE  STORAGE_ONECOMP   BOTTOM_DESBUT FAVORIT_LIST_ICON FAVORIT_LIST_ICON ON STAR SETUP_SORT1 SETUP_SLIDER_HOVER

						//Adapter.DrawRect(new Rect(0, selectionRect.y + selectionRect.height / 8 * 3, selectionRect.height / 4 * 3, selectionRect.height / 8 * 2), Color.red);
					}

					if (adapter.hashoveredItem && !adapter.HoverDisabled && adapter.par_e.HIGHLIGHTER_HIERARCHY_DRAW_BUTTON_RECTMARKER == 2)
					{
						if (adapter.hoverID == _o.id)
						{
							EditorGUI.DrawRect(
								//HoverRectNew(_o, selectionRect, adapter.par_e.HIGHLIGHTER_HIERARCHY_BUTTON_LOCATION),
								HoverRectNew(selectionRect),
								EditorGUIUtility.isProSkin ? Color.white : new Color(1, 0, 0, 0.5f));
							/*  GUI.DrawTexture( HoverRect( selectionRect, _S_bgButtonForIconsPlace ), GetIcon( "SETUP_SORT1" ), ScaleMode.ScaleToFit);*/

						}
					}
				}
			}






		}






		Color32 S_COL1 = new Color32(8, 8, 8, 50);
		Color32 S_COL2 = new Color32(132, 132, 132, 50);

		Color32 S_COL3 = new Color32(8, 8, 8, 20);
		Color32 S_COL4 = new Color32(255, 255, 255, 50);



		Rect ButtonRect(Rect selectionRect)
		{
			//var icon_rect = HighlighterGetRect.GetIconRect(selectionRect, false, IconPlace);

			switch (adapter.par_e.HIGHLIGHTER_HIERARCHY_BUTTON_LOCATION)
			{
				case 1:
				case 2:
					{
						var left_rect = selectionRect;
						left_rect.x = 0;
						if (UnityVersion.UNITY_CURRENT_VERSION >= UnityVersion.UNITY_2019_2_0_VERSION)
							//if (adapter.par_e.HIGHLIGHTER_HIERARCHY_BUTTON_LOCATION < 3)
							left_rect.x += adapter.hierarchyModification.LEFT_PADDING;

						left_rect.width = selectionRect.x + selectionRect.width - left_rect.x;
						if (adapter.par_e.HIGHLIGHTER_HIERARCHY_BUTTON_LOCATION == 1) left_rect.width = Mathf.Min(left_rect.height, left_rect.width);

						return left_rect;
					}
				case 3:
					{
						var label_rect = HighlighterGetRect.GetIconRectIfNextToLabel(selectionRect, GetIconRectIfNextToLabelType.CustomIcon);
						return label_rect;
					}
			}
			throw new Exception("ButtonRect");

			//internal static Rect GetIconRectIfNextToLabel(Rect selectionRect, GetIconRectIfNextToLabelType type)

		}
		internal Rect HoverFullRect(Rect selectionRect)
		{
			var icon_rect = HighlighterGetRect.GetIconRect(selectionRect, false, 0, 0);
			//  var size = selectionRect.height / 8 * 2;
			if (UnityVersion.UNITY_CURRENT_VERSION == UnityVersion.UNITY_2019_2_0_VERSION) icon_rect.x += adapter.hierarchyModification.LEFT_PADDING;
			return icon_rect;
		}
		internal Rect HoverRectNew(Rect selectionRect)
		{
			var icon_rect = ButtonRect(selectionRect);
			icon_rect.width = icon_rect.height;


			//var size = EditorGUIUtility.singleLineHeight / 4;
			var size = adapter.par_e.HIGHLIGHTER_HIERARCHY_BUTTON_RECTMARKER_SIZE;
			if (adapter.par_e.HIGHLIGHTER_HIERARCHY_BUTTON_LOCATION < 3) size -= 1;

			//  var size = EditorGUIUtility.singleLineHeight / 2.5f;
			//  var size = parLINE_HEIGHT / 1.5f;
			//if (IconPlace != 0)
			icon_rect.x += (icon_rect.width - size) / 2;

			icon_rect.y += (icon_rect.height - size) / 2;
			icon_rect.width = icon_rect.height = size;
			return icon_rect;
		}
		internal Rect HoverRect(HierarchyObject _o, Rect selectionRect, int IconPlace, int? overrideBgIconPlace = null)
		{
			var icon_rect = HighlighterGetRect.GetIconRect(selectionRect, false, IconPlace == 0 /*|| _S_bgButtonForIconsPlace == 2*/ ? 2 : 0, overrideBgIconPlace);

			//  var size = selectionRect.height / 8 * 2;
			if (IconPlace == 0)
			{
				if (UnityVersion.UNITY_CURRENT_VERSION == UnityVersion.UNITY_2019_2_0_VERSION)
				{
					icon_rect.x += adapter.hierarchyModification.LEFT_PADDING;
				}
			}

			if (HighlighterGetRect.lastIconPlace == 2 || HighlighterGetRect.lastIconPlace == 0)
			{
				/*if (SETACTIVE_POSITION == 1 && ENABLE_ACTIVEGMAOBJECTMODULE)
				{
					if ((_o.parent(this) != null || _o.ChildCount(this) == 0))
						icon_rect.x += 8;
					else
						icon_rect.x += 4;
				}*/
				icon_rect.x += 4;
				icon_rect.x += 2;
			}


			var size = EditorGUIUtility.singleLineHeight / 4;

			if (HighlighterGetRect.lastIconPlace == 0) size -= 1;

			//  var size = EditorGUIUtility.singleLineHeight / 2.5f;
			//  var size = parLINE_HEIGHT / 1.5f;
			if (IconPlace != 0)
				icon_rect.x += (icon_rect.width - size) / 2;

			icon_rect.y += (icon_rect.height - size) / 2;
			icon_rect.width = icon_rect.height = size;
			return icon_rect;
		}
		//internal int hoverID;



		/*	DrawStackMethodsWrapper __FOLD_ACTION_HASH = null; DrawStackMethodsWrapper FOLD_ACTION_HASH { get { return __FOLD_ACTION_HASH ?? (__FOLD_ACTION_HASH = new DrawStackMethodsWrapper(FOLD_ACTION)); } }
			void FOLD_ACTION(Rect worldOffset, Rect label_icon_rect, DrawStackMethodsWrapperData data, HierarchyObject _o)
			{

				var overlayButStr = (OverlayButtonStr)data.args;
				var treeItem = overlayButStr.treeItem;
				if (EditorGUIUtility.isProSkin)
				{
					var c = GUI.color;
					var CCC = _o.BACKGROUNDEsourceBgColorD;
					var l = (CCC.r + CCC.g + CCC.b);
					CCC.g = CCC.r = CCC.b = 1 - l / 2;
					CCC.a = 1;
					var expandedState = DoFoldout(label_icon_rect, treeItem, _o.id);
					GUI.color *= CCC;
					GUI.Toggle(label_icon_rect, expandedState, GUIContent.none, foldoutStyle);
					GUI.color = c;
				}
				else
				{
					var expandedState = DoFoldout(label_icon_rect, treeItem, _o.id);
					GUI.Toggle(label_icon_rect, expandedState, GUIContent.none, foldoutStyle);
				}
			}
			DrawStackMethodsWrapper __SKIP_CHILD_COUNT_ACTION_HASH = null; DrawStackMethodsWrapper SKIP_CHILD_COUNT_ACTION_HASH { get { return __SKIP_CHILD_COUNT_ACTION_HASH ?? (__SKIP_CHILD_COUNT_ACTION_HASH = new DrawStackMethodsWrapper(SKIP_CHILD_COUNT_ACTION)); } }
			void SKIP_CHILD_COUNT_ACTION(Rect worldOffset, Rect label_icon_rect, DrawStackMethodsWrapperData data, HierarchyObject _o)
			{
				adapter.SKIP_CHILDCOUNT = true;
			}
			internal struct OverlayButtonStr
			{
				internal UnityEditor.IMGUI.Controls.TreeViewItem treeItem;
				internal bool active;
			}
			OverlayButtonStr overlayButStr;
			DrawStackMethodsWrapper __ICON_OVERLAY_ACTION_HASH = null; 	DrawStackMethodsWrapper ICON_OVERLAY_ACTION_HASH { get { return __ICON_OVERLAY_ACTION_HASH ?? (__ICON_OVERLAY_ACTION_HASH = new DrawStackMethodsWrapper(ICON_OVERLAY_ACTION)); } }
			void ICON_OVERLAY_ACTION(Rect worldOffset, Rect label_icon_rect, DrawStackMethodsWrapperData data, HierarchyObject _o)
			{
				var overlayButStr = (OverlayButtonStr)data.args;
				IconOverlayGUI(label_icon_rect, overlayButStr.treeItem);
				OverlayIconGUI(label_icon_rect, overlayButStr.treeItem, overlayButStr.active);
			}
			DrawStackMethodsWrapper __SECOND_ICON_OVERLAY_ACTION_HASH = null;
			DrawStackMethodsWrapper SECOND_ICON_OVERLAY_ACTION_HASH { get { return __SECOND_ICON_OVERLAY_ACTION_HASH ?? (__SECOND_ICON_OVERLAY_ACTION_HASH = new DrawStackMethodsWrapper(SECOND_ICON_OVERLAY_ACTION)); } }
			void SECOND_ICON_OVERLAY_ACTION(Rect worldOffset, Rect label_icon_rect, DrawStackMethodsWrapperData data, HierarchyObject _o)
			{
				var overlayButStr = (OverlayButtonStr)data.args;
				IconOverlayGUI(label_icon_rect, overlayButStr.treeItem);
			}
			DrawStackMethodsWrapper __LABEL_OVERLAY_ACTION_HASH = null; DrawStackMethodsWrapper LABEL_OVERLAY_ACTION_HASH { get { return __LABEL_OVERLAY_ACTION_HASH ?? (__LABEL_OVERLAY_ACTION_HASH = new DrawStackMethodsWrapper(LABEL_OVERLAY_ACTION)); } }
			void LABEL_OVERLAY_ACTION(Rect worldOffset, Rect selectionRect, DrawStackMethodsWrapperData data, HierarchyObject _o)
			{
				var overlayButStr = (OverlayButtonStr)data.args;
				LabelOverlayGUI(selectionRect, overlayButStr.treeItem);
			}
			*/















		//#EMX_TODO remove pragma warning disable
#pragma warning disable
		internal struct ColorButtonStr
		{
			internal Rect localSelectionRect;
			internal Texture2D drawIcon;
		}
#pragma warning restore
		ColorButtonStr colButStr;
		DrawStackMethodsWrapper __BUTTON_ACTION_HASH = null; DrawStackMethodsWrapper BUTTON_ACTION_HASH { get { return __BUTTON_ACTION_HASH ?? (__BUTTON_ACTION_HASH = new DrawStackMethodsWrapper(BUTTON_ACTION)); } }
		void BUTTON_ACTION(Rect worldOffset, Rect inputRect, DrawStackMethodsWrapperData data, HierarchyObject _o)
		{

			var colButStr = (ColorButtonStr)data.args;
			Texture2D drawIcon = colButStr.drawIcon;
			var selectionRect = colButStr.localSelectionRect;

			//if (adapter.IsSceneHaveToSave(_o)) return;
			if (Event.current.button == 0 /*&& !Application.isPlaying*/)
			{   /*var pos = InputData.WidnwoRect( !callFromExternal(),
				                                            //  Event.current.mousePosition - new Vector2(0, EditorGUIUtility.singleLineHeight * 2)
				                                            new Vector2(Event.current.mousePosition.x, currentRect.y - EditorStyles.foldout.lineHeight * 1.25f)
				                                            , 0, 0, adapter, lockPos: true);*/



				selectionRect.x += worldOffset.x;
				selectionRect.y += worldOffset.y;
				//  var mp = Event.current.mousePosition;
				var mp = new Vector2(Event.current.mousePosition.x, selectionRect.y + selectionRect.height * 1.4f);
				// - new Vector2(0, EditorGUIUtility.singleLineHeight * 1.5f)
				var pos = new MousePos(mp, MousePos.Type.Highlighter_410_0, false, adapter);
				// var pos = new MousePos( mp, MousePos.Type.Highlighter_410_0, !callFromExternal(), adapter);

				/*  mp = GUIUtility.GUIToScreenPoint( mp );
				  var pos = new Rect(mp.x, mp.y, 0, 0);*/

				Action<Texture, string> _SetIconImage = (currentSelection, undoStr) =>
				{


					if (currentSelection == null)
					{
						HighlighterCache_Icons.SetIcon(_o, (Texture2D)null);
					}

					else
					{
						var library = AssetDatabase.GetAssetPath(currentSelection).StartsWith("Library/");
						string texName = library
										 ? currentSelection.name
										 : "GUID=" + AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(currentSelection));

						if (!library)
						{


							var last = HighLighterCommonData.GetIconsHistory();
							if (last.Count == 0) last.Add(texName);
							else last.Insert(0, texName);
							while (last.Count > 20) last.RemoveAt(20);
							HighLighterCommonData.SetDirty();
							//	Hierarchy_GUI.Undo(adapter, undoStr);
							//	Hierarchy_GUI.GetLastList(adapter).RemoveAll(t => t == texName);
							//	if (Hierarchy_GUI.GetLastList(adapter).Count == 0) Hierarchy_GUI.GetLastList(adapter).Add(texName);
							//	else Hierarchy_GUI.GetLastList(adapter).Insert(0, texName);
							//	while (Hierarchy_GUI.GetLastList(adapter).Count > 20) Hierarchy_GUI.GetLastList(adapter).RemoveAt(20);
							//Hierarchy_GUI.SetDirtyObject(adapter);
						}

						HighlighterCache_Icons.SetIcon(_o, (Texture2D)currentSelection);
					}



				};
				//

				var capture_o = _o;
				//var GET_TEXTURE = TODO_Tools.GetObjectBuildinIcon.ObjectContent_NoCacher(adapter, EditorUtility.InstanceIDToObject(_o.id), adapter.t_GameObject).add_icon;
				var _GetTexture = TODO_Tools.GetObjectBuildinIcon(EditorUtility.InstanceIDToObject(_o.id), Tools.unityGameObjectType).add_icon;

				Func<TempColorClass> _GetHiglightColor = () =>
				{
					var l_o = capture_o;
					TempColorClass _tempColor = new TempColorClass();
					if (!l_o.Validate(true)) return _tempColor.empty;
					//var c = GetHighlighterValue(l_o.scene, l_o);
					var c = HighlighterCache_Colors.GetHighlighterValue(l_o);
					//Debug.Log(c.Length);
					if (c != null && c.Length > 0) return _tempColor.AssignFromList(ref c);
					else return _tempColor.empty;
				};

				Action<TempColorClass, string> _SetHiglightColor = (el, undoName) =>
				{
					HighlighterCache_Icons.SetAction(capture_o, (in_o) =>
					{
						if (!in_o.Validate(true)) return;
						//var d = adapter.MOI.des(in_o.scene);
						//adapter.SET_UNDO(d, undoName);
						//SetHighlighterValue(el, in_o.scene, in_o);
						HighlighterCache_Colors.SetHighlighterValue(el, in_o);
						adapter.RepaintWindowInUpdate();
					});
				};


				Action<Color32, string> _SetIconColor =
					(c, undoStr) =>
					{
						HighlighterCache_Icons.SetAction(capture_o, (l_o) =>
						{
							if (!l_o.Validate(true)) return;
							HighlighterCache_Icons.SetImageColor_OnlyCacher(l_o, c);
							adapter.RepaintWindowInUpdate();
						});
					};
				Func<Color32?> _GetIconColor = () =>
			 {
				 var l_o = capture_o;
				 if (!l_o.Validate(true)) return null;
				 TempSceneObjectPTR ob_dat = HierarchyTempSceneDataGetter.GetObjectData(SaverType.ModManualIcons, _o);
				 if (ob_dat == null || ob_dat.iconData.Length == 0 || !ob_dat.iconData[0].has_icon_color) return null;
				 return ob_dat.iconData[0].icon_color;
			 };


				//	_SetIconImage, Texture _GetTexture, Action< TempColorClass, string> _SetHiglightColor , Action< Color32, string> _SetIconColor, Func<Color32?> _GetIconColor
				//M_Colors_Window.Init(pos, "Select Icon", SET_TEXTURE, _o.drawIcon != null && _o.drawIcon.add_icon ? GET_TEXTURE : null, SET_HIGLIGHT_COLOR, GET_HIGLIGHTER_COLOR
				HighlighterWindow.Init(pos, "Select Icon", _SetIconImage, drawIcon ? _GetTexture : null, _SetHiglightColor, _GetHiglightColor, _SetIconColor, _GetIconColor
				, _o
				, adapter

									);
			}

			if (Event.current.button == 1)
			{
				Tools.EventUse();

				var icon = drawIcon;
				//	var icon = _o.drawIcon.add_icon;
				/*if (adapter.IS_PROJECT())   
				{
					if (!icon)
					{
						icon = AssetDatabase.GetCachedIcon(_o.project.assetPath);
						if (AssetDatabase.GetAssetPath(icon) == "Library/unity editor resources" && icon.name == "Folder Icon") icon = null;
					}
				}
				*/
				var mp = new MousePos(Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
				if (icon)
				{
					//var ext = adapter.IS_HIERARCHY() ? null : _o.project.fileExtension;
					var ttt = icon.name.Replace(adapter.pluginname + "_KEY_#1", "default");
					Windows.SearchWindow.Init(mp, SearchHelper + " '" + ttt + "' icon", "'" + ttt + "' icon",
										   CallHeaderFiltered(icon),
										   this, adapter, _o, null);
				}
				else
				{
					Windows.SearchWindow.Init(mp, SearchHelper + " User's icons Only", "any user's icons", CallHeader(), this, adapter, _o);
				}
				// EditorGUIUtility.ic
			}
		}

	}
}
