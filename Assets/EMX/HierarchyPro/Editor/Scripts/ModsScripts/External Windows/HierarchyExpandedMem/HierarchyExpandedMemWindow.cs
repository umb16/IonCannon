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

	class HierarchyExpandedMemWindow : ExternalModRoot, IHasCustomMenu
	{


		HierarchyExpandedMemInstance __instance;
		internal HierarchyExpandedMemInstance instance { get { return __instance ?? (__instance = new HierarchyExpandedMemInstance()); } }
		const string NAME = "Hierarchy Expanded Mem Mod";
		const int priority = 10;
		static MemType cType = MemType.Hier;
		internal static void SubscribeButtonsAndMenu(EditorSubscriber sbs)
		{

			if (!Root.p[0].par_e.USE_HIER_EXPANDED_MOD) return;

			sbs.ExternalMod_Buttons.Add(new ExternalMod_Button(typeof(HierarchyExpandedMemWindow))
			{
				text = NAME,
				icon = () => "HIER_EXPAND_ICON",
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
		public void AddItemsToMenu(GenericMenu menu)
		{
			generate_menu(menu, NAME);
		}
		static void ICON_CLICK(int button, string name)
		{
			if (button == 0)
			{
				//controller = ;
				//if (W.minSize.x < 40 || W.minSize.y < 16) {W.minSize = new Vector2(40, 16); }
				//	W.ShowTab();
				//var W = Root.p[0].par_e.ATTACH_TO_INSPECT_ONOPEN ? LastScenesHistoryModWindow.GetWindow<LastScenesHistoryModWindow>(name, true, InspectorType) : LastScenesHistoryModWindow.GetWindow<LastScenesHistoryModWindow>(name, true);

				GetExternalWindow<HierarchyExpandedMemWindow>.Show(name, GameObjectsSelectionHistoryModWindow.bindTypes);
			}
			if (button == 1)
			{
				var menu = new GenericMenu();
				menu.AddItem(new GUIContent("Open " + NAME), false, () =>
				{

					GetExternalWindow<HierarchyExpandedMemWindow>.Show(name, GameObjectsSelectionHistoryModWindow.bindTypes);
				});
				menu.AddSeparator("");
				generate_menu(menu, name);

				menu.ShowAsContext();
			}
		}

		static void generate_menu(GenericMenu menu, string name)
		{
			DrawButtonsOld.SET_HIER(menu, lastController ?? new ExternalDrawContainer() { type = cType }, EditorSceneManager.GetActiveScene());
			menu.AddSeparator("");
			menu.AddItem(new GUIContent("Open Expanded Mem Settings"), false, () =>
			{
				Settings.MainSettingsEnabler_Window.Select<Settings.HE_Window>();
			});
		}
		//static Type[] lastTypes;
		internal override void SubscribeEditorInstance(EditorSubscriber sbs)
		{

			if (!Root.p[0].par_e.USE_LAST_SCENES_MOD) return;

			/*	sbs.OnSceneOpening += instance.SCENE_CHANGE;
				sbs.OnSelectionChanged += instance.CHANGE_SELECTION;
				sbs.OnPlayModeStateChanged += instance.CHANGEPLAYMODE;
				sbs.OnUpdate += instance.Update;*/
		}

		static ExternalDrawContainer lastController;
		internal override void OnGUI_Draw()
		{
			if (!Root.p[0].par_e.USE_LAST_SCENES_MOD)
			{
				Close();
				return;
			}

			lastController = controller;
			adapter.ChangeGUI();
			controller.type = cType;
			controller.tempRoot = this;
			instance.DoHier(new Rect(0, 0, position.width, position.height), controller, adapter.LastActiveScene);
			adapter.RestoreGUI();
		}
	}
}
