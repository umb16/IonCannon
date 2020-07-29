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

	class ScenesHistoryModWindow : ExternalModRoot
	{


		LastScenesHistoryModInstance __instance;
		internal LastScenesHistoryModInstance instance { get { return __instance ?? (__instance = new LastScenesHistoryModInstance()); } }
		const string NAME = "Scenes History Mod";
		const int priority = 10;
		static MemType cType = MemType.Scenes;
		internal static void SubscribeButtonsAndMenu(EditorSubscriber sbs)
		{

			if (!Root.p[0].par_e.USE_LAST_SCENES_MOD) return;

			sbs.ExternalMod_Buttons.Add(new ExternalMod_Button(typeof(ScenesHistoryModWindow))
			{
				text = NAME,
				icon = () => "LAST_SCENES_ICON",
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

			sbs.OnSceneOpening -= __AddSceneButton_OnSceneChanging;
			sbs.OnSceneOpening += __AddSceneButton_OnSceneChanging;
			//sbs.ExternalMod_MenuItems
		}


		static void __AddSceneButton_OnSceneChanging()
		{
			AddSceneButton_OnSceneChanging(false);
		}
		//	PluginInstance adapter { get { return Root.p[0]; } }
		static string[] lastScenesSelected = new string[0];
		internal static void AddSceneButton_OnSceneChanging(bool skipPlay = false)
		{
			if (Application.isPlaying && !skipPlay) return;


			//	if (_scene == -1) _scene = SceneManager.GetActiveScene().GetHashCode();
			//	var GUID = AssetDatabase.AssetPathToGUID(Adapter.GET_SCENE_BY_ID(_scene).path);
			Root.p[0].try_to_detect_scene_changing();

			if (PluginInstance.LastActiveScenesList_Guids.Length == 0) return;


			bool skip = true;
			if (lastScenesSelected.Length != PluginInstance.LastActiveScenesList_Guids.Length)
			{
				skip = false;
				Array.Resize(ref lastScenesSelected, PluginInstance.LastActiveScenesList_Guids.Length);
			}
			for (int i = 0; i < lastScenesSelected.Length; i++)
			{
				if (lastScenesSelected[i] != PluginInstance.LastActiveScenesList_Guids[i])
				{
					skip = false;
					lastScenesSelected[i] = PluginInstance.LastActiveScenesList_Guids[i];
				}
			}

			if (skip) return;

			//	if (!string.IsNullOrEmpty(GUID))
			{
				//	var newScene = new SceneId(GUID, (additiona_scenes ?? new Scene[0]).Select(s => AssetDatabase.AssetPathToGUID(s.path)).Where(guid => !string.IsNullOrEmpty(guid)).ToArray());
				var newScene = new EMX.HierarchyPlugin.Editor.Mods.DrawButtonsOld.SceneMemory(new ScenesTab_Saved()
				{
					guid = PluginInstance.LastActiveScenesList_Guids,
					path = PluginInstance.LastActiveScenesList_Guids.Select(AssetDatabase.GUIDToAssetPath).ToArray(),
					pin = false
				}, -1);
				var lastScenes = DrawButtonsOld.GET_OBJECTS_LIST(cType, null, EditorSceneManager.GetActiveScene());
				var row_param = DrawButtonsOld.GET_DISPLAY_PARAMS(cType);

				var pinned = lastScenes.Take(row_param.MaxItems).Select((s, i) => new { scene = s, index = i })
				.Where(r => r.scene.pin)
				.OrderBy(r => r.index)
				.ToArray();

				var haveScene = pinned.FirstOrDefault(s => s.scene.additional_GUID[0] == newScene.additional_GUID[0] && s.scene.unique_id == newScene.unique_id);
				if (haveScene != null && haveScene.scene != null && haveScene.scene.pin) return;

				var max = pinned.Length == 0 ? -1 : pinned.Max(p => p.index);
				for (int i = max; i >= 0; i--)
				{
					if (lastScenes[i].pin) lastScenes.RemoveAt(i);
				}
				//lastScenes.RemoveAll(s => s.pin);

				lastScenes.RemoveAll(s => s.additional_GUID[0] == newScene.additional_GUID[0] && s.unique_id == newScene.unique_id);

				if (lastScenes.Count == 0) lastScenes.Add(newScene);
				else lastScenes.Insert(0, newScene);

				for (int i = 0; i < pinned.Length; i++)
				{
					if (lastScenes.Count == pinned[i].index) lastScenes.Add(pinned[i].scene);
					else lastScenes.Insert(pinned[i].index, pinned[i].scene);
				}
				while (lastScenes.Count > 20) lastScenes.RemoveAt(20);

				DrawButtonsOld.SET_OBJECTS_LIST(lastScenes, cType, null, EditorSceneManager.GetActiveScene());
			}



		}
		static void ICON_CLICK(int button, string name)
		{
			if (button == 0)
			{
				//controller = ;
				//if (W.minSize.x < 40 || W.minSize.y < 16) {W.minSize = new Vector2(40, 16); }
				//	W.ShowTab();
				//var W = Root.p[0].par_e.ATTACH_TO_INSPECT_ONOPEN ? LastScenesHistoryModWindow.GetWindow<LastScenesHistoryModWindow>(name, true, InspectorType) : LastScenesHistoryModWindow.GetWindow<LastScenesHistoryModWindow>(name, true);
				/*var W = LastScenesHistoryModWindow.GetWindow<LastScenesHistoryModWindow>(name, true);
				W.Show();
				W.Init();*/
				GetExternalWindow<ScenesHistoryModWindow>.Show(name, GameObjectsSelectionHistoryModWindow.bindTypes);
			}
			if (button == 1)
			{
				var menu = new GenericMenu();
				menu.AddItem(new GUIContent("Open " + NAME), false, () =>
				{
					GetExternalWindow<ScenesHistoryModWindow>.Show(name, GameObjectsSelectionHistoryModWindow.bindTypes);
					/*var W = LastScenesHistoryModWindow.GetWindow<LastScenesHistoryModWindow>(name, true);
					W.Show();
					W.Init();*/
				});
				menu.AddSeparator("");
				DrawButtonsOld.SET_SCEN(menu, lastController ?? new ExternalDrawContainer() { type = cType }, EditorSceneManager.GetActiveScene());
				menu.AddSeparator("");
				menu.AddItem(new GUIContent("Open " + NAME + " Settings"), false, () =>
				{
					Settings.MainSettingsEnabler_Window.Select<Settings.LS_Window>();
				});
				menu.ShowAsContext();
			}
		}
		//static Type[] lastTypes;
		static ExternalDrawContainer lastController;
		internal override void SubscribeEditorInstance(EditorSubscriber sbs)
		{

			if (!Root.p[0].par_e.USE_LAST_SCENES_MOD) return;

			/*	sbs.OnSceneOpening += instance.SCENE_CHANGE;
				sbs.OnSelectionChanged += instance.CHANGE_SELECTION;
				sbs.OnPlayModeStateChanged += instance.CHANGEPLAYMODE;
				sbs.OnUpdate += instance.Update;*/
		}

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
			instance.DoScenes(new Rect(0, 0, position.width, position.height), controller, adapter.LastActiveScene);
			adapter.RestoreGUI();
		}
	}
}
