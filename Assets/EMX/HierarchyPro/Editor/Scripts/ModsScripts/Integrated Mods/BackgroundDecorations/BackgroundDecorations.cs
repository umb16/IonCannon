using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEditor.IMGUI.Controls;

namespace EMX.HierarchyPlugin.Editor.Mods
{





	internal class BackgroundDecorations
	{

		PluginInstance p;
		internal BackgroundDecorations( PluginInstance p )
		{
			this.p = p;
		}


		internal void Subscribe( EditorSubscriber sbs )
		{
			// sbs.OnPlayModeStateChanged += PlaymodeStateChanged;
			sbs.BuildedOnGUI_first += PreCalcRect;
			sbs.BuildedOnGUI_last_butBeforeGL += LastCalcRect;
			//  sbs.BuildedOnGUI_middle += Draw;
		}
		

		void PreCalcRect( )
		{

			if ( p.EVENT.type == EventType.Repaint && (p.par_e.DRAW_HIERARHCHY_CHESS_FILLS != 0 || p.par_e.DRAW_HIERARHCHY_CHESS_LINES != 0) )
			{
				p.par_e.DEFAULT_SHADER_SHADER.ExternalMaterialReference.SetPass( 0 );
				GL.PushMatrix();
				GL.Begin( GL.QUADS );
				if ( p.par_e.DRAW_HIERARHCHY_CHESS_FILLS != 0 ) DRAW_FILL();
				if ( p.par_e.DRAW_HIERARHCHY_CHESS_LINES != 0 ) DRAW_HR();
				// if ( p.par_e.DRAW_HIERARHCHY_CHESS == 2 || p.modsController.rightModsManager.CheckSpecialButtonIfRightHidingEnabled() ) DrawChess( selectionRect, selectionRect.x + selectionRect.width );
				// else if ( p.par_e.DRAW_HIERARHCHY_CHESS == 1 ) DrawChess( selectionRect, fadeRect.x );
				GL.End();
				GL.PopMatrix();
			}

		}

		void LastCalcRect( )
		{
			if ( p.EVENT.type == EventType.Repaint && p.par_e.DRAW_HIERARHCHY_CHILD_LINES != 0 )
			{
				p.par_e.DEFAULT_SHADER_SHADER.ExternalMaterialReference.SetPass( 0 );
				GL.PushMatrix();
				GL.Begin( GL.QUADS );
				CHILD_LINES();
				GL.End();
				GL.PopMatrix();
			}

			if ( p.EVENT.type == EventType.Repaint && p.par_e.DRAW_CHILDS_COUNT != 0 )
			{
				CHILD_COUNT();
			}



			//CHILD COUNT
		}
		private void CHILD_LINES( )
		{
			var OFF = p.par_e.USE_CHILD_INDENT ? p.par_e.CHILD_INDENT :    (int) (float) Window.k_IndentWidth.GetValue( p.gui_currentTree  );
			;
			if ( p.ha.IS_SEARCH_MOD_OPENED() ) return;

			var         activeColor = p.par_e.COLOR_HIERARHCHY_CHILD_LINES;
			var         passiveColor = p.par_e.COLOR_HIERARHCHY_CHILD_LINES;
			passiveColor.a /= 4;
			Rect r1, r2,r3;

			for ( int i = 0 ; i < p.drew_mods_count ; i++ )
			{
				var o = p.drew_mods_objects[i];
				// selectionRect.width -= 40;
				r1 = o.lastSelectionRect;
				r1.x -= 8;
				// r.x += TOTAL_LEFT_PADDING;
				if ( !o.ParentIsNull() )
				{
					if ( o.Active() )
					{
						// tempColor = GUI.color;
						r2 = r1;
						r2.x -= OFF;
						r2.y += r2.height / 2f;
						r2.height = 1;
						// if ( IS_HIERARCHY() ) 
						r2.width = OFF;
						//  else r2.width = OFF / 2;
						r2.width -= 3;
						// if ( !o.Active() ) activeColor.a /= 4;
						DRAW( r2, activeColor );
						// GUI.DrawTexture( r2, Texture2D.whiteTexture );
						if ( o.ChildCount() == 0 )
						{
							r2.x += 5 + 7 + OFF - 14;
							r2.y -= 2;
							r2.width = r2.height = 4;
							//   GUI.DrawTexture( r2, Texture2D.whiteTexture );
							DRAW( r2, activeColor );
						}
						// GUI.color = tempColor;
					}
				}
				r3 = r1;
				bool first = true;
				var _parent = o.go.transform;
				while ( _parent != null )
				{
					if ( _parent.parent == null ) break;

					r3.width = 1;
					r3.x -= OFF;
					r3.y = r1.y;
					r3.height = r1.height;

					if ( _parent.parent != null && _parent.GetSiblingIndex() == _parent.parent.childCount - 1 )
					{
						if ( !first )
						{
							_parent = _parent.parent;
							continue;
						}
						r3.height /= 2;
					} /*else if (o.transform.childCount != 0) {
				
                }*/

					// tempColor = GUI.color;
					// activeColor = CHILDREN_LINE_COLOR;

					// if ( _parent.parent && !_parent.parent.gameObject.activeInHierarchy ) activeColor.a /= 4;
					// GUI.color = activeColor;
					//    GUI.DrawTexture( r3, Texture2D.whiteTexture );
					DRAW( r3, _parent.parent && !_parent.parent.gameObject.activeInHierarchy ? passiveColor : activeColor );
					// GUI.color = tempColor;
					first = false;
					_parent = _parent.parent;
					// parCount++;
				}
			}

		}


