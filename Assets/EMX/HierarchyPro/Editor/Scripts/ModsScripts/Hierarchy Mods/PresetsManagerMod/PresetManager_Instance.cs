using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using EMX.HierarchyPlugin.Editor.Settings;
using EMX.HierarchyPlugin.Editor.Mods;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Text;
using EMX.HierarchyPlugin.Editor.PresetsManager;

namespace EMX.HierarchyPlugin.Editor.Windows
{
	public partial class HighlighterWindow : Windows.IWindow
	{



		HighlighterMod root;

		/*PluginInstance adapter;
		internal PresetManager_Instance(PluginInstance a, HighlighterMod root)
		{
			adapter = a; this.root = root;

		}
		*/



		static PresetsData cache;
		static void SaveCache()
		{
			//Hierarchy_GUI.Instance(adapter).GETPRESETS_DATA = Adapter.SERIALIZE_SINGLE(cache);
			//Hierarchy_GUI.SetDirtyObject(adapter);
			//HierarchyTempSceneDataGetter.SetAllObjectDataAndSave
			ClearCacheAndRepaint();
			foreach (var focusRoot in __inputWindow)
				if (focusRoot.Value) focusRoot.Value.Repaint();
			adapter.RepaintWindowInUpdate();
		}
		static void SaveCacheAndCreateUndo()
		{


			PresetsCommonData.Undo("Change Presets Data");
			PresetsCommonData.Instance().presetsData = cache.Save();
			PresetsCommonData.SetDirty();
			ClearCacheAndRepaint();
			foreach (var focusRoot in __inputWindow)
				if (focusRoot.Value) focusRoot.Value.Repaint();
			adapter.RepaintWindowInUpdate();
		}


		int PRESET_LINE_H = 18;
		internal int CATEGORY_HEIGHT(PresetsData presetData)
		{
			return PRESET_LINE_H * (4 + presetData.singlecomp_categories.categories.Length + presetData.fullhierarrchy_categories.categories.Length);

		}

		internal void SubscribeAutoColorHighLighter(EditorSubscriber sbs)
		{
			throw new NotImplementedException();
		}

		static void CHECK_CACHE()
		{




			if (cache == null)
			{
				try
				{
					if (cache == null) cache = new PresetsData();
					cache.Load(PresetsCommonData.Instance().presetsData);
				}
				catch       // ignored
				{
				}
			}
			if (cache == null) cache = new PresetsData();
			if (cache.singlecomp_categories == null) cache.singlecomp_categories = new PresetPart() { categories = new PresetSet[0], SetType = PRESET_TYPE.SINGLE_COMPONENT };
			if (cache.fullhierarrchy_categories == null) cache.fullhierarrchy_categories = new PresetPart() { categories = new PresetSet[0], SetType = PRESET_TYPE.FULL_HIERARCHY };
			cache.singlecomp_categories.SetType = PRESET_TYPE.SINGLE_COMPONENT;
			cache.fullhierarrchy_categories.SetType = PRESET_TYPE.FULL_HIERARCHY;
		}

		//  GUIStyle scroll
		float w1;
		float w2;
		float w3;
		float w3h;
		/*int w3h_0;
		int w3h_1;
		int w3h_2;
		int w3h_3;*/
		float w4;
		float w4h;
		Texture2D t2;
		Texture2D[] t2s;
		Texture2D t3;
		Texture2D[] t3s;
		Texture2D t4;
		Texture2D[] t4s;



		void INIT_SCROLL(float div)
		{
			var skin = adapter.GET_SKIN();
			w1 = skin.verticalScrollbar.fixedWidth;

			w2 = skin.verticalScrollbarThumb.fixedWidth;
			t2 = skin.verticalScrollbarThumb.normal.background;
			t2s = skin.verticalScrollbarThumb.normal.scaledBackgrounds;
			skin.verticalScrollbarThumb.normal.background = adapter.GetExternalModOld("HIPERUI_LINE_BOX");
			skin.verticalScrollbarThumb.normal.scaledBackgrounds = null;

			w3 = skin.verticalScrollbarUpButton.fixedWidth;
			w3h = skin.verticalScrollbarUpButton.fixedHeight;
			/* w3h_0 = skin.verticalScrollbarUpButton.border.bottom;
			 w3h_1 = skin.verticalScrollbarUpButton.border.left;
			 w3h_2 = skin.verticalScrollbarUpButton.border.right;
			 w3h_3 = skin.verticalScrollbarUpButton.border.top;*/
			w4 = skin.verticalScrollbarDownButton.fixedWidth;
			w4h = skin.verticalScrollbarDownButton.fixedHeight;

			t3 = skin.verticalScrollbarUpButton.normal.background;
			t3s = skin.verticalScrollbarUpButton.normal.scaledBackgrounds;
			skin.verticalScrollbarUpButton.normal.background = null; ;
			skin.verticalScrollbarUpButton.normal.scaledBackgrounds = null;

			t4 = skin.verticalScrollbarDownButton.normal.background;
			t4s = skin.verticalScrollbarDownButton.normal.scaledBackgrounds;
			skin.verticalScrollbarDownButton.normal.background = null; ;
			skin.verticalScrollbarDownButton.normal.scaledBackgrounds = null;

			/* skin.verticalScrollbarUpButton.border.bottom =
			 skin.verticalScrollbarUpButton.border.left =
			 skin.verticalScrollbarUpButton.border.right =
			 skin.verticalScrollbarUpButton.border.top = 0;*/

			skin.verticalScrollbar.fixedWidth /= div;
			skin.verticalScrollbarThumb.fixedWidth /= div;
			skin.verticalScrollbarUpButton.fixedWidth /= div;
			skin.verticalScrollbarUpButton.fixedHeight = 0;
			skin.verticalScrollbarDownButton.fixedWidth /= div;
			skin.verticalScrollbarDownButton.fixedHeight = 0;
		}
		void RESET_SCROLL()
		{
			var skin = adapter.GET_SKIN();

			skin.verticalScrollbar.fixedWidth = w1;
			skin.verticalScrollbarThumb.fixedWidth = w2;
			skin.verticalScrollbarUpButton.fixedWidth = w3;
			skin.verticalScrollbarUpButton.fixedHeight = w3h;
			skin.verticalScrollbarDownButton.fixedWidth = w4;
			skin.verticalScrollbarDownButton.fixedHeight = w4h;

			skin.verticalScrollbarThumb.normal.background = t2;
			skin.verticalScrollbarThumb.normal.scaledBackgrounds = t2s;

			skin.verticalScrollbarUpButton.normal.background = t3;
			skin.verticalScrollbarUpButton.normal.scaledBackgrounds = t3s;

			skin.verticalScrollbarDownButton.normal.background = t4;
			skin.verticalScrollbarDownButton.normal.scaledBackgrounds = t4s;

			/* skin.verticalScrollbarUpButton.border.bottom = w3h_0;
			 skin.verticalScrollbarUpButton.border.left = w3h_1;
			 skin.verticalScrollbarUpButton.border.right = w3h_2;
			 skin.verticalScrollbarUpButton.border.top = w3h_3;*/
		}

