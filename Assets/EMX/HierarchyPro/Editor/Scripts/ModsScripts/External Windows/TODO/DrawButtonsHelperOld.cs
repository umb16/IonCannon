using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using EMX.HierarchyPlugin.Editor.Windows;
using Object = UnityEngine.Object;
using UnityEditor.SceneManagement;
using EMX.HierarchyPlugin.Editor.Mods.BookObject;

namespace EMX.HierarchyPlugin.Editor.Mods
{

	/*
	internal class MemoryRoot
	{
		internal bool IsValid()
		{
			if (objects == null || objects.Length == 0) return false;
			return objects[0];
		}

		internal Object[] objects = new Object[0];
	}*/
	//BOOKMARKS_OB_KEY
	internal class ISET_ROW
	{
		static PluginInstance adapter { get { return Root.p[0]; } }
		string KEY;
		int def_rows, def_items;
		int _LastIndex = -1;
		internal int LastIndex
		{
			get { return _LastIndex; }
			set
			{
				_LastIndex = value;
			}
		}
		internal int LastSelectedRoot = -1;

		internal ISET_ROW(string key, int def_rows, int def_items)
		{
			this.KEY = key;
			this.def_rows = def_rows;
			this.def_items = def_items;
		}


		//internal int LineHeight { get { return adapter.par_e.GET(KEY + "_LINEHEIGHT", 11); } set { adapter.par_e.SET(KEY + "_FONTSIZE", value); } }
		internal string _fontSize_KEY { get { return "fontSize"; } }
		internal int fontSize { get { return adapter.par_e.GET(KEY + "_FONTSIZE", 10); } set { adapter.par_e.SET(KEY + "_FONTSIZE", value); } }
		internal string _Rows_KEY { get { return "Rows"; } }
		internal int Rows { get { return adapter.par_e.GET(KEY + "_ROWS", def_rows); } set { adapter.par_e.SET(KEY + "_ROWS", value); } }
		internal string _MaxItems_KEY { get { return "MaxItems"; } }
		internal int MaxItems { get { return adapter.par_e.GET(KEY + "_MAX_ITEMS", def_items); } set { adapter.par_e.SET(KEY + "_MAX_ITEMS", value); } }

		internal string _DrawHiglighter_KEY { get { return "DrawHiglighter"; } }
		internal bool DrawHiglighter { get { return adapter.par_e.GET(KEY + "_DRAW_HIGHLIGHTER", true); } set { adapter.par_e.SET(KEY + "_DRAW_HIGHLIGHTER", value); } }
		internal string _HiglighterOpacity_KEY { get { return "HiglighterOpacity"; } }
		internal float HiglighterOpacity { get { return adapter.par_e.GET(KEY + "_HIGHLIGHTER_OPACITY", 1); } set { adapter.par_e.SET(KEY + "_HIGHLIGHTER_OPACITY", value); } }
		internal string _DrawTooltips_KEY { get { return "DrawTooltips"; } }
		internal bool DrawTooltips { get { return adapter.par_e.GET(KEY + "_DRAW_TOOLTIPS", true); } set { adapter.par_e.SET(KEY + "_DRAW_TOOLTIPS", value); } }


		internal string _BgOverrideColor_KEY { get { return "BgOverrideColor"; } }
		internal Color BgOverrideColor { get { return adapter.par_e.GET(KEY + "_BG_COLOR", Color.clear); } set { adapter.par_e.SET(KEY + "_BG_COLOR", value); } }
		internal string _SortButtonsOrder_KEY { get { return "SortButtonsOrder"; } }
		internal int SortButtonsOrder { get { return adapter.par_e.GET(KEY + "_SORT_ORDER", 0); } set { adapter.par_e.SET(KEY + "_SORT_ORDER", value); } }

	}





	internal partial class DrawButtonsOld
	{

		internal static GUIContent categoryColorContent2 = new GUIContent() { text = "☰", tooltip = "Click - to change current category" };
		internal static GUIContent categoryColorContent = new GUIContent() { text = "☰", tooltip = "Click - to change current category" };
		// internal GUIContent categoryColorContent = new GUIContent() { text = "☰", tooltip = "Click to change background color for category" };
		internal static GUIContent categoryDescriptionContent = new GUIContent() { text = "i", tooltip = "Show all Descriptions in Hierarchy Window" };



		Scene scene { get { return SceneManager.GetActiveScene(); } }




		TempColorClass tempColor = new TempColorClass();
		internal static bool SkipRemove;




		bool needRestore = false;
		Color c1, c2, c3, c4, c5, c6, c7, c8;

		internal void SetStyleColor(GUIStyle style, Color color)
		{
			if (needRestore) throw new Exception("SetStyleColor");

			needRestore = true;
			c1 = style.normal.textColor;
			c2 = style.focused.textColor;
			c3 = style.active.textColor;
			c4 = style.hover.textColor;
			c5 = style.onNormal.textColor;
			c6 = style.onFocused.textColor;
			c7 = style.onActive.textColor;
			c8 = style.onHover.textColor;
			style.normal.textColor =
				style.focused.textColor =
					style.active.textColor =
						style.hover.textColor =
							style.onNormal.textColor =
								style.onFocused.textColor =
									style.onActive.textColor =
										style.onHover.textColor = color;
		}

		internal void RestoreStyleColor(GUIStyle style)
		{
			if (!needRestore) return;

			needRestore = false;

			style.normal.textColor = c1;
			style.focused.textColor = c2;
			style.active.textColor = c3;
			style.hover.textColor = c4;
			style.onNormal.textColor = c5;
			style.onFocused.textColor = c6;
			style.onActive.textColor = c7;
			style.onHover.textColor = c8;
		}

		static GUIContent ___GETTOOLTIPPEDCONTENT = new GUIContent();

