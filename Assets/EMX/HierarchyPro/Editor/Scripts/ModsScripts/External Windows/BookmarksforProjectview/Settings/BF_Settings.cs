using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;



namespace EMX.HierarchyPlugin.Editor
{

	partial class EditorSettingsAdapter
	{


		

		internal int BOOKMARKS_FOLDER_DEFAULT_ICON_SIZE { get { return GET("BOOKMARKS_FOLDER_DEFAULT_ICON_SIZE", 16); } set { SET("BOOKMARKS_FOLDER_DEFAULT_ICON_SIZE", value); } }
		internal int BOOKMARKS_FOLDER_FONTSIZE { get { return GET("BOOKMARKS_FOLDER_FONTSIZE", 11); } set { SET("BOOKMARKS_FOLDER_FONTSIZE", value); } }
		internal int BOOKMARKS_FOLDER_LINE_HEIGHT { get { return GET("BOOKMARKS_FOLDER_LINE_HEIGHT", 20); } set { SET("BOOKMARKS_FOLDER_LINE_HEIGHT", value); } }

		internal bool BOOKMARKS_FOLDER_SHOW_SELECTIONS { get { return false; } set {  } }
		internal bool BOOKMARKS_FOLDER_SHOW_ALL_DESCRIPTIONS_INHIER { get { return GET("BOOKMARKS_FOLDER_SHOW_ALL_DESCRIPTIONS_INHIER", true); } set { SET("BOOKMARKS_FOLDER_SHOW_ALL_DESCRIPTIONS_INHIER", value); } }
		internal bool BOOKMARKS_FOLDER_DRAW_BG_COLOR { get { return GET("BOOKMARKS_FOLDER_DRAW_BG_COLOR", true); } set { SET("BOOKMARKS_FOLDER_DRAW_BG_COLOR", value); } }
		internal Color BOOKMARKS_FOLDER_DEFULT_BG_COLOR { get { return GET("BOOKMARKS_FOLDER_DEFULT_BG_COLOR", Color.white); } set { SET("BOOKMARKS_FOLDER_DEFULT_BG_COLOR", value); } }

	}
}
