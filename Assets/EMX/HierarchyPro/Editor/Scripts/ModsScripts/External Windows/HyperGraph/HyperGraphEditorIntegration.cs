using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;



namespace EMX.HierarchyPlugin.Editor.Mods.HyperGraph
{
	partial class HyperGraphModInstance 
	{





		internal void CHANGEPLAYMODE()
		{
			StoptBroadcasting();
			StartBroadcasting();
			SCENE_CHANGE();
		}


		internal void ON_SCROLL(float sc)
		{
			//if (type == ScrollType.HyperGraphScroll)
			{
				if (sc < 0) SET_SCALE(Mathf.CeilToInt(adapter.par_e.HYPERGRAPH_SCALE * 10 + 0.001f) / 10f);
				else SET_SCALE(Mathf.FloorToInt(adapter.par_e.HYPERGRAPH_SCALE * 10 - 0.001f) / 10f);

				Tools.EventUse();
			}
			/*
						else if (type == ScrollType.HyperGraphScroll_Window)
						{
							if (sc < 0) SET_SCALE(ScrollType.HyperGraphScroll_Window, Mathf.CeilToInt(adapter.par.HiperGraphParams.WINDIOW_SCALE * 10 + 0.001f) / 10f);
							else SET_SCALE(ScrollType.HyperGraphScroll_Window, Mathf.FloorToInt(adapter.par.HiperGraphParams.WINDIOW_SCALE * 10 - 0.001f) / 10f);

							EventUse();
						}*/
		}


		/*	void OnDestroy()
			{

			}*/


		internal void SCENE_CHANGE()
		{
			CHANGE_SELECTION_OVVERIDE(true);
			CurrentSelection = null;
			StoptBroadcasting();
			RepaintNow();
		}

		internal void CHANGE_SELECTION()
		{
			if (!adapter.par_e.HYPERGRAPH_AUTOCHANGE) return;

			CHANGE_SELECTION_OVVERIDE();
			RepaintNow();
		}


		bool OBJECT_ISVALID(UnityEngine.Object o)
		{
			//if (adapter.IS_HIERARCHY()) 
			return o is GameObject && ((GameObject)o).scene.IsValid() && ((GameObject)o).scene.isLoaded;
			//else return o && !string.IsNullOrEmpty(adapter.bottomInterface.INSTANCEID_TOGUID(o.GetInstanceID()));
		}


		public void CHANGE_SELECTION_OVVERIDE(bool skipAutoHide = false, UnityEngine.Object selection = null)
		{
#if UNITY_EDITOR
			// MonoBehaviour.print("CHANGE_SELECTION_OVVERIDE");
#endif
			if (CURRENT_CONTROLLER == null) return;
			WASDRAW = false;
			CURRENT_CONTROLLER.wasInit = false;
			//adapter.bottomInterface.hyperGraph.WindowHyperController.wasInit = false;
			var active = selection ?? Selection.activeObject;
			UnityEngine.Object newSelection = null;


			if (active && OBJECT_ISVALID(active))
			{
				newSelection = active;
			}

			if (!skipAutoHide && adapter.par_e.HYPERGRAPH_AUTOHIDE && CurrentSelection != newSelection)
			{
				SWITCH_ACTIVE_SCAN(false);
				return;
			}

			if (CurrentSelection != newSelection)
			{
				if (newSelection)
				{
					comps = null;
					CurrentSelection = newSelection;

					if (!skipUndo) SETUNDO();

					skipUndo = false;
					StoptBroadcasting();
					StartBroadcasting();
				}

				else //StoptBroadcasting();
				{ }
			}
			RepaintNow();
		}










		private void INITIALIZE(ExternalDrawContainer controller)
		{
			if (controller.wasInit) return;

			// currentController = controller;




			//	adapter.OnScroll -= ON_SCROLL;
			//	adapter.OnScroll += ON_SCROLL;


			var v1 = controller.wasInit;
			//	var v2 = adapter.bottomInterface.hyperGraph.WindowHyperController.wasInit;

			if (CurrentSelection == null) CHANGE_SELECTION_OVVERIDE(false);

			controller.wasInit = v1;
			//adapter.bottomInterface.hyperGraph.WindowHyperController.wasInit = v2;
			controller.wasInit = true;

			controller.scrollPos.x = RECT.width / 2;
			var h = DRAWOBJECT(controller, true);
			controller.scrollPos.y = Math.Max((controller.HEIGHT) / 2 - h / 2, 10);
		}



		private void REFRESH()
		{
			CurrentSelection = null;
			CHANGE_SELECTION_OVVERIDE(true);
			RepaintNow();
		}

		internal void RefreshWithCurrentSelection()
		{
			comps = null;
			CHANGE_SELECTION_OVVERIDE(true, CurrentSelection);
			RepaintNow();
		}

	}
}
