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
	class BF_Window : ScriptableObject
	{
	}


	[CustomEditor(typeof(BF_Window))]
	class BookmarksforProjectviewSettingsEditor : MainRoot
	{

		internal static string set_text = "Use Bookmarks for Folders of Project";
		internal static string set_key = "USE_BOOKMARKS_PROJECT_MOD";
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

				// 
				Draw.FIELD("Objects Icons Size", "BOOKMARKS_FOLDER_DEFAULT_ICON_SIZE", 4, 50);
				Draw.FIELD("Labels Font Size", "BOOKMARKS_FOLDER_FONTSIZE", 4, 50);
				Draw.FIELD("Lines Height", "BOOKMARKS_FOLDER_LINE_HEIGHT", 4, 50);
				Draw.Sp(10);
				Draw.TOG("Display descriptions", "BOOKMARKS_FOLDER_SHOW_ALL_DESCRIPTIONS_INHIER");
				Draw.TOG("Use Custom Background Color", "BOOKMARKS_FOLDER_DRAW_BG_COLOR");
				//using (ENABLE.USE("BOOKMARKS_FOLDER_DRAW_BG_COLOR")) Draw.COLOR("Background Color", "BOOKMARKS_FOLDER_DEFULT_BG_COLOR");
			}




			Draw.HRx4RED();


			GUI.Label(Draw.R, "Quick tips:");
			Draw.HELP_TEXTURE("TAP_FOLDER");
			Draw.HELP("Use the right-click on the icon to open a special menu for quick access to mod functions.", drawTog: true);
			Draw.HELP_TEXTURE("HELP_FOLDER");
			Draw.HELP("Use right-click to open special menu for bookmark.", drawTog: true);
			Draw.HELP("You can include all content in selected folder.", drawTog: true);
			Draw.HELP("You can filter included content by files extension.", drawTog: true);
			Draw.HELP("You can enable flat hierarchy (without included folders).", drawTog: true);
			Draw.HELP("You can add description.", drawTog: true);
			//Draw.HELP("Use right-click to remove object.", drawTog: true);
			//Draw.HELP("You can also add descriptions and assign many objects to one button.", drawTog: true);


		}
	}
}