		private void DRAW_FILL( ) //Rect selectionRect, float content
		{
			var rect = p._first_FullLineRect;
			rect.x = p.hierarchyModification.LEFT_PADDING;
			/// if ( p.par_e.DRAW_HIERARHCHY_CHESS_FILLS == 1 ) rect.width = p._first_FullLineRect.x + p._first_FullLineRect.width - p.rightOffset - rect.x;
			if ( p.par_e.DRAW_HIERARHCHY_CHESS_FILLS == 1 ) rect.width = p._first_FullLineRect.x + p._first_FullLineRect.width - p.rightOffset - rect.x;
			// rect.width += PREFAB_BUTTON_SIZE;
			var col = p.par_e.COLOR_HIERARHCHY_CHESS_FILLS;
			int i = 0;
			do
			{
				// var cc = Mathf.RoundToInt(rect.y / rect.height) % 2;
				//  if ( cc == 0 ) 
				DRAW( rect, col );
				rect.y += rect.height;
				rect.y += rect.height;
				i++;

			} while ( rect.y < p._last_FullLineRect.y + p._last_FullLineRect.height );
			//  Graphics.DrawTexture( rect, Texture2D.whiteTexture, rect, 0, 0, 0, 0,  );
		}


		void DRAW_HR( )
		{
			var rect = p._first_FullLineRect;
			rect.x = p.hierarchyModification.LEFT_PADDING;
			//  if ( p.par_e.DRAW_HIERARHCHY_CHESS_LINES == 1 ) rect.width = p._first_FullLineRect.x + p._first_FullLineRect.width - p.rightOffset - rect.x;
			if ( p.par_e.DRAW_HIERARHCHY_CHESS_LINES == 1 ) rect.width = p._first_FullLineRect.x + p._first_FullLineRect.width - p.rightOffset - rect.x;
			// rect.width += PREFAB_BUTTON_SIZE;
			var col = p.par_e.COLOR_HIERARHCHY_CHESS_LINES;

			do
			{
				// var cc = Mathf.RoundToInt(rect.y / rect.height) % 2;
				//  if ( cc == 0 )
				var r= rect;
				r.y -= 1;
				r.height = 1;
				DRAW( r, col );
				rect.y += rect.height;

			} while ( rect.y < p._last_FullLineRect.y + p._last_FullLineRect.height );
		}


