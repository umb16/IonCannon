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
	class HG_Window : ScriptableObject
	{
	}


	[CustomEditor(typeof(HG_Window))]
	class HyperGraphSettingsEditor : MainRoot
	{
		internal static string set_text = "HyperGraph";
		internal static string set_key = "";
		public override VisualElement CreateInspectorGUI()
		{
			return base.CreateInspectorGUI();
		}
		public override void OnInspectorGUI()
		{
			Draw.RESET();

			Draw.BACK_BUTTON();
			Draw.TOG_TIT(set_text);
			Draw.Sp(10);
			using (GRO.UP())
			{


				Draw.TOG_TIT("Attach to Inspector window when opening a HyperGraph", "ATTACH_TO_INSPECT_ONOPEN");
				Draw.FIELD("Font Size", "HYPERGRAPH_FONTSIZE", 4, 50);
				Draw.Sp(10);


				GUI.Label(Draw.R, "Scan performance:");
				Draw.TOOLBAR(new[] { "20%", "40%", "60%", "80%", "∞" }, "HYPERGRAPH_SCANPERFOMANCE04");
				Draw.HELP("High value ​​reduces performance");
				Draw.Sp(10);

				using (GRO.UP(0))
				{
					Draw.TOG_TIT("UnityEvents Mode", "HYPERGRAPH_EVENTS_MODE_BOOL");
					Draw.HRx2();
					Draw.TOG_TIT("Display arrays", "HYPERGRAPH_SKIP_ARRAYS_BOOL");
					Draw.TOG_TIT("Display assets references", "HYPERGRAPH_DISPLAY_ASSETS");
					Draw.TOG_TIT("Display self references", "HYPERGRAPH_CONNECT_TO_SELFT");
					Draw.HRx2();
					Draw.TOG_TIT("Auto refresh when selection changed", "HYPERGRAPH_AUTOCHANGE");
					Draw.TOG_TIT("Display loading indicator", "HYPERGRAPH_SHOWUPDATINGINDICATOR");
					Draw.TOG_TIT("Red null references", "HYPERGRAPH_RED_HIGKLIGHTING");


				}
			}




			Draw.HRx4RED();


			GUI.Label(Draw.R, "Quick tips:");
			Draw.HELP_TEXTURE("TAP_HYPER");
			Draw.HELP("Use the right-click on the icon to open a special menu for quick access to mod functions.", drawTog: true);
			Draw.HELP_TEXTURE("HELP_HYPER");
			Draw.HELP("You can use left-click to select object.", drawTog: true);
			Draw.HELP("Use events mode to tracking unity events references.", drawTog: true);
			Draw.HELP("You can disable scan for arrays or internal structures.", drawTog: true);
			//Draw.HELP("Use right-click to remove object.", drawTog: true);
			//Draw.HELP("You can also add descriptions and assign many objects to one button.", drawTog: true);


			Draw.HRx2();
			Draw.HELP("You can find an example scene with hypergraph demos.", drawTog: true);
			//Draw.HELP("You can add your own items using 'ExtensionInterface_RightClickOnGameObjectMenuItem'.", drawTog: true);
			if (Draw.BUT("Select Example Scene")) { Selection.objects = new[] { Root.icons.example_folders[4] }; }

			/*   using (ENABLE.USE(set_key))
			   {
			   }*/
		}
	}
}