		internal static GUIContent GETTOOLTIPPEDCONTENT(MemType type, string upline, ExternalDrawContainer controller)
		{
			___GETTOOLTIPPEDCONTENT.text = "";
			___GETTOOLTIPPEDCONTENT.tooltip = "";

			// if ( type == MemType.Custom ) Debug.Log( (controller.selection_action != null) + " " + IsValidDrag() + " " + (upline != null).ToString() );
			if (controller.selection_action != null /*|| IsValidDrag() */) return ___GETTOOLTIPPEDCONTENT;

			if (upline != null) ___GETTOOLTIPPEDCONTENT.tooltip += upline + "\n";

			switch (type)
			{
				case MemType.Last:
					// ___GETTOOLTIPPEDCONTENT.tooltip += "You can switch between recently selected GameObjects";
					return ___GETTOOLTIPPEDCONTENT;

				case MemType.Custom:
					// ___GETTOOLTIPPEDCONTENT.tooltip += "You can store these GameObjects in this line";
					return ___GETTOOLTIPPEDCONTENT;

				case MemType.Scenes:

					// content.tooltip = objectIsHiddenAndLock ? "Object hided" : "Left CLICK/Left DRAG Show/Hide GameObject \n( Right CLICK/Right DRAG - Focus on the object in the SceneView )";
					//   ___GETTOOLTIPPEDCONTENT.tooltip += "Left CLICK - to load Scene\nShift+Left CLICK - to additive load Scene\nCtrl+Left CLICK - to select Scene in Project";
					return ___GETTOOLTIPPEDCONTENT;

				case MemType.Hier:
					___GETTOOLTIPPEDCONTENT.tooltip += "You can save an expanded objects and load it later";
					return ___GETTOOLTIPPEDCONTENT;

				default:
					throw new ArgumentOutOfRangeException("type", type, null);
			}
		}




		Rect lastDESrect;
		static PluginInstance adapter { get { return Root.p[0]; } }

		internal Rect GET_CELL_RECT(Rect cell, Rect line, MemType type, int interator, int itemscount, int countPerRow, int sort)
		{
			var lineIndex = interator / countPerRow;
			var currenItemsInLine = interator / countPerRow == itemscount / countPerRow ? itemscount % countPerRow : countPerRow;
			return __GET_CELL_RECT(cell, line, interator % countPerRow, lineIndex, currenItemsInLine, (itemscount - 1) / countPerRow + 1, sort);
		}

		int SPACE = 4;

		Rect __GET_CELL_RECT(Rect cell, Rect line, int x, int y, int rightX, int rightY, int sort)
		{
			var width = (line.width - SPACE * (rightX - 1)) / rightX;
			;
			var height = (line.height - 2 * rightY) / rightY;
			var zerox = line.x + line.width - cell.width + 5;

			//* INVERSION *//
			switch (sort)
			{
				case 1:
					x = rightX - 1 - x;
					break;
				case 0: break;
				case 3:
					x = rightX - 1 - x;
					y = rightY - 1 - y;
					break;
				case 2:
					y = rightY - 1 - y;
					break;
			}

			//* INVERSION *//

			//cell.x = Mathf.RoundToInt(cell.x);
			//cell.y = Mathf.RoundToInt(cell.y);
			cell.width = Mathf.RoundToInt(cell.width);
			cell.height = Mathf.RoundToInt(cell.height);

			cell.x = zerox + (width + SPACE) * x;
			cell.y = cell.y + (height + 2) * y;
			cell.width = width;

		

			return  cell;

		}



		static ISET_ROW BOOKMARKS_GO_KEY = new ISET_ROW("BOOKMARKS_GO_KEY", 2, 10);
		static ISET_ROW LAST_GO_KEY = new ISET_ROW("LAST_GO_KEY", 1, 10);
		static ISET_ROW LAST_SCENES_KEY = new ISET_ROW("LAST_SCENES_KEY", 1, 6);
		static ISET_ROW EXPANDED_HIER = new ISET_ROW("EXPANDED_HIER", 1, 5);
		internal static ISET_ROW GET_DISPLAY_PARAMS(MemType type)
		{
			switch (type)
			{
				case MemType.Custom: return BOOKMARKS_GO_KEY;
				case MemType.Last: return LAST_GO_KEY;
				case MemType.Hier: return EXPANDED_HIER;
				case MemType.Scenes: return LAST_SCENES_KEY;
			}

			return null;
		}



		static void SHOW_STRING(string title, string s, Action<string> action, ExternalDrawContainer controller)
		{
			if (Event.current == null) // adapter.__GUI_ONESHOT = true;
			{
				adapter.PUSH_GUI_ONESHOT(() => { _mSHOW_STRING(title, s, action, controller); });
				return;
				//throw new Exception("Error imnput in hui");
			}

			_mSHOW_STRING(title, s, action, controller);
		}

		/** SHOW_STRING */
		static void _mSHOW_STRING(string title, string s, Action<string> action, ExternalDrawContainer controller) // var pos = InputData.WidnwoRect(false, Event.current.mousePosition, 190, 68, controller.adapter);
		{
			var pos = new MousePos(Event.current.mousePosition, MousePos.Type.Input_190_68, false, controller.adapter);
			InputWindow.Init(pos, title, controller.adapter, action, null, s);
		}

