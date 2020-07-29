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





		internal bool BOOKMARKS_OB_DRAW_CATEGORY_NAME { get { return GET("BOOKMARKS_OB_DRAW_CATEGORY_NAME", true); } set { SET("BOOKMARKS_OB_DRAW_CATEGORY_NAME", value); } }

		internal bool BOOKMARKS_OB_DRAW_BG_COLOR { get { return GET("BOOKMARKS_OB_DRAW_BG_COLOR", false); } set { SET("BOOKMARKS_OB_DRAW_BG_COLOR", value); } }
		internal bool BOOKMARKS_OB_SHOWDESCRIPTS { get { return GET("BOOKMARKS_OB_SHOWDESCRIPTS", false); } set { SET("BOOKMARKS_OB_SHOWDESCRIPTS", value); } }
		internal bool BOOKMARKS_OB_SHIFT_TO_INSTANTIATE { get { return GET("BOOKMARKS_OB_SHIFT_TO_INSTANTIATE", true); } set { SET("BOOKMARKS_OB_SHIFT_TO_INSTANTIATE", value); } }
		// 0 -   default prefab positions // 1 - next to selected object
		internal int BOOKMARKS_OB_INSTANTIATE_POSITION { get { return GET("BOOKMARKS_OB_INSTANTIATE_POSITION", 1); } set { SET("BOOKMARKS_OB_INSTANTIATE_POSITION", value); } }


		




	}
}