		int _MainTex = Shader.PropertyToID("_MainTex");
		Material childMaterial = null;
		IconData childTexture;
		object[] obj_array = new object[1];
		float childTextureWidthFull;
		void CHILD_COUNT( )
		{
			if ( Event.current.type != EventType.Repaint ) return;

			if ( !childMaterial ) childMaterial = new Material( p.par_e.DEFAULT_SHADER_SHADER.ExternalMaterialReference );
			childTexture = p.GetNewIcon( NewIconTexture.RightMods, p.par_e.DRAW_CHILDS_INVERCE_COLOR ? "N1_I" : "N1" );
			childMaterial.SetTexture( _MainTex, childTexture.texture );
			childMaterial.SetPass( 0 );
			GL.PushMatrix();
			GL.Begin( GL.QUADS );
			GL.Color( p.par_e.DRAW_CHILDS_COLOR );

			// if ( p.par_e.DRAW_HIERARHCHY_CHESS == 2 || p.modsController.rightModsManager.CheckSpecialButtonIfRightHidingEnabled() ) DrawChess( selectionRect, selectionRect.x + selectionRect.width );
			// else if ( p.par_e.DRAW_HIERARHCHY_CHESS == 1 ) DrawChess( selectionRect, fadeRect.x );


			var hide_root=  p.par_e.HIDE_CHILDCOUNT_IFROOT;
			var hide_expanded=  p.par_e.HIDE_CHILDCOUNT_IFEXPANDED;

			var offset_x = p.par_e.CHILDCOUNT_OFFSET_X;
			var offset_y = p.par_e.CHILDCOUNT_OFFSET_Y;
			var aligment = p.par_e.CHILDCOUNT_ALIGMENT;

			childTextureWidthFull = childTexture.texture.width;

			Rect r1;

			for ( int i = 0 ; i < p.drew_mods_count ; i++ )
			{
				var o = p.drew_mods_objects[i];
				var c = o.ChildCount();

				if ( c != 0 )
				{
					bool draw = !(hide_root && o.ParentIsNull());

					if ( draw && hide_expanded )
					{
						obj_array[ 0 ] = o.id;
						draw = !(bool)p.data_m_dataIsExpanded.Invoke( p.data_currentTree, obj_array );
					}

					if ( draw )
					{
						r1 = o.lastSelectionRect;
						r1.y += (r1.height - EditorGUIUtility.singleLineHeight) / 2;
						switch ( aligment )
						{
							case 0:
								r1.x = 0;
								break;
							case 1:
								r1.x -= 3 + EditorGUIUtility.singleLineHeight;
								break;

							case 2:
								r1.x += o.GetContentSize().x + EditorGUIUtility.singleLineHeight / 1.75f;
								break;
						}

						//  r.x += TOTAL_LEFT_PADDING;
						//  r.width -= TOTAL_LEFT_PADDING;
						r1.height = r1.width = EditorGUIUtility.singleLineHeight;
						r1.x += offset_x;
						r1.y += offset_y;
						DRAW_SMALL_NUMBER( r1, c );
					}
				}
			}

			GL.End();
			GL.PopMatrix();

		}
		Vector3 tv3;
		internal void DRAW_SMALL_NUMBER( Rect rect, int number )
		{

			//	rect.x = rect.x + rect.width - childTexture.width;
			var oldH = rect.height;
			rect.height = 11;
			rect.y += (oldH - rect.height) / 2;
			var scale = rect.height / oldH;
			rect.width =scale * childTexture.width;
			float childSpace = -5;
			for ( int i = 0 ; number != 0 ; i++ )
			{

				_draw_one_number( rect, number%10 );
				number /=  10;

				rect.x -= rect.width + childSpace;
			}




			/* rect.x -= 1;
             smallNumbStyle.Draw( rect, number.ToString(), false, false, false, false );
             rect.x += 2;
             rect.y += 1;
             smallNumbStyle.Draw( rect, number.ToString(), false, false, false, false );
             rect.x -= 1;
             rect.y -= 1;*/
			///smallNumbStyleNormal.Draw( rect, number.ToString(), false, false, false, false );
		}

		void _draw_one_number( Rect rect, int number )
		{
			if ( number < 0 || number > 9 ) throw new Exception( "Number error " + number );

			var sv = childTexture.uv_start;
			var ev = childTexture.uv_end;
			var offset = (((9 + number)%10) * childTexture.width /childTextureWidthFull );
			sv.x += offset;
			ev.x += offset;

			tv3.Set( sv.x, sv.y, 0 );
			GL.TexCoord( tv3 );
			GL.Vertex3( rect.x, rect.y, 0 );

			tv3.Set( sv.x, ev.y, 0 );
			GL.TexCoord( tv3 );
			GL.Vertex3( rect.x, rect.y + rect.height, 0 );

			tv3.Set( ev.x, ev.y, 0 );
			GL.TexCoord( tv3 );
			GL.Vertex3( rect.x + rect.width, rect.y + rect.height, 0 );

			tv3.Set( ev.x, sv.y, 0 );
			GL.TexCoord( tv3 );
			GL.Vertex3( rect.x + rect.width, rect.y, 0 );
		}


		GUIStyle __smallNumbStyle;

		GUIStyle smallNumbStyle
		{
			get
			{
				if ( __smallNumbStyle == null )
				{
					__smallNumbStyle = new GUIStyle( p.label );
					__smallNumbStyle.alignment = TextAnchor.MiddleCenter;
					smallNumbStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.black : Color.white;
				}

				__smallNumbStyle.fontSize = p.FONT_8() - 1;
				return __smallNumbStyle;
			}
		}

		GUIStyle __smallNumbStyleNormal;

		GUIStyle smallNumbStyleNormal
		{
			get
			{
				if ( __smallNumbStyleNormal == null )
				{
					__smallNumbStyleNormal = new GUIStyle( p.label );
					__smallNumbStyleNormal.alignment = TextAnchor.MiddleCenter;
				}

				__smallNumbStyleNormal.fontSize = p.FONT_8() - 1;
				return __smallNumbStyleNormal;
			}
		}



		void DRAW( Rect rect, Color color )
		{
			GL.Color( color );
			GL.Vertex3( rect.x, rect.y, 0 );
			GL.Vertex3( rect.x, rect.y + rect.height, 0 );
			GL.Vertex3( rect.x + rect.width, rect.y + rect.height, 0 );
			GL.Vertex3( rect.x + rect.width, rect.y, 0 );
		}
	}

}
