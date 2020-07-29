using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EMX.HierarchyPlugin.Editor.Settings
{
	class BO_Window : ScriptableObject
	{
	}
	[CustomEditor(typeof(BO_Window))]
	class BookmarksforGameObjectsSettingsEditor : MainRoot
	{

		internal static string set_text = "Use Bookmarks for GameObjects";
		internal static string set_key = "USE_BOOKMARKS_HIERARCHY_MOD";
		public override VisualElement CreateInspectorGUI()
		{
			return base.CreateInspectorGUI();
		}
		public override void OnInspectorGUI()
		{
			Draw.RESET();

			Draw.BACK_BUTTON();
			Draw.TOG_TIT(set_text, set_key);
			Draw.Sp(10);

			using (ENABLE.USE(set_key))
			{




				using (GRO.UP(0))
				{
					Draw.TOG("Display category name", "BOOKMARKS_OB_DRAW_CATEGORY_NAME");
					Draw.TOG("Draw bg color", "BOOKMARKS_OB_DRAW_BG_COLOR");
					Draw.TOG("Display descriptions", "BOOKMARKS_OB_SHOWDESCRIPTS");
					Draw.TOG("Use shift+click to instantiate", "BOOKMARKS_OB_SHIFT_TO_INSTANTIATE");
					using (ENABLE.USE("BOOKMARKS_OB_SHIFT_TO_INSTANTIATE"))
					{
						GUI.Label(Draw.R, "New instantiate will place to:");
						Draw.TOOLBAR(new[] { "Prefab default", "Selected object" }, "BOOKMARKS_OB_INSTANTIATE_POSITION");
					}
				}



		Draw.Sp(10);

			

					var p = Mods.DrawButtonsOld.GET_DISPLAY_PARAMS(MemType.Custom);
					Draw.FIELD("Rows number", p._Rows_KEY, 1, 10, overrideObject: p);
					Draw.FIELD("Max buttons", p._MaxItems_KEY, 1, 30, overrideObject: p);
					Draw.FIELD("Font size", p._fontSize_KEY, 4, 30, overrideObject: p);
					Draw.COLOR("Background color", p._BgOverrideColor_KEY, overrideObject: p);
					GUI.Label(Draw.R, "Buttons direction order starts from:");
					Draw.TOOLBAR(new[] { "TOP/LEFT", "TOP/RIGHT", "BOTTOM/LEFT", "BOTTOM/RIGHT" }, p._SortButtonsOrder_KEY, overrideObject: p);

					Draw.TOG("Draw tooltips for buttons", p._DrawTooltips_KEY, overrideObject: p);

					Draw.TOG("Draw highlighter colors", p._DrawHiglighter_KEY, overrideObject: p);
					using (ENABLE.USE(set_key)) Draw.FIELD("Highlighter colors opacity", p._HiglighterOpacity_KEY, 0, 1, overrideObject: p);


			}





			Draw.HRx4RED();


			GUI.Label(Draw.R, "Quick tips:");
			Draw.HELP_TEXTURE("TAP_BOOK");
			Draw.HELP("Use the right-click on the icon to open a special menu for quick access to mod functions.", drawTog: true);
			Draw.HELP_TEXTURE("DRAG_BOOK");
			Draw.HELP("Use left-click to select object.", drawTog: true);
			Draw.HELP("Use left-drag to cahnge button position select object.", drawTog: true);
			Draw.HELP("Use left-click to special button to change or create category.", drawTog: true);
			Draw.HELP("Use right-click to remove object.", drawTog: true);
			Draw.HELP("You can also add descriptions and assign many objects to one button.", drawTog: true);
		}
	}


}