		static List<BookmarkRoot> emptyb = new List<BookmarkRoot>();
		internal static Dictionary<int, Dictionary<int, List<BookmarkRoot>>> cached_categories = new Dictionary<int, Dictionary<int, List<BookmarkRoot>>>();
		internal static List<BookmarkRoot> GET_OBJECTS_LIST(MemType type, ExternalDrawContainer controller, Scene scene, int? cat_override = null)
		{

			var SH = scene.GetHashCode();
			if (type == MemType.Custom)
			{

				//var sd = HierarchyExternalSceneData.GetHierarchyExternalSceneData(scene);
				//if (sd.BookMarksGlobal == null || sd.BookMarksGlobal.Count == 0) return new List<BookmarkRoot>();


				var ci = cat_override ?? controller.GetCategoryIndex(scene);
				var temp = HierarchyTempSceneData.InstanceFast(scene);
				if (temp.BookMarkCategory_Temp.Count == 0) return new List<BookmarkRoot>();

				if (!cached_categories.ContainsKey(SH) || !cached_categories[SH].ContainsKey((int)type) || cached_categories[SH][(int)type].Count > 0 && ci != cached_categories[SH][(int)type][0].CategoryIndex)
				{


					//HierarchyTempSceneDataGetter.TryToInitBookOrExpand(SaverType.Bookmarks, scene);
					if (!cached_categories.ContainsKey(SH)) cached_categories.Add(SH, new Dictionary<int, List<BookmarkRoot>>());

					if (temp.BookMarkCategory_Temp[ci].targets.Count > 0)
					{
						List<BookmarkRoot> res = new List<BookmarkRoot>();
						for (int c_i = 0; c_i < temp.BookMarkCategory_Temp[ci].targets.Count; c_i++)
						{
							BookmarkRoot root = new GameObjectMemory(temp.BookMarkCategory_Temp[ci].targets[c_i].targets.ToArray(), c_i, ci, temp);
							res.Add(root);
						}
						cached_categories[SH].Remove((int)type);
						cached_categories[SH].Add((int)type, res);
					}
					else
					{
						return emptyb;
					}
				}
				return cached_categories[SH][(int)type];
			}
			else if (type == MemType.Scenes)
			{

				//Dictionary<int, BookmarkRoot> res = new Dictionary<int, BookmarkRoot>();
				if (!cached_categories.ContainsKey(SH) || !cached_categories[SH].ContainsKey((int)type))
				{
					if (!cached_categories.ContainsKey(SH)) cached_categories.Add(SH, new Dictionary<int, List<BookmarkRoot>>());
					var sd = HierarchyCommonData.Instance();
					//	var sd = HierarchyExternalSceneData.GetHierarchyExternalSceneData(scene);
					var res = new List<BookmarkRoot>();
					for (int i = 0; i < sd.ScenesTabs.Count; i++) res.Add(new SceneMemory(sd.ScenesTabs[i], i));
					cached_categories[SH].Add((int)type, res);
				}
				return cached_categories[SH][(int)type];
			}
			else if (type == MemType.Last)
			{
				if (!cached_categories.ContainsKey(SH) || !cached_categories[SH].ContainsKey((int)type))
				{
					if (!cached_categories.ContainsKey(SH)) cached_categories.Add(SH, new Dictionary<int, List<BookmarkRoot>>());

					var temp = HierarchyTempSceneData.InstanceFast(scene);
					var res = new List<BookmarkRoot>();
					for (int i = 0; i < temp.LastSelection_Temp.Count; i++) res.Add(new GameObjectMemory(temp.LastSelection_Temp[i].targets.ToArray(), i, -1, temp));
					cached_categories[SH].Add((int)type, res);
				}
				return cached_categories[SH][(int)type];
			}
			else if (type == MemType.Hier)
			{
				if (!cached_categories.ContainsKey(SH) || !cached_categories[SH].ContainsKey((int)type))
				{
					if (!cached_categories.ContainsKey(SH)) cached_categories.Add(SH, new Dictionary<int, List<BookmarkRoot>>());

					HierarchyTempSceneDataGetter.TryToInitBookOrExpand(SaverType.SceneHierarchyExands, scene);
					var temp = HierarchyTempSceneData.InstanceFast(scene);

					var res = new List<BookmarkRoot>();
					for (int i = 0; i < temp.HierExpands_Temp.Count; i++) res.Add(new HierarchyMemory(temp.HierExpands_Temp[i].targets.ToArray(), i, temp.HierExpands_Temp[i].name, temp));
					cached_categories[SH].Add((int)type, res);
				}
				return cached_categories[SH][(int)type];
			}

			//	HierarchyTempSceneDataGetter.SetObjectData(SaverType.ModDescription, o, s);



			return null;
		}

		internal static void SET_OBJECTS_LIST(List<BookmarkRoot> list, MemType type, ExternalDrawContainer controller, Scene scene, int? cat_override = null)
		{

			if (!cached_categories.ContainsKey(scene.GetHashCode()))
			{
				GET_OBJECTS_LIST(type, controller, scene);
				//full if empty
			}

			if (type == MemType.Custom)
			{
				var temp = HierarchyTempSceneData.InstanceFast(scene);

				//	var ci = list[0].CategoryIndex;
				var ci = cat_override ?? controller.GetCategoryIndex(scene);
				if (ci >= temp.BookMarkCategory_Temp.Count) return;
				if (ci < 0) throw new Exception(ci.ToString());
				//var ci = controller.GetCategoryIndex(scene);
				temp.BookMarkCategory_Temp[ci].targets.Clear();
				foreach (var item in list)
				{
					var i = new HierExpands_Temp()
					{
						targets = item.gos_get().ToList()
					};
					temp.BookMarkCategory_Temp[ci].targets.Add(i);
				}
				HierarchyTempSceneDataGetter.SaveBookOrExpand(SaverType.Bookmarks, scene);
			}
			else if (type == MemType.Scenes)
			{
				var sd = HierarchyCommonData.Instance();
				sd.ScenesTabs.Clear();
				foreach (var _item in list)
				{
					var item = _item as SceneMemory;
					sd.ScenesTabs.Add(new ScenesTab_Saved()
					{
						guid = item.additional_GUID,
						path = item.additional_PATH,
						pin = item.pin,
					});
				}
				HierarchyCommonData.Instance().SetDirty();
			}
			else if (type == MemType.Last)
			{
				var temp = HierarchyTempSceneData.InstanceFast(scene);
				temp.LastSelection_Temp = list.Select(l => new HierExpands_Temp() { targets = l.gos_get().ToList() }).ToList();
			}
			else if (type == MemType.Hier)
			{
				//var sd = HierarchyExternalSceneData.GetHierarchyExternalSceneData(scene);

				//HierarchyTempSceneDataGetter.TryToInitBookOrExpand(SaverType.SceneHierarchyExands, scene);
				var temp = HierarchyTempSceneData.InstanceFast(scene);
				temp.HierExpands_Temp = list.Select(l => new HierExpands_Temp() { targets = l.gos_get().ToList(), name = ((HierarchyMemory)l).name }).ToList();
				HierarchyTempSceneDataGetter.SaveBookOrExpand(SaverType.SceneHierarchyExands, scene);

			}
			cached_categories[scene.GetHashCode()].Remove((int)type);
			//cached_categories[.Remove((int)type);
		}


		GUIContent faveContent = new GUIContent();
		private int pickerId;
#pragma warning disable
		private Action<Object> pickerAction;
#pragma warning restore
		Rect DragRect;


		//   ;
		// private  void DrawButtons(Rect line, MemoryRoot[] memoryRoot, int idOffset, MemType type)



		char[] tr = new[] { '\n', '\r', ' ', ';', '\0' };
		GUIStyle __countStyle;

		GUIStyle countStyle
		{
			get
			{
				if (__countStyle == null)
				{
					__countStyle = new GUIStyle(adapter.label);
					__countStyle.alignment = TextAnchor.MiddleCenter;
					__countStyle.fontStyle = FontStyle.Bold;
				}

				return __countStyle;
			}
		}