		void DRAWPRESETS(Rect inputrect)
		{

			if (clearHoverOnRepaint)
			{
				clearHoverOnRepaint = false;
				hover_cache.Clear();
			}
			CHECK_CACHE();

			if (!Selection.activeObject && source.Validate())
			{
				if (adapter.pluginID == 0) Selection.objects = new[] { (UnityEngine.Object)source.go };
				else Selection.objects = new[] { source.go };
			}

			PRESET_LINE_H = Mathf.RoundToInt(label.CalcHeight(nullContent, 60) + 2);
			// MonoBehaviour.print();

			Label(new Rect(inputrect.x + 10, 44, inputrect.width - 20, EditorGUIUtility.singleLineHeight), "Saving inspector settings for objects and components");

			var ov = adapter.par_e.PRESETS_SAVE_GAMEOBJEST ? 1 : 0;
			var nv = GUI.Toolbar(new Rect(inputrect.x + 10, 44 + EditorGUIUtility.singleLineHeight, inputrect.width - 30, EditorGUIUtility.singleLineHeight)
								  , ov, new[] { "No", "Save UnityEngine.Object" }, EditorStyles.toolbarButton);
			if (nv != ov)
			{
				adapter.par_e.PRESETS_SAVE_GAMEOBJEST = nv == 1;
			}

			/*var m = GUI.matrix;
			GUI.matrix = Matrix4x4.Scale( new Vector3( .8f , .8f , .8f ) );
			var sr = EditorGUI.ToggleLeft( new Rect( inputrect.x + 10 , inputrect.y / 0.8f - 2 + EditorGUIUtility.singleLineHeight , inputrect.width , EditorGUIUtility.singleLineHeight ) ,
					"Save UnityEngine.Object References" ,
					adapter.par.PresetManagerParams.SAVE_GAMEOBJEST );
			if ( sr != adapter.par.PresetManagerParams.SAVE_GAMEOBJEST ) {
				adapter.par.PresetManagerParams.SAVE_GAMEOBJEST = sr;
				adapter.SavePrefs();
			}
			GUI.matrix = m;*/

			inputrect.x += 3;
			inputrect.y += EditorGUIUtility.singleLineHeight * 2 + 4;
			inputrect.width -= EditorGUIUtility.singleLineHeight;

			inputrect.width = 128;
			inputrect.height = 366;

			var selected_index = SessionState.GetInt("EMX|PresetSectedIndex", 0);
			if (selected_index >= cache.Count) selected_index = cache.Count - 1;

			DRAW_CATEGORIES(inputrect, selected_index);


			inputrect.x += inputrect.width + 8;
			inputrect.width = 259;

			CHECK_CACHE();

			if (selected_index >= cache.Count) selected_index = cache.Count - 1;



			DRAW_PRESETS(inputrect, selected_index);


		}



		float SCROLL_W = 4;


		void DRAW_CATEGORIES(Rect inputrect, int selected_index)
		{

			adapter.INTERNAL_BOX(inputrect, "");
			var group_rect = inputrect;
			Shrink(ref group_rect, 1);

			var scroll = new Vector2(0, SessionState.GetInt("EMX|PresetScrollCat", 0));// Hierarchy_GUI.Instance(adapter).GETPRESETS_SCROLL_CAT);

			var rr = new Rect(0, 0, group_rect.width, CATEGORY_HEIGHT(cache));
			INIT_SCROLL(SCROLL_W);
			rr.height += PRESET_LINE_H; // ADD SCROLL
			var SC_COMP = group_rect.height < rr.height;
			if (SC_COMP) rr.width -= SCROLL_W;
			var ns = GUI.BeginScrollView(group_rect, scroll, rr, false, false);
			RESET_SCROLL();
			if (ns.y != scroll.y) SessionState.SetInt("EMX|PresetScrollCat", (int)ns.y);
			//Hierarchy_GUI.Instance(adapter).GETPRESETS_SCROLL_CAT = scroll.y;

			var r_line = new Rect(0, 0, group_rect.width, PRESET_LINE_H);
			if (SC_COMP) r_line.width -= SCROLL_W;

			Draw_Set(ref r_line, "Scripts Presets:", cache.singlecomp_categories, selected_index, 0);

			/* var enn = GUI.enabled;
			 GUI.enabled = false;

			 Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
			 Draw_Set( ref r_line , "GameObjects Presets" , cache.fullhierarrchy_categories , selected_index - cache.singlecomp_categories.categories.Length , cache.singlecomp_categories.categories.Length );
			 Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleCenter;
			 Label( r_line , "Comming soon" ); // ADD SCROLL
			 r_line.y += r_line.height;

			 GUI.enabled = enn;*/


			GUI.EndScrollView();
		}



		void Draw_Set(ref Rect r_line, string title, PresetPart categorie, int selected_index, int select_adding)
		{
			Label(r_line, title, TextAnchor.MiddleCenter);
			r_line.y += r_line.height;
			for (int i = 0; i < categorie.categories.Length; i++)
			{
				var button_rect = new Rect(r_line.x + 3, r_line.y + (r_line.height - 15) / 2, r_line.width - 6, 15);
				var numbers_rect = new Rect(button_rect.x + button_rect.width - 2 - 10, button_rect.y + 2, 12, 12);
				var close_rect = numbers_rect;
				close_rect.x -= close_rect.width;

				if (i == selected_index) GUI.DrawTexture(button_rect, adapter.GetExternalModOld("HIGHLIGHTER_PRESETS_SELECTION"));
				EditorGUIUtility.AddCursorRect(r_line, MouseCursor.Link);


				GUI.DrawTexture(close_rect, adapter.GetExternalModOld("HIPERUI_CLOSE"));
				if (Button(close_rect, ""))
				{
					REMOVE_CATEGORY(categorie, i);
					break;
				}

				//* LABEL *//
				if (Button(button_rect, ""))      //  GUI.contentColor = ccc;
				{
					if (Event.current.button == 0) SELECT_CATEGORY_ITEM(i + select_adding);
					if (Event.current.button == 1) RENAME_CATEGORY(categorie, i);

					break;
				}
				var ccc = GUI.contentColor;
				if (i == selected_index) GUI.contentColor = Color.black;
				Label(new Rect(button_rect.x + 8, button_rect.y, button_rect.width - 8, button_rect.height), categorie.categories[i].name);
				GUI.contentColor = ccc;
				//* LABEL *//

				//* ICON *//
				var h = label.CalcHeight(nullContent, 60) - 4;
				var r_icon = new Rect(button_rect.x - 1, button_rect.y + (button_rect.height - h) / 2 + 1, h, h);
				var count = categorie.categories[i].presets != null ? categorie.categories[i].presets.Length : 0;
				if (categorie.categories[i].cache_IAMGE == null)
				{

					if (select_adding != 0)
					{
						categorie.categories[i].cache_IAMGE = TODO_Tools.GetObjectBuildinIcon((UnityEngine.Object)null, typeof(Transform)).add_icon;
					}
					else if (count != 0)
					{
						var t = TODO_Tools.GET_TYPE_BY_STRING_SHORT(categorie.categories[i].PRESET_COMPONENT_TYPE);
						if (t != null)
						{
							categorie.categories[i].cache_TYPE = t;
							categorie.categories[i].cache_IAMGE = TODO_Tools.GetObjectBuildinIcon((UnityEngine.Object)null, t).add_icon;
							if (!categorie.categories[i].cache_IAMGE) categorie.categories[i].cache_IAMGE = TODO_Tools.GetObjectBuildinIcon(Tools.unityMonoBehaviour).add_icon;
						}
					}
				}
				// if (categorie.categories[i].cache_IAMGE) GUI.DrawTexture(new Rect(r_icon.x + 3, r_icon.y, r_icon.width / 1.5f, r_icon.height / 1.5f), categorie.categories[i].cache_IAMGE);
				if (categorie.categories[i].cache_IAMGE) GUI.DrawTexture(r_icon, categorie.categories[i].cache_IAMGE);
				else DRAW_SMALL_NUMBER(r_icon, "-");

				DRAW_SMALL_NUMBER(numbers_rect, count < 1 ? "-" : count.ToString());
				// DRAW_SMALL_NUMBER(new Rect(r_icon.x, r_icon.y, r_icon.width, r_icon.height), count < 1 ? "-" : count.ToString());
				//* ICON *//

				GUI.DrawTexture(new Rect(r_line.x + 9, r_line.y + r_line.height - 1, r_line.width - 18, 5), adapter.GetExternalModOld("HIGHLIGHTER_PRESETS_HL"));
				r_line.y += r_line.height;
			}

			EditorGUIUtility.AddCursorRect(r_line, MouseCursor.Link);
			if (Button(r_line, "+ Add Script +", TextAnchor.MiddleCenter))
			{
				ADD_CATEGORY(categorie);
			}

			r_line.y += r_line.height;
		}


