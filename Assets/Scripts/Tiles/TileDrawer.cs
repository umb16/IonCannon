using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks.Linq;

namespace IonCannon.Tiles
{
    public class TileDrawer : MonoBehaviour
    {
        public Material material1;
        public Material material2;
        public Material material3;
        public Material material4;
        [SerializeField] private Color _color1;
        [SerializeField] private Color _color2;
        [SerializeField] private Color _color3;
        [SerializeField] private Color _color4;
        [SerializeField] private Color _lineColor1;
        [SerializeField] private Color _lineColor2;
        [SerializeField] private Color _lineColor3;
       // [SerializeField] private float _liquidDropSize = 2;
        private Camera _camera;
        public static Dictionary<TileType, List<GLTile>> _tiles = new Dictionary<TileType, List<GLTile>>();

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            var types = TileType.GetValues(typeof(TileType)).Cast<TileType>();
            foreach (var tileType in types)
            {
                _tiles[tileType] = new List<GLTile>();
            }
        }

        public void Update()
        {
#if UNITY_EDITOR
            _camera.backgroundColor = _color1;
            material1.SetColor("_BackColor", _color1);
            
            material1.SetColor("_BaseColor", _color2);
            material2.SetColor("_BackColor", _color2);

            material2.SetColor("_BaseColor", _color3);
            material3.SetColor("_BackColor", _color3);

            material3.SetColor("_BaseColor", _color4);

            material1.SetColor("_LineColor", _lineColor1);
            material2.SetColor("_LineColor", _lineColor2);
            material3.SetColor("_LineColor", _lineColor3);
#endif
        }

        public void OnPostRender()
        {
            material1.SetPass(0);
            GL.Begin(GL.QUADS);
            foreach (var tile in _tiles[TileType.Standart])
            {
                tile.Draw();
            }
            //Quad(Vector3.zero, 2, false, false);
            GL.End();

            material2.SetPass(0);
            GL.Begin(GL.QUADS);
            foreach (var tile in _tiles[TileType.Standart2])
            {
                tile.Draw();
            }
            //Quad(Vector3.zero, 2, false, false);
            GL.End();
            material3.SetPass(0);
            GL.Begin(GL.QUADS);
            foreach (var tile in _tiles[TileType.Standart3])
            {
                tile.Draw();
            }
            //Quad(Vector3.zero, 2, false, false);
            GL.End();
            /*material4.SetPass(0);
            GL.Begin(GL.QUADS);
            foreach (var pos in LiquidTest.Instance.xxxx.Results)
            {
                if (pos.x == float.PositiveInfinity)
                    continue;
                GL.TexCoord2(0,0);
                GL.Vertex3(pos.x- _liquidDropSize, pos.y- _liquidDropSize, 0);
                 GL.TexCoord2(0,1);
                GL.Vertex3(pos.x - _liquidDropSize, pos.y + _liquidDropSize, 0);
                 GL.TexCoord2(1,1);
                GL.Vertex3(pos.x + _liquidDropSize, pos.y + _liquidDropSize, 0);
                 GL.TexCoord2(1,0);
                GL.Vertex3(pos.x + _liquidDropSize, pos.y - _liquidDropSize, 0);
            }
            GL.End();*/
        }
    }
}