		internal void DRAW_COUNT(Rect drawOffset, float LH, MemType type, BookmarkRoot memoryRoot, bool DRAW_CTN)
		{
			var HH = drawOffset.y + drawOffset.height;
			var WW = drawOffset.x + drawOffset.width;


			var oldH = drawOffset.height;
			drawOffset.height = LH + 2;
			drawOffset.y += (oldH - drawOffset.height) / 2;
			drawOffset.width = drawOffset.height;

			countStyle.fontStyle = type == MemType.Last ? FontStyle.Normal : FontStyle.Bold;
			countStyle.fontSize = type != MemType.Last ? adapter.STYLE_LABEL_10.fontSize : (adapter.STYLE_LABEL_8.fontSize - 1);
			drawOffset.y = HH - drawOffset.height;
			drawOffset.x = WW - drawOffset.width;


			drawOffset.height -= 1;
			//drawOffset.y++;
			/*drawOffset.width = drawOffset.height;
			drawOffset.height++;*/

			var cc = countStyle.normal.textColor;
			//                 var align = Adapter.GET_SKIN().label.alignment;
			//
			//                 var st = Adapter.GET_SKIN().label.fontStyle;
			//                 var font = Adapter.GET_SKIN().label.fontSize;
			//
			//                 Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleCenter;
			//                 Adapter.GET_SKIN().label.fontSize--;
			//                 Adapter.GET_SKIN().label.fontStyle = FontStyle.Bold;
			var print = type == MemType.Scenes ? (memoryRoot.additional_GUID.Length).ToString() : memoryRoot.gos_get().Length.ToString();
			countStyle.normal.textColor = Color.black;


			if (DRAW_CTN) GUI.DrawTexture(drawOffset, OBJECTCONTENTCOUNT);

			drawOffset.width += 2;
			drawOffset.y -= 1;
			GUI.Label(drawOffset, print, countStyle);
			drawOffset.y += 2;
			GUI.Label(drawOffset, print, countStyle);
			drawOffset.x -= 2;
			GUI.Label(drawOffset, print, countStyle);
			drawOffset.y -= 2;
			GUI.Label(drawOffset, print, countStyle);

			countStyle.normal.textColor = Color.white;
			drawOffset.y += 1;
			drawOffset.x += 1;


			GUI.Label(drawOffset, print, countStyle);

			//                 Adapter.GET_SKIN().label.alignment = align;
			//                 Adapter.GET_SKIN().label.fontSize = font;
			countStyle.normal.textColor = cc;
			//  Adapter.GET_SKIN().label.fontStyle = st;
		}

		GUIContent content_des = new GUIContent();
		Color coloAlpha = new Color(1, 1, 1, 0.6f);
		Rect r_des;

		private void DRAWSTYLE_A(GUIStyle style, Color bColor, Color cColor, Rect drawOffset, bool active,
			MemType type, ExternalDrawContainer controller, BookmarkRoot mem, int cintrolID, TempColorClass otherStyles)
		{
			var resof = drawOffset;

			if (type == MemType.Scenes && UnityVersion.UNITY_CURRENT_VERSION >= UnityVersion.UNITY_2019_3_0_VERSION)
			{ //drawOffset.y += drawOffset.height / 3;
				drawOffset.height /= 3 / 2.1f;
			}


			var bc = GUI.backgroundColor;
			var cc = GUI.contentColor;
			var oldOver = style.clipping;
			GUI.contentColor = Color.clear;
			GUI.backgroundColor = bc * bColor;
			style.clipping = TextClipping.Clip;
			var fs = style.fontSize;
			style.fontSize = GET_DISPLAY_PARAMS(type).fontSize;
			style.Draw(drawOffset, content, active, active, false, active);
			style.fontSize = fs;
			style.clipping = oldOver;
			drawOffset = resof;
			GUI.backgroundColor = bc;
			GUI.contentColor = cc;
		}

		private void DRAWSTYLE_B(GUIStyle style, Color bColor, Color cColor, Rect drawOffset, bool active,
			MemType type, ExternalDrawContainer controller, BookmarkRoot mem, int cintrolID, TempColorClass otherStyles, int fontSize, int lh)
		{
			// var resof = drawOffset;
			if (type == MemType.Scenes && UnityVersion.UNITY_CURRENT_VERSION >= UnityVersion.UNITY_2019_3_0_VERSION) //drawOffset.y += drawOffset.height / 3;
			{
				drawOffset.height /= 3 / 2.1f;
			}

			var bc = GUI.backgroundColor;
			var cc = GUI.contentColor;
			var oldOver = style.clipping;
			GUI.backgroundColor = Color.clear;

			if (otherStyles != null && otherStyles.LABEL_SHADOW && otherStyles.HAS_LABEL_COLOR) // var _oc2 =  Adapter.GET_SKIN().label.normal.textColor;
			{
				//var c2 = Adapter.GET_SKIN().label.normal.textColor;
				var c2 = Color.black;
				c2.a = cColor.a;
				GUI.contentColor = c2;
				drawOffset.y -= 0.5f;
				drawOffset.x -= 1f;
				style.clipping = TextClipping.Clip;
				var fs = style.fontSize;
				style.fontSize = GET_DISPLAY_PARAMS(type).fontSize;
				style.Draw(drawOffset, content, active, active, false, active);
				style.fontSize = fs;
				style.clipping = oldOver;
				drawOffset.y += 0.5f;
				drawOffset.x += 1f;
			}


			//  GUI.contentColor = (Adapter.GET_SKIN().button.normal.textColor ) * cColor;

			GUI.contentColor = cc * cColor;
			var casd = style.normal.textColor;

			if (EditorGUIUtility.isProSkin && !needRestore) style.normal.textColor = adapter.labelStyle.normal.textColor;

			{
				style.clipping = TextClipping.Clip;
				var fs = style.fontSize;
				style.fontSize = GET_DISPLAY_PARAMS(type).fontSize;
				style.Draw(drawOffset, content, active, active, false, active);
				style.fontSize = fs;
				style.clipping = oldOver;
				style.normal.textColor = casd;
			}



			GUI.backgroundColor = bc;
			GUI.contentColor = cc;


			var DES = (type == MemType.Custom && adapter.par_e.BOOKMARKS_OB_SHOWDESCRIPTS);

			if (DES)
			{
				r_des = drawOffset;
				r_des.height += DESCRIPTION_MULTY(fontSize,lh);

				var c = GUI.color;
				GUI.color *= coloAlpha;
				adapter.INTERNAL_BOX(r_des, "");
				GUI.color = c;

				//adapter.InitializeStyle
			}

			if (DES)
			{
				r_des.y = drawOffset.y + drawOffset.height;
				r_des.height -= drawOffset.height;


				var o = mem.gos_get() != null && mem.gos_get().Length != 0 && mem.gos_get()[0] ? mem.gos_get()[0] : null;

				//var o = mem.InstanceID != null ? mem.InstanceID.ActiveGameObject : null;

				if (o != null /*&& o.scene.IsValid() && o.scene.isLoaded*/)
				{
					var scene = o.scene;
					var _o = Cache.GetHierarchyObjectByInstanceID(o);
					var data = HierarchyTempSceneDataGetter.GetObjectData(SaverType.ModDescription, _o);
					if (data != null && !string.IsNullOrEmpty(data.stringValue))
					{
						content_des.text = content_des.tooltip = data.stringValue;
					}
					else
					{
						content_des.text = "-";
						content_des.tooltip = "No Description\nLeft CLICK on 'i' icon to add Description";
					}
				}

				else
				{
					content_des.text = content_des.tooltip = "- ...";
				}

				//                     var a = Adapter.GET_SKIN().label.alignment;
				//                     var f = Adapter.GET_SKIN().label.fontSize;

				descrStyle.fontSize = adapter.FONT_8();

				//if (controller.IS_MAIN) descrStyle.fontSize -= 1;


				content_des.tooltip = content_des.tooltip.Trim(tr);
				GUI.Label(r_des, content_des, descrStyle);
				//                     Adapter.GET_SKIN().label.alignment = a;
				//                     Adapter.GET_SKIN().label.fontSize = f;


				if (controller.selection_action != null && controller.selection_button == cintrolID + 100)
				{
					adapter.button.Draw(controller.lastRect, REALEMPTY_CONTENT, false, false, false, true);
				}
			}
		}

