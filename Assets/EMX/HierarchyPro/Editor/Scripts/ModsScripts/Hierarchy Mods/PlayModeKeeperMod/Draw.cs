using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using EMX.HierarchyPlugin.Editor.Mods.PlayModeSaver;
using UnityEngine.SceneManagement;

namespace EMX.HierarchyPlugin.Editor.Mods
{




    internal partial class PlayModeKeeperMod : DrawStackAdapter, ISearchable
    {

        Rect _drawRect, firstRect;
            int SI = 12;
       // static int WIDTH = 22;

        void PreCalcRect( )
        {
            _drawRect = adapter.fullLineRect;
            _drawRect.x += _drawRect.width - adapter.rightOffset;
            _drawRect.width = SI * 2;
			_drawRect.x -= _drawRect.width;
			if ( adapter.par_e.RIGHT_RIGHT_PADDING_AFFECT_TO_SETACTIVE_AND_KEEPER && adapter.par_e.USE_RIGHT_ALL_MODS ) _drawRect.x -= adapter.par_e.RIGHT_RIGHT_PADDING;

            adapter.rightOffset += _drawRect.width;

           // _drawRect.x -= adapter.par_e.RIGHT_RIGHT_PADDING;
            firstRect = _drawRect;
        }

        void Draw( )
        {
            //var _o = o2 ??  ;
            if ( !adapter.o.go ) return;

            _drawRect.x = firstRect.x;
			_drawRect.y = adapter.fullLineRect.y;

            if ( !START_DRAW( _drawRect, adapter.o ) ) return;


            // var FI = 20;

            borderR = _drawRect;
            borderR.x += borderR.width / 2 - SI;
            borderR.y += (borderR.height - SI) / 2;
            borderR.width = SI * 2;
            //drawRect.width = drawRect.height = FI;

            // var id = o.GetInstanceID();

            var contains = HierarchyTempSceneDataGetter.GetObjectData( SaverType.ModPlayKeeper , adapter.o) != null;

            var auto = adapter.par_e.PLAYMODESAVER_SAVE_USE_PERMANENT_LIST_OF_MONOSCRIPTS && HierarchyCommonData.Instance().DataKeeper_IsObjectIncluded(adapter.o);
            /*   if (auto)
                       MonoBehaviour.print(o);*/
            //SingleList currentList = null;


            if ( contains ) //MonoBehaviour.print("ASD");
            { /* currentList = */
                GET_CURRENT_LIST( adapter.o, ref contains );
            }

            if ( !contains )
            { //currentList = null;
                if (adapter.par_e.PLAYMODESAVER_HIDE_ICONS_FOR_UNASSIGNED && !auto)
                {
                    compstexture = adapter.GetNewIcon(NewIconTexture.RightMods, "TRANS");
                    storagetexture = adapter.GetNewIcon(NewIconTexture.RightMods, "TRANS");
                }
                else
                {
                    compstexture = adapter.GetNewIcon(NewIconTexture.RightMods, "STORAGE_NOCOMP");
                    storagetexture = adapter.GetNewIcon(NewIconTexture.RightMods, "STORAGE_PASSIVE");
                    
                }
                
            }


            leftR = rightR = borderR;
            leftR.width = leftR.height = rightR.width = rightR.height = SI;
            leftR.x += 1;
            rightR.x += rightR.width;
            rightR.x -= 2 + 1;
            rightR.y -= 2;
            rightR.width += 4;
            rightR.height += 4;

            /*var guic = GUI.color;
            if ( !o.activeInHierarchy ) GUI.color *= alpha;
            Adapter.DrawTexture( rightR, storagetexture );
            Adapter.DrawTexture( leftR, compstexture );
            
            {   if ( auto )
                {   labelrect = _drawRect;
                    labelrect.y = _drawRect.y + _drawRect.height - 7;
                    labelrect.height = 9;
                    labelrect.width = 23;
                    Adapter.DrawTexture( labelrect, adapter.GetIcon( "STORAGE_AUTO" ) );
                }
            }
            GUI.color = guic;*/
            var guic = Color.white;
            if ( !adapter.o.go.activeInHierarchy ) guic.a *= alpha.a;
            Draw_GUITexture( rightR, storagetexture, guic );
            Draw_GUITexture( leftR, compstexture, guic );
            /*    Adapter.DrawTexture( rightR, storagetexture );
            Adapter.DrawTexture( leftR, compstexture );*/

            {
                if ( auto )
                {
                    labelrect = _drawRect;
                    labelrect.y = _drawRect.y + _drawRect.height - 7;
                    labelrect.height = 9;
                    labelrect.width = 23;
                    //    Adapter.DrawTexture( labelrect, adapter.GetIcon( "STORAGE_AUTO" ) );
                    Draw_GUITexture( labelrect, adapter.GetNewIcon( NewIconTexture.RightMods, "STORAGE_AUTO" ), guic );
                }
            }
            // GUI.color = guic;


            // /*MOUSE RECTS*/
            leftR = rightR = borderR;
            leftR.y = rightR.y = _drawRect.y;
            leftR.height = rightR.height = _drawRect.height;
            leftR.width = rightR.width = SI;
            rightR.x += rightR.width;
            /*
                            leftR.x += 1;
                            rightR.x += 1;*/

            // Adapter.GET_SKIN().button.active.background = Hierarchy.GetIcon("BUT");

            Draw_ModuleButton( leftR, PLAY_CONT_STORE, BUTTON_ACTION_0_HASH, true, useContentForButton: true, args: null );
            Draw_ModuleButton( rightR, PLAY_CONT_LINES, BUTTON_ACTION_1_HASH, true, useContentForButton: true, args: null );

            /*_DRAW_BUTTON( 1, rightR, o );
            _DRAW_BUTTON( 0, leftR, o );*/


            END_DRAW( adapter.o );
        }