		GUIStyle __smallNumbStyle;
		GUIStyle smallNumbStyle
		{
			get
			{
				if (__smallNumbStyle == null)
				{
					__smallNumbStyle = new GUIStyle(label);
					__smallNumbStyle.alignment = TextAnchor.MiddleCenter;
					smallNumbStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.black : Color.white;
				}

				__smallNumbStyle.fontSize = adapter.FONT_8() - 1;
				return __smallNumbStyle;
			}
		}
		GUIStyle __smallNumbStyleNormal;
		GUIStyle smallNumbStyleNormal
		{
			get
			{
				if (__smallNumbStyleNormal == null)
				{
					__smallNumbStyleNormal = new GUIStyle(label);
					__smallNumbStyleNormal.alignment = TextAnchor.MiddleCenter;
				}

				__smallNumbStyleNormal.fontSize = adapter.FONT_8() - 1;
				return __smallNumbStyleNormal;
			}
		}
		internal void DRAW_SMALL_NUMBER(Rect rect, string number)
		{
			if (Event.current.type != EventType.Repaint) return;

			rect.x -= 1;
			rect.y += 1;
			smallNumbStyle.Draw(rect, number, false, false, false, false);
			rect.x += 2;
			smallNumbStyle.Draw(rect, number, false, false, false, false);
			rect.y -= 2;
			smallNumbStyle.Draw(rect, number, false, false, false, false);
			rect.x -= 2;
			smallNumbStyle.Draw(rect, number, false, false, false, false);
			rect.x += 1;
			rect.y += 1;
			smallNumbStyleNormal.Draw(rect, number, false, false, false, false);
		}


		void DRAW_PRESETS_HEADER(Rect inputrect, int i)
		{
			inputrect.height = PRESET_LINE_H;
			var h = label.CalcHeight(nullContent, 60);
			var r_icon = new Rect(inputrect.x, inputrect.y, h, h);
			inputrect.x += h;
			inputrect.width -= h;


			if (i < 0)
			{
				Label(inputrect, "...");
			}
			else
			{
				var item = cache.GetSetByIndex(i);

				if (item.presets.Length == 0) item.PRESET_COMPONENT_TYPE = null;

				if (item.cache_IAMGE) GUI.DrawTexture(r_icon, item.cache_IAMGE);



				// Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleCenter;
				if (string.IsNullOrEmpty(item.PRESET_COMPONENT_TYPE))
				{
					Label(inputrect, "...");

				}
				else
				{
					var compname = item.PRESET_COMPONENT_TYPE;
					var s = compname.LastIndexOf('.');
					if (s != -1 && s < compname.Length - 1) compname = compname.Substring(s + 1);
					Label(inputrect, compname + ":");
				}


			}

			// Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
			// var count = categorie.categories[i].presets != null ? categorie.categories[i].presets.Length : 0;

			//Hierarchy.DRAW_SMALL_NUMBER(r_icon, count);
		}


		/*UnityEngine.Object LocalIdInFileToObject(long inFileId)
		{


			foreach (var c in Resources.FindObjectsOfTypeAll<Component>())
			{
				var localID = Adapter.GetLocalIdentifierInFile(c);
				if (!localIdInFIle_to_instanceId.ContainsKey(localID)) localIdInFIle_to_instanceId.Add(localID, c.GetInstanceID());
			}
			foreach (var c in Resources.FindObjectsOfTypeAll<GameObject>())
			{
				var localID = Adapter.GetLocalIdentifierInFile(c);
				if (!localIdInFIle_to_instanceId.ContainsKey(localID)) localIdInFIle_to_instanceId.Add(localID, c.GetInstanceID());
			}
			// Debug.Log( GetLocalIdentifierInFile( Selection.activeGameObject.GetInstanceID() ) );
			if (localIdInFIle_to_instanceId.ContainsKey(inFileId)) return EditorUtility.InstanceIDToObject(localIdInFIle_to_instanceId[inFileId]);
			return null;
			//return Resources.FindObjectsOfTypeAll<Component>().FirstOrDefault( c => Adapter.GetLocalIdentifierInFile( c.GetInstanceID() ) == inFileId );
		}
		*/


