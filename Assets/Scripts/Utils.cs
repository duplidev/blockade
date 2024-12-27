using UnityEngine;
using UnityEngine.Tilemaps;

public class Utils : MonoBehaviour {
    
    public static void CreateBlocks(Structure.Structure blockStructure, Transform parent, int sortingOrder, float alpha, bool centered) {
        int height = blockStructure.GetGrid().GetLength(0);
        int width = blockStructure.GetGrid().GetLength(1);
        
        Vector2 offset = new Vector2(-(width / 2f), height / 2f);
        
        // adds an offset to blocks with an even width or height to fit in grid when rounded
        if (width % 2 == 0) {
            offset.x += 0.5f;
        }

        if (height % 2 == 0) {
            offset.y += 0.5f;
        }
        
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                GameObject blockObject = blockStructure.GetObjectAtIndex(j, i);
                if (blockObject != null) {
                    float x = centered ? offset.x + i : 0.5f + i;
                    float y = centered ? offset.y - j : -0.5f - j;
                    GameObject block = Instantiate(blockObject, new Vector2(x, y), Quaternion.identity, parent);
                    
                    SpriteRenderer spriteRenderer = block.GetComponent<SpriteRenderer>();
                    spriteRenderer.sortingOrder = sortingOrder;
                    
                    Color tempColor = spriteRenderer.material.color; 
                    tempColor.a = alpha;
                    spriteRenderer.material.color = tempColor;
                }
            }
        }
    }
    
    public static bool IsCellFull(Vector3Int position, Tilemap tilemap) {
        TileBase tile = tilemap.GetTile(position);

        bool isFull = tile != null;

        return isFull;
    }
    
    public static float EaseInCubic(float t) {
        return t * t * t;
    }
}