		DrawStackMethodsWrapper __BUTTON_ACTION_0_HASH = null;

		DrawStackMethodsWrapper BUTTON_ACTION_0_HASH
		{
			get { return __BUTTON_ACTION_0_HASH ?? (__BUTTON_ACTION_0_HASH = new DrawStackMethodsWrapper(BUTTON_ACTION_0)); }
		}

		void BUTTON_ACTION_0(Rect worldOffset, Rect inputRect, DrawStackMethodsWrapperData data, HierarchyObject _o)
		{
			_DRAW_BUTTON(0, inputRect, _o, data.args);
		}

		DrawStackMethodsWrapper __BUTTON_ACTION_1_HASH = null;

		DrawStackMethodsWrapper BUTTON_ACTION_1_HASH
		{
			get { return __BUTTON_ACTION_1_HASH ?? (__BUTTON_ACTION_1_HASH = new DrawStackMethodsWrapper(BUTTON_ACTION_1)); }
		}


		void BUTTON_ACTION_1(Rect worldOffset, Rect inputRect, DrawStackMethodsWrapperData data, HierarchyObject _o)
		{
			_DRAW_BUTTON(1, inputRect, _o, data.args);
		}

		void _DRAW_BUTTON(int index, Rect rect, HierarchyObject _o, object args) // var bg = Adapter.GET_SKIN().button.active.background;
		{
			// SingleList currentList = (SingleList)args;

			var o = _o.go;
			bool contains = false;
			List<int> currentList = GET_CURRENT_LIST(_o, ref contains);
			// if ( adapter.ModuleButton( rect, index == 0 ? PLAY_CONT_LINES : PLAY_CONT_STORE, true ) )
			{
				if (EVENT.button == adapter.MOUSE_BUTTON_0)
				{ /* if (EVENT.control)
                    {
                     SetValue(o, false, new int[0]);
                    } else*/
					{
						var control = EVENT.control;
						if (index == 1)
						{
							var obs = new[] { o }.ToList();
							if (control) obs.AddRange(o.GetComponentsInChildren<Transform>(true).Select(t => t.gameObject));
							var state = !(currentList != null && currentList.Count > 0 && currentList[0] == 1);
							foreach (var __o in obs)
								SetValue(__o, state, null);
							ResetStack();
						}

						//Debug.Log( index + " " +  currentList );
						if (index == 0)
						{
							GenericMenu menu = new GenericMenu();
							var comps = o.GetComponents<Component>();

							if (LAST_VALIDATE_UNDO(o, comps))
							{
								menu.AddItem(new GUIContent("Revert Last PlayMode Changes"), false, () =>
								{
									if (!o) return;
									LAST_DO_UNDO(o, comps);
									ResetStack();
								});
								menu.AddSeparator("");
							}


							var added = GetAdded(o, comps);

							menu.AddItem(new GUIContent("- none -"), false, () =>
							{
								var list = new[] { o }.ToList();
								if (control) list.AddRange(o.GetComponentsInChildren<Transform>(true).Select(t => t.gameObject));
								foreach (var __o in list)
									SetValue(__o, false, new int[0]);
								ResetStack();
							});
							// menu.AddSeparator("");

							List<string> was = new List<string>();
							for (int i = 0; i < comps.Length; i++) // MonoBehaviour.print(comps[i].GetType());
							{
								if (!comps[i]) continue;
								var captureID = comps[i].GetInstanceID();
								var cont = new GUIContent();
								var enabled = added.Contains(captureID);
								var auto = adapter.par_e.PLAYMODESAVER_SAVE_USE_PERMANENT_LIST_OF_MONOSCRIPTS && HierarchyCommonData.Instance().HasPlayModeSaverPersistScript((comps[i]));

								if (enabled || auto) cont = new GUIContent((comps[i].GetType().Name));
								else cont = new GUIContent("[ " + (comps[i].GetType().Name) + " ]");
								var rr = cont.text;
								var innn = 0;
								while (was.Contains(cont.text)) cont.text = rr + " " + (innn++).ToString();
								was.Add(cont.text);

								if (auto) cont.text += "  ( AUTO )";

								menu.AddItem(cont, enabled || auto, () =>
								{
									if (auto) return;
									var c = EditorUtility.InstanceIDToObject(captureID) as Component;
									if (!c) return;
									if (enabled) added.RemoveAll(a => a == captureID);
									else if (!added.Contains(captureID)) added.Add(captureID);
									// var all = added.Count == comps.Length;
									SetValue(o, false, added.ToArray());
									if (control)
									{
										var enablelist = new Dictionary<Component, bool> { { c, !enabled } };
										ApplyToChild(o, enablelist);
									}

									ResetStack();
								});
							}

							//  menu.AddSeparator("");
							menu.AddSeparator("");
							menu.AddItem(new GUIContent("Apply to Children"), false, () =>
							{
								var enablelist = comps.Where(c => c).ToDictionary(c => c, c => added.Contains(c.GetInstanceID()));
								ApplyToChild(o, enablelist);
								//  adapter.RESET_DRAW_STACKS();
								ResetStack();
							});
							menu.AddItem(new GUIContent("Add MonoScript"), false, () =>
							{
								//SessionState.SetInt( "EMX Menu Item", 3 );
								//adapter.SHOW_HIER_SETTINGS_PLAYMODE_KEEPER();
								Settings.MainSettingsEnabler_Window.Select<Settings.PK_Window>();
							});
							menu.ShowAsContext();
						}
					}


					Tools.EventUse();
				}


				if (EVENT.button == adapter.MOUSE_BUTTON_1)
				{
					Tools.EventUse();
					/* int[] contentCost = new int[0];
                     GameObject[] obs = new GameObject[0];
                    
                    
                     if (index == 0 && Validate(o))
                     {
                         if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeaderFiltered(out obs, out contentCost, currentList.list);
                         FillterData.Init(EVENT.mousePosition, SearchHelper, "such as " + o.name, obs, contentCost, null, this);
                     } else
                     {
                         CallHeader(out obs, out contentCost);
                         FillterData.Init(EVENT.mousePosition, SearchHelper, "All assigned", obs, contentCost, null, this);
                     }*/
					var mp = new MousePos(EVENT.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);

					Windows.SearchWindow.Init(mp, SearchHelper, index == 0 && Validate(Cache.GetHierarchyObjectByInstanceID(o)) ? "such as " + o.name : "All assigned",
						index == 0 && Validate(Cache.GetHierarchyObjectByInstanceID(o)) ? CallHeaderFiltered(currentList) : CallHeader(),
						this, adapter, _o);

					// EditorGUIUtility.ic
				}
			}

			//  Adapter.GET_SKIN().button.active.background = bg;
		}