		void DRAW_PRESETS(Rect inputrect, int selected_category_index)
		{
			adapter.INTERNAL_BOX(inputrect, "");






			if (selected_category_index < 0)
			{   //  Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleCenter;
				Label(new Rect(inputrect.x, inputrect.y + 2, inputrect.width, inputrect.height), "Category not selected", TextAnchor.MiddleCenter);
				//  Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
				return;
			}



			var r_header = inputrect;
			r_header.height = PRESET_LINE_H;
			DRAW_PRESETS_HEADER(r_header, selected_category_index);
			GUI.DrawTexture(new Rect(r_header.x + 9, r_header.y + r_header.height - 1, r_header.width - 18, 5), adapter.GetExternalModOld("HIGHLIGHTER_PRESETS_HL"));
			var item = cache.GetSetByIndex(selected_category_index);



			var haveSource = item.presets.FirstOrDefault(s => s.id_in_external_heap != -1);
			/*   var enn = GUI.enabled;
			   GUI.enabled = ;*/

			var sourceselect_rect = new Rect(r_header.x + r_header.width - 55, r_header.y, 55, r_header.height);
			if (haveSource != null && Button(new Rect(sourceselect_rect.x, sourceselect_rect.y, sourceselect_rect.width, sourceselect_rect.height - 2), ""))
			{


				//haveSource.external_heap_guid_copy

				//HierarchyTempSceneDataGetter.SetObjectData(SaverType.PresetsManager, o);
				//var data = HierarchyTempSceneDataGetter.GetObjectData(SaverType.PresetsManager, o);
				var finded = GetByLocalIdentifier(new externalheapdata() { ID_IN_EXTERNAL_HEAP = haveSource.id_in_external_heap, globaliddata = haveSource.external_heap_guid_copy });
				if (finded != null && finded.target)
				{
					Selection.objects = new[] { finded.target };
				}

				//var result = LocalIdInFileToObject(haveSource.id_in_external_heap);
				//if (!result) haveSource.id_in_external_heap = -1;
				//else Selection.objects = new[] { result };
			}
			if (haveSource != null) Label(sourceselect_rect, "Select");



			inputrect.y += PRESET_LINE_H;
			inputrect.height -= PRESET_LINE_H;





			var group_rect = inputrect;
			Shrink(ref group_rect, 1);


			if (item.presets == null) item.presets = new SetItem[0];
			string currentjson = ToPresetsJson(source.go, item.SetType == PRESET_TYPE.SINGLE_COMPONENT ? item.cache_TYPE : null);


			//var scroll = new Vector2(0, Hierarchy_GUI.Instance(adapter).GETPRESETS_SCROLL_PRES);
			var scroll = new Vector2(0, SessionState.GetInt("EMX|PresetScrollPres", 0));
			var rr = new Rect(0, 0, group_rect.width, item.presets.Length * PRESET_LINE_H);
			INIT_SCROLL(SCROLL_W);
			var SC_COMP = group_rect.height < rr.height;
			if (SC_COMP) rr.width -= SCROLL_W;
			var ns = GUI.BeginScrollView(group_rect, scroll, rr, false, false);
			RESET_SCROLL();
			//Hierarchy_GUI.Instance(adapter).GETPRESETS_SCROLL_PRES = scroll.y;
			if (ns.y != scroll.y) SessionState.SetInt("EMX|PresetScrollPres", (int)ns.y);

			var r_line = new Rect(0, 0, group_rect.width, PRESET_LINE_H);
			if (SC_COMP) r_line.width -= SCROLL_W;
			for (int i = 0; i < item.presets.Length; i++)
			{
				if (!hover_cache.ContainsKey(i))
				{
					hover_cache.Add(i, !string.IsNullOrEmpty(item.presets[i].json) && JSON_EQUALS(currentjson, item.presets[i].json));
				}
				DRAW_PRESET_LINE(ref r_line, i, item, hover_cache[i]);
			}
			// Adapter.GET_SKIN().button.alignment = TextAnchor.MiddleCenter;
			EditorGUIUtility.AddCursorRect(r_line, MouseCursor.Link);
			if (Button(r_line, "+ Add Preset +", TextAnchor.MiddleCenter))
			{
				ADD_PRESET(item);
			}
			// Adapter.GET_SKIN().button.alignment = TextAnchor.MiddleLeft;


			GUI.EndScrollView();
		}


		static Dictionary<int, bool> hover_cache = new Dictionary<int, bool>();

		//Color gra = new Color( 0.5f , 0.5f , 0.5f , 0.2f );

		void DRAW_PRESET_LINE(ref Rect r_line, int i, PresetSet category, bool isSelected)
		{
			var button_rect = new Rect(r_line.x + 3, r_line.y + (r_line.height - 15) / 2, r_line.width - 6, 15);
			var close_rect = new Rect(button_rect.x + button_rect.width - 2 - 10, button_rect.y + 2, 12, 12);
			var save_rect = close_rect;
			save_rect.width = 50;
			save_rect.x -= save_rect.width;

			if (isSelected) GUI.DrawTexture(button_rect, adapter.GetExternalModOld("HIGHLIGHTER_PRESETS_SELECTION"));
			EditorGUIUtility.AddCursorRect(r_line, MouseCursor.Link);

			//SAVE
			if (Button(save_rect, ""))
			{
				ADD_PRESET(category, i);
				SELECT_PRESET_ITEM(category, i);
				return;
			}

			EditorGUI.DrawRect(save_rect, new Color(Colors.EditorBGColor.r, Colors.EditorBGColor.g, Colors.EditorBGColor.b, 0.5f));
			var ds = label.fontSize;
			label.fontSize = adapter.FONT_8() - 1;
			Label(save_rect, "SAVE");
			label.fontSize = ds;
			//SAVE


			GUI.DrawTexture(close_rect, adapter.GetExternalModOld("HIPERUI_CLOSE"));
			if (Button(close_rect, ""))
			{
				REMOVE_PRESET(category, i);
				return;
			}


			// GUI.enabled = enn;

			//* LABEL *//
			if (Button(button_rect, ""))      //  GUI.contentColor = ccc;
			{
				if (Event.current.button == 0) SELECT_PRESET_ITEM(category, i);
				if (Event.current.button == 1) RENAME_PRESET(category, i);

				return;
			}
			var ccc = GUI.contentColor;
			if (isSelected) GUI.contentColor = Color.black;
			Label(new Rect(button_rect.x + 8, button_rect.y, button_rect.width - 8, button_rect.height), category.presets[i].name);
			GUI.contentColor = ccc;
			//* LABEL *//


			//  GUI.DrawTexture(new Rect(r_line.x + 9, r_line.y + r_line.height - 1, r_line.width - 18, 5), Hierarchy.GetIcon("HIGHLIGHTER_PRESETS_HL"));
			r_line.y += r_line.height;

		}























		///////////////////////////////////////// OPERATIONS
		void SELECT_CATEGORY_ITEM(int newIndex)
		{

			var presetsSelect = SessionState.GetInt("EMX|PresetSectedIndex", 0);
			if (presetsSelect == newIndex) return;
			SessionState.SetInt("EMX|PresetSectedIndex", newIndex);
			//Hierarchy_GUI.Instance(adapter).GETPRESETS_SELECT = newIndex;
			SaveCache();
		}
		void ADD_CATEGORY(PresetPart category)
		{
			CHECK_CACHE();


			var inter = cache.Count + 1;
			var newName = "Component " + inter++;
			while (cache.fullhierarrchy_categories.categories.Any(c => c.name == newName) || cache.singlecomp_categories.categories.Any(c => c.name == newName)) newName = "Set " + inter++;

			var pos = new MousePos(Event.current.mousePosition, MousePos.Type.Input_190_68, false, adapter);
			// var pos = InputData.WidnwoRect(WidnwoRectType.Full, Event.current.mousePosition, 190, 68, adapter);
			var w = InputWindow.Init(pos, "Component", adapter, (n) =>
			{
				if (string.IsNullOrEmpty(n)) return;
				var newItem = new PresetSet() { name = n, presets = new SetItem[0], SetType = category.SetType };
				ArrayUtility.Add(ref category.categories, newItem);
				SaveCache();
			}, null, newName);
			w.firstLaunch = true;


		}

