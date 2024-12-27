using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Structure {

	// to create a new structure extend this class and fill "Grid" with a 2d byte array filled id's in Start
    // example see /Scripts/Structure/Impl/SingleStructure

    public abstract class Structure : MonoBehaviour {
        
        public GameManager gameManager;
        
        protected byte[,] Grid;

        private int offset;

        private void Start() {
            int color = Random.Range(0, 4);
            offset = color * 16;
        }

        private Tile GetTile(byte block) {
            return gameManager.tiles[block + offset];
        }

        private GameObject GetObject(byte block) {
            return gameManager.objects[block + offset];
        }

        private GameObject GetRect(byte block) {
            return gameManager.rects[block + offset];
        }

        public Tile GetTileAtIndex(int i, int j) {
            return GetTile(Grid[i, j]);
        }

        public GameObject GetObjectAtIndex(int i, int j) {
            return GetObject(Grid[i, j]);
        }

        public GameObject GetRectAtIndex(int i, int j) {
            return GetRect(Grid[i, j]);
        }

        public byte[,] GetGrid() {
            return Grid;
        }
    }
}