		bool Validate(HierarchyObject o)
		{

			var l = HierarchyTempSceneDataGetter.GetObjectData(SaverType.ModPlayKeeper, o);
			// var l = DataKeeperCache.GetValue(o.go.scene, o);
			if (l == null) return false;
			var list = l.GetIntList();
			for (int i = list.Count - 1; i > 0; i--)
				if (!EditorUtility.InstanceIDToObject(list[i]))
					list.RemoveAt(i);
			// return !string.IsNullOrEmpty(o.tag) && o.tag != "Untagged";
			return list.Count != 0 && (list[0] == 1 || list.Count > 1);
		}

		bool Validate(HierarchyObject o, List<int> filter)
		{
			var l = HierarchyTempSceneDataGetter.GetObjectData(SaverType.ModPlayKeeper, o);
			// var l = DataKeeperCache.GetValue(o.go.scene, o);
			if (l == null) return false;
			var list = l.GetIntList();
			for (int i = list.Count - 1; i > 0; i--)
				if (!EditorUtility.InstanceIDToObject(list[i]))
					list.RemoveAt(i);
			// return !string.IsNullOrEmpty(o.tag) && o.tag != "Untagged";
			if (!(list.Count != 0 && (list[0] == 1 || list.Count > 1))) return false;

			if (filter[0] == 1 && list[0] == 1) return true;

			for (int i = 0; i < filter.Count; i++)
			{
				if (filter[i] != list[i]) return false;
			}

			return true;
		}



		/* FillterData.Init(Event.current.mousePosition, SearchHelper, LayerMask.LayerToName(o.layer),
                     Validate(o) ?
                     CallHeaderFiltered(LayerMask.LayerToName(o.layer)) :
                     CallHeader(),
                     this);*/
		/** CALL HEADER */
		public Windows.SearchWindow.FillterData_Inputs CallHeader()
		{
			var result = new Windows.SearchWindow.FillterData_Inputs(callFromExternal_objects)
			{
				Valudator = Validate,
				SelectCompareString = (d, i) =>
				{
					var k = HierarchyTempSceneDataGetter.GetObjectData(SaverType.ModPlayKeeper, d);
					//   var k = DataKeeperCache.GetValue(d.go.scene, d);
					if (k == null) return "";
					var list = k.GetIntList();
					var cost = list.Count;
					if (list[0] == 0) cost += 1000000;
					return cost.ToString();
				},
				SelectCompareCostInt = (d, i) =>
				{
					var cost = i;
					cost += d.go.activeInHierarchy ? 0 : 100000000;
					return cost;
				}
			};
			return result;
		}

		internal Windows.SearchWindow.FillterData_Inputs CallHeaderFiltered(List<int> filter)
		{
			var result = CallHeader();
			result.Valudator = s => Validate(s, filter);
			return result;
		}


		/** CALL HEADER */

	}
}