		GUIStyle __descrStyle;

		GUIStyle descrStyle
		{
			get
			{
				if (__descrStyle == null)
				{
					__descrStyle = new GUIStyle(adapter.label);
					__descrStyle.alignment = TextAnchor.MiddleLeft;
					__descrStyle.clipping = TextClipping.Clip;
				}

				return __descrStyle;
			}
		}

		GUIContent REALEMPTY_CONTENT = new GUIContent();
		// DrawButton()

		void Shrink(ref Rect rect, int amount)
		{
			rect.x += amount;
			rect.y += amount;
			rect.width -= 2 * amount;
			rect.height -= 2 * amount;
		}

		Rect Shrink(Rect rect, int amount)
		{
			rect.x += amount;
			rect.y += amount;
			rect.width -= 2 * amount;
			rect.height -= 2 * amount;
			return rect;
		}

		Rect zero = new Rect(0, 0, 0, 0);

		Rect GET_INFO_RECT(MemType type, Rect drawCell)
		{
			if (type != MemType.Custom) return zero;

			// var t = adapter.GetIcon("BOTTOM_INFO");
			var SIZE = 12;
			var r_info = drawCell;
			r_info.x -= 4;
			r_info = Shrink(r_info, 3);
			var iconSizey = Mathf.RoundToInt(Mathf.Min(SIZE, r_info.height - 6));
			var iconSizex = iconSizey;
			r_info.Set(r_info.x + r_info.width - iconSizex, r_info.y + (r_info.height - iconSizey) / 2, iconSizex, iconSizey);
			return r_info;
		}


		GUIContent pinScene = new GUIContent() { tooltip = "Press to 'Pin' this scene" };

		internal TempColorClass __GetContentEmpty = new TempColorClass().AddIcon(null);

		internal TempColorClass GetIconContent(HierarchyObject o) //  if (adapter.IS_PROJECT()) return AssetDatabase.GetCachedIcon( o.project.assetPath );
		{
			if (o == null || !o.Validate()) return __GetContentEmpty;


			/* if (adapter.IS_HIERARCHY() && adapter.HAS_LABEL_ICON() )
			 {   var context2 = Utilities.ObjectContent_NoCacher(adapter, o.go, o.GET_TYPE());
				 if (!context2.add_icon) return __GetContentEmpty;
				 return context2;
			 }*/


			// adapter.MOI.M_Colors.GetColorForObject

			return o.GetIconForExternal();

			/*	var context = Utilities.ObjectContent_IncludeCacher(adapter, o, o.GET_TYPE(adapter), true);

				if (!context.add_icon) return __GetContentEmpty;

				if (adapter.IS_HIERARCHY() && !adapter.HAS_LABEL_ICON()) //#COLUP
				{
					if (context.add_icon == Utilities.ObjectContent_NoCacher(adapter, (UnityEngine.Object)null, o.GET_TYPE(adapter)).add_icon
						|| Utilities.IsPrefabIcon(context.add_icon)) context = __GetContentEmpty;
				}

				return context;*/
			// return GetContent( EditorUtility.InstanceIDToObject( o.id ) );
		}

		internal static void OnSelectionChangeStatic()
		{
			if (!SkipRemove)
			{
				DrawButtonsOld.GET_DISPLAY_PARAMS(MemType.Last).LastIndex = -1;
				DrawButtonsOld.GET_DISPLAY_PARAMS(MemType.Last).LastSelectedRoot = -1;
				DrawButtonsOld.GET_DISPLAY_PARAMS(MemType.Custom).LastIndex = -1;
				DrawButtonsOld.GET_DISPLAY_PARAMS(MemType.Custom).LastSelectedRoot = -1;
			}
			SkipRemove = false;
		}

		/*internal Texture GetContent(UnityEngine.Object o)
{
  // var o = EditorUtility.InstanceIDToObject(instanceID);
  if (!o) return null;
  var context = Utilities.ObjectContent_NoCacher(adapter, o, Adapter.GetType_(o)).image;
  if (context == null || context == Utilities.ObjectContent_NoCacher( adapter, (UnityEngine.Object)null, Adapter.GetType_( o ) ).image || context.name == "PrefabNormal Icon" || context.name == "PrefabModel Icon") return null;
  return context;
}*/

		internal abstract class BookmarkRoot
		{
			/*	internal BottomInterface bottomInterface;

				internal BookmarkRoot( bottomInterface)
				{
					this.bottomInterface = bottomInterface;
				}*/


			//public Int32List InstanceID;

			internal HierarchyTempSceneData tmp_sd;

			internal int unique_id;
			GameObject[] _gos;
			public GameObject[] gos_get() { return _gos; }
			public void gos_set(GameObject[] value)
			{
				unique_id = 0xACFDAFC;
				foreach (var item in value)
					unique_id ^= item.GetInstanceID();
				_gos = value;
			}
			string[] _additional_GUID;
			public string[] additional_GUID
			{
				get { return _additional_GUID; }
				set
				{
					unique_id = 0;
					foreach (var item in value)
						unique_id ^= item.GetHashCode();
					_additional_GUID = value;
				}
			}
			public string[] additional_PATH;
			public bool pin;
			//public int ArrayIndex = -1;
			public int CategoryIndex;
			//public int DictionaryIndex;
			public int Arrayindex;
			public int RectBindIndex;
			//	public Scene scene;