		void REMOVE_CATEGORY(PresetPart category, int INDEX)
		{
			ArrayUtility.RemoveAt(ref category.categories, INDEX);
			SaveCacheAndCreateUndo();
		}

		void RENAME_CATEGORY(PresetPart category, int INDEX)
		{
			var target = category.categories[INDEX];
			var pos = new MousePos(Event.current.mousePosition, MousePos.Type.Input_190_68, false, adapter);
			// var pos = InputData.WidnwoRect(WidnwoRectType.Full, Event.current.mousePosition, 190, 68, adapter);
			var w = InputWindow.Init(pos, "Rename Set", adapter, (n) =>
			{
				if (string.IsNullOrEmpty(n)) return;
				if (target != null)
				{
					target.name = n;
					SaveCacheAndCreateUndo();
				}
			}, null, category.categories[INDEX].name);
			w.firstLaunch = true;

		}


		internal static Dictionary<long, int> localIdInFIle_to_instanceId = new Dictionary<long, int>();

		void SELECT_PRESET_ITEM(PresetSet category, int INDEX)
		{
			if (category.SetType == PRESET_TYPE.SINGLE_COMPONENT)     // Debug.Log(category.PRESET_COMPONENT_TYPE);
			{
				var t = TODO_Tools.GET_TYPE_BY_STRING_SHORT(category.PRESET_COMPONENT_TYPE);
				if (t == null)     // var pos = InputData.WidnwoRect(WidnwoRectType.Full, Event.current.mousePosition, 190, 68, adapter);
				{
					var pos = new MousePos(Event.current.mousePosition, MousePos.Type.Input_190_68, false, adapter);
					var n = InputWindow.Init(pos, "Warning", adapter, null, null, "'" + t + "' does not exist anymore");
					n.firstLaunch = true;
					return;
				}
				foreach (var o in Selection.gameObjects)
				{
					FromJsonOverwrite(o, t, category.presets[INDEX].json);
				}
			}
			else
			{
				throw new Exception("Work in progress");
			}
			ClearCacheAndRepaint();
			/* var presetsSelect = Hierarchy_GUI.Initialize().PRESETS_SELECT;
			 if (presetsSelect != null && presetsSelect == newIndex) return;
			 Hierarchy_GUI.Initialize().PRESETS_SELECT = newIndex;
			 SaveCache();*/
		}
		static bool clearHoverOnRepaint = false;
		static void ClearCacheAndRepaint()
		{
			clearHoverOnRepaint = true;
			repaint();
		}

		void REMOVE_PRESET(PresetSet category, int INDEX)
		{
			ArrayUtility.RemoveAt(ref category.presets, INDEX);
			SaveCacheAndCreateUndo();
		}

		void RENAME_PRESET(PresetSet category, int INDEX)
		{
			var target = category.presets[INDEX];
			// var pos = InputData.WidnwoRect(WidnwoRectType.Full, Event.current.mousePosition, 190, 68, adapter);
			var pos = new MousePos(Event.current.mousePosition, MousePos.Type.Input_190_68, false, adapter);
			var w = InputWindow.Init(pos, "Rename Set", adapter, (n) =>
			{
				if (string.IsNullOrEmpty(n)) return;
				if (target != null)
				{
					target.name = n;
					SaveCacheAndCreateUndo();
				}
			}, null, category.presets[INDEX].name);
			w.firstLaunch = true;

		}

		internal static bool CHECK_ADD_PRESET_TOLAST(GameObject o, PluginInstance _adapter)
		{
			adapter = _adapter;
			CHECK_CACHE();
			var presetsSelect = SessionState.GetInt("EMX|PresetSectedIndex", 0);
			//  MonoBehaviour.print(presetsSelect);
			if (presetsSelect >= cache.Count || presetsSelect < 0) return false;
			var target = cache.GetSetByIndex(presetsSelect);

			var all = Cache.GetHierarchyObjectByInstanceID(o).GetComponents();

			//	var all = HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll(o);

			// var all = o.GetComponents<Component>().Where(c => c).ToArray();
			//  MonoBehaviour.print(all.Select(c => c.GetType().FullName).Aggregate((a, b) => a + " " + b));
			//  MonoBehaviour.print(all.All(c => c.GetType().FullName != target.PRESET_COMPONENT_TYPE));
			if (all.All(c => c && c.GetType().FullName != target.PRESET_COMPONENT_TYPE)) return false;
			return true;
		}
		internal static void TRY_ADD_PRESET_TOLAST(GameObject o, PluginInstance _adapter)
		{
			adapter = _adapter;
			Selection.objects = new[] { o };
			source = Cache.GetHierarchyObjectByInstanceID(o);
			CHECK_CACHE();
			var presetsSelect = SessionState.GetInt("EMX|PresetSectedIndex", 0);
			if (presetsSelect >= cache.Count || presetsSelect < 0) return;
			var target = cache.GetSetByIndex(presetsSelect);
			/*
			 var all = comp.gameObject.GetComponents<Component>();
			 if (all.All(c => c.GetType().FullName != target.PRESET_COMPONENT_TYPE)) return ;*/
			ADD_PRESET(target);
		}

		/*static MousePos GET_DEFAULT_RECT(int width, int height)
		{  Rect rect;
			if (adapter.window())
			{   var p = adapter.window().position;
				return new Rect( p.x + p.width / 2 - width / 2, p.y + p.height / 2 - height / 2, width, height );
			}
			return new Rect( Screen.currentResolution.width / 2 - width / 2, Screen.currentResolution.height / 2 - height / 2, width, height );
			var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_190_68, false, adapter);
		}*/


		static bool JSON_EQUALS(string json1, string json2)
		{
			KeeperDataItem data1 = new KeeperDataItem();
			data1.Load(json1);
			//try { data1 = Adapter.DESERIALIZE_SINGLE<KeeperData>(json1); }
			//catch { }

			KeeperDataItem data2 = new KeeperDataItem();
			data2.Load(json2);
			//try { data2 = Adapter.DESERIALIZE_SINGLE<KeeperData>(json2); }
			//catch { }

			if (data1 == null && data2 == null) return true;
			if (data1 == null || data2 == null) return false;
			//  Debug.Log(data1.field_records.Values.Any(c => c == null) + " - " + data2.field_records.Values.Any(c => c == null));
			/*
			if (data1.field_records.Values.Any(c => c == null) || data2.field_records.Values.Any(c => c == null)) return false;

			var order1 = data1.field_records.Values.SelectMany(c => c.records.Values).OrderBy(c => c.index).ToArray();
			var order2 = data2.field_records.Values.SelectMany(c => c.records.Values).OrderBy(c => c.index).ToArray();
			if (order1.Length != order2.Length || order1.Length == 0) return false;
			return !order1.Where((t, i) => t != order2[i]).Any();*/

			//if (data1.field_records.Values.Any(c => c == null) || data2.field_records.Values.Any(c => c == null)) return false;

			var order1 = data1.records.Values.OrderBy(c => c.index).ToArray();
			var order2 = data2.records.Values.OrderBy(c => c.index).ToArray();
			if (order1.Length != order2.Length || order1.Length == 0) return false;
			return !order1.Where((t, i) => t != order2[i]).Any();
		}


