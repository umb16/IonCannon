using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace EMX.HierarchyPlugin.Editor.Mods
{

	class LastScenesHistoryModInstance
	{

		/*
					if (INT32_ACTIVE(newIds) && !SkipRemoveFix)
					{
						LastActiveScene = INT32_SCENE(newIds);
						if (adapter.MOI.des(LastActiveScene).GetHash3().Count == 0)
							adapter.MOI.des(LastActiveScene).GetHash3().Add(newIds);
						else adapter.MOI.des(LastActiveScene).GetHash3().Insert(0, newIds);
		LastIndex = 0;
					}

					if (adapter.MOI.des(LastActiveScene).GetHash3().Count > adapter.MAX20)
					{
						var scene = LastActiveScene;
						while (adapter.MOI.des(scene).GetHash3().Count > adapter.MAX20)
							adapter.MOI.des(scene).GetHash3().RemoveAt(adapter.MAX20);
}
*/

		static MemType cType = MemType.Scenes;

		PluginInstance adapter { get { return Root.p[0]; } }

		internal DrawButtonsOld dob = new DrawButtonsOld();

		internal void DoScenes(Rect line, ExternalDrawContainer controller, Scene scene)
		{
			var l = line;

			l.width -= ExternalModStyles.LINE_HEIGHT_FOR_BUTTONS(line.height);
			var plus = l;
			plus.x += plus.width - 2;
			plus.width = ExternalModStyles.LINE_HEIGHT_FOR_BUTTONS(line.height) + 2;
			if (Event.current.type == EventType.MouseDown && plus.Contains(Event.current.mousePosition))
			{
				var capturedPlus = plus;
				controller.selection_button = 12;
				controller.selection_window = controller.tempRoot;
				controller.selection_action = (mouseUp, deltaTIme) =>
				{
					if (mouseUp && capturedPlus.Contains(Event.current.mousePosition))
					{
						DrawButtonsOld.Add_Scenes(controller, scene);
					}

					return Event.current.delta.x == 0 && Event.current.delta.x == 0;
				}; // ACTION
			}

			// plus.height -= 3;
			if (Event.current.type == EventType.Repaint)
			{
				adapter.STYLE_HIERSEL_BUTTON.Draw(plus, ExternalModStyles.plusContent, false, false, false,
					plus.Contains(Event.current.mousePosition) && controller.selection_button == 12);
				/*  Adapter.GET_SKIN().button.Draw( plus , plusContent , plus.Contains( Event.current.mousePosition ) , false , false ,
                                              plus.Contains( Event.current.mousePosition ) && controller.selection_button == 12 );*/
			}

			GUI.Label(plus, ExternalModStyles.plusContentSceneLabel);
			EditorGUIUtility.AddCursorRect(plus, MouseCursor.Link);


			/* l.width -= LINE_HEIGHT( controller.IS_MAIN ) - 2;
             l.x += LINE_HEIGHT( controller.IS_MAIN ) - 2;*/


			//refStyle = EditorStyles.toolbarButton;
			//refColor = Color.white;
			//   wasSceneDraw = false;
			if (Event.current.type == EventType.Repaint)
				EditorStyles.helpBox.Draw(l, /* new GUIContent(""),*/ false, false, false, false);
			// line.x = 0;
			/*if (! DrawButtons(l, LH, cType, WHITE, controller, scene))
			{
				var tooltip = GETTOOLTIPPEDCONTENT(cType, null, controller);
				tooltip.text = "";
				Label(l, tooltip);
			}*/

			line.width -= plus.width;

			var rowParams = DrawButtonsOld.GET_DISPLAY_PARAMS(cType);

			if (!dob.DrawButtons(line, cType, rowParams.BgOverrideColor.a != 0 ? Color.Lerp(Color.white, rowParams.BgOverrideColor, rowParams.BgOverrideColor.a) : Color.white	, controller, scene))
			{
				var tooltip = DrawButtonsOld.GETTOOLTIPPEDCONTENT(cType, null, controller);
				tooltip.text = "";
				GUI.Label(line, tooltip);
				GUI.Label(l, "- - -", adapter.STYLE_LABEL_10_middle);
			}
		}





	}
}
