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
	class HighlighterDrawWorks
	{

		internal static Dictionary<int, new_child_struct> new_child_cache_dic = new Dictionary<int, new_child_struct>();
		internal static PluginInstance adapter { get { return Root.p[0]; } }
		internal static HighlighterMod HL_MOD { get { return Root.p[0].modsController.highLighterMod; } }


		internal static void ClearGroupingCache()
		{
			foreach (var nc in HighlighterDrawWorks.new_child_cache_dic)
			{
				nc.Value.wasInit = false;
				nc.Value.wasLastAssign = false;
			}
		}


		static Rect? lastBgRect;


		/// <summary>
		/// && (adapter.IS_LAYOUT)
		/// </summary>
		/// <returns></returns>
		static TempColorClass __masd123(int id, Rect? SelectionRect, int w, DynamicRect drawRect,
								 HierarchyObject o,
								 int needLabel_override, bool resetFonts, HierarchyObject parent, bool? returnAfterLastRect = null)
		{
			var drawhild = false;
			TempColorClass output;

			// if ( adapter.pluginID == 0 && o.name == "FootstepAudioSource" ) Debug.Log("start " + child_Res.BroadCast  + " " + adapter.IS_LAYOUT );
			// if ( /*!adapter.RedrawInit &&*/ child_Res.BroadCast != -1 && ( adapter.IS_LAYOUT || !new_child_cache_dic.ContainsKey( id ) ) || adapter.CHILD_GROUP_FIX_FLAG ) //
			if (child_Res.BroadCast != -1) //|| adapter.RedrawInit
			{
				lastBgRect = null;
				output = DrawColoredBG(drawRect, parent, o, true, needLabel_override, resetFonts,
									   returnAfterLastRect: returnAfterLastRect ?? true);

				if (lastBgRect.HasValue)
				{   //  bool shouldSet = adapter.IS_LAYOUT;

					if (!new_child_cache_dic.ContainsKey(id)
					   ) //  if ( adapter.pluginID == 0 && o.name == "Blob" ) Debug.Log( "BLob " + drawhild + " " + id );
					{   // Debug.Log( o.name + " " + id + " " + adapter.IS_LAYOUT );
						new_child_cache_dic.Add(id, new new_child_struct() { });
						/*  shouldSet = true;
						  if ( !adapter.IS_LAYOUT )
						  {   adapter.CHILD_GROUP_FIX_FLAG = true;
						  }*/
					}


					/*   if ( adapter.pluginID == 0
					           && o.name == "test" ) Debug.Log( SelectionRect.HasValue + " - " + (adapter.GROUPING_CHILD_MODE == 0) +  " - " +  child_Res.BroadCast + " - " + new_child_cache_dic[id].wasInit + " REPAINT " +
					                       adapter.CHILD_GROUP_FIX_FLAG + " " + id );*/

					//     if ( adapter.pluginID == 0 &&  o.name == "Directional Light (19)" ) Debug.Log( new_child_cache_dic[id].wasInit + " REPAINT " + adapter.CHILD_GROUP_FIX_FLAG + " " + id );
					//  if ( shouldSet )
					{
						if (o.id == parent.id)
						{
							if (!new_child_cache_dic[id].wasInit)
							{
								new_child_cache_dic[id].SetRect(lastBgRect.Value,
																		adapter.pluginID == 0 ? Prefabs.FindPrefabRoot(o.go) == o.go : false);
								// if ( adapter.pluginID == 0 &&  o.name == "Blob" ) Debug.Log( "ASD" );
							}

							else
							{
								var target = new_child_cache_dic[id].rect;
								var dif = lastBgRect.Value.y - target.y;
								target.y += dif;
								target.height -= dif;
								//new_child_cache[id].rect = target;

								new_child_cache_dic[id].SetRect(target,
																		adapter.pluginID == 0 ? Prefabs.FindPrefabRoot(o.go) == o.go : false);
								//  if ( adapter.pluginID == 0 &&  o.name == "Blob" ) Debug.Log( "ASD" );
							}
						}

						else
							if (!new_child_cache_dic[id].wasInit)
						{
							var target = lastBgRect.Value;

							if (o.id != parent.id)
							{
								target.y -= target.height;
								target.height += target.height;
							}


							new_child_cache_dic[id].SetRect(target,
																	adapter.pluginID == 0 ? Prefabs.FindPrefabRoot(o.go) == o.go : false);
							//  if ( adapter.pluginID == 0 &&  o.name == "Blob" ) Debug.Log( "ASD" );
						}
					}

					// if ( adapter.pluginID == 0 &&  o.name == "Blob" ) Debug.Log( lastBgRect.Value +  " "  + SelectionRect.Value );

					new_child_cache_dic[id].SetMax(lastBgRect.Value, SelectionRect.Value, adapter, output);
					// new_child_cache_dic[id].lastObjject = id;
				}
			}

			drawhild = new_child_cache_dic.ContainsKey(id);

			output = DrawColoredBG(drawRect, parent, o, true, needLabel_override, resetFonts,
								   overrideRect: drawhild
								   ? new_child_cache_dic[id].ConvertToLocal(SelectionRect.Value, adapter)
								   : null, returnAfterLastRect: returnAfterLastRect ?? false);

			return output;
		}



















		internal static TempColorClass DrawBackground(Rect? SelectionRect, bool IS_LAYOUT , DynamicRect drawRect,
											   HierarchyObject _o, int needLabel_override = 0, bool resetFonts = true)
		{   //if ( _o == null ) return null;

			//if (Event.current.type != EventType.Repaint && !adapter.IS_LAYOUT) return null;

	

			if (IS_LAYOUT) _o.ah.MIXINCOLOR = null;

			TempColorClass output = null;
			bool TryToMixOn = false;

			TempColorClass parent_o = null;
			//if (anyNeedBroadcast)
			//  if ( (child_Res = HighlighterHasKey_IncludeFilters( _o.scene, _o )).IsTrue( true ) )
			{
				var parent = _o.parent();

				while (parent != null)
				{
					if ((child_Res = HighlighterCache_Colors.HighlighterHasKey_IncludeFilters( parent)).IsTrue(true))
					{
						if (!SelectionRect.HasValue || adapter.par_e.HIGHLIGHTER_GROUPING_CHILD_MODE== 0 || child_Res.BroadCast == -1)
						{
							if (IS_LAYOUT)
								parent_o = DrawColoredBG(drawRect, parent, _o, true, needLabel_override, resetFonts,
														 returnAfterLastRect: true);
							else parent_o = DrawColoredBG(drawRect, parent, _o, true, needLabel_override, resetFonts);
						}

						else
						{
							var id = parent.id;

							if (IS_LAYOUT)
								parent_o = __masd123(id, SelectionRect, 5, drawRect, _o, needLabel_override, resetFonts,
													 parent, returnAfterLastRect: true);
							else
								parent_o = __masd123(id, SelectionRect, 5, drawRect, _o, needLabel_override, resetFonts,
													 parent);
						}

						if (IS_LAYOUT) /*&& (parent_o.HAS_LABEL_COLOR && !parent_o.HAS_BG_COLOR || !parent_o.HAS_LABEL_COLOR && parent_o.HAS_BG_COLOR)*/
						{
							TryToMixOn = true;
							break;
						}

						else
						{
							return parent_o;
						}
					}

					parent = parent.parent();
				}
			}

			// if ( TryToMixOn ) Debug.Log( _o.name );
			if ((child_Res = HighlighterCache_Colors.HighlighterHasKey_IncludeFilters( _o)).IsTrue(false))
			//  if ( adapter.pluginID == 0 && o.name == "BloodPos" ) Debug.Log( SelectionRect.HasValue + " - " + (adapter.GROUPING_CHILD_MODE == 0) +  " - " +  child_Res.BroadCast );
			{
				if (!SelectionRect.HasValue || adapter.par_e.HIGHLIGHTER_GROUPING_CHILD_MODE == 0 || child_Res.BroadCast == -1)
				//  if ( o.name == "Misc" ) Debug.Log( Event.current.type + " " +  DrawColoredBG( drawRect, o, o, false, needLabel_override, resetFonts, returnAfterLastRect: true ).BG_ALIGMENT_LEFT_CONVERTED );
				{
					if (IS_LAYOUT)
						output = DrawColoredBG(drawRect, _o, _o, false, needLabel_override, resetFonts,
											   returnAfterLastRect: true);
					else output = DrawColoredBG(drawRect, _o, _o, false, needLabel_override, resetFonts);
				}

				else
				{
					var id = _o.id;

					if (IS_LAYOUT)
						return __masd123(id, SelectionRect, 5, drawRect, _o, needLabel_override, resetFonts, _o,
										 returnAfterLastRect: true);
					else output = __masd123(id, SelectionRect, 5, drawRect, _o, needLabel_override, resetFonts, _o);
				}


				if (!TryToMixOn) return output;

				bool anyChange = false;

				if (output.HAS_BG_COLOR /* && parent_o.HAS_LABEL_COLOR*/)
				{
					TempColorClass.CopyFromTo(CopyType.BG, output, ref parent_o);
					anyChange = true;
				}

				if (output.HAS_LABEL_COLOR /*&& parent_o.HAS_BG_COLOR*/)
				{
					TempColorClass.CopyFromTo(CopyType.LABEL, output, ref parent_o);
					anyChange = true;
				}

				if (anyChange)
				{
					if (_o.ah.localTempColor == null) _o.ah.localTempColor = new TempColorClass().AssignFromList(0, true);

					// _o.localTempColor.CopyFromList( parent_o.el.list );
					TempColorClass.CopyFromTo(CopyType.BG, parent_o, ref _o.ah.localTempColor);
					TempColorClass.CopyFromTo(CopyType.LABEL, parent_o, ref _o.ah.localTempColor);
					// if ( _o.name == "SpawnPoint" ) Debug.Log( _o.MIXINCOLOR.HAS_LABEL_COLOR );
					return _o.ah.MIXINCOLOR = _o.ah.localTempColor;
				}

				else
				{
					return parent_o;
				}
			}

			return null;
		}












		static HighlighterResult child_Res;
		internal static TempColorClass needdrawGetColor(HierarchyObject o)
		{
			if (!adapter.par_e.USE_MANUAL_HIGHLIGHTER_MOD && !adapter.par_e.USE_AUTO_HIGHLIGHTER_MOD) return null;

			if ((child_Res = HighlighterCache_Colors.HighlighterHasKey_IncludeFilters(o)).IsTrue(false))
			{
				return DrawColoredBG(null, o, o, false);
			}
			else
			{
				var parent = o.parent();

				while (parent != null)
				{
					if ((child_Res = HighlighterCache_Colors.HighlighterHasKey_IncludeFilters(parent)).IsTrue(true))
					{
						return DrawColoredBG(null, parent, o, true);
					}
					parent = parent.parent();
				}
			}
			return null;
		}




		internal static TempColorClass DrawColoredBG(DynamicRect drawRect, HierarchyObject parentObject,
										  HierarchyObject currentObject, bool breakIfNoBroadCast, int SKIPLABEL = 0, bool resetFonts = true,
										  new_child_struct overrideRect = null, bool returnAfterLastRect = false)
		{
			if (drawRect != null) currentObject.ah.BACKGROUNDED = 0;


			if (parentObject.ah.localTempColor == null) parentObject.ah.localTempColor = new TempColorClass().AssignFromList(0, true);
			var tempColor = parentObject.ah.localTempColor;
			/* if (Event.current.type == EventType.Repaint)
			     if ( currentObject.name == "SpawnPoint" ) Debug.Log( currentObject.MIXINCOLOR.HAS_LABEL_COLOR );*/

			if (Event.current.type == EventType.Repaint && currentObject.ah.MIXINCOLOR != null)
			{
				tempColor = currentObject.ah.MIXINCOLOR;
			}
			else
			{
				bool anywayelse = true;
				var hk = HighlighterCache_Colors.HighlighterHasKey(parentObject);
				if (hk.IsTrue(breakIfNoBroadCast))
				{
					if (hk.id != -1)
					{
						anywayelse = false;
						//tempColor.CopyFromFilter(hk.data);
						tempColor.AssignFromList(ref hk.data);
						if (breakIfNoBroadCast && !tempColor.child) return null;
					}
				}
				if (anywayelse)
				{
					var filter = HL_MOD.autoMod.GetFilter(parentObject);
					if (filter == null || breakIfNoBroadCast && !filter.child || !filter.HAS_BG_COLOR && !filter.HAS_LABEL_COLOR) return null;
					// filter.OverrideTo(ref tempColor);
					tempColor.CopyFromFilter(filter);
				}
			}


			if (drawRect == null) return tempColor;


			var colorRect = drawRect.ref_selectionRect;
			colorRect.x -= adapter.hierarchyModification.LEFT_PADDING;
			/*  if ( adapter.HIGHLIGHTER_LEFT_OVERFLOW != 1 )
			      colorRect.x -= adapter.raw_old_leftpadding;*/
			if (UnityVersion.UNITY_CURRENT_VERSION < UnityVersion.UNITY_2019_2_0_VERSION)
				colorRect.x += adapter.hierarchyModification.LEFT_PADDING;


			var BgRect = overrideRect == null ? drawRect.ConvertToBGFromTempColor(tempColor) : overrideRect.LocalRect;
			if (returnAfterLastRect)
			{
				if (SKIPLABEL != 2 && tempColor.HAS_BG_COLOR)
				{
					lastBgRect = BgRect;
					return tempColor;
				}
				return tempColor;
			}



			DrawGradient(BgRect, tempColor, true, DrawBgDynamicColorHelper, currentObject);
			return tempColor;
			//throw new Exception("fallin");
		}











		static DynamicColor __DrawBgDynamicColorHelper;
	static	DynamicColor DrawBgDynamicColorHelper
		{   // if ( adapter.IsSelected( id ) ) Debug.Log( GUI.GetNameOfFocusedControl() );
			get
			{
				if (__DrawBgDynamicColorHelper == null)
				{
					__DrawBgDynamicColorHelper = new DynamicColor()
					{
						GET = (currentObject, args) =>
						{
							var str = (DrawBgDynamicColorHelperStruct)args;
							var BGCOLOR = str.BGCOLOR;


							if (HL_MOD.LocalIsSelected(currentObject.id))
							{
								BGCOLOR.a = (BGCOLOR.a * 200 / 255f);
							}

							else
								if (adapter.hashoveredItem && adapter.hoverID == currentObject.id && !adapter.par_e.HIDE_HOVER_BG)
							{
								BGCOLOR.a = (BGCOLOR.a * (adapter.current_DragSelection_List.Count == 0
														  ? 200 / 255f
														  : 200 / 255f));
							}

							if (!currentObject.Active()) BGCOLOR.a /= 2;

							//var sourceBgColor = SourceBGColor.GET(currentObject, null); //currentObject.id
							var sourceBgColor = BGCOLOR;
							sourceBgColor.a *= adapter.par_e.HIGHLIGHTER_COLOR_OPACITY;
							sourceBgColor.a *= str.alpha;
							currentObject.ah.BACKGROUNDEsourceBgColorD = sourceBgColor;
							return sourceBgColor;
						}
					};
				}

				return __DrawBgDynamicColorHelper;
			}
		}

		/*

		DynamicColor __SourceBGColor;

		DynamicColor SourceBGColor // if ( adapter.IsSelected( id ) ) Debug.Log( GUI.GetNameOfFocusedControl() );
		{
			get
			{
				if (__SourceBGColor == null)
				{
					__SourceBGColor = new DynamicColor()
					{
						GET = (_o, args) =>
						{
							return adapter.IsSelected(_o.id) ? adapter.SelectColor :
								   adapter.hoverID == _o.id && !adapter.par_e.HIDE_HOVER_BG ? Colors.HoverColor : Colors.EditorBGColor;
						}
					};
				}
				return __SourceBGColor;
			}
		}
		*/
















		static Rect MoveToClip(Rect rect, Rect clip)
		{
			rect.x -= clip.x;
			rect.y -= clip.y;
			return rect;
		}
		#pragma warning disable
		static Rect FULL_RECT;
		static Rect RIGHT;
		static Rect LEFT;
		#pragma warning restore

		//  Color asdasd4 = ;
		static void DrawGradient(Rect rect, TempColorClass tempColor, bool isMain, DynamicColor color,
						  HierarchyObject currentObject) // if ( Event.current.type != EventType.Repaint ) return;
		{
			var BGCOLOR = (Color)tempColor.BGCOLOR;

			if (tempColor.BG_ALIGMENT_RIGHT_CONVERTED == BgAligmentRight.WidthFixedGradient)
			{
				DRAW_BGTEXTURE(BGCOLOR, rect, adapter.GetExternalModOld(EditorGUIUtility.isProSkin ? "HL_GRADIENT" : "HL_GRADIENT PERSONAL"), color);
				DRAW_BGTEXTURE(BGCOLOR, MoveToClip(FULL_RECT, RIGHT), color);
			}

			else
			{
				bool RIGHTHasValue = false;
				bool LEFTHasValue = false;

				/*if (isMain && tempColor.BG_ALIGMENT_RIGHT_CONVERTED == BgAligmentRight.MaxRight)
				{
					if (UnityVersion.UNITY_CURRENT_VERSION >= UnityVersion.UNITY_2019_2_0_VERSION)
						rect.width -= adapter.hierarchyModification.PREFAB_BUTTON_SIZE;
					//rect.width -= adapter.raw_old_leftpadding;
					if (adapter.hierarchyModification.PREFAB_BUTTON_SIZE != 0 && (adapter.pluginID == 0
															? Prefabs.FindPrefabRoot(currentObject.go) == currentObject.go
															: false) )
					{
						var r = rect;
						r.x += rect.width;
						r.width = adapter.hierarchyModification.PREFAB_BUTTON_SIZE;
						RIGHTHasValue = true;
						RIGHT = r;
					}
					else
					{
						rect.width += adapter.hierarchyModification.PREFAB_BUTTON_SIZE;
					}
					// GUI.DrawTexture( r, BG_TEXTURE );
				}*/

				// if ( isMain && tempColor.BG_ALIGMENT_RIGHT_CONVERTED == BgAligmentRight.MaxRight && adapter.HIGHLIGHTER_LEFT_OVERFLOW == 1)
				if (isMain && tempColor.BG_ALIGMENT_LEFT_CONVERTED == BgAligmentLeft.MinLeft &&
						adapter.par_e.HIGHLIGHTER_LEFT_OVERFLOW == 1
						&& adapter.hierarchyModification.LEFT_PADDING != 0) // rect.x -= adapter.raw_old_leftpadding;
				{   // rect.width += adapter.raw_old_leftpadding;

					var r = rect;

					r.x -= adapter.hierarchyModification.LEFT_PADDING;
					r.width = adapter.hierarchyModification.LEFT_PADDING;
					LEFTHasValue = true;
					LEFT = r;
				}


				if (RIGHTHasValue || LEFTHasValue)
				{
					FULL_RECT = LEFTHasValue ? LEFT : rect;
					var t = RIGHTHasValue ? RIGHT : rect;
					FULL_RECT.width = t.x + t.width - FULL_RECT.x;
				}

				bool drawClip = adapter.par_e.HIGHLIGHTER_TEXTURE_BORDER != 0 && adapter.par_e.HIGHLIGHTER_TEXTURE_STYLE != 0;


				if (RIGHTHasValue)
				{
					if (drawClip) // GUI.BeginClip( RIGHT );
					{
						//Draw_BeginClip(RIGHT);
						DRAW_BGTEXTURE(BGCOLOR, MoveToClip(FULL_RECT, RIGHT), color, 0.2f, RIGHT);
						//Draw_EndClip();
					}

					else
						DRAW_BGTEXTURE(BGCOLOR, RIGHT, color, 0.2f);
				}

				if (LEFTHasValue)
				{
					if (drawClip) // GUI.BeginClip( RIGHT );
					{
						//Draw_BeginClip(LEFT);
						DRAW_BGTEXTURE(BGCOLOR, MoveToClip(FULL_RECT, LEFT), color, 0.4f, LEFT);
						//Draw_EndClip();
					}

					else
						DRAW_BGTEXTURE(BGCOLOR, LEFT, color, 0.4f);
				}


				if ((RIGHTHasValue || LEFTHasValue) && drawClip) // GUI.BeginClip( rect );
				{
					//Draw_BeginClip(rect);
					DRAW_BGTEXTURE(BGCOLOR, MoveToClip(FULL_RECT, rect), color, clipRect: rect);
					//Draw_EndClip();
				}

				else
				{
					DRAW_BGTEXTURE(BGCOLOR, rect, color);
				}
			}
		}
		static DrawBgDynamicColorHelperStruct DrawBgDynamicColorHelperStructStr;


		static internal void DRAW_BGTEXTURE(Color BGCOLOR, Rect rect, DynamicColor color, float alpha = 1, Rect? clipRect = null)
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
					HL_MOD.Draw_GUITextureWithBorders(rect, t, border, color, DrawBgDynamicColorHelperStructStr, clipRect);
				else
					HL_MOD.Draw_GUITextureWithBordersAndMaterial(rect, t, border, adapter.par_e.HIghlighterExternalMaterial, color, DrawBgDynamicColorHelperStructStr, clipRect);
			}
		}

		static internal void DRAW_BGTEXTURE(Color BGCOLOR, Rect rect, Texture2D BG_TEXTURE, DynamicColor color)
		{
			if (Event.current.type == EventType.Repaint)
			{
				DrawBgDynamicColorHelperStructStr.alpha = 1;
				DrawBgDynamicColorHelperStructStr.BGCOLOR = BGCOLOR;
				var t = Icons.GetIconDataFromTexture(adapter.par_e.BG_TEXTURE ?? Texture2D.whiteTexture);

				if (!adapter.par_e.HIGHLIGHTER_USE_SPECUAL_SHADER || !adapter.par_e.HIghlighterExternalMaterial || adapter.par_e.HIGHLIGHTER_TEXTURE_STYLE == 0 || !BG_TEXTURE)
					HL_MOD.Draw_GUITextureWithBorders(rect, t, 0, color, DrawBgDynamicColorHelperStructStr);
				else
					HL_MOD.Draw_GUITextureWithBordersAndMaterial(rect, t, 0, adapter.par_e.HIghlighterExternalMaterial, color, DrawBgDynamicColorHelperStructStr);
			}
		}



















		/*
		void _old()
		{

#pragma warning disable

			if (Adapter.USE2018_3 && adapter.pluginID == Initializator.HIERARCHY_ID
					|| adapter.pluginID == Initializator.PROJECT_ID
			   ) // var IH = Math.Min(EditorGUIUtility.singleLineHeight, drawRect.Value.height);
			{
				colorRect.x += adapter.DEFAULT_ICON_SIZE;
				colorRect.width -= adapter.DEFAULT_ICON_SIZE;
			}

			//  else if (adapter.UNITYVERSION <= 55) colorRect.x += 3;
#pragma warning restore
			// var oc = GUI .color;
			var LABEL_STYLE = adapter.labelStyle;
			var LABEL_COLOR = LABEL_STYLE.normal.textColor;

			var oldLabelColor = LABEL_COLOR;
			var al1 = tempColor.BG_ALIGMENT_RIGHT_CONVERTED;
			var al2 = tempColor.BG_ALIGMENT_LEFT_CONVERTED;

			var NEW_NEED_LABEL = al2 == BgAligmentLeft.EndLabel || al2 == BgAligmentLeft.Modules ||
								 al1 == BgAligmentRight.BeginLabel || al1 == BgAligmentRight.Icon;
			NEW_NEED_LABEL = !NEW_NEED_LABEL;

			bool needLabel = false;

			if (SKIPLABEL != 1)
			{
#pragma warning disable
#pragma warning restore

				if (tempColor.HAS_LABEL_COLOR)
				{
					needLabel = true;

					LABEL_COLOR = tempColor.LABELCOLOR;
				}

				else
				{
					var type = adapter.IS_HIERARCHY()
							   ? adapter.GetPrefabType(currentObject.go)
							   : PrefabInstanceStatus.NotAPrefab;

					// if (!(type == PrefabType.None || type == PrefabType.DisconnectedModelPrefabInstance || type == PrefabType.DisconnectedPrefabInstance))
					if (!(type == PrefabInstanceStatus.NotAPrefab || type == PrefabInstanceStatus.Disconnected))
					{
						var co = EditorGUIUtility.isProSkin ? prefabColorPro : prefabColorPersonal;

						if (!currentObject.Active()) co.a /= 2;

						LABEL_COLOR = co;
						needLabel = NEW_NEED_LABEL;
					}

					else
						if ((type == PrefabInstanceStatus.MissingAsset))
					{
						var co = EditorGUIUtility.isProSkin ? prefabMissinColorPro : prefabMissinColorPersonal;

						if (!currentObject.Active()) co.a /= 2;

						LABEL_COLOR = co;
						needLabel = NEW_NEED_LABEL;
					}

					else
					// {   Adapter.GET_SKIN().label.normal.textColor = parentObject.cache_prefab ? m_PrefabColorPro : adapter.oldColl;
					{
						LABEL_COLOR = oldLabelColor;
					}
				}
			}


	


			if (( needLabel) && SKIPLABEL != 1)
			{
				var sourceBgColor = SourceBGColor; //currentObject.id


				BEGIN_DRAW_SWITCHER(sourceBgColor_fadeBg_swithcer_HASH);

				// if ( adapter.hashoveredItem && adapter.hoverID == currentObject.id )
				{   // var _oc = LABEL_COLOR;
					//var _oc = adapter.oldColl;
					//LABEL_COLOR = sourceBgColor;
					//  colorRect.x -= 0.5f;
					var DDD = 0.5f;
					// var O = 0.75f;
					var frect = colorRect;
					Draw_LabelWithTextDynamicColor(frect, currentObject.name, sourceBgColor, LABEL_STYLE, false,
												   SWITCHER: 0);
					frect.x += DDD;
					frect.y -= DDD;

					Draw_LabelWithTextDynamicColor(frect, currentObject.name, sourceBgColor, LABEL_STYLE, false,
												   SWITCHER: 0);
					// GUI.Label( frect, currentObject.name, LABEL_STYLE );
					// frect.x += 1;
					frect.y += DDD * 2;
					Draw_LabelWithTextDynamicColor(frect, currentObject.name, sourceBgColor, LABEL_STYLE, false,
												   SWITCHER: 0);
					//  GUI.Label( frect, currentObject.name, LABEL_STYLE );
					frect.x -= DDD * 2;

					// frect.x -= DDD;
					Draw_LabelWithTextDynamicColor(frect, currentObject.name, sourceBgColor, LABEL_STYLE, false,
												   SWITCHER: 0);
					// GUI.Label( frect, currentObject.name, LABEL_STYLE );
					// frect.x += 1;
					frect.y -= DDD * 2;
					Draw_LabelWithTextDynamicColor(frect, currentObject.name, sourceBgColor, LABEL_STYLE, false,
												   SWITCHER: 0);
					// GUI.Label( frect, currentObject.name, LABEL_STYLE );
					// frect.x -= DDD;
					//   GUI.Label( frect, currentObject.name, LABEL_STYLE );
					// frect.y -= O;
					// colorRect.x -= 0.5f;
					//  colorRect.x += 1;
					//GUI.Label( colorRect, currentObject.name );
					//LABEL_COLOR = _oc;
				}
				//   else
				{  

					Draw_AdapterTextureWithDynamicColor(colorRect, sourceBgColor, SWITCHER: 1);
				}

				END_DRAW_SWITCHER();
			}

			if (SKIPLABEL != 2)
				if (tempColor.HAS_BG_COLOR)
				{   //GUI .color *= sourceBgColor;
					//GU I.color = oc;
					lastBgRect = BgRect;


					if (overrideRect != null)
						//GUI.BeginClip( overrideRect.ModifiedSelectionRect( adapter ) );
						Draw_BeginClip(overrideRect.ModifiedSelectionRect(adapter));

					DrawGradient(BgRect, tempColor, drawRect.isMain, DrawBgDynamicColorHelper, currentObject);

					if (overrideRect != null)
						//GUI.EndClip();
						Draw_EndClip();

					needLabel = NEW_NEED_LABEL;


					var LEFT_BG = overrideRect != null
								  ? overrideRect.ModifiedSelectionRect(adapter).x + BgRect.x
								  : BgRect.x;
					var LEFT_LABEL = colorRect.x;
#pragma warning disable

					if (Adapter.USE2018_3 && adapter.pluginID == Initializator.HIERARCHY_ID
							|| adapter.pluginID == Initializator.PROJECT_ID
					   ) // var IH = Math.Min(EditorGUIUtility.singleLineHeight, drawRect.Value.height);
					{
						LEFT_LABEL -= adapter.DEFAULT_ICON_SIZE + 16;
					}

#pragma warning restore
				
				

					var LEFT_SKIP = LEFT_LABEL < LEFT_BG;

					if (tempColor.BG_ALIGMENT_RIGHT_CONVERTED == BgAligmentRight.BeginLabel ||
							tempColor.BG_ALIGMENT_RIGHT_CONVERTED == BgAligmentRight.Icon)
						LEFT_SKIP |= LEFT_LABEL > LEFT_BG + BgRect.width;

					// var RIGHT_SKIP =  RIGHT_LABEL > RIGHT_BG;
					var RIGHT_SKIP = false;

					if (tempColor.BG_ALIGMENT_LEFT_CONVERTED == BgAligmentLeft.Modules) RIGHT_SKIP = true;

					if (!RIGHT_SKIP) currentObject.FLAGS |= 1;

					var worldRect = BgRect;

					if (overrideRect != null)
					{
						worldRect.x += overrideRect.ModifiedSelectionRect(adapter).x;
						worldRect.y += overrideRect.ModifiedSelectionRect(adapter).y;
					}

					currentObject.BG_RECT = worldRect;
					// || colorRect.x > X
				
					currentObject.BACKGROUNDED =
						(tempColor.BG_HEIGHT == 2 && overrideRect == null || LEFT_SKIP) ? 2 : 1;
				}


			if ((needLabel || tempColor.HAS_LABEL_COLOR) && SKIPLABEL != 1)
			{

				//  Adapter.GET_SKIN().label.fontStyle = FontStyle.Italic;
				var labelRect = colorRect;

				if (adapter.par_e.USE_LINE_HEIGHT)
				{
					if (UnityVersion.UNITY_CURRENT_VERSION < UnityVersion.UNITY_2019_3_0_VERSION)
					{
						if (UnityVersion.UNITY_CURRENT_VERSION < UnityVersion.UNITY_2019_VERSION)
						{
							if (Mathf.RoundToInt(adapter.par_e.LINE_HEIGHT) % 2 == 1)
								labelRect.y = Mathf.CeilToInt(labelRect.y - 0.1f);
							else
								labelRect.y = Mathf.CeilToInt(labelRect.y + 0.1f);
						}

						else
						{
							if (Mathf.RoundToInt(adapter.par_e.LINE_HEIGHT) % 2 != 1) labelRect.y -= 1;
						}
					}
				}




				if (tempColor != null && tempColor.HAS_LABEL_COLOR && tempColor.LABEL_SHADOW == true)
				{
					var _oc2 = LABEL_COLOR;
					// var c2 = adapter.oldColl;
					var c2 = Color.black;
					c2.a = _oc2.a;
					LABEL_COLOR = c2;
					// if ( currentObject.name == "CubeMaps" ) Debug.Log( needLabel && SKIPLABEL != 1 );

					if (UnityVersion.UNITY_CURRENT_VERSION < UnityVersion.UNITY_2019_3_0_VERSION)
					{
						labelRect.y -= 0.5f; //0.5f;
						labelRect.x -= 1; //0.5f;
										  // GUI.Label( labelRect, currentObject.name, LABEL_STYLE );
						Draw_LabelWithTextColor(labelRect, currentObject.name, LABEL_COLOR, LABEL_STYLE, false);
						labelRect.y += 0.5f; // 0.5f;
						labelRect.x += 1; // 0.5f;
					}

					else
					{
						labelRect.y -= 1f; //0.5f;
						labelRect.x -= 1; //0.5f;
						Draw_LabelWithTextColor(labelRect, currentObject.name, LABEL_COLOR, LABEL_STYLE, false);
						//GUI.Label( labelRect, currentObject.name, LABEL_STYLE );
						labelRect.x -= 1f; //0.5f;
						Draw_LabelWithTextColor(labelRect, currentObject.name, LABEL_COLOR, LABEL_STYLE, false);
						//  GUI.Label( labelRect, currentObject.name, LABEL_STYLE );
						labelRect.y += 1; // 0.5f;
						labelRect.x += 2; // 0.5f;
					}

					// var fs = LABEL_STYLE.fontStyle;
					// LABEL_STYLE.fontStyle = FontStyle .Bold;
					// LABEL_STYLE.fontStyle = fs;
					LABEL_COLOR = _oc2;
				}


				Draw_LabelWithTextColor(labelRect, currentObject.name, LABEL_COLOR, LABEL_STYLE, false);

			}



			return tempColor;
		}

	*/















	}






	internal class new_child_struct
	{
		// internal Rect lastRect;
		Rect __rect;
		internal Rect rect { get { return __rect; } }
		internal void SetRect(Rect rect, bool isPrefab)
		{
			__rect = rect;
			isMaxRight = false;
			this.isPrefab = isPrefab;

			if (!wasInit) wasInit = true;
		}
#pragma warning disable
		internal bool wasInit;
		internal bool wasLastAssign;
		bool isPrefab = false;
		bool isMaxRight = false;
#pragma warning restore
		internal void SetMax(Rect value, Rect SelectionRect, PluginInstance adapter, TempColorClass tc)
		{
			if (tc.BG_ALIGMENT_RIGHT_CONVERTED == BgAligmentRight.BeginLabel || tc.BG_ALIGMENT_RIGHT_CONVERTED == BgAligmentRight.Icon)
			{
				if (value.x < __rect.x)
				{
					__rect.width += __rect.x - value.x;
					__rect.x = value.x;
				}

				if (value.x + value.width < __rect.x + __rect.width) __rect.width = value.x + value.width - __rect.x;
			}

			else
				if (tc.BG_ALIGMENT_LEFT_CONVERTED == BgAligmentLeft.EndLabel)
			{
				if (value.x > __rect.x)
				{
					__rect.width += __rect.x - value.x;
					__rect.x = value.x;
				}

				if (value.x + value.width > __rect.x + __rect.width) __rect.width = value.x + value.width - __rect.x;
			}

			else
			{
				if (value.x < __rect.x)
				{
					__rect.width += __rect.x - value.x;
					__rect.x = value.x;
				}

				if (value.x + value.width > __rect.x + __rect.width) __rect.width = value.x + value.width - __rect.x;
			}

			if (value.y < __rect.y)
			{
				__rect.height += __rect.y - value.y;
				__rect.y = value.y;
			}

			if (value.y + value.height > __rect.y + __rect.height) __rect.height = value.y + value.height - __rect.y;

			isMaxRight = tc.BG_ALIGMENT_RIGHT_CONVERTED == BgAligmentRight.MaxRight;
			SelectionRect.width += adapter.hierarchyModification.PREFAB_BUTTON_SIZE;

			if (__rect.x + __rect.width > SelectionRect.x + SelectionRect.width) __rect.width = SelectionRect.x + SelectionRect.width - __rect.x;

			if (!wasLastAssign)
			{
				wasLastAssign = true;

				if (__rect.y + __rect.height > SelectionRect.y + SelectionRect.height) __rect.height = SelectionRect.y + SelectionRect.height - __rect.y;
			}
		}

		internal Rect ModifiedSelectionRect(PluginInstance adapter)
		{
			var res = SelectionRect;
			res.width += HighlighterDrawWorks.adapter.hierarchyModification.PREFAB_BUTTON_SIZE;
			return res;
		}

		internal Rect SelectionRect;
		internal Rect LocalRect;
		internal new_child_struct ConvertToLocal(Rect SelectionRect, PluginInstance adapter)
		{
			LocalRect = __rect;

			if (LocalRect.x + LocalRect.width > SelectionRect.x + SelectionRect.width + adapter.hierarchyModification.PREFAB_BUTTON_SIZE) LocalRect.width = SelectionRect.x + SelectionRect.width - LocalRect.x +
					  adapter.hierarchyModification.PREFAB_BUTTON_SIZE;



			LocalRect.x -= SelectionRect.x;
			LocalRect.y -= SelectionRect.y;

			//if (adapter.hierarchyModification.PREFAB_BUTTON_SIZE != 0 && !isPrefab)     //LocalRect.width += adapter.PREFAB_BUTTON_SIZE;
			{
			}


			this.SelectionRect = SelectionRect;
			return this;
		}
		// internal int lastObjject;
	}













}
