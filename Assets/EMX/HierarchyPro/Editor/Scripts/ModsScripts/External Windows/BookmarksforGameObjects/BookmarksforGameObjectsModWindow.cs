using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEditor.SceneManagement;

namespace EMX.HierarchyPlugin.Editor.Mods.BookObject
{

	class BookmarksforGameObjectsModWindow : ExternalModRoot, IHasCustomMenu
	{
		BookmarksforGameObjectsModInstance __instance;
		internal BookmarksforGameObjectsModInstance instance { get { return __instance ?? (__instance = new BookmarksforGameObjectsModInstance()); } }
		internal const string NAME = "Bookmarks Mod";
		const int priority = 0;
		static MemType cType = MemType.Custom;
		internal static void SubscribeButtonsAndMenu(EditorSubscriber sbs)
		{
			if (!Root.p[0].par_e.USE_BOOKMARKS_HIERARCHY_MOD) return;


			sbs.ExternalMod_Buttons.Add(new ExternalMod_Button(typeof(BookmarksforGameObjectsModWindow))
			{
				text = NAME,
				icon = () => "BOOKMARKS_ICON",
				priority = priority,
				release = ICON_CLICK
			});

			sbs.ExternalMod_MenuItems.Add(new ExternalMod_MenuItem()
			{
				text = NAME,
				path = "Open " + NAME,
				priority = priority,
				release = ICON_CLICK
			});
			//sbs.ExternalMod_MenuItems
		}

		void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
		{
			generate_menu(menu, NAME);
		}

		public static void OpenWindow()
		{
			GetExternalWindow<BookmarksforGameObjectsModWindow>.TryToClose_Or_Show(NAME);
		}

		static void ICON_CLICK(int button, string name)
		{
			if (button == 0)
			{
				//controller = ;
				//if (W.minSize.x < 40 || W.minSize.y < 16) {W.minSize = new Vector2(40, 16); }
				//	W.ShowTab();
				//var W = Root.p[0].par_e.ATTACH_TO_INSPECT_ONOPEN ? LastScenesHistoryModWindow.GetWindow<LastScenesHistoryModWindow>(name, true, InspectorType) : LastScenesHistoryModWindow.GetWindow<LastScenesHistoryModWindow>(name, true);
				GetExternalWindow<BookmarksforGameObjectsModWindow>.Show(NAME);
			}
			if (button == 1)
			{
				var menu = new GenericMenu();
				menu.AddItem(new GUIContent("Open " + NAME), false, () =>
				{
					GetExternalWindow<BookmarksforGameObjectsModWindow>.Show(name);
				});
				menu.AddSeparator("");
				generate_menu(menu, NAME);
				menu.ShowAsContext();
			}
		}
		static void generate_menu(GenericMenu menu, string name)
		{
			DrawButtonsOld.SET_BOOK(menu, lastController ?? (lastController = new ExternalDrawContainer() { type = cType }), EditorSceneManager.GetActiveScene(),
				GetStaticInstance);
			menu.AddSeparator("");
			menu.AddItem(new GUIContent("Open " + NAME + " Settings"), false, () =>
			{
				Settings.MainSettingsEnabler_Window.Select<Settings.BO_Window>();
			});
		}
		//static Type[] lastTypes;
		internal override void SubscribeEditorInstance(EditorSubscriber sbs)
		{

			if (!Root.p[0].par_e.USE_BOOKMARKS_HIERARCHY_MOD) return;

			/*	sbs.OnSceneOpening += instance.SCENE_CHANGE;
				sbs.OnSelectionChanged += instance.CHANGE_SELECTION;
				sbs.OnPlayModeStateChanged += instance.CHANGEPLAYMODE;
				sbs.OnUpdate += instance.Update;*/
		}



		internal override void OnEnable()
		{
			base.OnEnable();
			if (Root.p == null || Root.p.Length == 0 || Root.p[0] == null) return;
			controller.CHECK_CONTENT(controller.GetCurerentCategoryName(adapter.LastActiveScene));
		}


		internal static BookmarksforGameObjectsModInstance GetStaticInstance { get { return lastInstance ?? (lastInstance = new BookmarksforGameObjectsModInstance()); } }

		static BookmarksforGameObjectsModInstance lastInstance;
		static ExternalDrawContainer lastController;

		internal override void OnGUI_Draw()
		{

			if (!Root.p[0].par_e.USE_BOOKMARKS_HIERARCHY_MOD)
			{
				Close();
				return;
			}

			lastInstance = instance;
			lastController = controller;
			adapter.ChangeGUI();
			controller.type = cType;
			controller.tempRoot = this;
			controller.CHECK_CONTENT(controller.GetCurerentCategoryName(adapter.LastActiveScene));
			instance.DoCustom(new Rect(0, 0, position.width, position.height), controller, adapter.LastActiveScene);
			adapter.RestoreGUI();
		}
	}
}
