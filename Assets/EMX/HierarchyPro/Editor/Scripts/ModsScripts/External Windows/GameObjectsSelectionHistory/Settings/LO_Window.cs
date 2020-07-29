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
	class LO_Window : ScriptableObject
	{
	}


	[CustomEditor(typeof(LO_Window))]
	class LastSelectionHistorySettingsEditor : MainRoot
	{

		internal static string set_text = "Use GameObjects Selection History";
		internal static string set_key = "USE_LAST_SELECTION_MOD";
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

				var p = Mods.DrawButtonsOld.GET_DISPLAY_PARAMS(MemType.Last);
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
			Draw.HELP_TEXTURE("TAP_LAST");
			Draw.HELP("Use the right-click on the icon to open a special menu for quick access to mod functions.", drawTog: true);
			Draw.HELP_TEXTURE("DRAG_LAST");
			Draw.HELP("Use left-click to select object.", drawTog: true);
			Draw.HELP("Use left-drag to cahnge button position select object.", drawTog: true);
			Draw.HELP("Use left-drag outside the window to drop object in any other window or any inspector field.", drawTog: true);
			Draw.HELP("Use right-click to remove object.", drawTog: true);

		}
	}
}
