using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour {
    
    [SerializeField] public List<Tile> tiles;
    [SerializeField] public List<GameObject> objects;
    [SerializeField] public List<GameObject> rects;
}