		static void ADD_PRESET(PresetSet cat, int? useIndex = null)
		{
			if (cat.SetType == PRESET_TYPE.FULL_HIERARCHY || cat.SetType == PRESET_TYPE.NOT_INITIALIZED) return;

			if (cat.SetType == PRESET_TYPE.SINGLE_COMPONENT)
			{
				var o = source;
				var types = o.GetComponents().Where(c => c).Select(c => c.GetType()).Distinct().ToArray();
				//  var pos = Event.current == null ? GET_DEFAULT_RECT(190, 68) : InputData.WidnwoRect(WidnwoRectType.Full, Event.current.mousePosition, 190, 68, adapter);
				var pos = new MousePos(Event.current == null ? (Vector2?)null : Event.current.mousePosition, MousePos.Type.Input_190_68, false, adapter);

				if (cat.presets.Length == 0) cat.PRESET_COMPONENT_TYPE = "";




				if (string.IsNullOrEmpty(cat.PRESET_COMPONENT_TYPE))
				{
					var menu = new GenericMenu();
					menu.AddDisabledItem(new GUIContent("Select the component to create the preset"));
					menu.AddSeparator("");
					foreach (var c in types)
					{
						var captureComponent = c;
						menu.AddItem(new GUIContent(captureComponent.Name), false, () =>
					 {

						 // final(captureComponent.FullName);
						 var newJson = m_final_add_preset(cat, o, pos, types, captureComponent.FullName, useIndex);
						 if (newJson != null)
						 {
							 if (useIndex != null) m_fnal_add_save(cat, o, "", newJson, captureComponent.FullName, useIndex);
							 else m_final_add_window(cat, o, pos, newJson, captureComponent.FullName);
						 }
					 });
					}
					menu.ShowAsContext();
				}
				else
				{
					var newJson = m_final_add_preset(cat, o, pos, types, null, useIndex);
					if (newJson != null)
					{

						if (useIndex != null) m_fnal_add_save(cat, o, "", newJson, null, useIndex);
						else m_final_add_window(cat, o, pos, newJson, null);
					}


					//final(null);
				}

			}
		}//ADD_PRESET


		static void m_final_add_window(PresetSet cat, HierarchyObject o, MousePos pos, string newJson, string presetType)
		{
			var inter = cat.presets.Length + 1;
			var newName = "Preset " + inter++;
			while (cat.presets.Any(c => c.name == newName)) newName = "Preset " + inter++;
			var w = InputWindow.Init(pos, "New Preset", adapter, (n) =>
			{
				m_fnal_add_save(cat, o, n, newJson, presetType);
			}, null, newName);
			w.firstLaunch = true;
		}

		static void m_fnal_add_save(PresetSet cat, HierarchyObject o, string n, string newJson, string presetType, int? useIndex = null)
		{
			if (useIndex.HasValue) n = cat.presets[useIndex.Value].name;

			if (string.IsNullOrEmpty(n)) return;

			/*if (presetType != null) cat.PRESET_COMPONENT_TYPE = presetType;
			var type = types.FirstOrDefault(t => t.FullName == cat.PRESET_COMPONENT_TYPE);
			if (type == null) return;*/


			if (presetType != null) cat.PRESET_COMPONENT_TYPE = presetType;

			HierarchyTempSceneDataGetter.SetObjectData(SaverType.PresetsManager, o, true);
			var data = HierarchyTempSceneDataGetter.GetObjectData(SaverType.PresetsManager, o);
			var external_objects = HierarchyExternalSceneData.LoadObjects(SaverType.PresetsManager, o.go.scene);
			var ex_o = external_objects.First(t2 => t2.id_in_external_heap == data.id_in_external_heap);

			var newpreset = new SetItem() { name = n, id_in_external_heap = data.id_in_external_heap, external_heap_guid_copy = ex_o.GetGuidData() };
			// Debug.Log(cat.PRESET_COMPONENT_TYPE);

			newpreset.json = newJson;

			if (useIndex.HasValue) cat.presets[useIndex.Value] = newpreset;
			else ArrayUtility.Add(ref cat.presets, newpreset);

			SaveCache();
		}

		static string m_final_add_preset(PresetSet cat, HierarchyObject o, MousePos pos, Type[] types, string presetType, int? useIndex = null)          //Action<string> final = ( presetType ) => {
		{
			if (!o.go) return null;

			if (presetType != null) cat.PRESET_COMPONENT_TYPE = presetType;
			if (types.All(t => t.FullName != cat.PRESET_COMPONENT_TYPE))     // Hierarchy.Mody = () => {
			{
				var compname = cat.PRESET_COMPONENT_TYPE;
				var s = compname.LastIndexOf('.');
				if (s != -1 && s < compname.Length - 1) compname = compname.Substring(s + 1);
				var n = InputWindow.Init(pos, "Info", adapter, null, null, "'" + o.go.name + "' does not contain '" + compname + "'");
				if (n) n.firstLaunch = true;
				/* };
				Hierarchy.NeedApplyMod = true;*/
				return null;
			}


			if (presetType != null) cat.PRESET_COMPONENT_TYPE = presetType;
			var type = types.FirstOrDefault(t => t.FullName == cat.PRESET_COMPONENT_TYPE);
			var newJson = ToPresetsJson(o.go, type);

			//if ( useIndex.HasValue ) return newJson;

			if (cat.presets.Any(p => JSON_EQUALS(p.json, newJson)))      //Hierarchy.Mody = () => {
			{
				var n2 = InputWindow.Init(pos, "Info", adapter, null, null, "Such preset already exists '" + cat.presets.First(p => JSON_EQUALS(p.json, newJson)).name + "'");
				if (n2) n2.firstLaunch = true;
				/* };
				Hierarchy.NeedApplyMod = true;*/

				return null;
			}


			return newJson;
			//
			//};
		}


		///////////////////////////////////////// OPERATIONS


























		///// FLUSH
		///


		static string ToPresetsJson(GameObject o, Type type)
		{
			KeeperDataItem data = new KeeperDataItem();
			//  MonoBehaviour.print( "ASD" );
			if (type == null)
			{

				return "";
				/*
				var chld = o.GetComponentsInChildren<Transform>(true).ToList();
				chld.Remove(o.transform);
				if (chld.Count == 0) chld.Add(o.transform);
				else chld.Insert(0, o.transform);

				foreach (var transform in chld)
				{
					var l = transform.GetComponents<Component>().Where(c => c).ToArray();
					for (int i = 0; i < l.Length; i++)
					{
						INTERNAL_RECORD_FLUSH(data, l[i], i);
					}
				}
				*/
			}
			else
			{

				var all = Cache.GetHierarchyObjectByInstanceID(o).GetComponents().Where(c => c).ToList();
				var l = o.GetComponents(type);
				if (l.Length == 0) return "";
				//	foreach (var component in l) INTERNAL_RECORD_FLUSH(data, component, all.IndexOf(component));
				data.COMP_TYPE = type.Name;
				INTERNAL_RECORD_FLUSH(data, l[0], 0);
				// return l.Select(EditorJsonUtility.ToJson).Aggregate((a, b) => a + "|JSON_SEPARATE|");
			}

			return data.Save();
		}

