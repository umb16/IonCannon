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

        

        internal bool PRESETS_SAVE_GAMEOBJEST { get { return GET("PRESETS_SAVE_GAMEOBJEST", true); } set { SET("PRESETS_SAVE_GAMEOBJEST", value); } }

    }
}
