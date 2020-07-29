using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;



namespace EMX.HierarchyPlugin.Editor
{

    partial class EditorSettingsAdapter
    {

        
        
        internal bool PLAYMODESAVER_SAVE_UNITYOBJECT { get { return GET( "PLAYMODESAVER_SAVE_UNITYOBJECT", true ); } set { SET( "PLAYMODESAVER_SAVE_UNITYOBJECT", value ); p.RESET_DRAWSTACK(); } }
        internal bool PLAYMODESAVER_USE_ADD_REMOVE { get { return GET( "PLAYMODESAVER_USE_ADD_REMOVE", false ); } set { SET( "PLAYMODESAVER_USE_ADD_REMOVE", value ); p.RESET_DRAWSTACK(); } }
        internal bool PLAYMODESAVER_SAVE_ENABLINGDISABLING_GAMEOBJEST { get { return GET( "PLAYMODESAVER_SAVE_ENABLINGDISABLING_GAMEOBJEST", false ); } set { SET( "PLAYMODESAVER_SAVE_ENABLINGDISABLING_GAMEOBJEST", value ); p.RESET_DRAWSTACK(); } }
        internal bool PLAYMODESAVER_SAVE_GAMEOBJET_HIERARCHY { get { return GET( "PLAYMODESAVER_SAVE_GAMEOBJET_HIERARCHY", false ); } set { SET( "PLAYMODESAVER_SAVE_GAMEOBJET_HIERARCHY", value ); p.RESET_DRAWSTACK(); } }
        internal bool PLAYMODESAVER_SAVE_USE_PERMANENT_LIST_OF_MONOSCRIPTS { get { return GET( "PLAYMODESAVER_SAVE_USE_PERMANENT_LIST_OF_MONOSCRIPTS", true ); } set { SET( "PLAYMODESAVER_SAVE_USE_PERMANENT_LIST_OF_MONOSCRIPTS", value ); p.RESET_DRAWSTACK(); } }


        internal bool PLAYMODESAVER_HIDE_ICONS_FOR_UNASSIGNED { get { return GET("PLAYMODESAVER_HIDE_ICONS_FOR_UNASSIGNED", false); } set { SET("PLAYMODESAVER_HIDE_ICONS_FOR_UNASSIGNED", value); p.RESET_DRAWSTACK(); } }




        //TEMP
        //internal bool PLAYMODESAVER_TEMP_WERE_PLAYED { get { return GET( "PLAYMODESAVER_TEMP_WERE_PLAYED", false ); } set { SET( "PLAYMODESAVER_TEMP_WERE_PLAYED", value ); } }
       // internal bool PLAYMODESAVER_TEMP_SKIP_INIT { get { return GET( "PLAYMODESAVER_TEMP_SKIP_INIT", false ); } set { SET( "PLAYMODESAVER_TEMP_SKIP_INIT", value ); } }
       // internal bool PLAYMODESAVER_TEMP_WERE_LAST_SAVED { get { return GET( "PLAYMODESAVER_TEMP_WERE_LAST_SAVED", false ); } set { SET( "PLAYMODESAVER_TEMP_WERE_LAST_SAVED", value ); } }
        internal bool PLAYMODESAVER_TEMP_WERE_PLAYED { get { return SessionState.GetBool("PLAYMODESAVER_TEMP_WERE_PLAYED", false); } set { SessionState.SetBool("PLAYMODESAVER_TEMP_WERE_PLAYED", value); } }
        internal bool PLAYMODESAVER_TEMP_SKIP_INIT { get { return SessionState.GetBool("PLAYMODESAVER_TEMP_SKIP_INIT", false); } set { SessionState.SetBool("PLAYMODESAVER_TEMP_SKIP_INIT", value); } }
        internal bool PLAYMODESAVER_TEMP_WERE_LAST_SAVED { get { return SessionState.GetBool("PLAYMODESAVER_TEMP_WERE_LAST_SAVED", false); } set { SessionState.SetBool("PLAYMODESAVER_TEMP_WERE_LAST_SAVED", value); } }



    }
}