		static void FromJsonOverwrite(GameObject o, Type type, string json)
		{


			var UNDO_TEXT = "Apply Preset";
			if (type == null)     /** WORK IN PROGRESS */
			{
				throw new Exception("Work in progress");
			}
			else
			{
				var l = o.GetComponents(type);

				KeeperDataItem data = new KeeperDataItem();
				data.Load(json);



				/*	if (data == null || data.field_records.Count == 0)
					{
						Debug.LogWarning("Error to load '" + type.Name + "' data");
						return;
					}*/

				var old = data.COMP_TYPE;
				if (old != type.FullName)     //var pos = InputData.WidnwoRect(WidnwoRectType.Full, Event.current.mousePosition, 190, 68, adapter);
				{
					var pos = new MousePos(Event.current.mousePosition, MousePos.Type.Input_190_68, false, adapter);

					var n = InputWindow.Init(pos, "Warning", adapter, null, null, "Preset manager cannot read '" + old + "' " + "type");
					n.firstLaunch = true;
					return;
				}


				var currentPos = 0;
				var records = data.records.Values.OrderBy(r => r.index).ToArray();

				for (; currentPos < records.Length; currentPos++)
				{
					if (currentPos >= l.Length)
					{
						var newC = o.AddComponent(type);
						Undo.RegisterCreatedObjectUndo(newC, UNDO_TEXT);
						l = o.GetComponents(type);
					}

					var targetComp = l[currentPos];


					Undo.RecordObject(targetComp, UNDO_TEXT);
					INTERNAL_FromJsonOverwrite(records[currentPos], targetComp);
					if (!Application.isPlaying)
					{
						EditorUtility.SetDirty(targetComp);
						if (targetComp.gameObject) EditorUtility.SetDirty(targetComp.gameObject);
						if (targetComp.gameObject.scene.IsValid()) EditorSceneManager.MarkSceneDirty(targetComp.gameObject.scene);
					}

				}
				if (currentPos < l.Length)
				{
					for (int i = currentPos; i < l.Length; i++)
					{
						if (!l[i]) continue;
						if (!Application.isPlaying) Undo.DestroyObjectImmediate(l[i]);
						else GameObject.Destroy(l[i]);
					}
				}
			}
		}

		/*   [MenuItem("ASDASDASD/asdasd")]
		   static void asdasd() {
			   Debug.Log( Adapter.GetLocalIdentifierInFile( Selection.activeObject ) );
		   }*/

		static externalheapdata GetLocalIdentifierInFile(UnityEngine.Object o)
		{
			HierarchyTempSceneDataGetter.SetObjectDataAsComponent(SaverType.PresetsManager, o, source.go.scene, true);
			var data = HierarchyTempSceneDataGetter.GetObjectData(SaverType.PresetsManager, o, source.go.scene);

			var external_objects = HierarchyExternalSceneData.LoadObjects(SaverType.PresetsManager, source.go.scene);
			var ex_o = external_objects.First(t2 => t2.id_in_external_heap == data.id_in_external_heap);

			return new externalheapdata()
			{
				ID_IN_EXTERNAL_HEAP = data.id_in_external_heap,
				globaliddata = ex_o.GetGuidData()
			};

		}

		static TempSceneObjectPTR GetByLocalIdentifier(externalheapdata inp)
		{
			if (inp.ID_IN_EXTERNAL_HEAP == -1) return null;
			/*var external_objects = HierarchyExternalSceneData.LoadObjects(SaverType.PresetsManager, source.go.scene);
			int i = 0;
			for (; i < external_objects.Length; i++) if (external_objects[i].id_in_external_heap == haveSource.id_in_external_heap)
				{
					external_objects[i].SetGuidData(haveSource.external_heap_guid_copy);
				}
			if (i == external_objects.Length)
			{
				Array.Resize(ref external_objects, external_objects.Length + 1);
				external_objects[i] = new SavedObjectData(haveSource.id_in_external_heap);
				external_objects[i].SetGuidData(haveSource.external_heap_guid_copy);
			}
			HierarchyExternalSceneData.WriteObjects(SaverType.PresetsManager, source.go.scene, external_objects);
			HierarchyTempSceneDataGetter.ClearSceneCache(SaverType.PresetsManager, source.go.scene);
			var data = HierarchyTempSceneDataGetter.GetAllObjectData(SaverType.PresetsManager, source.go.scene);
			var finded = data.Values.FirstOrDefault(d => d.id_in_external_heap == haveSource.id_in_external_heap);
			*/

			var external_objects = HierarchyExternalSceneData.LoadObjects(SaverType.PresetsManager, source.go.scene);
			int i = 0;
			for (; i < external_objects.Length; i++) if (external_objects[i].id_in_external_heap == inp.ID_IN_EXTERNAL_HEAP)
				{
					external_objects[i].SetGuidData(inp.globaliddata);
				}
			if (i == external_objects.Length)
			{
				Array.Resize(ref external_objects, external_objects.Length + 1);
				external_objects[i] = new SavedObjectData(inp.ID_IN_EXTERNAL_HEAP);
				external_objects[i].SetGuidData(inp.globaliddata);
			}
			HierarchyExternalSceneData.WriteObjects(SaverType.PresetsManager, source.go.scene, external_objects);
			HierarchyTempSceneDataGetter.ClearSceneCache(SaverType.PresetsManager, source.go.scene);
			var data = HierarchyTempSceneDataGetter.GetAllObjectData(SaverType.PresetsManager, source.go.scene);
			var finded = data.Values.FirstOrDefault(d => d.id_in_external_heap == inp.ID_IN_EXTERNAL_HEAP);
			return finded;
			/*	if (finded != null && finded.target)
				{
					Selection.objects = new[] { finded.target };
				}*/
		}

