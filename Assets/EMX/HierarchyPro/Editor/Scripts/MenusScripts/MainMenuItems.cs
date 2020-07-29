
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EMX.HierarchyPlugin.Editor
{
	class MainMenuItems
	{
		// removing chars like %#z or %#y will disable hotkeys
		//---------------------------------------------//
		const string SETTINGS = MENU_PATH + "Settings";
		//---------------------------------------------//
		const string SEL_BACK = MENU_PATH + "Selection Backward &%#z";
		const string SEL_FORW = MENU_PATH + "Selection Forward &%#y";
		//---------------------------------------------//
		const string FRZ_TOGGLE = MENU_PATH + "Lock(Unlock) GameObject &#l";
		const string FRZ_UNLOCKALL = MENU_PATH + "Unlock All &%#l";
		//---------------------------------------------//
		const string EXT_HYPERGRAPH = MENU_PATH + "Open HyperGrapg Mod &%#x";
		const string EXT_BOOKMARKS = MENU_PATH + "Open BookMarks Mod &%#n";
		const string EXT_PROJECTFOLDERS = MENU_PATH + "Open Project Folders Mod &%#m";
		//---------------------------------------------//



		const string MENU_PATH = "Window/" + Root.PN + "/";
		internal const int P = 10000;
		static PluginInstance adapter { get { return Root.p[0]; } }

		[MenuItem(SETTINGS, true, P + 3)]
		static bool OpenSettings_IsValid() { return true; }
		[MenuItem(SETTINGS, false, P + 3)]
		static void OpenSettings() { Settings.MainSettingsEnabler_Window.Select<Settings.MainSettingsEnabler_Window>(); }

		//---------------------------------------------//

		[MenuItem(SEL_BACK, true, P + 16)]
		public static bool MoveSelPrev_IsValid() { return adapter.par_e.ENABLE_ALL && !adapter.par_e.USE_LAST_SELECTION_MOD; }
		[MenuItem(SEL_BACK, false, P + 16)]
		public static void MoveSelPrev() { Mods.LastSelectionHistoryModWindowInstance.MoveSelect(+1); }
		[MenuItem(SEL_FORW, true, P + 17)]
		public static bool MoveSelNext_IsValid() { return adapter.par_e.ENABLE_ALL && !adapter.par_e.USE_LAST_SELECTION_MOD; }
		[MenuItem(SEL_FORW, false, P + 17)]
		public static void MoveSelNext() { Mods.LastSelectionHistoryModWindowInstance.MoveSelect(-1); }

		//---------------------------------------------//

		[MenuItem(FRZ_TOGGLE, true, P + 85)]
		public static bool ToggleLock_IsValid() { return Mods.Mod_Freeze.IsValid(); }
		[MenuItem(FRZ_TOGGLE, false, P + 85)]
		public static void ToggleLock() { Mods.Mod_Freeze.ToggleFreeze(); }
		[MenuItem(FRZ_UNLOCKALL, true, P + 89)]
		public static bool UnlockAll_IsValid() { return Mods.Mod_Freeze.IsValid(); }
		[MenuItem(FRZ_UNLOCKALL, false, P + 89)]
		public static void UnlockAll() { Mods.Mod_Freeze.UnclockAll(); }

		//---------------------------------------------//



/*
		[MenuItem(MENU_PATH + "Welcome Screen", false, P + 129)]
		public static void OpenWelcomeScreen() { WelcomeScreen.Init(null); }
		*/




		[MenuItem(EXT_HYPERGRAPH, true, P + 185)]
		public static bool HyperGraph_IsValid() { return adapter.par_e.ENABLE_ALL; }
		[MenuItem(EXT_HYPERGRAPH, false, P + 185)]
		public static void HyperGraph() { Mods.HyperGraph.HyperGraphModWindow.OpenWindow(); }


		[MenuItem(EXT_BOOKMARKS, true, P + 186)]
		public static bool OpenBookmark_IsValid() { return adapter.par_e.ENABLE_ALL && adapter.par_e.USE_BOOKMARKS_HIERARCHY_MOD; }
		[MenuItem(EXT_BOOKMARKS, false, P + 186)]
		public static void OpenBookmark() { EMX.HierarchyPlugin.Editor.Mods.BookObject.BookmarksforGameObjectsModWindow.OpenWindow(); }


		[MenuItem(EXT_PROJECTFOLDERS, true, P + 187)]
		public static bool ProjectFolders_IsValid() { return adapter.par_e.ENABLE_ALL && adapter.par_e.USE_BOOKMARKS_PROJECT_MOD; }
		[MenuItem(EXT_PROJECTFOLDERS, false, P + 187)]
		public static void ProjectFolders() { Mods.BookProject.BookmarksforProjectviewModWindow.OpenWindow(); }




	}
}

