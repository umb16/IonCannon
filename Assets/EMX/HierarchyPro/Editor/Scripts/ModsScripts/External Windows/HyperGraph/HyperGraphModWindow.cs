using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEngine.UIElements;

namespace EMX.HierarchyPlugin.Editor.Mods.HyperGraph
{



	// internal  Action OnGUIV;
	/*	HyperGraphWindow hyperwindow
		{
			get { return hyperGraph.editorWindow as HyperGraphModWindow; }
			set { hyperGraph.editorWindow = value; }

		}*/

	/*internal static void ShowW(Adapter adapter)
{
	foreach (var w in Resources.FindObjectsOfTypeAll<_6__BottomWindow_HyperGraphWindow>().Where(w => w.adapter == adapter)) w.Close();
	var wd = EditorWindow.GetWindow<_6__BottomWindow_HyperGraphWindow>("HyperGraph - " + adapter.pluginname);
	wd.adapter = adapter;
	hyperGraph.editorWindow = wd;

	wd.ShowAuxWindow();
	wd.wasInit = false;
}*/

	public partial class HyperGraphModWindow : ExternalModRoot, IHasCustomMenu
	{


		HyperGraphModInstance __instance;
		internal HyperGraphModInstance instance { get { return __instance ?? (__instance = new HyperGraphModInstance()); } }
		const string NAME = "HyperGraph";
		const int priority = 0;
		internal static void SubscribeButtonsAndMenu(EditorSubscriber sbs)
		{
			sbs.ExternalMod_Buttons.Add(new ExternalMod_Button(typeof(HyperGraphModWindow))
			{
				text = NAME,
				icon = () => "HYPER_ICON",
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
		public static void OpenWindow()
		{
			GetExternalWindow<HyperGraphModWindow>.TryToClose_Or_Show(NAME);
		}
		static void ICON_CLICK(int button, string name)
		{
			if (button == 0)
			{
				//controller = ;
				//if (W.minSize.x < 40 || W.minSize.y < 16) {W.minSize = new Vector2(40, 16); }
				//	W.ShowTab();
				var W = Root.p[0].par_e.ATTACH_TO_INSPECT_ONOPEN ? HyperGraphModWindow.GetWindow<HyperGraphModWindow>(name, true, InspectorType) : HyperGraphModWindow.GetWindow<HyperGraphModWindow>(name, true);
				W.Show();
				W.Init();
			}
			if (button == 1)
			{
				var menu = new GenericMenu();
				menu.AddItem(new GUIContent("Open HyperGraph"), false, () =>
				{
					GetExternalWindow<HyperGraphModWindow>.Show(name);
				});
				menu.AddItem(new GUIContent("Open HyperGraph and attach to Inspector"), false, () =>
				{
					GetExternalWindow<HyperGraphModWindow>.Show(name, InspectorType);
					/*var W = HyperGraphModWindow.GetWindow<HyperGraphModWindow>(name, true, InspectorType);
					W.Show();
					W.Init();*/
				});
				menu.AddSeparator("");
				generate_menu(menu, name);
				menu.ShowAsContext();
			}
		}

		static void generate_menu(GenericMenu menu, string name)
		{
			menu.AddItem(new GUIContent("UnityEvents Mode"), Root.p[0].par_e.HYPERGRAPH_EVENTS_MODE_BOOL, () =>
			{
				Root.p[0].par_e.HYPERGRAPH_EVENTS_MODE_BOOL = !Root.p[0].par_e.HYPERGRAPH_EVENTS_MODE_BOOL;
			});
			menu.AddSeparator("");
			menu.AddItem(new GUIContent("Auto refresh when selection changed"), Root.p[0].par_e.HYPERGRAPH_AUTOCHANGE, () =>
			{
				Root.p[0].par_e.HYPERGRAPH_AUTOCHANGE = !Root.p[0].par_e.HYPERGRAPH_AUTOCHANGE;
			});
			menu.AddItem(new GUIContent("Display loading indicator"), Root.p[0].par_e.HYPERGRAPH_SHOWUPDATINGINDICATOR, () =>
			{
				Root.p[0].par_e.HYPERGRAPH_SHOWUPDATINGINDICATOR = !Root.p[0].par_e.HYPERGRAPH_SHOWUPDATINGINDICATOR;
			});
			menu.AddItem(new GUIContent("Red null references"), Root.p[0].par_e.HYPERGRAPH_RED_HIGKLIGHTING, () =>
			{
				Root.p[0].par_e.HYPERGRAPH_RED_HIGKLIGHTING = !Root.p[0].par_e.HYPERGRAPH_RED_HIGKLIGHTING;
			});
			menu.AddSeparator("");
			menu.AddItem(new GUIContent("Open HyperGraph Settings"), false, () =>
			{
				Settings.MainSettingsEnabler_Window.Select<Settings.HG_Window>();
			});
		}
		//static Type[] lastTypes;
		internal override void SubscribeEditorInstance(EditorSubscriber sbs)
		{
			sbs.OnUpdate += FocusDetector;
			FocusDetector();
			if (lastFocus == false) return;
			sbs.OnSceneOpening += instance.SCENE_CHANGE;
			sbs.OnSelectionChanged += instance.CHANGE_SELECTION;
			sbs.OnPlayModeStateChanged += instance.CHANGEPLAYMODE;
			sbs.OnUpdate += instance.Update;
		}

		bool? lastFocus;
		//	float? oldH, oldW;
		//static VisualElement lastTypes;

		void FocusDetector()
		{

			var enabled = rootVisualElement.parent != null;
			if (!lastFocus.HasValue)
			{
				lastFocus = enabled;
			}
			if (lastFocus != enabled)
			{
				lastFocus = enabled;
				adapter.modsController.REBUILD_PLUGINS(true);
				instance.SCENE_CHANGE();
			}

			/*	if (enabled && (oldH != position.height || oldW != position.width))
				{
					oldH = position.height;
					oldW = position.width;
					lastTypes = rootVisualElement.parent;
					//Debug.Log(lastTypes.Length);
				}*/

			//	Debug.Log(rootVisualElement);
			//	Debug.Log();
			//	Debug.Log(rootVisualElement.parent.enabledInHierarchy);
			//	Debug.Log(rootVisualElement.parent.childCount);
			//	Debug.Log(rootVisualElement.panel.visualTree.childCount);
			//	Debug.Log(rootVisualElement.panel.focusController.focusedElement.focusable);
		}



		float? oldHeight;
		float? oldWidth;

		bool mayscroll;
		//	HyperGraphMod hyperGraph { get { return adapter.modsController.ex_HyperGraphMod; } }


		internal override void OnGUI_Draw()
		{
			/*	if (lastTypes != null)
		{
			rootVisualElement.RemoveFromHierarchy();
			lastTypes.Add(rootVisualElement);
			lastTypes.MarkDirtyRepaint();
			lastTypes = null;
		}*/

			if (WAS_INIT)
			{
				instance.CHECK_SCAN();
			}


			if (Event.current.type == EventType.ScrollWheel && new Rect(0, 0, position.width, position.height).Contains(Event.current.mousePosition))
			{
				if (mayscroll)
				{
					//if (adapter.OnScroll != null) adapter.OnScroll(Adapter.ScrollType.HyperGraphScroll_Window, Event.current.delta.y);
					instance.ON_SCROLL(Event.current.delta.y);
					mayscroll = false;
				}
			}

			if (Event.current.type == EventType.Repaint)
			{
				mayscroll = true;
			}


			if (!oldHeight.HasValue) oldHeight = position.height;
			if (oldHeight.Value != position.height)
			{
				var oldH = oldHeight.Value;
				oldHeight = position.height;
				//  controller.HEIGHT = (startHeight + (startPos.y - p.y));
				//  CHECK_HEIGHT();
				controller.scrollPos.y -= (oldH - controller.HEIGHT) / 2;
				// Hierarchy.BottomInterface.HyperGraph.RESET_SMOOTH_HEIGHT();
			}


			if (!oldWidth.HasValue) oldWidth = position.width;
			if (oldWidth.Value != position.width)
			{
				var oldW = oldWidth.Value;
				oldWidth = position.width;
				//  controller.HEIGHT = (startHeight + (startPos.y - p.y));
				//  CHECK_HEIGHT();
				controller.scrollPos.x -= (oldW - controller.WIDTH) / 2;
				// Hierarchy.BottomInterface.HyperGraph.RESET_SMOOTH_HEIGHT();
			}




			/*	if (!wasInit)
				{
					if (Event.current.type == EventType.Repaint)
					{ // MonoBehaviour.print(hyperwindow.position);
						if (position.x < 15 && position.y < 50)
						{
							var p = position;
							p.x = WinBounds.MAX_WINDOW_WIDTH.x + (WinBounds.MAX_WINDOW_WIDTH.y - p.width) / 2;
							p.y = WinBounds.MAX_WINDOW_HEIGHT.x + (WinBounds.MAX_WINDOW_HEIGHT.y - p.height) / 2;
							position = p;
						}

						wasInit = true;
					}

					// return;
				}*/

			adapter.ChangeGUI();

			instance.EXTERNAL_HYPER_DRAWER(new Rect(0, 0, position.width, position.height), controller, this);

			adapter.RestoreGUI();



		}

	
	}
}
