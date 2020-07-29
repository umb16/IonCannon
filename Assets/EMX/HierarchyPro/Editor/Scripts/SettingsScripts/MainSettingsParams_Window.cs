using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EMX.HierarchyPlugin.Editor.Settings
{
    class MainSettingsParams_Window : ScriptableObject
    {

    }



    [CustomEditor( typeof(MainSettingsParams_Window) )]
    class MainSettingsParamsEditor : MainRoot
    {




        public override void OnInspectorGUI( )
        {

            Draw.RESET();

            Draw.BACK_BUTTON();

            GUI.Button( Draw.R2, "Common Settings", Draw.s( "preToolbar" ) );
            // GUI.Button( Draw.R, "Common Settings", Draw.s( "insertionMarker" ) );
            using ( GRO.UP() )
            {

                //MAIN
              //  using ( GRO.UP( 0 ) )
                {
                   // GUI.Label( Draw.R, Draw.CONT( "Hot Keys Placement ('Bookmarks','Hypergraph')" ) ); Draw.Sp( 2 ); //, Draw.s( "preBackground" )
                  ///  Draw.TOOLBAR( new[] { "TopBar", "Scene Name", "Bottom", "Disable" }, "HOTBUTTONS_BAR_PLACE" );
                }

          //      Draw.HRx2();

                Draw.TOG_TIT( "Override lines height", "USE_LINE_HEIGHT" );
                using ( ENABLE.USE( Draw.GetSetter( "USE_LINE_HEIGHT" ) ) )
                {
                    Draw.FIELD( "Line Height '16'", "LINE_HEIGHT", 4, 60 );
                //    if ( p.par_e.USE_LINE_HEIGHT && p.par_e.LINE_HEIGHT < 16 ) Draw.HELP( "Warning! Since Unity 2017, line height < 16 sometimes throws DrawSelection Exeption!", new Color( 0.9f, 0.7f, 0.3f, 1 ) );
                }

                Draw.TOG_TIT( "Override child indent", "USE_CHILD_INDENT" );
                using ( ENABLE.USE( Draw.GetSetter( "USE_CHILD_INDENT" ) ) ) Draw.FIELD( "Child Indent", "CHILD_INDENT", 0, 60 );
                Draw.TOG_TIT( "Override default icons size", "USE_OVERRIDE_DEFAULT_ICONS_SIZE" );
                using ( ENABLE.USE( Draw.GetSetter( "USE_OVERRIDE_DEFAULT_ICONS_SIZE" ) ) ) Draw.FIELD( "default icons size", "OVERRIDE_DEFAULT_ICONS_SIZE", 2, 60 );


                //     Draw.TOG( "Use horizontal scroll bar 'TEST'", "USE_HORISONTAL_SCROLL" );
              //  using ( GRO.UP( 0 ) )
                {
                    Draw.TOG_TIT( "Override font size for Labels of GameObjects names", "USE_OVERRIDE_FOR_GAMEOBJECTS_NAMES_LABELS_FONT_SIZE" );
                    using ( ENABLE.USE( Draw.GetSetter( "USE_OVERRIDE_FOR_GAMEOBJECTS_NAMES_LABELS_FONT_SIZE" ) ) ) 
                        Draw.FIELD( "Objects Labels font size '11'", "OVERRIDE_FOR_GAMEOBJECTS_NAMES_LABELS_FONT_SIZE", 4, 60 );
                }
             //   using ( GRO.UP( 0 ) )
                {
                    Draw.TOG_TIT( "Fun 'TEST' override all editor fonts size", "USE_WHOLE_FUN_UNITY_FONT_SIZE", ( valBefore ) =>
               {
                   if ( !valBefore.AS_BOOL && EditorUtility.DisplayDialog("" + Root.PN + "", "Do you want to Override all editor fonts size? After disabling, you need to restart the editor", "Yes, I wanna try", "No, please stop!" ) ) return true;
                   if ( valBefore.AS_BOOL && EditorUtility.DisplayDialog("" + Root.PN + "", "Do you want to disable fun custom editor fonts size? You also need to restart the editor to fully restore fonts sizes", "Yes, turn off this bugable feature", "No" ) ) return true;
                   return false;
               },
                ( valAfter ) =>
                {
                    p.modsController.REBUILD_PLUGINS();
                    if ( !valAfter.AS_BOOL ) FunEditorFontsModification.Modificate( 12 );
                } );
                    using ( ENABLE.USE( Draw.GetSetter( "USE_WHOLE_FUN_UNITY_FONT_SIZE" ) ) ) Draw.FIELD( "Fun unity font size '12'", "WHOLE_FUN_UNITY_FONT_SIZE", 4, 60 );
                }
                //OTHER
                
                Draw.HRx2();
                Draw.TOG( "Disable hover gray rect", "HIDE_HOVER_BG" );
                using (ENABLE.USE("HIDE_HOVER_BG", 0,true)) Draw.TOG("Right arrow key expands hover item", "RIGHTARROW_EXPANDS_HOVERED" );
                Draw.TOG( "Up/Down arrow keys move multiline separated selection together", "SELECTION_MOVETOGETHER_UPDOWNARROWS" );
                Draw.TOG( "Use hover color for buttons", "USE_HOVER_FOR_BUTTONS");
                using (ENABLE.USE("USE_HOVER_FOR_BUTTONS", 0)) Draw.HELP("For some strange reason unity 2019 has strange behavior, it works only for internal styles with null textures, but mby in any latest version it will be fixed.");
                Draw.TOG( "Use expansion animation", "USE_EXPANSION_ANIMATION" );
                using ( ENABLE.USE( "USE_EXPANSION_ANIMATION", 0) ) Draw.HELP( "I could not catch a rectangle for animating elements, the unity always returns strange 0 y positions" );
                Draw.TOG("Use dynamic GL batching for drawing", "USE_DINAMIC_BATCHING");
                using (ENABLE.USE("USE_DINAMIC_BATCHING", 0)) Draw.HELP("You can turn it off if you see any artifacts with textures.");
                Draw.TOG("Swap Left/Right mouse buttons for Mods", "USE_SWAP_FOR_BUTTONS_ACTION");
                using (ENABLE.USE("USE_SWAP_FOR_BUTTONS_ACTION", 0))  Draw.HELP("-Default: Left to open Menu, Right to Search\n- Swapped: Left to Search, the Right to open Menu.");
                Draw.TOG("Use OnMosueDown instead OnMouseUp for Mods", "ONDOWN_ACTION_INSTEAD_ONUP");




                //  Draw.TOG( "Use horisontal scroll", "USE_HORISONTAL_SCROLL" );
            //    using ( GRO.UP( 0 ) )
                {
                    Draw.TOG_TIT( "Escape closes edit prefab mode", "ESCAPE_CLOSES_PREFABMODE" );
                    using ( ENABLE.USE( Draw.GetSetter( "ESCAPE_CLOSES_PREFABMODE" ) ) ) Draw.TOG( "Close only if Hierarchy or SceneView are focus", "CLOSE_PREFAB_KEY_FORHIER_ANDSCENE" );
                }

             //   using ( GRO.UP( 0 ) )
                {
                    Draw.TOG_TIT( "Double click expands item", "DOUBLECLICK_TO_EXPAND" );
                    // Draw.HELP( "Use the right mouse button on the SetActive button to frame a GameObject" );
                }

                // Draw.TOG( "Double click expands item", "DRAW_HIERARHCHY_CHESS_FILLS" );

                Draw.HRx2();
                Draw.TOG_TIT( "Use background decorations", "USE_BACKGROUNDDECORATIONS_MOD" );
                using ( GRO.UP( 0 ) ) using ( ENABLE.USE( Draw.GetSetter( "USE_BACKGROUNDDECORATIONS_MOD" ), 0 ) )
                {


                    Draw.TOG( "Draw hierarchy child lines", "DRAW_HIERARHCHY_CHILD_LINES" );
                    using ( ENABLE.USE( Draw.GetSetter( "DRAW_HIERARHCHY_CHILD_LINES" ) ) ) Draw.COLOR( "Child lines color", "COLOR_HIERARHCHY_CHILD_LINES" );
                    
                    GUI.Label( Draw.R, Draw.CONT( "Draw background chess fills:" ) );
                    Draw.TOOLBAR( new[] { "No", "Clamped fills", "Full fills" }, "DRAW_HIERARHCHY_CHESS_FILLS" );
                    using ( ENABLE.USE( Draw.GetSetter( "DRAW_HIERARHCHY_CHESS_FILLS" ) ) ) Draw.COLOR( "Background fill color", "COLOR_HIERARHCHY_CHESS_FILLS" );
                    GUI.Label( Draw.R, Draw.CONT( "Draw horisontal separation lines:" ) );
                    Draw.TOOLBAR( new[] { "No", "Clamped separations", "Full separations" }, "DRAW_HIERARHCHY_CHESS_LINES" );
                    using ( ENABLE.USE( Draw.GetSetter( "DRAW_HIERARHCHY_CHESS_LINES" ) ) ) Draw.COLOR( "Horisontal lines color", "COLOR_HIERARHCHY_CHESS_LINES" );

                    //CHILD COUNT
                    Draw.TOG( "Draw child count number", "DRAW_CHILDS_COUNT" );
                    using ( ENABLE.USE( Draw.GetSetter( "DRAW_CHILDS_COUNT" ) ) )
                    {
						Draw.COLOR( "Numbers Color", "DRAW_CHILDS_COLOR" );
                        Draw.TOG( "Invers numbers colors", "DRAW_CHILDS_INVERCE_COLOR" );
                        Draw.TOG( "Hide numbs for root object", "HIDE_CHILDCOUNT_IFROOT" );
                        Draw.TOG( "Hide numbs for expanded object", "HIDE_CHILDCOUNT_IFEXPANDED" );
                        Draw.TOOLBAR( new[] { "Align Left", "Align Midlle", "Align Right" }, "CHILDCOUNT_ALIGMENT" );
                        Draw.FIELD( "Child numbs offset X", "CHILDCOUNT_OFFSET_X", -200, 200 );
                        Draw.FIELD( "Child numbs offset Y", "CHILDCOUNT_OFFSET_Y", -200, 200 );

                    }
                }

                Draw.HRx2();
                //FINAL

                Draw.TOG( "Custom windows animation", "ENABLE_CUSTOMWINDOWS_OPENANIMATION" );
                Draw.TOG( "Ping changed objects", "ENABLE_OBJECTS_PING" );
                Draw.TOG( "Tracking compile time", "TRACKING_COMPILE_TIME" );
            }
        }


    }
}
