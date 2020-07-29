using System;
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

	// Add false or true if you want to disable or enable any module without using the menu.
	class HardDisableMods
	{

		static internal bool? USE_TOPBAR_MOD = null;
		static internal bool? USE_BACKGROUNDDECORATIONS_MOD = null;
		static internal bool? USE_SETACTIVE_MOD = null;
		static internal bool? USE_COMPONENTS_ICONS_MOD = null;
		static internal bool? USE_PLAYMODE_SAVER_MOD = null;
		static internal bool? USE_CUSTOM_PRESETS_MOD = false; // in progress
		static internal bool? USE_MANUAL_HIGHLIGHTER_MOD = false; // in progress
		static internal bool? USE_AUTO_HIGHLIGHTER_MOD = false; // in progress
		static internal bool? USE_RIGHT_ALL_MODS = null;
		static internal bool? USE_BOOKMARKS_HIERARCHY_MOD = null;
		static internal bool? USE_BOOKMARKS_PROJECT_MOD = null;
		static internal bool? USE_LAST_SELECTION_MOD = null;
		static internal bool? USE_LAST_SCENES_MOD = null;
		static internal bool? USE_HIER_EXPANDED_MOD = null;
		static internal bool? USE_AUTOSAVE_MOD = null;
		static internal bool? USE_SNAP_MOD = null;
		static internal bool? USE_RIGHT_CLICK_MENU_MOD = null;
		static internal bool? USE_PROJECT_GUI_EXTENSIONS = null;

	}
}
