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



		// 0 - Hide / 1 - Left Narrow ; 1 - Left Wide ; 2 - Icon
		internal int HIGHLIGHTER_HIERARCHY_BUTTON_LOCATION { get { return GET("HIGHLIGHTER_HIERARCHY_BUTTON_LOCATION", 1); } set { SET("HIGHLIGHTER_HIERARCHY_BUTTON_LOCATION", value); } }
		// 0 - Hide / 1 - Show Window / 2 - Show Always
		internal int HIGHLIGHTER_HIERARCHY_DRAW_BUTTON_RECTMARKER { get { return GET("HIGHLIGHTER_HIERARCHY_DRAW_BUTTON_RECTMARKER", 2); } set { SET("HIGHLIGHTER_HIERARCHY_DRAW_BUTTON_RECTMARKER", value); } }
		internal int HIGHLIGHTER_HIERARCHY_BUTTON_RECTMARKER_SIZE { get { return GET("HIGHLIGHTER_HIERARCHY_BUTTON_RECTMARKER_SIZE", 3); } set { SET("HIGHLIGHTER_HIERARCHY_BUTTON_RECTMARKER_SIZE", value); } }
		// 0 - every line separately / 1 - group close lines
		internal int HIGHLIGHTER_GROUPING_CHILD_MODE { get { return GET("HIGHLIGHTER_GROUPING_CHILD_MODE", 1); } set { SET("HIGHLIGHTER_GROUPING_CHILD_MODE", value); } }


		

		internal float HIGHLIGHTER_COLOR_OPACITY { get { return GET("HIGHLIGHTER_COLOR_OPACITY", 1f); } set { SET("HIGHLIGHTER_COLOR_OPACITY", value); } }
		internal int HIGHLIGHTER_USE_SPECUAL_SHADER_TYPE { get { return GET("HIGHLIGHTER_USE_SPECUAL_SHADER_TYPE", 0); } set { SET("HIGHLIGHTER_USE_SPECUAL_SHADER_TYPE", value); } }
		internal bool HIGHLIGHTER_USE_SPECUAL_SHADER { get { return GET("HIGHLIGHTER_USE_SPECUAL_SHADER", true); } set { SET("HIGHLIGHTER_USE_SPECUAL_SHADER", value); } }

	

		internal int HIGHLIGHTER_DEFAULT_ICON_SIZE { get { return GET("HIGHLIGHTER_DEFAULT_ICON_SIZE", 16); } set { SET("HIGHLIGHTER_DEFAULT_ICON_SIZE", value); } }


		internal int HIGHLIGHTER_BGCOLOR_PADDING { get { return GET("HIGHLIGHTER_BGCOLOR_PADDING", 0); } set { SET("HIGHLIGHTER_BGCOLOR_PADDING", value); } }
		internal int HIGHLIGHTER_TEXTURE_STYLE { get { return GET("HIGHLIGHTER_TEXTURE_STYLE", 3); } set { SET("HIGHLIGHTER_TEXTURE_STYLE", value); } }
		internal int HIGHLIGHTER_TEXTURE_BORDER { get { return GET("HIGHLIGHTER_TEXTURE_BORDER", 6); } set { SET("HIGHLIGHTER_TEXTURE_BORDER", value); } }
		internal bool HIGHLIGHTER_TEXTURE_BORDER_ALLOW { get { return HIGHLIGHTER_TEXTURE_STYLE != 0; } }


		bool? _HIGHLIGHTER_USE_LABEL_OFFSET;
		internal bool HIGHLIGHTER_USE_LABEL_OFFSET
		{
			get { return _HIGHLIGHTER_USE_LABEL_OFFSET ?? (_HIGHLIGHTER_USE_LABEL_OFFSET = HIGHLIGHTER_TEXTURE_GUID == "19b7d3f9eb031ad4a9d63d48600cb49b").Value; }
		}

		internal bool HIGHLIGHTER_TEXTURE_GUID_ALLOW { get { return HIGHLIGHTER_TEXTURE_STYLE == 3; } }
		internal string HIGHLIGHTER_TEXTURE_GUID
		{
			get { return GET("HIGHLIGHTER_TEXTURE_GUID", "19b7d3f9eb031ad4a9d63d48600cb49b"); }
			set
			{
				SET("HIGHLIGHTER_TEXTURE_GUID", value);
				_HIGHLIGHTER_USE_LABEL_OFFSET = null;
			}
		}


		internal int HIGHLIGHTER_LEFT_OVERFLOW
		{
			get
			{
				if (UnityVersion.UNITY_CURRENT_VERSION < UnityVersion.UNITY_2019_VERSION) return 0;
				return GET("HIGHLIGHTER_LEFT_OVERFLOW", 1);
			}
			set { SET("HIGHLIGHTER_LEFT_OVERFLOW", value); }
		}


		bool HIghlighterExternalTexture_init;
		Texture2D _HIghlighterExternalTexture;
		internal Texture2D HIghlighterExternalTexture
		{
			get
			{
				if (!HIghlighterExternalTexture_init)
				{
					HIghlighterExternalTexture_init = true;
					if (!string.IsNullOrEmpty(HIGHLIGHTER_TEXTURE_GUID))
					{
						var path = AssetDatabase.GUIDToAssetPath(HIGHLIGHTER_TEXTURE_GUID);
						if (!string.IsNullOrEmpty(path)) _HIghlighterExternalTexture = AssetDatabase.LoadAssetAtPath<Texture>(path) as Texture2D;
					}
				}
				return _HIghlighterExternalTexture;
			}

			set
			{
				if (value != HIghlighterExternalTexture)
				{
					_HIghlighterExternalTexture = value;
					if (!value) HIGHLIGHTER_TEXTURE_GUID = "";
					else
					{
						var path = AssetDatabase.GetAssetPath(value);
						if (!string.IsNullOrEmpty(path))
						{
							var guid = AssetDatabase.AssetPathToGUID(path);
							if (!string.IsNullOrEmpty(guid)) HIGHLIGHTER_TEXTURE_GUID = guid;
							else HIGHLIGHTER_TEXTURE_GUID = "";
						}
						else HIGHLIGHTER_TEXTURE_GUID = "";
					}
					if (HIGHLIGHTER_TEXTURE_GUID == "") _HIghlighterExternalTexture = null;
				}
			}
		}
		GUIStyle __BG_TEXTURE_STYLE;

		internal GUIStyle BG_TEXTURE_STYLE
		{
			get { return __BG_TEXTURE_STYLE ?? (__BG_TEXTURE_STYLE = new GUIStyle()); }
		}

		Texture2D __BG_TEXTURE_TEXT;
		Texture2D __BG_TEXTURE_BOX;
		Rect tr;

		internal Texture2D BG_TEXTURE
		{
			get
			{
				if (HIGHLIGHTER_TEXTURE_STYLE == 0) return Texture2D.whiteTexture;
				if (HIGHLIGHTER_TEXTURE_STYLE == 1)
				{
					if (__BG_TEXTURE_BOX == null) __BG_TEXTURE_BOX = Root.p[0].GET_SKIN().box.normal.background ?? Root.p[0].GET_SKIN().box.normal.scaledBackgrounds[0];
					return __BG_TEXTURE_BOX;
				}
				if (HIGHLIGHTER_TEXTURE_STYLE == 2)
				{
					if (__BG_TEXTURE_TEXT == null) __BG_TEXTURE_TEXT = Root.p[0].GET_SKIN().textArea.normal.background ?? Root.p[0].GET_SKIN().textArea.normal.scaledBackgrounds[0];
					return __BG_TEXTURE_TEXT;
				}
				if (HIGHLIGHTER_TEXTURE_STYLE == 3) return HIghlighterExternalTexture;
				return null;
			}
		}







		internal class SHADER_HELPER
		{
			string keyMat /*, keyShader*/;
			PluginInstance adapter;

			internal SHADER_HELPER(string key, PluginInstance adapter)
			{
				this.adapter = adapter;
				this.keyMat = key + "-Material";
				// this.keyShader = key + "-Shader";
			}

			internal Func<string> GET_SHADER_GUID;
			internal Func<string> GET_SHADER_LOCAL_PATH;
			internal Action<string> SET_SHADER_GUID;

			Shader oldSHader;
			Material _HIghlighterExternalMaterial;



			int matID
			{
				get { return adapter.par_e.GET(keyMat, -1); }
				set { adapter.par_e.SET(keyMat, value); }
			}


			internal Material ExternalMaterialReference
			{
				get
				{
					if (oldSHader != ExternalShaderReference)
					{
						oldSHader = ExternalShaderReference;
						if (oldSHader == null) _HIghlighterExternalMaterial = null;
						else _HIghlighterExternalMaterial = new Material(_HIghlighterExternalShader);
						matID = _HIghlighterExternalMaterial == null ? -1 : _HIghlighterExternalMaterial.GetInstanceID();
					}
					if (!_HIghlighterExternalMaterial && matID != -1)
					{
						_HIghlighterExternalMaterial = EditorUtility.InstanceIDToObject(matID) as Material;
						if (!_HIghlighterExternalMaterial && ExternalShaderReference)
						{
							oldSHader = null;
							return ExternalMaterialReference;
						}
					}
					return _HIghlighterExternalMaterial;
				}
			}


			bool HIghlighterExternalShader_init;
			Shader _HIghlighterExternalShader;

			internal Shader ExternalShaderReference
			{
				get
				{
					if (!HIghlighterExternalShader_init || _HIghlighterExternalShader == null)
					{
						HIghlighterExternalShader_init = true;

						if (!string.IsNullOrEmpty(GET_SHADER_GUID()))
						{
							var path = AssetDatabase.GUIDToAssetPath(GET_SHADER_GUID());
							if (string.IsNullOrEmpty(path) && GET_SHADER_LOCAL_PATH != null) path = Folders.PluginInternalFolder + GET_SHADER_LOCAL_PATH();
							if (!string.IsNullOrEmpty(path))
							{
								_HIghlighterExternalShader = AssetDatabase.LoadAssetAtPath<Shader>(path) as Shader;
							}
						}
					}

					return _HIghlighterExternalShader;
				}

				set
				{
					if (value != ExternalShaderReference)
					{
						_HIghlighterExternalShader = value;

						if (!value)
						{
							SET_SHADER_GUID("");
						}

						else
						{
							var path = AssetDatabase.GetAssetPath(value);

							if (!string.IsNullOrEmpty(path))
							{
								var guid = AssetDatabase.AssetPathToGUID(path);

								if (!string.IsNullOrEmpty(guid))
								{
									SET_SHADER_GUID(guid);
								}

								else SET_SHADER_GUID("");
							}

							else SET_SHADER_GUID("");
						}

						if (GET_SHADER_GUID() == "") _HIghlighterExternalShader = null;
					}
				}
			}
		}


		internal SHADER_HELPER _DEFAULT_SHADER_SHADER;
		internal SHADER_HELPER DEFAULT_SHADER_SHADER
		{
			get
			{
				if (_DEFAULT_SHADER_SHADER == null)
				{
					_DEFAULT_SHADER_SHADER = new SHADER_HELPER("DEFAULT_SHADER_SHADER", Root.p[pluginID])
					{
						SET_SHADER_GUID = (guid) => { },
						GET_SHADER_GUID = () => { return "70c76382e3a8a0e4f9f719883a135eff"; },
						GET_SHADER_LOCAL_PATH = () => { return "/Editor/Materials/Highlighter - Default GUI Shader.shader"; }
					};
				}

				return _DEFAULT_SHADER_SHADER;
			}
		}
		internal Material HIghlighterExternalMaterialNormal
		{
			get { return SHADER_A.ExternalMaterialReference; }
		}
		internal Material HIghlighterExternalMaterial
		{
			get { return HIGHLIGHTER_USE_SPECUAL_SHADER_TYPE == 0 ? SHADER_A.ExternalMaterialReference : SHADER_B.ExternalMaterialReference; }
		}
		internal string HIGHLIGHTER_SHADER_GUID_MAIN { get { return GET("HIGHLIGHTER_SHADER_GUID_MAIN", "830e0b361750b98468ce6493b692d717"); } set { SET("HIGHLIGHTER_SHADER_GUID_MAIN", value); } }
		internal string HIGHLIGHTER_SHADER_GUID_ADD { get { return GET("HIGHLIGHTER_SHADER_GUID_ADD", "12ace602f83e8b941a0cec6ee38c1a79"); } set { SET("HIGHLIGHTER_SHADER_GUID_ADD", value); } }



		SHADER_HELPER _SHADER_A, _SHADER_B;

		SHADER_HELPER SHADER_A
		{
			get
			{
				if (_SHADER_A == null)
				{
					_SHADER_A = new SHADER_HELPER("SHADER_A", Root.p[pluginID])
					{
						SET_SHADER_GUID = (guid) => { HIGHLIGHTER_SHADER_GUID_MAIN = guid; },
						GET_SHADER_GUID = () => { return HIGHLIGHTER_SHADER_GUID_MAIN; },
						GET_SHADER_LOCAL_PATH = () => { return "/Editor/Materials/Textures for background/Highlighter - Neon Background.shader"; }
					};
				}

				return _SHADER_A;
			}
		}

		SHADER_HELPER SHADER_B
		{
			get
			{
				if (_SHADER_B == null)
				{
					_SHADER_B = new SHADER_HELPER("SHADER_B", Root.p[pluginID])
					{
						SET_SHADER_GUID = (guid) => { HIGHLIGHTER_SHADER_GUID_ADD = guid; },
						GET_SHADER_GUID = () => { return HIGHLIGHTER_SHADER_GUID_ADD; },
						GET_SHADER_LOCAL_PATH = () => { return "/Editor/Materials/Textures for background/Highlighter - Neon Background Soft Additive.shader"; }
					};
				}

				return _SHADER_B;
			}
		}


	}
}
