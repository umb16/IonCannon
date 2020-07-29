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
	class IconDrawWorks
	{

		internal static void Draw()
		{
			/*
			 * 
			 * 	if (callFromExternal() || !_o.internalIcon) _o.drawIcon = GET_CONTENT(_o);

						_o.internalIcon = false;
						var labelPlace = _o.switchType == 1 && USE2018_3;
						var icon_place = labelPlace ? 0 : adapter._S_bgIconsPlace;

						if (callFromExternal()) icon_place = 2;


						var icon_rect = labelPlace ? GetIconRect(selectionRect, overrideSBGIconPlace: 0) : GetIconRect(selectionRect);

						EditorGUIUtility.SetIconSize(Vector2.zero);


						bool auto = _o.switchType > 0 || _o.cache_prefab;
						// float __IS = adapter.par.COLOR_ICON_SIZE;
						float __IS = adapter.par.COLOR_ICON_SIZE - (adapter.DEFAULT_ICON_SIZE - EditorGUIUtility.singleLineHeight) / 2;

						// if  (__o. cache_prefab )__IS = adapter.DEFAULT_ICON_SIZE;
						if (_o.switchType == 1) __IS = adapter.DEFAULT_ICON_SIZE;

						if (_o.switchType == 2) __IS = EditorGUIUtility.singleLineHeight;

						var ICON_SIZE = Mathf.CeilToInt(__IS - EditorGUIUtility.singleLineHeight);

						if (icon_place != 2) icon_rect.x -= Mathf.CeilToInt(ICON_SIZE / 2f);

						icon_rect.y -= ICON_SIZE / 2;
						icon_rect.width += ICON_SIZE;
						icon_rect.height += ICON_SIZE;










						if (_o.drawIcon.add_icon)
						{




							//if (__o.switchType == 0)  Debug.Log(__o.drawIcon.add_hasiconcolor);
							var COLOR = Color.white;

							if (_o.switchType == 0 && _o.drawIcon.add_hasiconcolor)
							{   //backCol = GUI.color;

								COLOR *= _o.drawIcon.add_iconcolor;
								// if ( IconImageCacher.HasKey( __o.scene , __o ) ) Debug.Log( GUI.color );
							}


							var c = (_o.switchType == 2 || _o.Active() ? Color.white : inactiveColor) * COLOR;

							if (_o.switchType == 1)
							{
								var dw = (icon_rect.width - 7) / 2;
								var dh = (icon_rect.height - 7) / 2;
								// var d = (icon_rect.width - adapter.parLINE_HEIGHT) / 2;
								//   Adapter.DrawTexture( new Rect( icon_rect.x + dw, icon_rect.y + dh, 7, 7 ), _o.drawIcon.add_icon, _o.switchType == 2 || _o.Active() ? Color.white : inactiveColor );
								ICON_STACK.Draw_AdapterTexture(new Rect(icon_rect.x + dw, icon_rect.y + dh, 7, 7), (Texture2D)_o.drawIcon.add_icon, c, false);
							}

							else
							{   //Adapter.DrawTexture( icon_rect, _o.drawIcon.add_icon, _o.switchType == 2 || _o.Active() ? Color.white : inactiveColor );
								ICON_STACK.Draw_AdapterTexture(icon_rect, (Texture2D)_o.drawIcon.add_icon, c, false);

								if (adapter._S_bgIconsPlace == 2)
								{
									ICON_STACK.Draw_Action(icon_rect, SKIP_CHILD_COUNT_ACTION_HASH, null);
								}
							}

							if (!adapter.IS_SEARCH_MOD_OPENED() && !callFromExternal())
							{   //IconOverlayGUI( icon_rect, _o.GetTreeItem( adapter ) );
								overlayButStr.treeItem = _o.GetTreeItem(adapter);
								ICON_STACK.Draw_Action(icon_rect, SECOND_ICON_OVERLAY_ACTION_HASH, overlayButStr);
							}

							if (!auto && adapter.par.BottomParams.DRAW_FOLDER_STARMARK)
							{   //Adapter.DrawTexture( icon_rect, adapter.GetIcon( "FOLDER_STARMARK" ), Color.white );
								ICON_STACK.Draw_AdapterTexture(icon_rect, adapter.GetIcon("FOLDER_STARMARK"), Color.white, true);

							}


							if (_o.switchType == 0 && _o.drawIcon.add_hasiconcolor)
							{   // GUI.color = backCol;
							}


							switch (_o.switchType)
							{
								case 0: switchedConten.tooltip = !_o.drawIcon.add_icon ? "" : _o.drawIcon.add_icon.name; break;

								case 1: switchedConten.tooltip = ""; break; //contentNull.tooltip

								case 2: switchedConten.tooltip = contentMis.tooltip; break;
							}
						}

						else
							if (!adapter.IS_PROJECT())
						{  

							//  if ( __o.name == "Directional Light (6)" ) Debug.Log( "ASD" );
							// CHeckIcon( __o ,)
							switchedConten.tooltip = content.tooltip;

						}

				*/

		}
	}
}
