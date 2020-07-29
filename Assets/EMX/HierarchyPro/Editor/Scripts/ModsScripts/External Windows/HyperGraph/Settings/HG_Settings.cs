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




		internal bool HYPERGRAPH_EVENTS_MODE_BOOL
		{
			get { return GET("HYPERGRAPH_EVENTS_MODE_BOOL", false); }
			set
			{
				SET("HYPERGRAPH_EVENTS_MODE_BOOL", value);
				p.modsController.INVOKE_FOR_EXTERNAL<Mods.HyperGraph.HyperGraphModWindow>(h => h.instance.RefreshWithCurrentSelection());
			}
		}
		internal int HYPERGRAPH_EVENTS_MODE { get { return HYPERGRAPH_EVENTS_MODE_BOOL ? 1 : 0; } }
		internal bool HYPERGRAPH_SKIP_ARRAYS_BOOL
		{
			get { return GET("HYPERGRAPH_SKIP_ARRAYS_BOOL", false); }
			set
			{
				SET("HYPERGRAPH_SKIP_ARRAYS_BOOL", value);
				p.modsController.INVOKE_FOR_EXTERNAL<Mods.HyperGraph.HyperGraphModWindow>(h => h.instance.RefreshWithCurrentSelection());
			}
		}
		internal int HYPERGRAPH_SKIP_ARRAYS { get { return HYPERGRAPH_SKIP_ARRAYS_BOOL ? 2 : 0; } }
		internal bool HYPERGRAPH_DISPLAY_ASSETS
		{
			get { return GET("HYPERGRAPH_DISPLAY_ASSETS", true); }
			set
			{
				SET("HYPERGRAPH_DISPLAY_ASSETS", value);
				p.modsController.INVOKE_FOR_EXTERNAL<Mods.HyperGraph.HyperGraphModWindow>(h => h.instance.RefreshWithCurrentSelection());
			}
		}
		internal bool HYPERGRAPH_CONNECT_TO_SELFT
		{
			get { return GET("HYPERGRAPH_CONNECT_TO_SELFT", true); }
			set
			{
				SET("HYPERGRAPH_CONNECT_TO_SELFT", value);
				p.modsController.INVOKE_FOR_EXTERNAL<Mods.HyperGraph.HyperGraphModWindow>(h => h.instance.RefreshWithCurrentSelection());
			}
		}

		internal bool ATTACH_TO_INSPECT_ONOPEN { get { return GET("ATTACH_TO_INSPECT_ONOPEN", false); } set { SET("ATTACH_TO_INSPECT_ONOPEN", value); } }

		internal int HYPERGRAPH_FONTSIZE { get { return GET("HYPERGRAPH_FONTSIZE", 12); } set { SET("HYPERGRAPH_FONTSIZE", value); } }

		internal float HYPERGRAPH_SCALE { get { return Mathf.Clamp(GET("HYPERGRAPH_SCALE", 1f), 0.5f, 2f); } set { SET("HYPERGRAPH_SCALE", value); } }
		internal float HYPERGRAPH_HEIGHT { get { return GET("HYPERGRAPH_HEIGHT", 200f); } set { SET("HYPERGRAPH_HEIGHT", value); } }
		internal int HYPERGRAPH_SCANPERFOMANCE04 { get { return (HYPERGRAPH_SCANPERFOMANCE - 2) / 2; } set { HYPERGRAPH_SCANPERFOMANCE = value * 2 + 2; } }
		internal int HYPERGRAPH_SCANPERFOMANCE { get { return Mathf.Clamp((GET("HYPERGRAPH_SCANPERFOMANCE", 4) - 2) / 2, 0, 4) * 2 + 2; } set { SET("HYPERGRAPH_SCANPERFOMANCE", value); } }
		internal bool HYPERGRAPH_AUTOHIDE { get { return GET("HYPERGRAPH_AUTOHIDE", false); } set { SET("HYPERGRAPH_AUTOHIDE", value); } }
		internal bool HYPERGRAPH_AUTOCHANGE { get { return GET("HYPERGRAPH_AUTOCHANGE", true); } set { SET("HYPERGRAPH_AUTOCHANGE", value); } }
		internal bool HYPERGRAPH_SHOWUPDATINGINDICATOR { get { return GET("HYPERGRAPH_SHOWUPDATINGINDICATOR", true); } set { SET("HYPERGRAPH_SHOWUPDATINGINDICATOR", value); } }
		internal bool HYPERGRAPH_RED_HIGKLIGHTING { get { return GET("HYPERGRAPH_RED_HIGKLIGHTING", true); } set { SET("HYPERGRAPH_RED_HIGKLIGHTING", value); } }






	}
}