			//	public virtual void SetStringValues(SceneId scene, int arrayIndex) { }
			//	public virtual void SetIntValues(Int32List instanceId, int arrayIndex) { }
			//	public virtual void SetObjectValues(object instanceId, int arrayIndex) { }
			public abstract bool OnClick(bool par1, int scene, ExternalDrawContainer controller);
			public new abstract string ToString();

			public virtual string FullString()
			{
				return "";
			}

			public abstract bool IsActive();
			public abstract bool IsValid();
			public abstract bool IsSelectedHadrScan();
			public abstract bool IsSelectablePlus();
			public abstract bool IsSelectableMinus();

			public int GET_SELECTION_STATE()
			{
				var STATE = 0;
				var mayMinus = false;

				var selected = false;

				if (Event.current.control)
				{
					/*selected = this is GameObjectMemory
						? InstanceID.list.Any(o => o && bottomInterface.adapter.IsSelected(o.GetInstanceID())) // IsSelectedHadrScan();
						: false;*/
				}

				// var selected = IsSelectedHadrScan();


				if (IsSelectablePlus())
					if ((Event.current.control && !selected || Event.current.shift))
						STATE = 1;
					else
						mayMinus = true;

				if (IsSelectableMinus() && mayMinus)
					if (Event.current.control && selected)
						STATE = 2;

				if (Event.current.alt) STATE = 3;

				return STATE;
			}





			internal bool GuidEquals()
			{
				var guid_equals = PluginInstance.LastActiveScenesList_Guids.FirstOrDefault() == additional_GUID.FirstOrDefault();
				if (guid_equals)
				{
					if (additional_GUID != null && additional_GUID.Length != 0 && PluginInstance.LastActiveScenesList_Guids.Length != 0)
					{
						if (additional_GUID.Length != PluginInstance.LastActiveScenesList_Guids.Length) guid_equals = false;
						if (guid_equals)
							for (int b = 0, length = additional_GUID.Length; b < length; b++)
							{
								if (!PluginInstance.LastActiveScenesList_Guids.Contains(additional_GUID[b]))
								{
									guid_equals = false;
									break;
								}
							}
					}
				}
				return guid_equals;
			}
		}

		internal class SceneMemory : BookmarkRoot
		{

			public override bool IsActive()
			{
				return true;
			}

			internal SceneMemory(ScenesTab_Saved tab, int Arrayindex)
			{
				if (tab.guid == null || tab.guid.Length == 0) return;

				this.additional_GUID = tab.guid.ToArray();
				this.additional_PATH = tab.path.ToArray();
				this.pin = tab.pin;
				this.Arrayindex = Arrayindex;
				this.RectBindIndex = Arrayindex;
				this.validCache = null;
				UpdateSceneName();
			}

			private string sceneName;
			private string sceneName_Full;

			private void UpdateSceneName()
			{
				var path = AssetDatabase.GUIDToAssetPath(additional_GUID[0]);
				if (!path.Contains('/'))
				{
					sceneName_Full = sceneName = "...";
					return;
				}

				sceneName = sceneName_Full = Path_ToSceneNamr(path);
				if (additional_PATH == null) additional_PATH = new string[0];

				for (int i = 0; i < additional_PATH.Length; i++)
				{
					if (additional_PATH[i] != AssetDatabase.GUIDToAssetPath(additional_GUID[i]))
						additional_PATH[i] = AssetDatabase.GUIDToAssetPath(additional_GUID[i]);
				}
				for (int i = 1; i < additional_PATH.Length; i++)
					sceneName_Full += '\n' + Path_ToSceneNamr(path);
			}

			string Path_ToSceneNamr(string path)
			{
				var tempName = path.LastIndexOf('/') != -1 ? path.Substring(path.LastIndexOf('/') + 1) : path;
				return tempName.LastIndexOf('.') != -1 ? tempName.Remove(tempName.LastIndexOf('.')) : tempName;
			}


			public override bool OnClick(bool par1, int scene, ExternalDrawContainer controller)
			{
				if (Application.isPlaying) return true;
				if (additional_GUID == null || additional_GUID.Length == 0) return false;

				if (additional_GUID.All(g => string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(g)))) return false;


				if (Event.current != null && (Event.current.control || Event.current.alt))
				{
					//  var ass = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(GUID);
					// if (!ass) return false;
					//var path = AssetDatabase.GUIDToAssetPath(GUID);

					List<Object> result = new List<Object>();
					// if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
					/*{
						try
						{
							AssetDatabase.LoadMainAssetAtPath(path).GetInstanceID();
							result.Add(AssetDatabase.LoadMainAssetAtPath(path));
						}

						catch
						{
							try
							{
								result.Add(AssetDatabase.LoadMainAssetAtPath(PATH));
							}

							catch
							{
								return false;
							}
						}
					}*/

					for (int Index = 0; Index < additional_GUID.Length; Index++)
					{
						var path = AssetDatabase.GUIDToAssetPath(additional_GUID[Index]);
						var PATH = additional_PATH[Index];

						try
						{
							AssetDatabase.LoadMainAssetAtPath(path).GetInstanceID();
							result.Add(AssetDatabase.LoadMainAssetAtPath(path));
						}

						catch
						{
							try
							{
								result.Add(AssetDatabase.LoadMainAssetAtPath(PATH));
							}

							catch { }
						}
					}

					Selection.objects = result.ToArray();
				}

				else
				{

					if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return true;

					//var path = AssetDatabase.GUIDToAssetPath(GUID);
					UpdateSceneName();

					var type = Event.current != null && Event.current.shift ? OpenSceneMode.Additive : OpenSceneMode.Single;
					if (additional_GUID != null && additional_GUID.Length != 0)
					{
						for (int Index = 0; Index < additional_GUID.Length; Index++)
						{
							var path = AssetDatabase.GUIDToAssetPath(additional_GUID[Index]);
							var PATH = additional_PATH[Index];
							var tape = Index == 0 ? type : OpenSceneMode.Additive;
							try
							{
								EditorSceneManager.OpenScene(path, tape);
							}

							catch
							{
								try
								{
									EditorSceneManager.OpenScene(PATH, tape);
								}

								catch { }
							}
						}
					}

					//bottomInterface.adapter.LastActiveScene = bottomInterface.adapter.GET_ACTIVE_SCENE;
					//bottomInterface.Scene_WriteLastScene((bottomInterface.adapter.GET_ACTIVE_SCENE));


					adapter.invoke_SceneChanging();
					//	bottomInterface.Scene_RefreshGUIAndClearActions(bottomInterface.adapter.GET_ACTIVE_SCENE);
					controller.ClearAction();
					controller.RepaintNow();
				}


