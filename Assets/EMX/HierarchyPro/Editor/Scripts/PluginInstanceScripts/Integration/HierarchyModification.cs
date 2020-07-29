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
    class HierarchyModification
    {
        PluginInstance p;
        internal HierarchyModification(PluginInstance p)
        {
            this.p = p;
        }


        internal Color? hoveredBackgroundColor;
        internal Type gs;
        // internal FieldInfo __m_AssetTreeState, __m_FolderTreeState;

        object GetValue_Field(Type type, string field)
        {
            var res = type.GetField(field, (BindingFlags)(-1));

            if (res == null) return null;

            return res.GetValue(null);
        }

        void SetValue_Field(Type type, string field, object value)
        {
            var res = type.GetField(field, (BindingFlags)(-1));

            if (res == null) return;

            res.SetValue(null, value);
        }

        void FixStyle(GUIStyle style)
        {
            style.fixedHeight = 0;
            style.stretchHeight = true;
            style.alignment = TextAnchor.MiddleLeft;
            style.padding.top = 0;
            style.padding.bottom = 0;
            style.margin.top = 0;
            style.overflow.top = 0;
        }

        bool TryToInitializeDefaultStylesWasInit;
        internal class internalGuiTyle
        {
            internal internalGuiTyle(GUIStyle style)
            {
                this.style = style;
            }
            internal GUIStyle style;
            internal bool setHeight;
            internal void SetHeight(int height)
            {
                if (setHeight) style.fixedHeight = height;
            }
        }
        internal List<internalGuiTyle> INTERNAL_LABEL_STYLES = new List<internalGuiTyle>();
        Color? backBG;
        bool currentDisabled;
        internal void UpdateBGHover()
        {
            if (!p.hashoveredItem) return;

            if (!p.par_e.HIDE_HOVER_BG)
            {
                if (currentDisabled)
                {
                    currentDisabled = false;
                    SetValue_Field(gs, "hoveredBackgroundColor", backBG.Value);
                    hoveredBackgroundColor = backBG.Value;
                }
                return;
            }
            p.ha.DisableHover();

            if (currentDisabled) return;
            currentDisabled = true;

            var v = GetValue_Field(gs, "hoveredBackgroundColor");
            if (v == null) return;
            if (!backBG.HasValue) backBG = (Color)v;

            if (p.par_e.HIDE_HOVER_BG)
            {
                SetValue_Field(gs, "hoveredBackgroundColor", Color.clear);
                hoveredBackgroundColor = Color.clear;
            }

            else
            {
                SetValue_Field(gs, "hoveredBackgroundColor", backBG.Value);
                hoveredBackgroundColor = backBG.Value;
            }
            /*  var v = GetValue_Field(gs, "hoveredBackgroundColor");

			  if ( v == null ) return;

			  if ( !backBG.HasValue ) backBG = (Color)v;


			  if ( p.par_e.HIDE_HOVER_BG )
			  {
				  SetValue_Field( gs, "hoveredBackgroundColor", Color.clear );
				  hoveredBackgroundColor = Color.clear;
			  }

			  else
			  {
				  SetValue_Field( gs, "hoveredBackgroundColor", backBG.Value );
				  hoveredBackgroundColor = backBG.Value;
			  }*/
        }

        internal GUIStyle prebapButtonStyle, hoveredItemBackgroundStyle;

        // GUIStyle sceneHeaderBg;
        internal void TryToInitializeDefaultStyles()
        {
            if (TryToInitializeDefaultStylesWasInit) return;

            TryToInitializeDefaultStylesWasInit = true;

            gs = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameObjectTreeViewGUI+GameObjectStyles") ?? throw new Exception("UnityEditor.GameObjectTreeViewGUI+GameObjectStyles");
            INTERNAL_LABEL_STYLES.Clear();

            if (gs != null)
            {
                /*foreach ( var m in typeof( EditorWindow ).Assembly.GetTypes() )
                 {   if (m.Name.Contains( "GameObjectStyles" ) )
                         Debug.Log( m.FullName );

                 }*/
                /*  gs.get
                  if ( gs != null ) {*/
                //UnityEditor.GameObjectTreeViewGUI.GameObjectStyles

                var ls = typeof(EditorWindow).Assembly.GetType("UnityEditor.IMGUI.Controls.TreeViewGUI+Styles");

                if (ls != null)
                {
                    var l = GetValue_Field(ls, "lineStyle") as GUIStyle;

                    if (l != null)
                    {
                        FixStyle(l);
                        INTERNAL_LABEL_STYLES.Add(new internalGuiTyle(l));
                    }
                }

                if (ls != null)
                {
                    var l = GetValue_Field(ls, "lineBoldStyle") as GUIStyle;

                    if (l != null)
                    {
                        FixStyle(l);
                        INTERNAL_LABEL_STYLES.Add(new internalGuiTyle(l));
                    }
                }



                var disabledLabel = GetValue_Field(gs, "disabledLabel") as GUIStyle;

                if (disabledLabel != null)
                {
                    FixStyle(disabledLabel);
                    INTERNAL_LABEL_STYLES.Add(new internalGuiTyle(disabledLabel));

                    if (UnityVersion.UNITY_CURRENT_VERSION >= UnityVersion.UNITY_2019_VERSION && UnityVersion.UNITY_CURRENT_VERSION < UnityVersion.UNITY_2019_3_0_VERSION) disabledLabel.padding.bottom = 2;
                }


                var sceneHeaderBg = GetValue_Field(gs, "sceneHeaderBg") as GUIStyle;

                if (sceneHeaderBg != null)
                {
                    FixStyle(sceneHeaderBg);
                    INTERNAL_LABEL_STYLES.Add(new internalGuiTyle(sceneHeaderBg) { setHeight = true });

                    if (UnityVersion.UNITY_CURRENT_VERSION >= UnityVersion.UNITY_2019_VERSION && UnityVersion.UNITY_CURRENT_VERSION < UnityVersion.UNITY_2019_3_0_VERSION) disabledLabel.padding.bottom = 2;
                }




                var prefabLabel = GetValue_Field(gs, "prefabLabel") as GUIStyle;
                var disabledPrefabLabel = GetValue_Field(gs, "disabledPrefabLabel") as GUIStyle;
                var brokenPrefabLabel = GetValue_Field(gs, "brokenPrefabLabel") as GUIStyle;
                var disabledBrokenPrefabLabel = GetValue_Field(gs, "disabledBrokenPrefabLabel") as GUIStyle;

                // sceneHeaderBg = GetValue_Field(gs, "sceneHeaderBg" ) as GUIStyle;
                if (prefabLabel != null)
                {
                    FixStyle(prefabLabel);
                    INTERNAL_LABEL_STYLES.Add(new internalGuiTyle(prefabLabel));
                }

                if (disabledPrefabLabel != null)
                {
                    FixStyle(disabledPrefabLabel);
                    INTERNAL_LABEL_STYLES.Add(new internalGuiTyle(disabledPrefabLabel));
                }

                if (brokenPrefabLabel != null)
                {
                    FixStyle(brokenPrefabLabel);
                    INTERNAL_LABEL_STYLES.Add(new internalGuiTyle(brokenPrefabLabel));
                }

                if (disabledBrokenPrefabLabel != null)
                {
                    FixStyle(disabledBrokenPrefabLabel);
                    INTERNAL_LABEL_STYLES.Add(new internalGuiTyle(disabledBrokenPrefabLabel));
                }


                if (p.ha.hasShowingPrefabHeader)
                {
                    prebapButtonStyle = GetValue_Field(gs, "rightArrow") as GUIStyle;
                    prebapButtonStyle.margin.right = 0;
                }

                if (p.hashoveredItem)
                {
                    hoveredItemBackgroundStyle = GetValue_Field(gs, "hoveredItemBackgroundStyle") as GUIStyle;
                }
                
                //  }
            }
            else
            {
                throw new Exception("Cannot read styles");
            }



            var ns = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneVisibilityHierarchyGUI+Styles") ?? throw new Exception("UnityEditor.SceneVisibilityHierarchyGUI+Styles");

            if (ns != null)
            {
                var vicContent = GetValue_Field(ns, "sceneVisibilityStyle") as GUIStyle;

                if (vicContent != null) FixStyle(vicContent);

                INTERNAL_LABEL_STYLES.Add(new internalGuiTyle(vicContent));
            }



            {//PING
                var GameObjectTreeViewGUI = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameObjectTreeViewGUI") ?? throw new Exception("UnityEditor.GameObjectTreeViewGUI");
                var treeViewBaseType = GameObjectTreeViewGUI.BaseType;
                //var m_Ping = treeViewBaseType.GetField("m_Ping", ~(BindingFlags.Static | BindingFlags.InvokeMethod))?? throw new Exception("m_Ping");
                //var  m_PingStyle = m_Ping.FieldType.GetField("m_PingStyle", ~(BindingFlags.Static | BindingFlags.InvokeMethod))?? throw new Exception("m_PingStyle");
                var m_Ping = treeViewBaseType.GetProperty("pingStyle", ~(BindingFlags.Static | BindingFlags.InvokeMethod)) ?? throw new Exception("pingStyle");

                var style = m_Ping.GetValue(p.gui_currentTree, null) as GUIStyle;
                FixStyle(style);
                m_Ping.SetValue(p.gui_currentTree, style, null);

                INTERNAL_LABEL_STYLES.Add(new internalGuiTyle(style) { setHeight = true });
            }


            /*  gs =  typeof( EditorWindow ).Assembly.GetType( "UnityEditor.PrefabUtility+GameObjectStyles" );
              if ( gs != null )
              {

                  var prefabLabel =  GetValue_Field(gs, "prefabLabel") as GUIStyle;
                  var disabledPrefabLabel = GetValue_Field( gs, "disabledPrefabLabel") as GUIStyle;
                  var brokenPrefabLabel = GetValue_Field(gs, "brokenPrefabLabel") as GUIStyle;
                  var disabledBrokenPrefabLabel = GetValue_Field(gs, "disabledBrokenPrefabLabel") as GUIStyle;
                  if ( prefabLabel != null ) FixStyle( prefabLabel );
                  if ( disabledPrefabLabel != null ) FixStyle( disabledPrefabLabel );
                  if ( brokenPrefabLabel != null ) FixStyle( brokenPrefabLabel );
                  if ( disabledBrokenPrefabLabel != null ) FixStyle( disabledBrokenPrefabLabel );
                  //  }
              }*/
        }







        float? _TOTAL_LEFT_PADDING;
        internal float LEFT_PADDING
        {
            get
            {
                if (p.pluginID != 0) return 0;

                if (!_TOTAL_LEFT_PADDING.HasValue)
                {
                    if (UnityVersion.UNITY_CURRENT_VERSION >= UnityVersion.UNITY_2019_VERSION)
                    {
                        _TOTAL_LEFT_PADDING =
                            (float)p.SceneHierarchyWindowRoot.Assembly.GetType("UnityEditor.SceneVisibilityHierarchyGUI").GetField("utilityBarWidth", (BindingFlags)(-1))
                                .GetValue(null);
                        _TOTAL_LEFT_PADDING = _TOTAL_LEFT_PADDING.Value;
                    }
                    else _TOTAL_LEFT_PADDING = 0;
                }
                return _TOTAL_LEFT_PADDING.Value;
            }
        }

        internal float PREFAB_BUTTON_SIZE
        {
            get
            { //if ( UNITY_CURRENT_VERSION >= UNITY_2019_1_1_VERSION ) return 0;

                return p.pluginID == 0 ? Tools.singleLineHeight : 0;
            }
        }
    }
}
