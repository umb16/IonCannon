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

		// 0 - Label ; 1 - Fold ; 2 - Align Left
		internal int HIGHLIGHTER_CUSTOM_ICON_LOCATION { get { return GET("HIGHLIGHTER_CUSTOM_ICON_LOCATION", 2); } set { SET("HIGHLIGHTER_CUSTOM_ICON_LOCATION", value); } }


		  internal bool HIGHLIGHTER_SHOW_PREFAB_ICON { get { return GET("HIGHLIGHTER_SHOW_PREFAB_ICON", false ); } set { SET("HIGHLIGHTER_SHOW_PREFAB_ICON", value ); } }

		

		//   internal bool OVERRIDE_OBJECT_DEFAULT_ICON_SIZE { get { return GET("OVERRIDE_OBJECT_DEFAULT_ICON_SIZE", false ); } set { SET("OVERRIDE_OBJECT_DEFAULT_ICON_SIZE", value ); } }
		//   internal int OBJECT_DEFAULT_ICON_SIZE { get { return GET("OBJECT_DEFAULT_ICON_SIZE", Tools.singleLineHeight ); } set { SET("OBJECT_DEFAULT_ICON_SIZE", value ); } }


	}
}
