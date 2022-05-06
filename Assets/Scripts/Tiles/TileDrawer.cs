using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IonCannon.Tiles
{
    public record Point(int x, int y);

    public class TileDrawer : MonoBehaviour
    {
        public Material material1;
        public Material material2;
        public Material material3;
        public static Dictionary<TileType, List<GLTile>> _tiles = new Dictionary<TileType, List<GLTile>>();

        private void Awake()
        {
            Point x = new Point(1,0);
            Point newP = x with { x = 10 };
            var (xx, y) = x;
            Debug.Log(x.GetHashCode());
            var types = TileType.GetValues(typeof(TileType)).Cast<TileType>();
            foreach (var tileType in types)
            {
                _tiles[tileType] = new List<GLTile>();
            }
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
        }
    }
}
