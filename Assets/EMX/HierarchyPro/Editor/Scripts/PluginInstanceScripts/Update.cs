using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Linq;

namespace EMX.HierarchyPlugin.Editor
{


	internal partial class PluginInstance
	{

		internal float deltaTime = 0;
		internal double lastTime = 0;
		internal bool SKIP_PREFAB_ESCAPE;
		internal bool wereUndoForLockerMod = false;
		internal bool need_ClearHierarchyObjects1 = false;

		internal void RepaintWindowInUpdate_PlusResetStack()
		{
			_RepaintWindowInUpdate_PlusResetStack = true;
			hierarchyWindows.Clear();
			RepaintWindowInUpdate();
		}
		internal void RepaintWindowInUpdate()
		{
			_RepaintAfterUpadate = true;
		}
		bool _RepaintWindowInUpdate_PlusResetStack;
		bool _RepaintAfterUpadate;

		internal bool try_to_detect_scene_changing()
		{

			if (!Application.isPlaying)
			{
				if (EditorSceneManager.sceneCount != LastActiveScenesList_HashCode.Length) Array.Resize(ref LastActiveScenesList_HashCode, EditorSceneManager.sceneCount);
				if (EditorSceneManager.sceneCount != LastActiveScenesList_Guids.Length) Array.Resize(ref LastActiveScenesList_Guids, EditorSceneManager.sceneCount);
				bool sceneChanged = false;
				var ac = EditorSceneManager.GetActiveScene();
				for (int i = 0; i < LastActiveScenesList_HashCode.Length; i++)
				{
					if (LastActiveScenesList_HashCode[i] != (EditorSceneManager.GetSceneAt(i).GetHashCode() ^ EditorSceneManager.GetSceneAt(i).path.GetHashCode()))
					{
						sceneChanged = true;
						LastActiveScenesList_HashCode[i] = (EditorSceneManager.GetSceneAt(i).GetHashCode() ^ EditorSceneManager.GetSceneAt(i).path.GetHashCode());
						var p = EditorSceneManager.GetSceneAt(i).path;
						if (p != null && p != "") p = AssetDatabase.AssetPathToGUID(p);
						LastActiveScenesList_Guids[i] = p;
					}
				}


				if (sceneChanged)
				{
					int activeIndex = 0;
					for (int i = 0; i < LastActiveScenesList_HashCode.Length; i++)
					{
						if (EditorSceneManager.GetSceneAt(i).GetHashCode() == ac.GetHashCode()) activeIndex = i;
					}
					if (activeIndex != 0)
					{
						var a1 = LastActiveScenesList_Guids[activeIndex];
						var a2 = LastActiveScenesList_HashCode[activeIndex];
						LastActiveScenesList_Guids[activeIndex] = LastActiveScenesList_Guids[0];
						LastActiveScenesList_HashCode[activeIndex] = LastActiveScenesList_HashCode[0];
						LastActiveScenesList_Guids[0] = a1;
						LastActiveScenesList_HashCode[0] = a2;
					}
					return true;
				}
			}
			return false;
		}

		internal void Update()
		{

			if (_RepaintAfterUpadate) //Hierarchy.RepaintAllViews();
			{
				if (_RepaintWindowInUpdate_PlusResetStack)
				{
					_drawStack_reset_stacks();
					_RepaintWindowInUpdate_PlusResetStack = false;
				}
				RepaintWindow(true);
				_RepaintAfterUpadate = false;
			}

			if (_oneShotUpdate.Count != 0)
			{
				foreach (var item in _oneShotUpdate) item();
				_oneShotUpdate.Clear();
			}

			if (try_to_detect_scene_changing())
				invoke_SceneChanging();

			if (wasSceneMoved)
				invoke_SceneMoved();

			if (SKIP_PREFAB_ESCAPE) SKIP_PREFAB_ESCAPE = false;
			if (wereUndoForLockerMod) wereUndoForLockerMod = false;
			_deltatime();
			_clean_updater();

			if (par_e.TRACKING_COMPILE_TIME)
			{
				if (EditorApplication.isCompiling && !compileFlag)
				{
					compileFlag = true;
					PlayerPrefs.SetFloat(""+ Root.PN_NS + "/CompilationTime1", (float)(EditorApplication.timeSinceStartup % 1000));
					PlayerPrefs.SetFloat("" + Root.PN_NS + "/CompilationTime2", (float)((EditorApplication.timeSinceStartup + 500) % 1000));
				}
				else if (!EditorApplication.isCompiling && compileFlag)
				{
					compileFlag = false;
					var f1 = PlayerPrefs.GetFloat("" + Root.PN_NS + "/CompilationTime1");
					var f2 = PlayerPrefs.GetFloat("" + Root.PN_NS + "/CompilationTime2");
					var t1 = (float)(EditorApplication.timeSinceStartup % 1000);
					var t2 = (float)((EditorApplication.timeSinceStartup + 500) % 1000);
					float compileTime;
					if (t1 > f1) compileTime = t1 - f1;
					else compileTime = t2 - f2;
					Debug.Log("Compile: " + compileTime.ToString("F2") + "s");
				}
			}

			if (_OnUpdate != null) _OnUpdate();



			if (UNDO.wasUndo) UNDO.wasUndo = false;

		}




		void _deltatime()
		{
			if (lastTime == 0) lastTime = EditorApplication.timeSinceStartup;
			deltaTime = (float)(EditorApplication.timeSinceStartup - lastTime);
			lastTime = EditorApplication.timeSinceStartup;
			if (deltaTime > 0.4f) deltaTime = 0.4f;
		}
		void _clean_updater()
		{
			/*    if ( NEW_PERFOMANCE ) return;
                if ( need_ClearHierarchyObjects1) // lock (m_Handle)
            {
                    need_ClearHierarchyObjects1 = false;
                    if ( !EditorApplication.isCompiling )
                    {
                        ClearHierarchyObjects( true );
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }
                }*/
		}

		internal void RepaintAllViews()
		{
			_drawStack_reset_stacks();
			InternalEditorUtility.RepaintAllViews();
		}
		internal void RepaintWindow(bool force = false)
		{
			if (!force)
				if (EVENT != null)
				{
					RepaintWindowInUpdate();
					return;
				}

			_RepaintAfterUpadate = false;

			foreach (var w in hierarchyWindows)
			{
				if (!w.Value.w) continue;
				w.Value.w.Repaint();
			}
			foreach (var e in modsController.externalMods.ToList())
			{
				if (!e.Alive()) continue;
				e.RepaintNow();
			}

		}

		internal void RepaintExternalNow()
		{
			foreach (var e in modsController.externalMods.ToList())
			{
				if (!e.Alive()) continue;
				e.RepaintNow();
			}
		}

	}
}