		static KeeperDataUnityJsonData INTERNAL_ToJson(UnityEngine.Object obj, Scene s)
		{
			var jsonData = new KeeperDataUnityJsonData();
			jsonData.default_json = EditorJsonUtility.ToJson(obj);
			if (adapter.par_e.PRESETS_SAVE_GAMEOBJEST)
			{
				var f = Tools.GET_FIELDS_AND_VALUES(obj, obj.GetType());

				jsonData.fields_name = new string[f.Length];
				// f.Keys.CopyTo( jsonData.fields_name , 0 );
				jsonData.fields_new_value = new KeeperDataFieldValue[f.Length];
				for (int i = 0; i < jsonData.fields_name.Length; i++)
				{   //var v = ((UnityEngine.Object)f[jsonData.fields_name[i]].GetValue(obj));
					jsonData.fields_name[i] = f[i].Key;
					var v = f[i].Value.Value as UnityEngine.Object;
					if (v != null)
					{
						if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(v)))
						{
							jsonData.fields_new_value[i] = new KeeperDataFieldValue() { GUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(v)) };
						}
						else
						{
							jsonData.fields_new_value[i] = new KeeperDataFieldValue() { EXTERNAL_HEAP = GetLocalIdentifierInFile(v) };
						}
					}

				}
			}
			return jsonData;
		}

		static void INTERNAL_FromJsonOverwrite(KeeperDataUnityJsonData jsonData, UnityEngine.Object obj)      //  if (json.Length != 2) return;
		{
			if (jsonData == null) return;
			/*  MonoBehaviour.print(obj);
			  MonoBehaviour.print( jsonData );
			  MonoBehaviour.print( jsonData.default_json);*/
			if (!string.IsNullOrEmpty(jsonData.default_json))
			{

				var fff = Tools.GET_FIELDS(obj.GetType());
				var f = fff.Values.Select(_f => new { _f, value = _f.GetAllValues(obj, 0, 0) }).ToArray();

				EditorJsonUtility.FromJsonOverwrite(jsonData.default_json, obj);
				foreach (var item in f)
				{
					item._f.SetAllValues(obj, item.value);
				}

				/*

				 var fff = Adapter.GET_FIELDS(obj.GetType());
				 // var f = fff.Values.Select(_f => new {_f, value = _f.GetValue(obj) } ).ToArray();
				 var f = fff.Values.Select(_f => new {_f, value = _f.GetValue(obj) } ).ToArray();
				 //Debug.Log( fff.Values.First().GetValue(obj) + " " + fff.Keys.First());
				 foreach ( var field in f ) {
					 field._f.SetValue( obj , field.value );
				 }*/
			}
			//bool wasTry = false;
			if (jsonData.fields_name != null && adapter.par_e.PRESETS_SAVE_GAMEOBJEST)     //  MonoBehaviour.print(obj.name);
			{
				var fff = Tools.GET_FIELDS_AND_VALUES(obj, obj.GetType(), searchVals: 4);//.ToDictionary(v=>v.Key, v=>v.Value)
				Dictionary<Tools.FieldAdapter, Dictionary<string, object>> result = new Dictionary<Tools.FieldAdapter, Dictionary<string, object>>();
				foreach (var item in fff)
				{
					if (!result.ContainsKey(item.Value.Key)) result.Add(item.Value.Key, new Dictionary<string, object>());
					result[item.Value.Key].Add(item.Key, item.Value.Value);
				}

				foreach (var item in result)
				{   //var f = item.Value;

					foreach (var k in item.Value.Keys.ToArray())
					{
						if (!item.Value.ContainsKey(k)) continue;
						item.Value[k] = null;
					}

					for (int i = 0; i < jsonData.fields_name.Length; i++)
					{

						if (!item.Value.ContainsKey(jsonData.fields_name[i]) /*|| !Adapter.unityObjectType.IsAssignableFrom( f[jsonData.fields_name[i]].FieldType )*/) continue;


						if (jsonData.fields_new_value[i] == null || jsonData.fields_new_value[i].EXTERNAL_HEAP.ID_IN_EXTERNAL_HEAP == -1 && string.IsNullOrEmpty(jsonData.fields_new_value[i].GUID)) continue;
						// f[jsonData.fields_name[i]].SetValue( obj , (UnityEngine.Object)null );
						// else
						{

							UnityEngine.Object oo = null;

							if (!string.IsNullOrEmpty(jsonData.fields_new_value[i].GUID))
							{
								var p = AssetDatabase.GUIDToAssetPath(jsonData.fields_new_value[i].GUID);
								if (!string.IsNullOrEmpty(p)) oo = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p);
							}
							else
							{



								/*	if (!wasTry)
									{
										if (localIdInFIle_to_instanceId.Count == 0 || !localIdInFIle_to_instanceId.ContainsKey(jsonData.fields_new_value[i].ID_IN_EXTERNAL_HEAP))
										{
											wasTry = true;
											foreach (var c in Resources.FindObjectsOfTypeAll<Component>())
											{
												var localID = Adapter.GetLocalIdentifierInFile(c);
												if (!localIdInFIle_to_instanceId.ContainsKey(localID)) localIdInFIle_to_instanceId.Add(localID, c.GetInstanceID());
											}
											foreach (var c in Resources.FindObjectsOfTypeAll<GameObject>())
											{
												var localID = Adapter.GetLocalIdentifierInFile(c);
												if (!localIdInFIle_to_instanceId.ContainsKey(localID)) localIdInFIle_to_instanceId.Add(localID, c.GetInstanceID());
											}
										}
									}

									if (wasTry && !localIdInFIle_to_instanceId.ContainsKey(jsonData.fields_new_value[i].ID_IN_EXTERNAL_HEAP))
										localIdInFIle_to_instanceId.Add(jsonData.fields_new_value[i].ID_IN_EXTERNAL_HEAP, -1);
										*/

								//oo = localIdInFIle_to_instanceId[jsonData.fields_new_value[i].ID_IN_EXTERNAL_HEAP] == -1 ? null : Adapter.GET_OBJECT(localIdInFIle_to_instanceId[jsonData.fields_new_value[i].ID_IN_EXTERNAL_HEAP]);
								if (jsonData.fields_new_value[i].EXTERNAL_HEAP.ID_IN_EXTERNAL_HEAP == -1) oo = null;
								else
								{
									var ext = GetByLocalIdentifier(jsonData.fields_new_value[i].EXTERNAL_HEAP);
									if (ext == null) oo = null;
									else oo = ext._target;
								}

							}



							//MonoBehaviour.print( oo.name );
							item.Value[jsonData.fields_name[i]] = oo;
							try
							{   // f[jsonData.fields_name[i]].SetValue( obj , oo );
							}
							catch
							{

							}

							//MonoBehaviour.print( EditorUtility.InstanceIDToObject( jsonData.fields_value[i] ).name );

							//f[jsonData.fields_name[i]].SetValue(obj, EditorUtility.InstanceIDToObject(jsonData.fields_value[i]));
						}
					}


					item.Key.SetAllValues(obj, item.Value);

				}


			}

		}


		//static long gameObject_ID;
		//static long comp_ID;

		internal static void INTERNAL_RECORD_FLUSH(KeeperDataItem source, Component comp, int index)        /* RECORD */
		{
			//gameObject_ID = Adapter.GetLocalIdentifierInFile(comp.gameObject);
			//comp_ID = Adapter.GetLocalIdentifierInFile(comp);
			//if (!source.comp_to_Type.ContainsKey(comp_ID))     //  MonoBehaviour.print(comp.GetType().FullName);
			//{
			//	source.comp_to_Type.Add(comp_ID, comp.GetType().FullName);
			//}

			//if (!source.field_records.ContainsKey(gameObject_ID)) source.field_records.Add(gameObject_ID, new KeeperDataItem() );
			//if (!source.field_records[gameObject_ID].records.ContainsKey(index))
			if (!source.records.ContainsKey(index))
			{
				var j = INTERNAL_ToJson(comp, comp.gameObject.scene);
				j.index = index;
				source.records.Add(index, j);
			}
		}


		///// FLUSH
		///



















	}
}