				//

				return true;
			}

			public override bool IsSelectedHadrScan()
			{
				return false;
			}

			public override bool IsSelectablePlus()
			{
				return true;
			}

			public override bool IsSelectableMinus()
			{
				return false;
			}

			public override string ToString()
			{
				return sceneName;
			}

			bool? validCache = null;

			public override bool IsValid()
			{

				if (additional_GUID == null || additional_GUID.Length == 0) return false;
				if (!validCache.HasValue)
				{
					validCache = !string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(additional_GUID[0]));
				}
				return validCache.Value; /*(validCache ?? (validCache =File.Exists(adapter.PluginExternalFolder +  AssetDatabase.GUIDToAssetPath( GUID ))*/
				;
			}

			public override string FullString()
			{
				return sceneName_Full;
			}

		}
		internal class GameObjectMemory : BookmarkRoot
		{

			public override bool IsActive()
			{
				return gos_get()[0];
			}
			//public GameObjectMemory(BottomInterface bottomInterface) : base(bottomInterface) { }

			public override bool IsValid()
			{
				return gos_get() != null && gos_get().Length != 0 && gos_get().Any(g => g);
			}

			string m_FullString;
			public GameObjectMemory(GameObject[] gosin, int ArrayIndex, int CategoryIndex, HierarchyTempSceneData tmp_sd)
			{
				this.CategoryIndex = CategoryIndex;
				this.Arrayindex = ArrayIndex;
				this.tmp_sd = tmp_sd;
				this.gos_set(gosin);
				this.RectBindIndex = ArrayIndex;


				/*  if (InstanceID != null)
				  {   if (!string.IsNullOrEmpty(InstanceID.GUIDsActiveGameObject) &&
							  string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(InstanceID.GUIDsActiveGameObject)))
					  {   if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(InstanceID.PATHsActiveGameObject))
							  InstanceID.GUIDsActiveGameObject = AssetDatabase.AssetPathToGUID(InstanceID.PATHsActiveGameObject);
						  for (int i = 0; i < InstanceID.PATHsList.Count; i++)
						  {   if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(InstanceID.PATHsList[i]))
								  InstanceID.GUIDsList[i] = AssetDatabase.AssetPathToGUID(InstanceID.PATHsList[i]);

						  }
					  }
				  }*/


				var result = "";
				var incr = 0;

				foreach (var name in gosin)
				{
					{
						if (incr == 8)
						{
							result += "...";
							break;
						}
						result += name + "; ";
						++incr;
					}
				}

				m_FullString = incr == 0 ? "-" : result;
				m_FullString = m_FullString.Trim('\n');
				m_FullString = m_FullString.Trim(' ');
				// m_FullString = m_FullString.Trim(';');
			}

			public override bool OnClick(bool selectLocking, int scne, ExternalDrawContainer controller)
			{
				//if (InstanceID == null || bottomInterface.INT32_ISNULL(InstanceID) || bottomInterface.INT32_COUNT(InstanceID) == 0) return false;
				if (gos_get().Length == 0) return false;

				//var result = InstanceID.list.Select(EditorUtility.InstanceIDToObject).Where(o => o).ToArray();
				//  if (result.Length == 0) return false;
				var result = gos_get().Where(o => o && (!selectLocking || (o.hideFlags & HideFlags.NotEditable) == 0)).ToArray();

				if (!selectLocking)
				{
					if (gos_get().Any(o => o && (o.hideFlags & HideFlags.NotEditable) != 0))
						BookmarksforGameObjectsModInstance.ignoreLock = gos_get().Where(o => (o.hideFlags & HideFlags.NotEditable) != 0).ToArray();
				}

				if (result.Length == 0) return false;


				SkipRemove = true;
				//bottomInterface.SkipRemoveFix = false;
				//	LastIndex = -1;

				if (Event.current != null && Event.current.isMouse)
				{
					var STATE = GET_SELECTION_STATE();

					if (Event.current != null && Event.current.shift && controller.type == MemType.Custom && adapter.par_e.BOOKMARKS_OB_SHIFT_TO_INSTANTIATE)
					{
						var selectedObject = EditorUtility.InstanceIDToObject(adapter.ha._IsSelectedCache_lastID) as GameObject;
						var root = selectedObject ? selectedObject.transform.parent : null;
						var sib = selectedObject ? (selectedObject.transform.GetSiblingIndex() + 1) : -1;
						var isCanvas = root ? (root.GetComponent<Canvas>() ?? root.GetComponentInParent<Canvas>()) : null;

						List<GameObject> targetToSelect = new List<GameObject>();


						if (!isCanvas)
						{
							if (adapter.par_e.BOOKMARKS_OB_INSTANTIATE_POSITION == 0 || SceneView.sceneViews.Count == 0)
							{
								foreach (var _item in result)
								{
									var item = _item as GameObject;
									var inst = GameObject.Instantiate(item, root) as GameObject;
									inst.name = inst.name.Replace("(Clone)", "");

									if (sib != -1) inst.transform.SetSiblingIndex(sib);

									inst.transform.position = item.transform.position;
									inst.transform.rotation = item.transform.rotation;
									Undo.RegisterCreatedObjectUndo(inst, "Instantiate");
									targetToSelect.Add(inst);
								}

								var res = targetToSelect.ToArray();
								if (res.Length != 0) Selection.SetActiveObjectWithContext(res[0], null);
								Selection.objects = res;
							}

							else
							{
								foreach (var _item in result)
								{
									var item = _item as GameObject;
									var inst = GameObject.Instantiate(item, root) as GameObject;
									inst.name = inst.name.Replace("(Clone)", "");

									if (sib != -1) inst.transform.SetSiblingIndex(sib);

									targetToSelect.Add(inst);
								}

#pragma warning disable
								var dif = targetToSelect[0].transform.position;
								var r = targetToSelect[0].transform.rotation;
								var s = SceneView.sceneViews[0] as SceneView;
								s.MoveToView(targetToSelect[0].transform);
								dif = targetToSelect[0].transform.position - dif;
								targetToSelect[0].transform.rotation = r;

								for (int i = 0; i < targetToSelect.Count; i++)
								{
									var item = result[i] as GameObject;
									var inst = targetToSelect[i];

									//if (i != 0)inst.transform.position = item.transform.position + dif;
									if (i != 0) inst.transform.position = targetToSelect[0].transform.position;

									inst.transform.rotation = item.transform.rotation;
									Undo.RegisterCreatedObjectUndo(inst, "Instantiate");
								}

#pragma warning restore
								var res = targetToSelect.ToArray();
								if (res.Length != 0) Selection.SetActiveObjectWithContext(res[0], null);
								Selection.objects = res;
							}
						}

						else
						{
							if (adapter.par_e.BOOKMARKS_OB_INSTANTIATE_POSITION == 0 || !selectedObject)
							{
								foreach (var _item in result)
								{
									var item = _item as GameObject;
									var inst = GameObject.Instantiate(item, root) as GameObject;
									inst.name = inst.name.Replace("(Clone)", "");

									if (sib != -1) inst.transform.SetSiblingIndex(sib);

									inst.transform.position = isCanvas.transform.position;
									inst.transform.rotation = isCanvas.transform.rotation;
									Undo.RegisterCreatedObjectUndo(inst, "Instantiate");
									targetToSelect.Add(inst);
								}

								var res = targetToSelect.ToArray();
								if (res.Length != 0) Selection.SetActiveObjectWithContext(res[0], null);
								Selection.objects = res;
							}

							else
							{
								foreach (var _item in result)
								{
									var item = _item as GameObject;
									var inst = GameObject.Instantiate(item, root) as GameObject;
									inst.name = inst.name.Replace("(Clone)", "");

									if (sib != -1) inst.transform.SetSiblingIndex(sib);

									inst.transform.position = selectedObject.transform.position;
									inst.transform.rotation = selectedObject.transform.rotation;
									Undo.RegisterCreatedObjectUndo(inst, "Instantiate");
									targetToSelect.Add(inst);
								}

								var res = targetToSelect.ToArray();
								if (res.Length != 0) Selection.SetActiveObjectWithContext(res[0], null);
								Selection.objects = res;
							}
						}
					}

					else
					{
						switch (STATE)
						{
							case 0:
							case 3:

								//  Selection.Set
								/*if (STATE == 3) bottomInterface.adapter.SAVE_SCROLL();

								Selection.objects = result;
								bottomInterface.LastIndex = ArrayIndex;*/
								Selection.SetActiveObjectWithContext(result[0], null);
								Selection.objects = result;
								//Selection.activeObject = result[0];
								break;

							case 1:
								var res1 = Selection.objects.Concat(result).ToArray();
								Selection.SetActiveObjectWithContext(res1[0], null);
								Selection.objects = res1;
								break;

							case 2:
								var res2 = Selection.objects.Except(result).ToArray();
								if (res2.Length != 0) Selection.SetActiveObjectWithContext(res2[0], null);
								Selection.objects = res2;
								break;
						}
					}
				}
				else
				{
					Selection.SetActiveObjectWithContext(result[0], null);
					Selection.objects = result;
				}

				var par = GET_DISPLAY_PARAMS(CategoryIndex == -1 ? MemType.Last : MemType.Custom);
				par.LastIndex = Arrayindex;
				par.LastSelectedRoot = (unique_id);

				adapter.ha.InternalClearDrag();
				/*  bottomInterface.adapter.OnSelectionChanged();*/
				//  MonoBehaviour.print("ASD");
				// SelectChange = false;
				return true;
			}

			static List<string> asda;
			static string ts;

			public override bool IsSelectedHadrScan()
			{

				var par = GET_DISPLAY_PARAMS(CategoryIndex == -1 ? MemType.Last : MemType.Custom);
				return par.LastSelectedRoot == (unique_id);
				/*if (InstanceID.list.Count > 500) return false;
				if (bottomInterface.adapter.IS_HIERARCHY())
				{
					return InstanceID.list.All(o => o && bottomInterface.adapter.IsSelected(o.GetInstanceID())) && InstanceID.list.Count == bottomInterface.adapter.selMax;
				}
				var ts = InstanceID.GUIDsActiveGameObject;
				var getted = bottomInterface.adapter.GetHierarchyObjectByGUID(ref ts, InstanceID.PATHsActiveGameObject);
				if (ts != InstanceID.GUIDsActiveGameObject)
				{
					InstanceID.GUIDsActiveGameObject = ts;
					bottomInterface.adapter.ON_GUID_BACKCHANGED();
				}
				if (getted == null || !getted.Validate()) return false;
				return bottomInterface.adapter.IsSelected(getted.id);*/
			}

			public override bool IsSelectablePlus()
			{
				return true;
			}

			public override bool IsSelectableMinus()
			{
				return true;
			}

			//while only the first object is checked for optimization
			public override string ToString()
			{
				if (gos_get() == null || gos_get().Length == 0) return "-";
				if (!gos_get()[0])
				{
					gos_set(gos_get().Where(g => g).ToArray());
					if (gos_get().Length == 0) return "-";
				}
				return gos_get()[0].name;
			}

			public override string FullString()
			{
				return m_FullString;
			}


		}



		internal class HierarchyMemory : BookmarkRoot
		{
			public override bool IsActive()
			{
				return true;
			}
			//string[] GUIDids;
			//string[] PATHids;
			//GameObject[] ids;
			internal string name;

			//public HierarchyMemory(BottomInterface bottomInterface) : base(bottomInterface) { }
			public HierarchyMemory(GameObject[] gosin, int Arrayindex, string name, HierarchyTempSceneData tmp_sd)
			{
				this.Arrayindex = Arrayindex;
				this.RectBindIndex = Arrayindex;
				this.tmp_sd = tmp_sd;
				//	unique_id = UnityEngine.Random.Range(0,int,max)
				gos_set(gosin);
				this.name = name;
			}



			public override bool OnClick(bool par1, int scene, ExternalDrawContainer controller)
			{
				if (Application.isPlaying) return true;

				//if (GUIDids == null) GUIDids = new string[0];

				//if (GUIDids.Length != PATHids.Length) PATHids = new string[GUIDids.Length];

				Tools.SET_EXPAND_GO_SNAPSHOT(gos_get(), null, null, scene);

				return true;
			}

			public override bool IsSelectedHadrScan()
			{
				return false;
			}

			public override bool IsSelectablePlus()
			{
				return false;
			}

			public override bool IsSelectableMinus()
			{
				return false;
			}

			public override string ToString()
			{
				return name;
			}

			//  bool? validCache = null;
			public override bool IsValid()
			{
				return true;
				//return gos_get() != null && gos_get().Length != 0;
			}

			public override string FullString()
			{
				return name;
			}

		}
	}
}
