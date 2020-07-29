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

namespace EMX.HierarchyPlugin.Editor.Mods
{

	class GameObjectsSelectionHistoryModWindow : ExternalModRoot, IHasCustomMenu
	{

		LastSelectionHistoryModWindowInstance __instance;
		internal LastSelectionHistoryModWindowInstance instance { get { return __instance ?? (__instance = new LastSelectionHistoryModWindowInstance()); } }
		const string NAME = "GameObjects Selection History Mod";
		const int priority = 5;
		static MemType cType = MemType.Last;

		internal static void SubscribeButtonsAndMenu(EditorSubscriber sbs)
		{

			if (!Root.p[0].par_e.USE_LAST_SELECTION_MOD) return;

			sbs.ExternalMod_Buttons.Add(new ExternalMod_Button(typeof(GameObjectsSelectionHistoryModWindow))
			{
				text = NAME,
				icon = () => "LAST_SELECTION_ICON",
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

			sbs.OnSelectionChanged -= LastSelectionHistoryModWindowInstance.OnSelectionChangeStatic;
			sbs.OnSelectionChanged += LastSelectionHistoryModWindowInstance.OnSelectionChangeStatic;
			sbs.OnSelectionChanged -= DrawButtonsOld.OnSelectionChangeStatic;
			sbs.OnSelectionChanged += DrawButtonsOld.OnSelectionChangeStatic;


			//sbs.ExternalMod_MenuItems
		}


		void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
		{
			generate_menu(menu, NAME);
		}




		internal static Type[] bindTypes = { typeof(BookObject.BookmarksforGameObjectsModWindow) , typeof(GameObjectsSelectionHistoryModWindow) ,
			typeof(ScenesHistoryModWindow), typeof(HierarchyExpandedMemWindow)
			};
		static void ICON_CLICK(int button, string name)
		{
			if (button == 0)
			{
				//controller = ;
				//if (W.minSize.x < 40 || W.minSize.y < 16) {W.minSize = new Vector2(40, 16); }
				//	W.ShowTab();
				//var W = Root.p[0].par_e.ATTACH_TO_INSPECT_ONOPEN ? LastScenesHistoryModWindow.GetWindow<LastScenesHistoryModWindow>(name, true, InspectorType) : LastScenesHistoryModWindow.GetWindow<LastScenesHistoryModWindow>(name, true);

				GetExternalWindow<GameObjectsSelectionHistoryModWindow>.Show(name, bindTypes);
			}
			if (button == 1)
			{
				var menu = new GenericMenu();
				menu.AddItem(new GUIContent("Open " + NAME), false, () =>
				{
					GetExternalWindow<GameObjectsSelectionHistoryModWindow>.Show(name, bindTypes);
				});
				menu.AddSeparator("");
				generate_menu(menu, name);
				menu.ShowAsContext();
			}
		}

		static void generate_menu(GenericMenu menu, string name)
		{
			DrawButtonsOld.SET_LAST(menu, lastController ?? new ExternalDrawContainer() { type = cType }, EditorSceneManager.GetActiveScene());
			menu.AddSeparator("");
			menu.AddItem(new GUIContent("Open " + NAME + " Settings"), false, () =>
			{
				Settings.MainSettingsEnabler_Window.Select<Settings.LO_Window>();
			});
		}
		static ExternalDrawContainer lastController;
		//static Type[] lastTypes;
		internal override void SubscribeEditorInstance(EditorSubscriber sbs)
		{

			if (!Root.p[0].par_e.USE_LAST_SELECTION_MOD) return;



			/*	sbs.OnSceneOpening += instance.SCENE_CHANGE;
				sbs.OnSelectionChanged += instance.CHANGE_SELECTION;
				sbs.OnPlayModeStateChanged += instance.CHANGEPLAYMODE;
				sbs.OnUpdate += instance.Update;*/
		}
		// ExternalDrawContainer controller = new ExternalDrawContainer();


		internal override void OnGUI_Draw()
		{


			if (!Root.p[0].par_e.USE_LAST_SELECTION_MOD)
			{
				Close();
				return;
			}

			lastController = controller;
			adapter.ChangeGUI();
			controller.type = cType;
			controller.tempRoot = this;
			instance.DoLast(new Rect(0, 0, position.width, position.height), controller, adapter.LastActiveScene);
			adapter.RestoreGUI();

		}
	}
}
