using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace EMX.HierarchyPlugin.Editor
{



	#pragma warning disable
	internal struct AutoHighlighterColor
	{
		internal bool filterAssigned;
		internal TempColorClass filterColor;

		internal bool internalIcon;
		internal TempColorClass drawIcon;
		internal TempColorClass localTempColor;
		internal Color BACKGROUNDEsourceBgColorD;
		internal int BACKGROUNDED ;
		internal int FLAGS;
		internal TempColorClass MIXINCOLOR;

		internal Rect? BG_RECT ;
	}
	#pragma warning restore

	internal class HierarchyObject : IEqualityComparer<HierarchyObject>, IComparable<HierarchyObject>, IEquatable<HierarchyObject>, ICloneable
	{



		internal int pluginID;
		internal int id;
		internal GameObject go;
		internal HierarchyObject_ProjectExt project;
		internal bool InvalideProjectAsset = false;
		internal Component cachedComp;
		Type cachedType = null;
	#pragma warning disable
		internal AutoHighlighterColor ah;
#pragma warning restore
		internal HierarchyObject(int pluginID)
		{
			this.pluginID = pluginID;
		}



		//   internal bool cache_prefab;
		//   internal int switchType;
		//   internal Rect? BG_RECT = null;


		internal UnityEngine.Object _GetHardLoadObjectSlow; // return InternalEditorUtility.GetLoadedObjectFromInstanceID( id );
		internal UnityEngine.Object GetHardLoadObjectSlow() // return InternalEditorUtility.GetLoadedObjectFromInstanceID( id );
		{
			if (pluginID == 0) return go;
			if (_GetHardLoadObjectSlow == null) _GetHardLoadObjectSlow = EditorUtility.InstanceIDToObject(id);
			return _GetHardLoadObjectSlow;
		}


		internal bool CompAsNull, HasMissing;
		Component[] _comps;
		internal Component[] GetComponents()
		{
			if (_comps != null) return _comps;
			_comps = go.GetComponents<Component>();
			for (int i = 0; i < _comps.Length; i++) if (!_comps[i]) HasMissing = true;
			if (_comps.Length == 1) CompAsNull = true;
			if (HasMissing) _comps = _comps.Where(c => c).ToArray();
			return _comps;
		}



		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		internal int scene
		{
			get
			{
				if (pluginID == 0) return go.scene.GetHashCode();
				return -1;
			}
		}
		public override string ToString()
		{
			if (pluginID == 0) return go ? go.name : "";


			return project != null ? !string.IsNullOrEmpty(project.assetName) ? project.assetName : project.assetPath : "";
		}

		public string name
		{
			get { return ToString(); }
		}

		bool? _HasBrefabButton;
		public bool HasBrefabButton
		{
			get
			{
				if (!_HasBrefabButton.HasValue)
				{
					if (!Root.p[pluginID].ha.hasShowingPrefabHeader) return (_HasBrefabButton = false).Value;
					var tree = GetTreeItem();
					_HasBrefabButton = (bool)Root.p[pluginID].showPrefabModeButton.GetValue(tree, null);
				}
				return _HasBrefabButton.Value;
			}

		}

		internal bool Validate()
		{
			if (pluginID == 0) return go;
			return true;
			// return !string.IsNullOrEmpty( AssetDatabase.AssetPathToGUID( project.assetPath ) );
		}

		internal bool Validate(int sceneHash)
		{
			if (pluginID == 0) return go && go.scene.GetHashCode() == sceneHash;
			return true;
			// return !string.IsNullOrEmpty( AssetDatabase.AssetPathToGUID( project.assetPath ) );
		}



		internal bool Validate(bool checkScene)
		{
			if (pluginID == 0) return go && go.scene.IsValid();
			return true;
			// return !string.IsNullOrEmpty( AssetDatabase.AssetPathToGUID( project.assetPath ) );
		}

		static HierarchyObject()
		{
			NewClearHelper.OnFontSizeChanged += clear_labels_size;
		}
		static void clear_labels_size()
		{
			content_size.Clear();
		}
		static Dictionary<int, Vector2> content_size = new Dictionary<int, Vector2>();
		static Vector2 tv2;
		static GUIContent tcont = new GUIContent();
		internal Vector2 GetContentSize()
		{

			var key = ToString().GetHashCode() ^ Root.p[pluginID].hierarchyModification.INTERNAL_LABEL_STYLES[0].style.fontSize;
			if (!content_size.TryGetValue(key, out tv2))
			{
				tcont.text = ToString();
				content_size.Add(key, tv2 = Root.p[pluginID].hierarchyModification.INTERNAL_LABEL_STYLES[0].style.CalcSize(tcont));
			}
			return tv2;

		}

		internal Type GET_TYPE()
		{
			if (pluginID == 0) return go.GetType();
			if (cachedType == null) //InternalEditorUtility.GetTypeWithoutLoadingObject
			{
				cachedType = Tools.GetTypeByInstanceID(id, pluginID);
				if (cachedType == null) cachedType = typeof(UnityEngine.Object);
			}

			return cachedType;
		}


		internal bool Active()
		{
			if (pluginID == 0) return go && go.activeInHierarchy;

			return true;
		}


		internal HierarchyObject[] GetAllParents()
		{
			var result = new List<HierarchyObject>();
			var current = parent();

			while (current != null)
			{
				result.Add(current);
				current = current.parent();
			}

			return result.ToArray();
		}

		internal HierarchyObject rootSlow()
		{
			return GetAllParents().LastOrDefault() ?? this;
		}

		internal HierarchyObject parent()
		{
			if (pluginID == 0)
			{
				if (!go) return null;
				var p = go.transform.parent;
				if (!p) return null;
				return Cache.GetHierarchyObjectByInstanceID(p.gameObject);
			}
			if (project.assetFolder == null || project.assetFolder == "Assets") return null;
			if (!project.IsMainAsset) return Cache.m_PathToObject[project.assetPath];
			if (!Cache.m_PathToObject.ContainsKey(project.assetFolder))
			{
				var guid = AssetDatabase.AssetPathToGUID(project.assetFolder);
				if (string.IsNullOrEmpty(guid)) return null;
				var load = Cache.GetHierarchyObjectByGUID(ref guid, ref project.assetFolder);
				if (!Cache.m_PathToObject.ContainsKey(project.assetFolder)) Cache.m_PathToObject.Add(project.assetFolder, load);
				if (load == null) return null;
			}
			return Cache.m_PathToObject[project.assetFolder];
		}

		internal int GetSiblingIndex()
		{
			if (pluginID == 0) return go.transform.GetSiblingIndex();
			return project.sibling;
		}


		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


		int backedLastSibForProject()
		{
			var item = GetTreeItem();
			if (item == null || item.children.Count == 0) return 0;
			return item.children[item.children.Count - 1].id;
			/* if ( !__mBackedLastSib.HasValue )
             {
                 var item = GetTreeItem();
                 if ( item == null ) return 0;
                 var children = adapter.m_data_treeitem_children.GetValue(item, null) as IList;
                 if ( children == null || children.Count == 0 ) __mBackedLastSib = 0;
                 else __mBackedLastSib = (int)adapter.m_data_treeitem_m_ID.GetValue( children[ children.Count - 1 ] );
             }
             return __mBackedLastSib.Value;*/
		}
		//int? __mBackedLastSib;


		internal UnityEditor.IMGUI.Controls.TreeViewItem _visibleTreeItem;
		internal Rect lastFullLineRect;
		internal Rect lastSelectionRect;

		internal UnityEditor.IMGUI.Controls.TreeViewItem GetTreeItem()
		{
			if (_visibleTreeItem != null) return _visibleTreeItem;
			return (_visibleTreeItem = Root.p[pluginID].GetTreeItem(id));
		}


		internal bool IsLastSibling()
		{
			if (pluginID == 0) return go.transform.parent && go.transform.GetSiblingIndex() == go.transform.parent.childCount - 1;

			var p = parent();
			return p != null && p.backedLastSibForProject() == id;
		}



		public int parentCount()
		{
			if (pluginID == 0) return go.GetComponentsInParent<Transform>(true).Length;

			if (project.parentCount.HasValue) return project.parentCount.Value;

			project.parentCount = Math.Max(0, project.assetPath.ToCharArray().Count(c => c == '/') - 1);
			return project.parentCount.Value;
		}

		internal bool ParentIsNull()
		{
			if (pluginID == 0) return go.transform.parent == null;
			return project.assetFolder == "Assets";
		}

		//  static SortedList<string, HierarchyObject> tl;
		// static object[] ob_arr = new object[1];
		//int? backedChild;

		internal int ChildCount()
		{
			if (pluginID == 0) return go ? go.transform.childCount : 0;

			/*if ( !project.IsFolder ) return 0;
            adapter.GetPathToChildrens( ref project.assetPath , out tl );
            return tl.Count;*/

			//    if ( backedChild.HasValue ) return backedChild.Value;

			/*   var data = adapter.m_data.GetValue(adapter.m_TreeView(adapter.window()), null);
               ob_arr[ 0 ] = id;
               var item = adapter.m_dataFindItem.Invoke(data, ob_arr);*/
			var item = GetTreeItem();

			if (item == null) return 0;
			return item.children.Count;
			/* ob_arr[0] = row;
             var item =  adapter.m_dataGetItem.Invoke(data, ob_arr );
             TreeViewUtility.*/
			// var children = adapter.m_data_treeitem_children.GetValue(item, null) as IList;
			//  return (backedChild = (children != null ? children.Count : 0)).Value;
			/*
            if (project.childCount.HasValue) return project.childCount.Value;
            //if ( project.assetName != null ) return (project.childCount = 0).Value;
            if (!project.IsFolder) return 0;
            //adapter.GetPathToChildrens( ref project.assetPath , out tl );
            
            //project.childCount = adapter.m_PathToChildrens.ContainsKey( project.assetPath ) ? adapter.m_PathToChildrens[project.assetPath].Count : 0;
            
            project.childCount =
                Directory.GetDirectories( UNITY_SYSTEM_PATH + project.assetPath, "*.*", SearchOption.TopDirectoryOnly ).Count( d => File.Exists( d + ".meta" ) ) +
                Directory.GetFiles( UNITY_SYSTEM_PATH + project.assetPath, "*.*", SearchOption.TopDirectoryOnly )
                    .Count( f => !f.EndsWith( ".meta" ) );
            
            //if (project.assetName == "lightmaps") Debug.Log(project.childCount.Value);
            
            return (project.childCount).Value;*/
		}







		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


		/*
        long? m_fileID;
        internal long fileID
        {
            get
            {
                if ( m_fileID.HasValue ) return m_fileID.Value;
                if ( pluginID != 0 ) m_fileID = project.guid.GetHashCode();
                else
                    m_fileID = go ? ObjectTools.GetFileIDWithOutPrefabChecking( ObjectTools.GetPrefabInstanceHandle( go ) as GameObject, go ) : 0;
                return m_fileID.Value;
            }
            set
            {
                if ( value == 0 ) return;
                m_fileID = value;
            }
        }

            */




		public object Clone()
		{
			var result = (HierarchyObject)MemberwiseClone();
			//  result.localTempColor = new TempColorClass().AssignFromList( new SingleList() { list = Enumerable.Repeat( 0, 9 ).ToList() }, true );
			// result.localTempColor = new TempColorClass().AssignFromList( 0, true );
			//result.m_fileID = null;  ////////////////// WERE TRUE
			result.project = (HierarchyObject_ProjectExt)result.project.Clone();
			return result;
		}




		/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



		public static bool operator ==(HierarchyObject x, HierarchyObject y)
		{
			if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return ReferenceEquals(x, null) && ReferenceEquals(y, null);

			return x.Equals(y);
		}

		public static bool operator !=(HierarchyObject x, HierarchyObject y)
		{
			return !(x == y);
		}

		public bool Equals(HierarchyObject x, HierarchyObject y)
		{
			if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return ReferenceEquals(x, null) && ReferenceEquals(y, null);
			//if ( x == null || y == null ) return x == null && y == null;
			return x.Equals(y);
		}

		public int GetHashCode(HierarchyObject obj)
		{
			return obj.GetHashCode();
		}

		public int CompareTo(HierarchyObject other)
		{
			if (pluginID == 0) return id - other.id;
			// if ( pluginID == 0 ) return (int)((fileID - other.fileID) % int.MaxValue);

			return project.guid.CompareTo(other.project.guid);
		}

		public bool Equals(HierarchyObject other)
		{
			if (ReferenceEquals(other, null)) return false;
			//  if ( pluginID == 0 ) return fileID == other.fileID;
			if (pluginID == 0) return id == other.id;
			if (project == null || other.project == null) return false;
			return project.guid == other.project.guid;
		}

		public override int GetHashCode()
		{
			//   if ( pluginID == 0 ) return (int)(fileID % int.MaxValue);
			if (pluginID == 0) return id;
			else return project.guid.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;

			return GetHashCode() == obj.GetHashCode();
		}

		bool init_GetIconForExternal;
		TempColorClass _GetIconForExternal;
		static TempColorClass _GetIconForExternal_Empty = new TempColorClass();
		internal TempColorClass GetIconForExternal()
		{
			if (!init_GetIconForExternal)
			{
				if (!go) return _GetIconForExternal_Empty;

				init_GetIconForExternal = true;
				_GetIconForExternal = Cache.LOAD_CUSTOM_ICON_USING_HIGHLIGHTER();
				if (_GetIconForExternal == null) _GetIconForExternal = TODO_Tools.GetObjectBuildinIcon(go, Tools.unityGameObjectType);
			}
			return _GetIconForExternal ?? _GetIconForExternal_Empty;
		}
	}




	internal class HierarchyObject_ProjectExt : ICloneable
	{
		//internal UnityEngine.Object obj;
		internal string guid;
		internal string assetPath;
		internal string assetFolder;
		internal string assetName;
		internal string fileExtension;
		internal bool IsFolder;

		internal Dictionary<int, HierarchyObject> nonMainAssets;

		//#pragma warning disable
		internal bool IsMainAsset = true;
		// internal int? childCount;
		//#pragma warning restore

		internal int? parentCount;
		public int sibling;

		public object Clone()
		{
			return MemberwiseClone();
		}
	}



}
